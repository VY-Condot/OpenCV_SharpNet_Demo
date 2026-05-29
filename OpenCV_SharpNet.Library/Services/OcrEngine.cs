using CsplCam.Library.Enums;
using CsplCam.Library.Models;
using CsplCam.Library.Services._1D_BarCode;
using OpenCvSharp;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using ZXingCpp;

using CvRect = OpenCvSharp.Rect;

namespace CsplCam.Library.Services
{
    /// <summary>
    /// Provides OCR (Optical Character Recognition) functionalities, including character recognition, template management, and skew correction.
    /// Primary class for character recoginition tasks.it inlcudes methods for training,recognizaing etc etc. It also manages the character templates and provides utilities for image processing specific to OCR tasks.
    /// </summary>
    //[DebuggerStepThrough]
    public static class OcrEngine
    {
        /// <summary>
        /// The root directory of the character dataset.
        /// </summary>
        // --- CONFIGURATION ---
        public static string DatasetRoot { get; set; } = @"D:\py\char_dataset"; 

        /// <summary>
        /// Indicates whether to ignore characters with low confidence during character recognition.
        /// </summary>
        public static bool IsNeglectGarabageChar { get; set; } = true;

        /// <summary>
        /// The size of the character templates.
        /// </summary>
        private static readonly OpenCvSharp.Size TemplateSize = new(30, 30);
        private static readonly int NumPixels = TemplateSize.Width * TemplateSize.Height;

        /// <summary>
        /// user for final confifidence match of ocv template matching for accuracy boost as well as speed boost
        /// </summary>
        public static double OcvTargetMatchConfidence { get; set; } = 0.75;

        /// <summary>
        /// Angle in degrees to deskew the character crops during training and recognition. This is a global setting that applies to all ROIs. You can adjust it based on your specific use case and the typical skew you encounter in your images. A value of 20 degrees is a common starting point for many OCR applications, but you may want to experiment with it to find the optimal angle for your dataset.
        /// </summary>
        public static double SkewAngle { get; set; } =  10; //double.TryParse(ConfigurationManager.AppSettings["SkewAngle"], out var skew) ? skew :

        // --- STATE ---
        /// <summary>
        /// Holds the character vectors with aspect ration and char for lightning-fast global fallback search when the targeted search fails.
        /// </summary>
        public static Dictionary<string, List<CharTemplate>> TemplateVectors { get; set; } = new Dictionary<string, List<CharTemplate>>();

        // NEW: FLATTENED MEMORY FOR MASSIVE DATASETS
        // Struct arrays are contiguous in memory. This prevents Cache Misses as the dataset grows!
        /// <summary>
        /// Holds the flattened templates for lightning-fast global fallback search when the targeted search fails.
        /// </summary>
        public struct FastTemplate
        {
            public string Label;
            public float[] Vector;
            public double AspectRatio;
            public double FillDensity;
            public double Cx; // <--- ADD THIS
            public double Cy; // <--- ADD THIS

            //public OcrMaskType CharType; // <--- NEW TAG
        }

        /// <summary>
        /// set the min and max confidence of the anchor character (the character with the highest anchor confidence in the image).
        /// </summary>
        public struct AnchorConfidence
        {
            public static double Min { get; set; } = 0.35;
            public static double Max { get; set; } = 0.50;
        }

        private static FastTemplate[] _globalFlatTemplates = Array.Empty<FastTemplate>();

        private static readonly object _templateLock = new object();

        public static Dictionary<string, string> SpecialReverse = new Dictionary<string, string>
        {
            { "/", "slash" }, { "\\", "backslash" }, { ":", "colon" }, { "*", "asterisk" },
            { "?", "question" }, { "\"", "quote" }, { "<", "less" }, { ">", "greater" },
            { "|", "pipe" }, { ".", "dot" }, { ",", "comma" }, { ";", "semicolon" }
        };

        // =========================================================
        // STATIC KERNELS
        // =========================================================
        private static readonly Mat Kernel2x2 = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(2, 2));
        private static readonly Mat Kernel2x3 = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(2, 3));
        private static readonly Mat Kernel3x3 = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(3, 3));
        private static readonly Mat Kernel5x3 = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(5, 3));
        private static readonly Mat Kernel5x5 = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(5, 5));

        // =========================================================
        // THREAD-SAFE ZERO ALLOCATION HARDWARE POOLS
        // =========================================================
        public class OcrThreadState : IDisposable
        {
            public Mat Resized = new Mat();
            public Mat Bin = new Mat();
            public float[] FloatArray = new float[NumPixels];
            public void Dispose() { Resized.Dispose(); Bin.Dispose(); }
        }

        public class BarcodeThreadState : IDisposable
        {
            public Mat Gray = new Mat();
            public Mat Bin = new Mat();
            public Mat Pad = new Mat();
            public Mat Scale = new Mat();
            public void Dispose() { Gray.Dispose(); Bin.Dispose(); Pad.Dispose(); Scale.Dispose(); }
        }

        private static readonly ConcurrentBag<OcrThreadState> _ocrStatePool = new();
        private static OcrThreadState GetOcrState() => _ocrStatePool.TryTake(out var state) ? state : new OcrThreadState();
        private static void ReturnOcrState(OcrThreadState state) => _ocrStatePool.Add(state);

        private static readonly ConcurrentBag<BarcodeThreadState> _bcStatePool = new();
        private static BarcodeThreadState GetBcState() => _bcStatePool.TryTake(out var state) ? state : new BarcodeThreadState();
        private static void ReturnBcState(BarcodeThreadState state) => _bcStatePool.Add(state);

        // =========================================================
        // Z-XING OPTIONS
        // =========================================================
        public static TextMode BarCodeTextMode => TextMode.Plain;

        private static readonly ReaderOptions _fastOpts = new() 
        { 
            TryRotate = false, 
            TryHarder = false, 
            TextMode = BarCodeTextMode,
            MaxNumberOfSymbols = 255 // <--- THIS ALLOWS MULTIPLE BARCODES!
        };
        private static readonly ReaderOptions _hardOpts = new() 
        { 
            TryRotate = true, 
            TryHarder = true, 
            TextMode = BarCodeTextMode,
            MaxNumberOfSymbols = 255 // <--- THIS ALLOWS MULTIPLE BARCODES!
        };

        // =========================================================
        // EXACT PRECISION SIMD DOT PRODUCT
        // =========================================================
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float FastDotProduct(float[] a, float[] b)
        {
            ref float aRef = ref MemoryMarshal.GetArrayDataReference(a);
            ref float bRef = ref MemoryMarshal.GetArrayDataReference(b);
            int i = 0, length = a.Length;
            float sum = 0;

            if (Vector.IsHardwareAccelerated)
            {
                int limit = length - Vector<float>.Count;
                var vSum = Vector<float>.Zero;
                for (; i <= limit; i += Vector<float>.Count)
                {
                    vSum += Vector.LoadUnsafe(ref aRef, (nuint)i) * Vector.LoadUnsafe(ref bRef, (nuint)i);
                }
                sum += Vector.Dot(vSum, Vector<float>.One);
            }
            // Safely process the remainder for absolute mathematical accuracy
            for (; i < length; i++) sum += Unsafe.Add(ref aRef, i) * Unsafe.Add(ref bRef, i);
            return sum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Mat GetMatObject() => new Mat();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="roiArea"></param>
        /// <returns></returns>
        public static Mat GetMatObject(Mat image,CvRect roiArea) => new Mat(image, roiArea);

        /// <summary>
        /// Function for trained the speacific image area for character recoginition
        /// </summary>
        /// <param name="image">image on which roi is drawn.</param>
        /// <param name="selectedRoi">retangle area which drawn on image</param>
        /// <returns></returns>
        public static List<Mat> TrainRoi(Mat image, RoiObject selectedRoi)
        {
            List<Mat> crops = new List<Mat>();

            CvRect safeBox = selectedRoi.Box.Intersect(new CvRect(0, 0, image.Width, image.Height));
            if (safeBox.Width == 0 || safeBox.Height == 0) return crops;

            using Mat crop = new Mat(image, safeBox);
            Mat rotatedCrop = null;
            Mat deskewedCrop = null;
            Mat trainingSourceImage = null;
            Mat tempMorphedImage = null;

            try
            {
                RotateImage(crop, out rotatedCrop, selectedRoi.RotationAngle);
                deskewedCrop = DeskewImage(rotatedCrop, out double skewAngle);

                if (selectedRoi.MorphOp == MorphOperation.None)
                {
                    trainingSourceImage = deskewedCrop;
                }
                else
                {
                    tempMorphedImage = new Mat();
                    using Mat tempGray = new Mat();
                    if (deskewedCrop.Channels() == 3) Cv2.CvtColor(deskewedCrop, tempGray, ColorConversionCodes.BGR2GRAY);
                    else deskewedCrop.CopyTo(tempGray);

                    using Mat tempTh = new Mat();
                    ProcessImageForMode(tempGray, tempTh, selectedRoi.SegmentationMode);
                    MorphologyProcessor.Apply(tempTh, selectedRoi.MorphOp, selectedRoi.MorphKernelWidth, selectedRoi.MorphKernelHeight, selectedRoi.MorphIterations);

                    Cv2.BitwiseNot(tempTh, tempMorphedImage);
                    trainingSourceImage = tempMorphedImage;
                }

                bool oldFilterState = IsNeglectGarabageChar;
                IsNeglectGarabageChar = false;

                var boxes = GetCharacterBoxes(deskewedCrop, selectedRoi);

                IsNeglectGarabageChar = oldFilterState;

                if (boxes.Count == 0) return crops;

                foreach (var b in boxes) crops.Add(new Mat(trainingSourceImage, b).Clone());
            }
            finally
            {
                if (rotatedCrop != null && !rotatedCrop.IsDisposed) rotatedCrop.Dispose();
                if (deskewedCrop != null && !deskewedCrop.IsDisposed) deskewedCrop.Dispose();
                if (tempMorphedImage != null && !tempMorphedImage.IsDisposed) tempMorphedImage.Dispose();
            }

            return crops;
        }

        // --------------------------------------------------------
        // 1. TEMPLATE LOADING 
        // --------------------------------------------------------
        /// <summary>
        /// funcation which is responsible for loading character dataset templates
        /// </summary>
        public static void ReloadTemplates()
        {
            var newDict = new Dictionary<string, List<CharTemplate>>();
            var flatList = new List<FastTemplate>();

            if (!Directory.Exists(DatasetRoot)) Directory.CreateDirectory(DatasetRoot);

            foreach (var dir in Directory.GetDirectories(DatasetRoot))
            {
                string label = new DirectoryInfo(dir).Name;
                if (!newDict.ContainsKey(label)) newDict[label] = new List<CharTemplate>();

                foreach (var file in Directory.GetFiles(dir))
                {
                    using Mat img = Cv2.ImRead(file, ImreadModes.Grayscale);
                    if (img.Empty()) continue;

                    double ar = (double)img.Width / img.Height;
                    double density = (double)Cv2.CountNonZero(img) / (img.Width * img.Height);

                    using Mat resized = new();
                    Cv2.Resize(img, resized, TemplateSize);
                    using Mat bin = new();
                    Cv2.Threshold(resized, bin, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);
                    using Mat vec = new();
                    bin.ConvertTo(vec, MatType.CV_32F);
                    using Mat flat = vec.Reshape(1, 1);

                    // DOUBLE PRECISION NORM TO MATCH EXACT ACCURACY OF ORIGINAL CODE
                    double norm = Cv2.Norm(flat);

                    using Mat finalFlat = new Mat();
                    flat.ConvertTo(finalFlat, MatType.CV_32F, 1.0 / (norm + 1e-6));

                    float[] floatArray = new float[finalFlat.Total()];
                    Marshal.Copy(finalFlat.Data, floatArray, 0, floatArray.Length);

                    // ====================================================================
                    // NEW ADDITION: Calculate Center of Mass ONCE during load time.
                    // This makes the Recognition phase lightning fast while solving I vs T.
                    // ====================================================================
                    double tSumX = 0, tSumY = 0, tTotal = 0;
                    for (int y = 0; y < TemplateSize.Height; y++)
                    {
                        int rowOff = y * TemplateSize.Width;
                        for (int x = 0; x < TemplateSize.Width; x++)
                        {
                            float val = floatArray[rowOff + x];
                            tSumX += val * x;
                            tSumY += val * y;
                            tTotal += val;
                        }
                    }
                    // Normalize the coordinates to be between 0.0 and 1.0
                    double tmplCx = tTotal > 0 ? (tSumX / tTotal) / TemplateSize.Width : 0.5;
                    double tmplCy = tTotal > 0 ? (tSumY / tTotal) / TemplateSize.Height : 0.5;
                    // ====================================================================

                    // Save to the Dictionary (Targeted Search)
                    newDict[label].Add(new CharTemplate
                    {
                        Vector = floatArray,
                        AspectRatio = ar,
                        FillDensity = density,
                        Cx = tmplCx,
                        Cy = tmplCy
                    });

                    // Save to the Flattened Array (Global Fallback Search)
                    flatList.Add(new FastTemplate
                    {
                        Label = label,
                        Vector = floatArray,
                        AspectRatio = ar,
                        FillDensity = density,
                        Cx = tmplCx,
                        Cy = tmplCy
                    });
                }
            }

            var flatArray = flatList.ToArray();
            var oldDict = TemplateVectors;

            lock (_templateLock)
            {
                TemplateVectors = newDict;
                _globalFlatTemplates = flatArray;
            }
            if (oldDict != null) oldDict.Clear();
        }

        /// <summary>
        /// The purpose of this method is to save an image (charImg) with a specific label (label) to a designated folder.
        /// </summary>
        /// <param name="charImg">recognised char image</param>
        /// <param name="label">recognised character</param>
        public static void SaveTemplate(Mat charImg, string label)
        {
            string folderName = label;
            if (SpecialReverse.ContainsKey(label)) folderName = SpecialReverse[label];
            else if (label.Length == 1 && char.IsLetter(label[0])) folderName = char.IsLower(label[0]) ? "lower_" + label : "upper_" + label;

            string folderPath = Path.Combine(DatasetRoot, folderName);
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            int i = 1;
            string fpath;
            do { fpath = Path.Combine(folderPath, $"{folderName}_{i}.png"); i++; } while (File.Exists(fpath));
            Cv2.ImWrite(fpath, charImg);
        }

        // --------------------------------------------------------
        // 2. SEGMENTATION 
        // --------------------------------------------------------
        /// <summary>
        /// The purpose of this method is to segment an image (gray) into a binary image (binaryOut) based on a specific segmentation mode (mode).
        /// </summary>
        /// <param name="gray">images which u want to process</param>
        /// <param name="binaryOut">final image after processing</param>
        /// <param name="mode">mode of processing</param>
        public static void ProcessImageForMode(Mat gray, Mat binaryOut, SegmentationMode mode)
        {
            switch (mode)
            {
                case SegmentationMode.Punctuation:
                    Cv2.Threshold(gray, binaryOut, 0, 255, ThresholdTypes.BinaryInv | ThresholdTypes.Otsu);
                    Cv2.MorphologyEx(binaryOut, binaryOut, MorphTypes.Close, Kernel2x2);
                    break;
                case SegmentationMode.NoiseHeavy:
                    Cv2.AdaptiveThreshold(gray, binaryOut, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.BinaryInv, 35, 10);
                    Cv2.MorphologyEx(binaryOut, binaryOut, MorphTypes.Open, Kernel2x3, iterations: 2);
                    break;
                case SegmentationMode.HighAccuracy:
                    using (var blurred = new Mat())
                    {
                        Cv2.GaussianBlur(gray, blurred, new OpenCvSharp.Size(3, 3), 0);
                        Cv2.AdaptiveThreshold(blurred, binaryOut, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, 25, 12);
                    }
                    break;
                case SegmentationMode.Industrial:
                    using (var filtered = new Mat())
                    {
                        Cv2.BilateralFilter(gray, filtered, 9, 40, 40);
                        Cv2.Threshold(filtered, binaryOut, 0, 255, ThresholdTypes.BinaryInv | ThresholdTypes.Otsu);
                        Cv2.MorphologyEx(binaryOut, binaryOut, MorphTypes.Close, Kernel2x3);
                    }
                    break;
                case SegmentationMode.Balanced:
                default:

                    //Cv2.AdaptiveThreshold(gray, binaryOut, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, 25, 15);

                    Cv2.AdaptiveThreshold(gray, binaryOut, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, 41, 30);

                    Cv2.MorphologyEx(binaryOut, binaryOut, MorphTypes.Close, Kernel5x3);
                    break;
            }
        }

        // 2. BACKWARD-COMPATIBLE OVERLOAD (Used by your CameraDemo.cs UI to prevent compile errors)
        public static Mat ProcessImageForMode(Mat gray, SegmentationMode mode)
        {
            Mat binaryOut = new Mat();
            ProcessImageForMode(gray, binaryOut, mode);
            return binaryOut;
        }


        // Exact restored Math logic for character alignment accuracy
        /// <summary>
        /// core method to find the dark pixels of the characters from the image
        /// </summary>
        /// <param name="srcImg">image which u want to process</param>
        /// <param name="roi">meta data of region of interest</param>
        /// <returns></returns>
        public static List<Rect> GetCharacterBoxes(Mat srcImg, RoiObject roi)
        {
            Mat gray = srcImg;
            bool mustDisposeGray = false;
            if (srcImg.Channels() == 3)
            {
                gray = srcImg.CvtColor(ColorConversionCodes.BGR2GRAY);
                mustDisposeGray = true;
            }

            try
            {
                using Mat th = new Mat();
                ProcessImageForMode(gray, th, roi.SegmentationMode);

                if (roi.MorphOp != MorphOperation.None && roi.MorphKernelWidth > 0 && roi.MorphKernelHeight > 0 && roi.MorphIterations > 0)
                {
                    MorphologyProcessor.Apply(th, roi.MorphOp, roi.MorphKernelWidth, roi.MorphKernelHeight, roi.MorphIterations);
                }

                Cv2.FindContours(th, out var contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
                var initialBoxes = new List<Rect>(contours.Length);
                int effectiveMinH = roi.MinBlobH, effectiveMinW = roi.MinBlobW;
                int imgW2 = srcImg.Width - 2, imgH2 = srcImg.Height - 2;

                foreach (var cnt in contours)
                {
                    var rect = Cv2.BoundingRect(cnt);
                    if (rect.Width >= effectiveMinW && rect.Height >= effectiveMinH && rect.Width <= roi.MaxBlobW && rect.Height <= roi.MaxBlobH)
                    {
                        if (IsNeglectGarabageChar && (rect.X <= 2 || rect.Y <= 2 || rect.Right >= imgW2 || rect.Bottom >= imgH2)) continue;


                        //// ---> ADD THIS NEW CODE HERE <---
                        //// Calculate the actual area of the contour vs the bounding box area
                        //double contourArea = Cv2.ContourArea(cnt);
                        //double boundingBoxArea = rect.Width * rect.Height;
                        //double fillRatio = contourArea / boundingBoxArea;

                        //// If the contour takes up less than 15% of its own bounding box, it is noise/lines
                        //if (fillRatio < 10)
                        //    continue;
                        //// --------------------------------

                        initialBoxes.Add(rect);
                    }
                }
               
                if (initialBoxes.Count == 0) return initialBoxes;

                var solidBoxes = new List<Rect>(initialBoxes.Count);
                foreach (var b in initialBoxes) if (b.Height > 8) solidBoxes.Add(b);
                if (solidBoxes.Count == 0) solidBoxes = initialBoxes;

                solidBoxes.Sort((a, b) => a.Height.CompareTo(b.Height));
                int medianH = solidBoxes.Count > 0 ? solidBoxes[solidBoxes.Count / 2].Height : 20;

                bool mergedAny;
                do
                {
                    mergedAny = false;
                    for (int i = 0; i < initialBoxes.Count; i++)
                    {
                        for (int j = i + 1; j < initialBoxes.Count; j++)
                        {
                            Rect b1 = initialBoxes[i], b2 = initialBoxes[j];
                            int minW = Math.Min(b1.Width, b2.Width);
                            int horizontalOverlap = Math.Max(0, Math.Min(b1.Right, b2.Right) - Math.Max(b1.Left, b2.Left));

                            if (horizontalOverlap >= minW * 0.3)
                            {
                                int verticalGap = Math.Max(0, Math.Max(b1.Y, b2.Y) - Math.Min(b1.Bottom, b2.Bottom));
                                int combinedHeight = Math.Max(b1.Bottom, b2.Bottom) - Math.Min(b1.Y, b2.Y);
                                int maxAllowedGap = (int)(medianH * 0.6);

                                if (b1.Height < medianH * 0.5 && b2.Height < medianH * 0.5) maxAllowedGap = Math.Max(b1.Height, b2.Height) * 3;

                                if (verticalGap <= maxAllowedGap && combinedHeight <= (medianH * 1.15))
                                {
                                    int newX = Math.Min(b1.X, b2.X), newY = Math.Min(b1.Y, b2.Y);
                                    initialBoxes[i] = new Rect(newX, newY, Math.Max(b1.Right, b2.Right) - newX, Math.Max(b1.Bottom, b2.Bottom) - newY);
                                    initialBoxes.RemoveAt(j);
                                    mergedAny = true; break;
                                }
                            }
                        }
                        if (mergedAny) break;
                    }
                } while (mergedAny);

                // RESTORED EXACT ACCURACY FOR LINE MATH
                var boxesWithCenters = initialBoxes.Select(b => new { Box = b, CenterY = b.Y + (b.Height / 2.0) }).OrderBy(x => x.CenterY).ToList();
                var lines = new List<List<Rect>>();
                double lineThreshold = medianH * 0.65;

                foreach (var item in boxesWithCenters)
                {
                    bool placed = false;
                    foreach (var line in lines)
                    {
                        double avgLineCenterY = 0;
                        foreach (var b in line) avgLineCenterY += b.Y + (b.Height / 2.0);
                        avgLineCenterY /= line.Count;

                        if (Math.Abs(item.CenterY - avgLineCenterY) <= lineThreshold)
                        {
                            line.Add(item.Box);
                            placed = true;
                            break;
                        }
                    }
                    if (!placed) lines.Add(new List<Rect> { item.Box });
                }

                var sortedBoxes = new List<Rect>();
                lines = lines.OrderBy(l => {
                    double avg = 0;
                    foreach (var b in l) avg += b.Y + (b.Height / 2.0);
                    return avg / l.Count;
                }).ToList();

                foreach (var line in lines)
                {
                    var sortedLine = line.OrderBy(b => b.X).ToList();
                    sortedBoxes.AddRange(sortedLine);
                }

                if (roi.SegmentationMode == SegmentationMode.HighAccuracy)
                {
                    double minAllowed = medianH * 0.25;
                    sortedBoxes.RemoveAll(b => b.Height < minAllowed);
                }
                else if (roi.SegmentationMode == SegmentationMode.Industrial)
                    sortedBoxes.RemoveAll(b => b.Height < 3);

                return sortedBoxes;
            }
            finally
            {
                if (mustDisposeGray) gray.Dispose();
            }
        }

        /// <summary>
        /// Rotate image upto specific angle
        /// </summary>
        /// <param name="srcImg">incoming image for processing</param>
        /// <param name="dstImg">output image after processing of rotation</param>
        /// <param name="rotationAngles">rotatio angle upti which image will be rotated</param>
        /// <param name="deepCopyForZero">if true then deep copy will be done</param>
        public static void RotateImage(Mat srcImg, out Mat dstImg, RotationAngles rotationAngles, bool deepCopyForZero = false)
        {
            switch (rotationAngles)
            {
                case RotationAngles.Ninety: dstImg = new Mat(); Cv2.Rotate(srcImg, dstImg, RotateFlags.Rotate90Counterclockwise); break;
                case RotationAngles.OneEighty: dstImg = new Mat(); Cv2.Rotate(srcImg, dstImg, RotateFlags.Rotate180); break;
                case RotationAngles.TwoSeventy: dstImg = new Mat(); Cv2.Rotate(srcImg, dstImg, RotateFlags.Rotate90Clockwise); break;
                default: dstImg = deepCopyForZero ? srcImg.Clone() : srcImg; break;
            }
        }

        public static (string label, double score) Recognize(Mat charImg, double ThresholdRatio, string targetLabel = null)
        {
            var ts = GetOcrState();
            try { return RecognizeImpl(charImg, ThresholdRatio, targetLabel, ts); }
            finally { ReturnOcrState(ts); }
        }

        /// <summary>
        /// the core function which is reponsible for character recognition based on contours and template matching
        /// </summary>
        /// <param name="charImg">image for processing</param>
        /// <param name="ThresholdRatio">minimum thresold for ignoring the garbage characters</param>
        /// <param name="targetLabel">expected character to be recognised.default is null</param>
        /// <param name="ts">thread is used for parallel processing</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static (string label, double score) RecognizeImpl(Mat charImg, double ThresholdRatio, string targetLabel, OcrThreadState ts)
        {
            var flatArray = _globalFlatTemplates;
            if (flatArray.Length == 0) return ("?", 0.0);

            Mat gray = charImg.Channels() == 3 ? charImg.CvtColor(ColorConversionCodes.BGR2GRAY) : charImg;

            try
            {
                double inputAR = (double)charImg.Width / charImg.Height;
                double inputDensity = (double)Cv2.CountNonZero(gray) / (charImg.Width * charImg.Height);

                Cv2.Resize(gray, ts.Resized, TemplateSize);
                Cv2.Threshold(ts.Resized, ts.Bin, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

                // --- Pre-parse string masks ---
                bool isHash = targetLabel == "#";
                bool isAt = targetLabel == "@";
                bool hasSpecificTarget = !string.IsNullOrEmpty(targetLabel) && !isHash && !isAt && targetLabel != "*";
                // ------------------------------

                double inSumX = 0, inSumY = 0, inTotal = 0;
                unsafe
                {
                    byte* pSrc = (byte*)ts.Bin.DataPointer;
                    ref float dstRef = ref MemoryMarshal.GetArrayDataReference(ts.FloatArray);
                    double sumSq = 0.0;

                    int x = 0, y = 0;
                    int w = TemplateSize.Width;

                    for (int i = 0; i < NumPixels; i++)
                    {
                        float val = pSrc[i];
                        Unsafe.Add(ref dstRef, i) = val;
                        sumSq += val * val;

                        inSumX += val * x;
                        inSumY += val * y;
                        inTotal += val;

                        x++;
                        if (x >= w) { x = 0; y++; }
                    }
                    float invNorm = (float)(1.0 / (Math.Sqrt(sumSq) + 1e-6));
                    for (int i = 0; i < NumPixels; i++) Unsafe.Add(ref dstRef, i) *= invNorm;
                }

                double inputCx = inTotal > 0 ? (inSumX / inTotal) / TemplateSize.Width : 0.5;
                double inputCy = inTotal > 0 ? (inSumY / inTotal) / TemplateSize.Height : 0.5;

                string bestLabel = "?";
                double bestScore = -1.0;
                bool skipGlobalSearch = false;

                // --- STEP 1: TARGETED OCV VERIFICATION (Ultra Fast) ---
                if (hasSpecificTarget)
                {
                    string folderName = targetLabel;
                    if (SpecialReverse.ContainsKey(targetLabel)) folderName = SpecialReverse[targetLabel];
                    else if (targetLabel.Length == 1 && char.IsLetter(targetLabel[0])) folderName = char.IsLower(targetLabel[0]) ? "lower_" + targetLabel : "upper_" + targetLabel;

                    if (TemplateVectors.TryGetValue(folderName, out var templates))
                    {
                        if (templates.Count == 0) return ("?", 0.0);

                        //convert into unsafe pointer for faster access
                        
                        //1.convert into span for direct acces
                         var charTempates = CollectionsMarshal.AsSpan(templates);

                        //convert into usafe array for direct access
                        ref CharTemplate templateArray = ref MemoryMarshal.GetReference(charTempates);

                        //3.start accessing items
                        for (int i = 0; i < charTempates.Length; i++)
                        {
                            //get actual chartemplate item
                            ref CharTemplate item = ref Unsafe.Add(ref templateArray,i);

                            double arDiff = Math.Abs(inputAR - item.AspectRatio);
                            double densityDiff = Math.Abs(inputDensity - item.FillDensity);
                            double cxDiff = Math.Abs(inputCx - item.Cx);
                            double cyDiff = Math.Abs(inputCy - item.Cy);

                            double penalty = (arDiff * 0.8) + (densityDiff * 0.5) + (cxDiff * 1.5) + (cyDiff * 1.5);
                            if (arDiff > 0.15) penalty += (arDiff * 2.5);

                            double finalScore = FastDotProduct(ts.FloatArray, item.Vector) - penalty;

                            if (finalScore > bestScore) 
                            { 
                                bestScore = finalScore; 
                                bestLabel = folderName; 
                            }
                        }

                        // ====================================================================
                        // THE HYBRID FIX: "The Confidence Check"
                        // If the expected character is a VERY STRONG match (> 85%), 
                        // we trust it completely and skip the global search (Massive Speed Boost).
                        // If it's a weak match (like 52%), we DO NOT trust it. We force a global 
                        // search to see if another character is actually a better fit.
                        // ====================================================================
                        double confidenceThreshold = Math.Max(OcvTargetMatchConfidence, ThresholdRatio); // Use 85%, or the user's threshold if they set it higher.

                        //check global search based on confidence
                        skipGlobalSearch = bestScore >= confidenceThreshold;

                        ////// THE FIX: If the user provided a specific letter/number, NEVER search the whole dataset!
                        ////skipGlobalSearch = true;
                        //if(!skipGlobalSearch && bestLabel.ToUpper().Equals(folderName.ToUpper()))
                        //    skipGlobalSearch = true;
                    }
                    else
                    {
                        // Safety: The user expects a character, but hasn't trained it yet. 
                        //bestLabel = "?";
                        //bestScore = 0.0;

                        ////fall bakc to global serach
                        skipGlobalSearch = false;
                    }

                    //// only run when the global search is true  
                    //if (skipGlobalSearch)
                    //{
                    //    // ====================================================================
                    //    // THE HYBRID FIX: "The Confidence Check"
                    //    // If the expected character is a VERY STRONG match (> 85%), 
                    //    // we trust it completely and skip the global search (Massive Speed Boost).
                    //    // If it's a weak match (like 52%), we DO NOT trust it. We force a global 
                    //    // search to see if another character is actually a better fit.
                    //    // ====================================================================
                    //    double confidenceThreshold = Math.Max(OcvTargetMatchConfidence, ThresholdRatio); // Use 85%, or the user's threshold if they set it higher.

                    //    skipGlobalSearch = bestScore >= confidenceThreshold;
                    //}
                }

                // --- STEP 2: GLOBAL FALLBACK OCR (Only runs for *, #, @, or empty Expected Text) ---
                if (!skipGlobalSearch)
                {
                    ref FastTemplate flatRef = ref MemoryMarshal.GetArrayDataReference(flatArray);
                    for (int i = 0; i < flatArray.Length; i++)
                    {
                        ref FastTemplate item = ref Unsafe.Add(ref flatRef, i);

                        if (isHash && !char.IsDigit(item.Label[0])) continue;
                        if (isAt && !char.IsLetter(item.Label[0])) continue;

                        double arDiff = Math.Abs(inputAR - item.AspectRatio);
                        double densityDiff = Math.Abs(inputDensity - item.FillDensity);
                        double cxDiff = Math.Abs(inputCx - item.Cx);
                        double cyDiff = Math.Abs(inputCy - item.Cy);

                        double penalty = (arDiff * 0.8) + (densityDiff * 0.5) + (cxDiff * 1.5) + (cyDiff * 1.5);
                        if (arDiff > 0.15) penalty += (arDiff * 2.5);

                        if (1.0 - penalty < bestScore) continue;

                        double finalScore = FastDotProduct(ts.FloatArray, item.Vector) - penalty;
                        if (finalScore > bestScore) { bestScore = finalScore; bestLabel = item.Label; }
                    }
                }

                if (bestLabel.StartsWith("lower_") || bestLabel.StartsWith("upper_")) 
                    bestLabel = bestLabel.Substring(6);
                var specialEntry = SpecialReverse.FirstOrDefault(x => x.Value == bestLabel);
                if (specialEntry.Key != null) bestLabel = specialEntry.Key;


                // ====================================================================
                // THE CASE-SENSITIVITY FIX (Your brilliant observation!)
                // Because a 30x30 'S' and 's' are mathematically identical, the engine 
                // might guess the wrong case. If the letter matches but the case is wrong, 
                // we force it to match the Expected Text case!
                // ====================================================================
                //if (hasSpecificTarget && string.Equals(bestLabel, targetLabel, StringComparison.OrdinalIgnoreCase))
                //{
                //    bestLabel = targetLabel;
                //

                if (hasSpecificTarget && !string.Equals(bestLabel, targetLabel, StringComparison.Ordinal))
                {
                    //bestLabel = targetLabel;
                    bestLabel = "?";
                    bestScore = 0.0;
                }

                return (bestLabel, Math.Clamp(bestScore, 0, 1));
            }
            finally { if (charImg.Channels() == 3) gray.Dispose(); }
        }


        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static (string label, double score) RecognizeImpl_New(Mat charImg, double ThresholdRatio, string targetLabel, OcrThreadState ts)
        {
            var flatArray = _globalFlatTemplates;
            if (flatArray.Length == 0) return ("?", 0.0);

            Mat gray = charImg.Channels() == 3 ? charImg.CvtColor(ColorConversionCodes.BGR2GRAY) : charImg;

            try
            {
                double inputAR = (double)charImg.Width / charImg.Height;
                double inputDensity = (double)Cv2.CountNonZero(gray) / (charImg.Width * charImg.Height);

                Cv2.Resize(gray, ts.Resized, TemplateSize);
                Cv2.Threshold(ts.Resized, ts.Bin, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

                // --- Masks ONLY ---
                bool isHash = targetLabel == "#";
                bool isAt = targetLabel == "@";

                double inSumX = 0, inSumY = 0, inTotal = 0;
                unsafe
                {
                    byte* pSrc = (byte*)ts.Bin.DataPointer;
                    ref float dstRef = ref MemoryMarshal.GetArrayDataReference(ts.FloatArray);
                    double sumSq = 0.0;

                    int x = 0, y = 0;
                    int w = TemplateSize.Width;

                    for (int i = 0; i < NumPixels; i++)
                    {
                        float val = pSrc[i];
                        Unsafe.Add(ref dstRef, i) = val;
                        sumSq += val * val;

                        inSumX += val * x;
                        inSumY += val * y;
                        inTotal += val;

                        x++;
                        if (x >= w) { x = 0; y++; }
                    }
                    float invNorm = (float)(1.0 / (Math.Sqrt(sumSq) + 1e-6));
                    for (int i = 0; i < NumPixels; i++) Unsafe.Add(ref dstRef, i) *= invNorm;
                }

                double inputCx = inTotal > 0 ? (inSumX / inTotal) / TemplateSize.Width : 0.5;
                double inputCy = inTotal > 0 ? (inSumY / inTotal) / TemplateSize.Height : 0.5;

                string bestLabel = "?";
                double bestScore = -1.0;

                // ====================================================================
                // TRUE GLOBAL OCR
                // Always search all templates to find the True Character.
                // This guarantees the UI outputs 'G' even if the user incorrectly typed 'd'.
                // ====================================================================
                ref FastTemplate flatRef = ref MemoryMarshal.GetArrayDataReference(flatArray);
                for (int i = 0; i < flatArray.Length; i++)
                {
                    ref FastTemplate item = ref Unsafe.Add(ref flatRef, i);

                    // Skip templates only if the user explicitly used a Format Mask
                    if (isHash && !char.IsDigit(item.Label[0])) continue;
                    if (isAt && !char.IsLetter(item.Label[0])) continue;

                    double arDiff = Math.Abs(inputAR - item.AspectRatio);
                    double densityDiff = Math.Abs(inputDensity - item.FillDensity);

                    double cxDiff = (item.Cx == 0 && item.Cy == 0) ? 0 : Math.Abs(inputCx - item.Cx);
                    double cyDiff = (item.Cx == 0 && item.Cy == 0) ? 0 : Math.Abs(inputCy - item.Cy);

                    double penalty = (arDiff * 0.8) + (densityDiff * 0.5) + (cxDiff * 1.5) + (cyDiff * 1.5);
                    if (arDiff > 0.15) penalty += (arDiff * 2.5);

                    if (1.0 - penalty < bestScore) continue;

                    double finalScore = FastDotProduct(ts.FloatArray, item.Vector) - penalty;
                    if (finalScore > bestScore) { bestScore = finalScore; bestLabel = item.Label; }
                }

                if (bestLabel.StartsWith("lower_") || bestLabel.StartsWith("upper_")) bestLabel = bestLabel.Substring(6);
                var specialEntry = SpecialReverse.FirstOrDefault(x => x.Value == bestLabel);
                if (specialEntry.Key != null) bestLabel = specialEntry.Key;

                return (bestLabel, Math.Clamp(bestScore, 0, 1));
            }
            finally { if (charImg.Channels() == 3) gray.Dispose(); }
        }
        private static BarcodeFormats GetBarCode(bool isAuto, string StrFomat)
        {
            if (isAuto || !Enum.TryParse(typeof(BarcodeFormats), StrFomat, out var format) || format == null) return BarcodeFormats.Any;
            return (BarcodeFormats)format;
        }

        /// <summary>
        /// function used to read barcode only.implemented multiple fall back mechanism to read barcodes in even harder
        /// </summary>
        /// <param name="crop">crop image with barcode</param>
        /// <param name="roi">meta data of roi</param>
        public static void ReadBarcode(Mat crop, RoiObject roi)
        {
            if (crop.Empty()) return;

            var bcState = GetBcState();
            var selectedFormat = GetBarCode(roi.isBarCodeFormatAuto, roi.BarCodeFormat);
            if (selectedFormat == BarcodeFormats.None) selectedFormat = BarcodeFormats.Any;

            _fastOpts.Formats = selectedFormat;
            _hardOpts.Formats = selectedFormat;

            // PHARMACODE SPECIAL CASE (Kept as separate logic per your business rules)
            if (roi.BarCodeFormat.Equals("Pharma", StringComparison.OrdinalIgnoreCase))
            {
                TryDecodePharmacode(crop, roi);
                if (string.IsNullOrEmpty(roi.DecodedText)) roi.DecodedText = "Failed";
                ReturnBcState(bcState);
                return;
            }

            try
            {
                Mat graySrc = crop;
                if (crop.Channels() == 3)
                {
                    Cv2.CvtColor(crop, bcState.Gray, ColorConversionCodes.BGR2GRAY);
                    graySrc = bcState.Gray;
                }

                roi.CharResults.Clear();

                // =================================================================
                // TIER 2: PARALLEL BRUTE-FORCE GRID RECOVERY (High Speed & Superior)
                // =================================================================
                if (roi.UseBruteForceGridRecovery)
                {
                    //call brute force method
                    BruteForceDecode(crop,roi);

                    //check and return or pass 
                    if (!string.IsNullOrWhiteSpace(roi.DecodedText)) return;
                }



                // =================================================================
                // TIER 1: THE SPEED PATH (0-2ms) (Only runs if Brute Force is OFF)
                // Best for clean, high-contrast labels.
                // =================================================================

                // 1. Raw Grayscale
                TryDecodeFast(graySrc, _fastOpts, 0, 1.0, roi, crop.Width, crop.Height);
                if (!string.IsNullOrEmpty(roi.DecodedText)) return;

                // 2. Standard Global Binarization (Otsu)
                Cv2.Threshold(graySrc, bcState.Bin, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);
                TryDecodeFast(bcState.Bin, _fastOpts, 0, 1.0, roi, crop.Width, crop.Height);
                if (!string.IsNullOrEmpty(roi.DecodedText)) return;

                // =================================================================
                // TIER 2: THE ROBUST PATH (2-8ms)
                // Solves 90% of industrial issues: Shadows, Gradients, and Low Light.
                // =================================================================

                // 3. Universal Adaptive Threshold (Pierces shadows on ANY code type)
                using Mat adaptiveBin = new Mat();
                Cv2.AdaptiveThreshold(graySrc, adaptiveBin, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary, 25, 10);
                TryDecodeFast(adaptiveBin, _hardOpts, 0, 1.0, roi, crop.Width, crop.Height);
                if (!string.IsNullOrEmpty(roi.DecodedText)) return;

                // Padded Version
                int globalPad = 20;
                Cv2.CopyMakeBorder(graySrc, bcState.Pad, globalPad, globalPad, globalPad, globalPad, BorderTypes.Constant, Scalar.White);
                TryDecodeFast(bcState.Pad, _hardOpts, globalPad, 1.0, roi, crop.Width, crop.Height);
                if (!string.IsNullOrEmpty(roi.DecodedText)) return;

                // 4. Multi-Scale Logic (Finds tiny codes)
                double scale = 2.0;
                Cv2.Resize(bcState.Bin, bcState.Scale, new OpenCvSharp.Size(0, 0), scale, scale, InterpolationFlags.Nearest);
                TryDecodeFast(bcState.Scale, _hardOpts, 0, scale, roi, crop.Width, crop.Height);
                if (!string.IsNullOrEmpty(roi.DecodedText)) return;

                // step 7. brute force grid recovery (Global Enhanced Pass)
                // A. PRE-PROCESS THE WHOLE CROP FIRST
                using Mat enhanced_BruteForce = new Mat();
                using var clahe_BruteForce = Cv2.CreateCLAHE(clipLimit: 3.0, tileGridSize: new OpenCvSharp.Size(8, 8));
                clahe_BruteForce.Apply(graySrc, enhanced_BruteForce);
                using Mat bin_BruteForce = new Mat();
                Cv2.AdaptiveThreshold(enhanced_BruteForce, bin_BruteForce, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary, 35, 10);

                // B. TRY SINGLE-PASS ON ENHANCED IMAGE
                TryDecodeFast(bin_BruteForce, _hardOpts, 0, 1.0, roi, crop.Width, crop.Height);
                if (!string.IsNullOrEmpty(roi.DecodedText)) return;


                // Run the stretch sweep
                //TryDecodeCylindrical(graySrc, roi, crop.Width, crop.Height);

                // If it worked, we are done! Return instantly.
                //if (!string.IsNullOrEmpty(roi.DecodedText)) return;

                // =================================================================
                // TIER 3: EXTREME DPM & CURVED CYLINDER PATH
                // Kills glare, merges laser dots, and stretches out cylindrical distortion
                // =================================================================

                // Note: I see you have an "Advanced Mode" checkbox in your UI. 
                // If you have a variable for it, you can wrap this in: if(roi.IsAdvancedMode)
                //TryDecodeExtremeDPM(graySrc, _hardOpts, roi);
                //if (!string.IsNullOrEmpty(roi.DecodedText) && roi.DecodedText != "Failed") return;

                // 6. PHARMACODE AUTO-DETECT FALLBACK
                if (roi.isBarCodeFormatAuto)
                {
                    TryDecodePharmacode(crop, roi);
                    if (!string.IsNullOrEmpty(roi.DecodedText)) return;
                }

                roi.DecodedText = "Failed";
            }
            catch { roi.DecodedText = "Failed"; }
            finally { ReturnBcState(bcState); }
        }

        /// <summary>
        /// adavanced barcode processing which uses the brute force grid recovery mode by dividing the barcode image into multiple section
        /// </summary>
        /// <param name="crop">image to be process</param>
        /// <param name="roi">meta data of the image</param>
        public static void BruteForceDecode(Mat crop, RoiObject roi)
        {
            if (crop == null || crop.Empty()) return;

            // 1. FAST PRE-PROCESS
            using Mat gray = new Mat();
            if (crop.Channels() == 3) Cv2.CvtColor(crop, gray, ColorConversionCodes.BGR2GRAY);
            else crop.CopyTo(gray);

            // 2. THE USER-PROOF FIX: ADD MASSIVE QUIET ZONE (PADDING)
            // Industrial users draw tight boxes. ZXing needs white space. 
            // We add 60 pixels of white space around the entire ROI first.
            int globalPad = 60;
            using Mat paddedContainer = new Mat();
            Cv2.CopyMakeBorder(gray, paddedContainer, globalPad, globalPad, globalPad, globalPad, BorderTypes.Constant, Scalar.White);

            roi.CharResults.Clear();
            ConcurrentBag<CharResult> concurrentResults = new ConcurrentBag<CharResult>();

            //// 3. TIER 1: FULL ROI CHECK (Zero Math - 2ms)
            //// This fixes the bug where Advanced Mode misses the "easy" codes.
            //TryDecodeFast(paddedContainer, _hardOpts, 0, 1.0, roi, paddedContainer.Width, paddedContainer.Height);
            //if (roi.CharResults.Count > 0)
            //{
            //    // Move these initial results to our bag and clear roi results for the tiling phase
            //    foreach (var r in roi.CharResults) concurrentResults.Add(r);
            //    roi.CharResults.Clear();
            //}

            // 4. TILE MATH
            int tileW = Math.Min(450, paddedContainer.Width);
            int step = (int)(tileW * 0.6); // 40% overlap for safety
            int numTiles = ((paddedContainer.Width - tileW) / step) + 1;
            if (paddedContainer.Width > tileW && (paddedContainer.Width - tileW) % step != 0) numTiles++;
            if (paddedContainer.Width <= tileW) numTiles = 1;

            // 5. PARALLEL MULTI-PASS SCANNING
            Parallel.For(0, numTiles, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, i =>
            {
                int currentX = i * step;
                if (currentX + tileW > paddedContainer.Width) currentX = paddedContainer.Width - tileW;

                using Mat tileRaw = new Mat(paddedContainer, new Rect(currentX, 0, tileW, paddedContainer.Height));
                RoiObject tileRes = new RoiObject { CharResults = new List<CharResult>() };

                // --- PASS A: ENHANCED GRAY (Kills glare, keeps small QR detail) ---
                using Mat enhanced = new Mat();
                using (var clahe = Cv2.CreateCLAHE(clipLimit: 3.0, tileGridSize: new OpenCvSharp.Size(8, 8)))
                    clahe.Apply(tileRaw, enhanced);

                TryDecodeFast(enhanced, _hardOpts, 0, 1.0, tileRes, tileW, paddedContainer.Height);

                // --- PASS B: ADAPTIVE (Pierces shadows) ---
                if (tileRes.CharResults.Count == 0)
                {
                    using Mat bin = new Mat();
                    Cv2.AdaptiveThreshold(enhanced, bin, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary, 35, 10);
                    TryDecodeFast(bin, _hardOpts, 0, 1.0, tileRes, tileW, paddedContainer.Height);
                }

                // --- PASS C: DPM SLEDGEHAMMER (Only if enabled AND previous failed) ---
                // We use dilation ONLY here to join laser dots.
                if (tileRes.CharResults.Count == 0 && roi.UseBruteForceGridRecovery)
                {
                    using Mat dilated = new Mat();
                    using Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(3, 3));
                    Cv2.Dilate(tileRaw, dilated, kernel); // Makes laser dots touch
                    TryDecodeFast(dilated, _hardOpts, 0, 1.0, tileRes, tileW, paddedContainer.Height);
                }

                // 6. MAP COORDINATES BACK TO UI SPACE
                foreach (var found in tileRes.CharResults)
                {
                    found.Box = new Rect((found.Box.X + currentX) - globalPad, found.Box.Y - globalPad, found.Box.Width, found.Box.Height);
                    if (found.Polygon != null)
                    {
                        for (int p = 0; p < 4; p++)
                        {
                            found.Polygon[p].X = (found.Polygon[p].X + currentX) - globalPad;
                            found.Polygon[p].Y = found.Polygon[p].Y - globalPad;
                        }
                    }
                    concurrentResults.Add(found);
                }
            });

            // 7. DEDUPLICATE (If a barcode was found in multiple tiles)
            roi.CharResults.Clear();
            foreach (var found in concurrentResults)
            {
                if (!roi.CharResults.Any(r => r.Text == found.Text && Math.Abs(r.Box.X - found.Box.X) < 40))
                {
                    roi.CharResults.Add(found);
                }
            }


            // 8. FINAL ASSEMBLY
            if (roi.CharResults.Count > 0)
            {
                var sorted = roi.CharResults.OrderBy(r => r.Box.X).ToList();
                roi.CharResults.Clear();
                roi.CharResults.AddRange(sorted);
                roi.DecodedText = string.Join(" | ", roi.CharResults.Select(r => r.Text));
            }
            else
            {
                roi.DecodedText = "Failed";
            }
        }

        public static void BruteForceDecode_old(Mat crop, RoiObject roi)
        {
            if (crop == null || crop.Empty())
                return;

            Mat gray = new Mat();
            if (crop.Channels() == 3)
                Cv2.CvtColor(crop, gray, ColorConversionCodes.BGR2GRAY);
            else
                gray = crop.Clone();

            using (gray)
            {
                using Mat binOtsuGlobal = new Mat();
                Cv2.Threshold(gray, binOtsuGlobal, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

                using Mat blurred = new Mat();
                Cv2.GaussianBlur(gray, blurred, new OpenCvSharp.Size(3, 3), 0);

                using Mat localEnhanced = new Mat();
                using var clahe = Cv2.CreateCLAHE(clipLimit: 2.0, tileGridSize: new OpenCvSharp.Size(8, 8));
                clahe.Apply(blurred, localEnhanced);

                using Mat binAdaptiveGlobal = new Mat();
                Cv2.AdaptiveThreshold(localEnhanced, binAdaptiveGlobal, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary, 45, 10);

                int tileW = Math.Min(300, gray.Width);
                int step = Math.Max(1, tileW / 2);
                int pad = 20;

                int numTiles = gray.Width <= tileW ? 1 : ((gray.Width - tileW) / step) + 1;
                if (gray.Width > tileW && (gray.Width - tileW) % step != 0) numTiles++;

                ConcurrentBag<CharResult> concurrentResults = new ConcurrentBag<CharResult>();

                Parallel.For(0, numTiles, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, i =>
                {
                    int currentX = i * step;
                    if (currentX + tileW > gray.Width)
                        currentX = gray.Width - tileW;

                    using Mat tileRaw = new Mat(gray, new Rect(currentX, 0, tileW, gray.Height));
                    using Mat tileOtsu = new Mat(binOtsuGlobal, new Rect(currentX, 0, tileW, gray.Height));
                    using Mat tileAdaptive = new Mat(binAdaptiveGlobal, new Rect(currentX, 0, tileW, gray.Height));

                    RoiObject tileRes = new RoiObject { CharResults = new List<CharResult>() };

                    using Mat paddedRaw = new Mat();
                    Cv2.CopyMakeBorder(tileRaw, paddedRaw, pad, pad, pad, pad, BorderTypes.Constant, Scalar.White);
                    TryDecodeFast(paddedRaw, _hardOpts, pad, 1.0, tileRes, tileW, gray.Height);

                    using Mat paddedOtsu = new Mat();
                    Cv2.CopyMakeBorder(tileOtsu, paddedOtsu, pad, pad, pad, pad, BorderTypes.Constant, Scalar.White);
                    TryDecodeFast(paddedOtsu, _hardOpts, pad, 1.0, tileRes, tileW, gray.Height);

                    using Mat paddedAdaptive = new Mat();
                    Cv2.CopyMakeBorder(tileAdaptive, paddedAdaptive, pad, pad, pad, pad, BorderTypes.Constant, Scalar.White);
                    TryDecodeFast(paddedAdaptive, _hardOpts, pad, 1.0, tileRes, tileW, gray.Height);

                    foreach (var found in tileRes.CharResults)
                    {
                        found.Box = new Rect(found.Box.X + currentX, found.Box.Y, found.Box.Width, found.Box.Height);
                        if (found.Polygon != null)
                        {
                            for (int p = 0; p < 4; p++)
                                found.Polygon[p].X += currentX;
                        }
                        concurrentResults.Add(found);
                    }
                });

                roi.CharResults.Clear();
                foreach (var found in concurrentResults)
                {
                    if (!roi.CharResults.Any(r => r.Text == found.Text && Math.Abs(r.Box.X - found.Box.X) < 20))
                        roi.CharResults.Add(found);
                }

                if (roi.CharResults.Count > 0)
                {
                    var sortedResults = roi.CharResults.OrderBy(r => r.Box.X).ToList();
                    roi.CharResults.Clear();
                    roi.CharResults.AddRange(sortedResults);
                    roi.DecodedText = string.Join(" | ", roi.CharResults.Select(r => r.Text));
                }
            }
        }

        private static void TryDecodeCylindrical(Mat graySrc, RoiObject roi, int origW, int origH)
        {
            // Stretch the image horizontally by 30%, 60%, and 100% to undo the curve compression
            double[] stretchFactors = { 1.3, 1.6, 2.0 };

            foreach (double stretch in stretchFactors)
            {
                using Mat stretched = new Mat();
                Cv2.Resize(graySrc, stretched, new OpenCvSharp.Size(graySrc.Width * stretch, graySrc.Height), 0, 0, InterpolationFlags.Cubic);

                // Binarize it so ZXing can see it clearly
                using Mat bin = new Mat();
                Cv2.Threshold(stretched, bin, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

                try
                {
                    var iv = new ImageView(bin.Data, bin.Width, bin.Height, ImageFormat.Lum, (int)bin.Step());
                    var results = BarcodeReader.Read(iv, _hardOpts);

                    if (results.Length > 0)
                    {
                        foreach (var res in results)
                        {
                            var p = res.Position;

                            // =========================================================
                            // THE FIX: Un-stretch the X coordinates by dividing by the stretch factor.
                            // This ensures the green box draws perfectly on the original curved image!
                            // =========================================================
                            var poly = new OpenCvSharp.Point[4];
                            poly[0] = new OpenCvSharp.Point((int)(p.TopLeft.X / stretch), p.TopLeft.Y);
                            poly[1] = new OpenCvSharp.Point((int)(p.TopRight.X / stretch), p.TopRight.Y);
                            poly[2] = new OpenCvSharp.Point((int)(p.BottomRight.X / stretch), p.BottomRight.Y);
                            poly[3] = new OpenCvSharp.Point((int)(p.BottomLeft.X / stretch), p.BottomLeft.Y);

                            int rMinX = poly.Min(pt => pt.X);
                            int rMaxX = poly.Max(pt => pt.X);
                            int rMinY = poly.Min(pt => pt.Y);
                            int rMaxY = poly.Max(pt => pt.Y);

                            roi.CharResults.Add(new CharResult
                            {
                                Box = new Rect(rMinX, rMinY, rMaxX - rMinX, rMaxY - rMinY),
                                Polygon = poly,
                                Text = res.Text,
                                Score = 1.0,
                                IsGood = true
                            });
                        }

                        roi.DecodedText = string.Join(" | ", roi.CharResults.Select(r => r.Text).Distinct());
                        return; // We found it! Exit the sweep.
                    }
                }
                catch { }
            }
        }

        private static void TryDecodeExtremeDPM(Mat grayCrop, ReaderOptions options, RoiObject roi)
        {
            // 1. CREATE QUIET ZONE
            // ZXing will instantly fail if the barcode touches the edge of the image.
            int pad = 20;
            using Mat padded = new Mat();
            Cv2.CopyMakeBorder(grayCrop, padded, pad, pad, pad, pad, BorderTypes.Constant, Scalar.White);

            // 2. KILL THE GLARE (CLAHE)
            // Contrast Limited Adaptive Histogram Equalization flattens the lighting so the 
            // bright middle and dark edges become uniform.
            using Mat enhanced = new Mat();
            using var clahe = Cv2.CreateCLAHE(clipLimit: 4.0, tileGridSize: new OpenCvSharp.Size(8, 8));
            clahe.Apply(padded, enhanced);

            // 3. MERGE THE DOTS (Gaussian Blur)
            // DPM laser dots are disconnected. Blurring them slightly forces them to bleed 
            // together into solid shapes that ZXing can actually read.
            using Mat blurred = new Mat();
            Cv2.GaussianBlur(enhanced, blurred, new OpenCvSharp.Size(3, 3), 0);

            // 4. FIX THE CURVE (Cylindrical Un-wrapping)
            // Because the metal is curved, the barcode looks squeezed horizontally.
            // We will stretch the image horizontally (1.3x and 1.6x) to "flatten" the cylinder!
            double[] stretches = { 1.0, 1.3, 1.6 };

            foreach (double stretch in stretches)
            {
                using Mat stretched = new Mat();
                if (stretch == 1.0) blurred.CopyTo(stretched);
                else Cv2.Resize(blurred, stretched, new OpenCvSharp.Size(blurred.Width * stretch, blurred.Height));

                // 5. SWEEP ADAPTIVE THRESHOLDS
                // We don't know the exact size of the dots, so we sweep through 3 block sizes
                int[] blockSizes = { 15, 35, 55 };

                foreach (int blockSize in blockSizes)
                {
                    using Mat bin = new Mat();
                    Cv2.AdaptiveThreshold(stretched, bin, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary, blockSize, 10);

                    // First Attempt: Normal Dark-on-Light
                    TryDecodeFast(bin, options, pad, stretch, roi, grayCrop.Width, grayCrop.Height);
                    if (!string.IsNullOrEmpty(roi.DecodedText) && roi.DecodedText != "Failed") return;

                    // Second Attempt: Inverted Light-on-Dark (Very common in laser etching)
                    using Mat inv = new Mat();
                    Cv2.BitwiseNot(bin, inv);
                    TryDecodeFast(inv, options, pad, stretch, roi, grayCrop.Width, grayCrop.Height);
                    if (!string.IsNullOrEmpty(roi.DecodedText) && roi.DecodedText != "Failed") return;
                }
            }
        }


        //============================================================================
        //added on 22-04-2026
        //==========================================================================
        /// <summary>
        /// function used to read barcode only.implemented multiple fall back mechanism to read barcodes in even harder
        /// </summary>
        /// <param name="img">image for processing</param>
        /// <param name="options">reader options for processing</param>
        /// <param name="pad">padding area from edges of barcode</param>
        /// <param name="scale">scaling for processing for better results</param>
        /// <param name="roii">meta data of image</param>
        /// <param name="origCropW">width of image</param>
        /// <param name="origCropH">height of image</param>
        private static void TryDecodeFast(Mat img, ReaderOptions options, int pad, double scale, RoiObject roii, int origCropW, int origCropH)
        {
            try
            {
                var iv = new ImageView(img.Data, img.Width, img.Height, ImageFormat.Lum, (int)img.Step());

                // IMPORTANT: Make sure options.MaxNumberOfSymbols is set > 1 before calling this!
                var results = BarcodeReader.Read(iv, options);

                if (results.Length > 0)
                {
                    List<string> allDecodedTexts = new List<string>();

                    // Combine distinct formats (e.g. "DataMatrix, QRCode" if both exist in the ROI)
                    roii.BarCodeFormat = string.Join(", ", results.Select(r => r.Format.ToString()).Distinct());

                    // =================================================================
                    // THE FIX: Loop through EVERY barcode found in the image
                    // =================================================================
                    foreach (var res in results)
                    {
                        string currentText = res.Text;

                        // Strip Codabar start/stop characters per individual barcode
                        if (res.Format.ToString().Equals("Codabar", StringComparison.OrdinalIgnoreCase) && currentText.Length > 2)
                        {
                            currentText = currentText.Substring(1, currentText.Length - 2);
                        }

                        allDecodedTexts.Add(currentText);

                        var p = res.Position;

                        // Capture the 4 exact tilted corners from ZXing
                        var poly = new OpenCvSharp.Point[4];
                        poly[0] = new OpenCvSharp.Point(Math.Clamp((int)(p.TopLeft.X / scale) - pad, 0, origCropW), Math.Clamp((int)(p.TopLeft.Y / scale) - pad, 0, origCropH));
                        poly[1] = new OpenCvSharp.Point(Math.Clamp((int)(p.TopRight.X / scale) - pad, 0, origCropW), Math.Clamp((int)(p.TopRight.Y / scale) - pad, 0, origCropH));
                        poly[2] = new OpenCvSharp.Point(Math.Clamp((int)(p.BottomRight.X / scale) - pad, 0, origCropW), Math.Clamp((int)(p.BottomRight.Y / scale) - pad, 0, origCropH));
                        poly[3] = new OpenCvSharp.Point(Math.Clamp((int)(p.BottomLeft.X / scale) - pad, 0, origCropW), Math.Clamp((int)(p.BottomLeft.Y / scale) - pad, 0, origCropH));

                        var pts = new[] { p.TopLeft, p.TopRight, p.BottomLeft, p.BottomRight }.Where(pt => pt.X != 0 || pt.Y != 0).ToList();
                        if (pts.Count == 0) continue;

                        int rMinX = pts.Min(pt => pt.X), rMaxX = pts.Max(pt => pt.X);
                        int rMinY = pts.Min(pt => pt.Y), rMaxY = pts.Max(pt => pt.Y);

                        if (rMaxY - rMinY < 20)
                        {
                            int visualHeight = Math.Max(40, (rMaxX - rMinX) / 2);
                            int centerY = rMinY + (rMaxY - rMinY) / 2;
                            rMinY = centerY - visualHeight / 2;
                            rMaxY = centerY + visualHeight / 2;
                        }

                        int finalMinX = Math.Clamp((int)(rMinX / scale) - pad, 0, origCropW - 1);
                        int finalMaxX = Math.Clamp((int)(rMaxX / scale) - pad, 0, origCropW);
                        int finalMinY = Math.Clamp((int)(rMinY / scale) - pad, 0, origCropH - 1);
                        int finalMaxY = Math.Clamp((int)(rMaxY / scale) - pad, 0, origCropH);

                        roii.CharResults.Add(new CharResult
                        {
                            Box = new Rect(finalMinX - 2, finalMinY - 2, (finalMaxX - finalMinX) + 4, (finalMaxY - finalMinY) + 4),
                            Text = currentText,
                            Score = 1.0,
                            IsGood = true,

                            // Exact points of barcode corners for better visualization in UI
                            ExactCorners = new Point2f[] {
                        new Point2f(p.TopLeft.X, p.TopLeft.Y),
                        new Point2f(p.TopRight.X, p.TopRight.Y),
                        new Point2f(p.BottomRight.X, p.BottomRight.Y),
                        new Point2f(p.BottomLeft.X, p.BottomLeft.Y)
                    },

                            Polygon = poly // Add the tilted polygon here
                        });
                    }

                    // Finally, combine all decoded texts into the main ROI property
                    roii.DecodedText = string.Join(" | ", allDecodedTexts);
                }
            }
            catch { }
        }

        //============================================================================
        //added on 22-04-2026
        //==========================================================================
        /// <summary>
        /// /// function used to read pharma barcode only.implemented multiple fall back mechanism to read barcodes in even harder
        /// </summary>
        /// <param name="crop">imaeg for processing</param>
        /// <param name="roi">meta data of image</param>
        private static void TryDecodePharmacode(Mat crop, RoiObject roi)
        {
            try
            {
                using Mat bin = new Mat();
                using Mat minTemp = new Mat();

                if (crop.Channels() == 3)
                {
                    Mat[] channels = Cv2.Split(crop);
                    Cv2.Min(channels[0], channels[1], minTemp);
                    Cv2.Min(minTemp, channels[2], minTemp);
                    foreach (var c in channels) c.Dispose();
                }
                else crop.CopyTo(minTemp);

                Cv2.Normalize(minTemp, minTemp, 0, 255, NormTypes.MinMax);
                Cv2.Threshold(minTemp, bin, 0, 255, ThresholdTypes.BinaryInv | ThresholdTypes.Otsu);

                using Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(1, 7));
                Cv2.MorphologyEx(bin, bin, MorphTypes.Close, kernel);

                Cv2.FindContours(bin, out var contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

                var verticalRects = contours.Select(c => Cv2.BoundingRect(c))
                                            .Where(r => r.Height >= 10 && r.Height >= r.Width * 1.2)
                                            .OrderBy(r => r.X)
                                            .ToList();
                if (verticalRects.Count < 2) return;

                List<List<Rect>> clusters = new List<List<Rect>>();
                List<Rect> currentCluster = new List<Rect> { verticalRects[0] };

                for (int i = 1; i < verticalRects.Count; i++)
                {
                    var prev = verticalRects[i - 1];
                    var curr = verticalRects[i];
                    double maxGap = Math.Max(prev.Width, curr.Width) * 5.0;

                    if ((curr.X - prev.Right) > maxGap)
                    {
                        clusters.Add(currentCluster);
                        currentCluster = new List<Rect>();
                    }
                    currentCluster.Add(curr);
                }
                clusters.Add(currentCluster);

                List<string> allDecodedPharmacodes = new List<string>();

                // =================================================================
                // THE FIX: Loop through ALL clusters instead of just taking the "best" one
                // =================================================================
                foreach (var cluster in clusters)
                {
                    if (cluster.Count < 2 || cluster.Count > 32) continue;
                    double clusterMaxHeight = cluster.Max(r => r.Height);
                    var validBars = cluster.Where(r => r.Height >= clusterMaxHeight * 0.35).ToList();
                    if (validBars.Count < 2) continue;

                    var tallBars = validBars.Where(r => r.Height >= clusterMaxHeight * 0.7).ToList();
                    if (tallBars.Count == 0) tallBars = validBars;

                    var sortedByY = tallBars.OrderBy(r => r.Y + r.Height / 2.0).ToList();
                    double laserLineY = sortedByY[sortedByY.Count / 2].Y + sortedByY[sortedByY.Count / 2].Height / 2.0;
                    double tolerance = clusterMaxHeight * 0.2;

                    validBars = validBars.Where(r => r.Y <= laserLineY + tolerance && r.Bottom >= laserLineY - tolerance)
                                         .OrderByDescending(r => r.X).ToList();

                    if (validBars.Count < 2) continue;

                    double maxHeight = validBars.Max(b => b.Height);
                    bool isTwoTrack = validBars.Any(b => b.Height < maxHeight * 0.7);
                    long pharmacodeValue = 0;

                    if (!isTwoTrack)
                    {
                        double minW = validBars.Min(b => b.Width);
                        double maxW = validBars.Max(b => b.Width);
                        double widthThresh = minW + ((maxW - minW) / 2.0);

                        for (int i = 0; i < validBars.Count; i++)
                        {
                            int weight = (validBars[i].Width > widthThresh) ? 2 : 1;
                            pharmacodeValue += weight * (long)Math.Pow(2, i);
                        }
                    }
                    else
                    {
                        var fullBarsForMath = validBars.Where(b => b.Height >= maxHeight * 0.7).ToList();
                        double trueMidline = fullBarsForMath.Count > 0
                            ? fullBarsForMath.Average(b => b.Y + b.Height / 2.0)
                            : validBars.Average(b => b.Y + b.Height / 2.0);

                        for (int i = 0; i < validBars.Count; i++)
                        {
                            int weight;
                            if (validBars[i].Height >= maxHeight * 0.7) weight = 3;
                            else
                            {
                                double barCenterY = validBars[i].Y + validBars[i].Height / 2.0;
                                weight = (barCenterY < trueMidline) ? 1 : 2;
                            }
                            pharmacodeValue += weight * (long)Math.Pow(3, i);
                        }
                    }

                    if (pharmacodeValue >= 3 && pharmacodeValue <= 64570080)
                    {
                        int minX = validBars.Min(b => b.X);
                        int minY = validBars.Min(b => b.Y);
                        int maxX = validBars.Max(b => b.Right);
                        int maxY = validBars.Max(b => b.Bottom);

                        string decodedPharmaText = pharmacodeValue.ToString();
                        allDecodedPharmacodes.Add(decodedPharmaText);

                        var poly = new OpenCvSharp.Point[4] {
                    new OpenCvSharp.Point(minX - 2, minY - 2),
                    new OpenCvSharp.Point(maxX + 2, minY - 2),
                    new OpenCvSharp.Point(maxX + 2, maxY + 2),
                    new OpenCvSharp.Point(minX - 2, maxY + 2)
                };

                        roi.CharResults.Add(new CharResult
                        {
                            Box = new Rect(minX - 2, minY - 2, (maxX - minX) + 4, (maxY - minY) + 4),
                            Text = decodedPharmaText,
                            Score = 1.0,
                            IsGood = true,
                            Polygon = poly,
                        });
                    }
                }

                // Apply results only if we found at least one valid pharmacode
                if (allDecodedPharmacodes.Count > 0)
                {
                    roi.DecodedText = string.Join(" | ", allDecodedPharmacodes);
                    roi.BarCodeFormat = "Pharma";
                }
            }
            catch { }
        }

        /// <summary>
        /// entry point of all ocr operations including text and barcodes 
        /// </summary>
        /// <param name="currentImage">image for processing</param>
        /// <param name="roi">meta data of images</param>
        public static void DecodeRoi(Mat currentImage, RoiObject roi)
        {
            roi.DecodedText = "";
            roi.CharResults.Clear();

            CvRect safeBox = roi.Box.Intersect(new CvRect(0, 0, currentImage.Width, currentImage.Height));
            if (safeBox.Width <= 0 || safeBox.Height <= 0) return;

            var localSw = Stopwatch.StartNew();

            using Mat crop = currentImage[safeBox];
            int origW = crop.Width, origH = crop.Height;
            Mat rotatedCrop = null, deskewedCrop = null, tempMorphedImage = null;

            try
            {
                RotateImage(crop, out rotatedCrop, roi.RotationAngle);

                switch (roi.Type)
                {
                    case RoiType.Text:
                        deskewedCrop = DeskewImage(rotatedCrop, out double skewAngle);

                        // Store the skew angle in the ROI object for potential use in visualization or debugging
                        roi.SkewAngleOfRoi = skewAngle;

                        var boxes = GetCharacterBoxes(deskewedCrop, roi);

                        // =========================================================
                        // CRITICAL FIX: PREVENT CRASH IF ROI IS COMPLETELY EMPTY
                        // ADDED ON :- 08-04-2026
                        // =========================================================
                        if (boxes.Count == 0)
                        {
                            roi.DecodedText = string.Empty;
                            roi.RoiScore = 0;
                            roi.OverAllResult = !string.IsNullOrWhiteSpace(roi.ExpectedText) ? "Fail" : string.Empty;
                            break; // Skip the parallel loop entirely
                        }
                        // =========================================================


                        Mat ocrSourceImage = deskewedCrop;

                        if (roi.MorphOp != MorphOperation.None && roi.MorphKernelWidth > 0 && roi.MorphKernelHeight > 0 && roi.MorphIterations > 0)
                        {
                            tempMorphedImage = new Mat();
                            using Mat tempGray = new Mat();
                            if (deskewedCrop.Channels() == 3) Cv2.CvtColor(deskewedCrop, tempGray, ColorConversionCodes.BGR2GRAY);
                            else deskewedCrop.CopyTo(tempGray);

                            using Mat tempTh = new Mat();
                            ProcessImageForMode(tempGray, tempTh, roi.SegmentationMode);
                            MorphologyProcessor.Apply(tempTh, roi.MorphOp, roi.MorphKernelWidth, roi.MorphKernelHeight, roi.MorphIterations);
                            Cv2.BitwiseNot(tempTh, tempMorphedImage);
                            ocrSourceImage = tempMorphedImage;
                        }

                        string cleanExpected = roi.ExpectedText?.Replace(" ", "") ?? "";
                        int cleanLen = cleanExpected.Length;
                        float centerX = rotatedCrop.Width / 2f, centerY = rotatedCrop.Height / 2f;

                        var fastResults = new CharResult[boxes.Count];


                        // Calculate median height of the whole ROI to use for Context Checking
                        var allHeights = boxes.Select(b => b.Height).OrderBy(h => h).ToList();
                        int globalMedianH = allHeights.Count > 0 ? allHeights[allHeights.Count / 2] : 20;

                        Parallel.ForEach(Partitioner.Create(0, boxes.Count),
                            new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                            () => GetOcrState(),
                            (range, loopState, threadState) =>
                            {
                                for (int i = range.Item1; i < range.Item2; i++)
                                {
                                    Rect b = boxes[i];
                                    using Mat charImg = new Mat(ocrSourceImage, b);

                                    string targetForThisChar = (i < cleanLen) ? cleanExpected[i].ToString() : null;
                                    
                                    var (label, score) = RecognizeImpl(charImg, roi.Threshold, targetForThisChar, threadState);

                                    Rect visualBox = Math.Abs(skewAngle) > 0.001 ? InverseDeskewRect(b, skewAngle, centerX, centerY) : b;

                                    fastResults[i] = new CharResult
                                    {
                                        Box = UnrotateBox(visualBox, origW, origH, roi.RotationAngle),
                                        Text = label,
                                        Score = score,
                                        IsGood = score >= roi.Threshold,
                                        IsExpectedMatch = targetForThisChar != null && string.Equals(label, targetForThisChar, StringComparison.Ordinal)
                                    };
                                }
                                return threadState;
                            },
                            (threadState) => ReturnOcrState(threadState)
                        );

                        var sb = new StringBuilder(boxes.Count);
                        double minScore = double.MaxValue;

                        // 1. Calculate Median Height ONCE before the loop
                        double medianHeight = 20;

                        if (fastResults.Length > 0)
                        {
                            //height
                            var heights = fastResults.Where(r => r != null).Select(r => r.Box.Height).OrderBy(h => h).ToList();
                            medianHeight = heights.Count > 0 ? heights[heights.Count / 2] : 20;
                        }

                        // 2. Process everything in ONE single loop
                        for (int i = 0; i < fastResults.Length; i++)
                        {
                            var res = fastResults[i];
                            if (res == null) continue;

                            //// --- DYNAMIC HEIGHT CORRECTOR (o vs O) ---
                            //if (res.Text == "O" || res.Text == "o" || res.Text == "0")
                            //{
                            //    // Lowercase 'o' is usually 60-80% the height of a normal letter.
                            //    if (res.Box.Height <= medianHeight * 0.82)
                            //    {
                            //        res.Text = "o";
                            //    }
                            //    //// If the engine guessed lowercase 'o', but it's actually tall, fix it to Uppercase 'O'
                            //    //else if (res.Text == "o")
                            //    //{


                            //    //    res.Text = "O";
                            //    //}
                            //}

                            //get the actual char after post processing
                            res.Text = CharPostProcessing(res.Text, res.Box.Height, medianHeight);

                            // --- ORIGINAL APPEND & SCORE LOGIC ---
                            sb.Append(res.Text);
                            roi.CharResults.Add(res);
                            if (res.Score < minScore) minScore = res.Score;
                        }

                        //========================================================================

                        roi.DecodedText = sb.ToString();
                        roi.RoiScore = fastResults.Length > 0 ? minScore : 0;

                        //updated at 14-04-2026 to handle empty expected text scenario correctly
                        //roi.OverAllResult = !string.IsNullOrWhiteSpace(roi.ExpectedText) ?
                        //                    (string.Equals(roi.ExpectedText, roi.DecodedText, StringComparison.OrdinalIgnoreCase) ? "Pass" : "Fail") : "";

                        roi.OverAllResult = !string.IsNullOrWhiteSpace(roi.ExpectedText) ?
                                            IsResultPass(roi.ExpectedText, roi.DecodedText) : string.Empty;
                        break;

                    case RoiType.Barcode:
                        ReadBarcode(rotatedCrop, roi);
                        bool isDecodedSuccessfully = roi.CharResults.Count > 0 && roi.DecodedText != "Failed";

                        if (roi.IsRunGS1QcCheck && roi.CharResults.Count == 1) //&& roi.CharResults.Count > 0
                        {
                            using (Mat qcGray = new Mat())
                            {
                                if (rotatedCrop.Channels() >= 3) Cv2.CvtColor(rotatedCrop, qcGray, ColorConversionCodes.BGR2GRAY);
                                else rotatedCrop.CopyTo(qcGray);

                                Rect barcodeBox = isDecodedSuccessfully ? roi.CharResults[0].Box : new Rect(0, 0, qcGray.Width, qcGray.Height);
                                if (roi.BarCodeFormat.Equals("datamatrix", StringComparison.OrdinalIgnoreCase) || roi.BarCodeFormat.Equals("QRCode", StringComparison.OrdinalIgnoreCase) || roi.BarCodeFormat.Equals("Aztec", StringComparison.OrdinalIgnoreCase) || roi.BarCodeFormat.Equals("PDF", StringComparison.OrdinalIgnoreCase))
                                {
                                    //roi.Gs1QcResult = new GS1_QC_Check.GS1_QC_Check().EvaluateISO15415Quality(qcGray, barcodeBox, roi.CharResults[0].ExactCorners, isDecodedSuccessfully);

                                    roi.Gs1QcResult = new GS1_QC_Check.GS1_QC_Check().EvaluateISO15415Quality(qcGray, roi, barcodeBox, roi.CharResults[0].ExactCorners, isDecodedSuccessfully);
                                }
                                //roi.Gs1QcResult = new GS1_QC_Check().EvaluateISO15415Quality(qcGray, barcodeBox, isDecodedSuccessfully);
                                else
                                    roi.Gs1QcResult = new Verifier1D_Linear().EvaluateQuality(qcGray, barcodeBox, isDecodedSuccessfully, roi.BarCodeFormat);
                            }
                        }
                        else
                        {
                            roi.Gs1QcResult = new CsplCam.Library.Models.GS1_QC.GS1_QC_CheckResult();
                        }

                        for (int i = 0; i < roi.CharResults.Count; i++)
                        {
                            var cr = roi.CharResults[i];
                            cr.Box = UnrotateBox(cr.Box, origW, origH, roi.RotationAngle);


                            // =================================================================
                            // THE FIX: Unrotate the 4 corner points!
                            // =================================================================
                            if (cr.Polygon != null)
                            {
                                for (int p = 0; p < 4; p++)
                                {
                                    cr.Polygon[p] = UnrotatePoint(cr.Polygon[p], origW, origH, roi.RotationAngle);
                                }
                            }


                            roi.CharResults[i] = cr;
                        }

                        roi.isBarCodeFormatAuto = false;
                        break;

                    case RoiType.TemplateMatch:
                        PerformFeatureMatching(roi, crop);
                        break;
                }

                localSw.Stop();
                roi.TimeTakenForDecoding = localSw.Elapsed;

                // ====================================================================
                // 2. COORDINATE MAPPING & FINAL IMAGE CLAMPING
                // This shifts coordinates from the 'Crop' back to the 'User UI' and 
                // ensures NO POINTS exist outside the physical image pixels.
                // ====================================================================
                int offsetX = safeBox.X - roi.Box.X;
                int offsetY = safeBox.Y - roi.Box.Y;
                int imgW = currentImage.Width;
                int imgH = currentImage.Height;

                for (int i = 0; i < roi.CharResults.Count; i++)
                {
                    var cr = roi.CharResults[i];

                    // --- A. Remap & Clamp Polygon (The Green Box) ---
                    if (cr.Polygon != null)
                    {
                        for (int p = 0; p < 4; p++)
                        {
                            // Calculate Absolute pixel on full image, snap to [0, ImageEdge]
                            int absX = Math.Clamp(roi.Box.X + cr.Polygon[p].X + offsetX, 0, imgW - 1);
                            int absY = Math.Clamp(roi.Box.Y + cr.Polygon[p].Y + offsetY, 0, imgH - 1);

                            // Remap back to ROI-relative so your UI Paint function works
                            cr.Polygon[p].X = absX - roi.Box.X;
                            cr.Polygon[p].Y = absY - roi.Box.Y;
                        }
                    }

                    // --- B. Remap & Clamp ExactCorners (For GS1 QC) ---
                    if (cr.ExactCorners != null)
                    {
                        for (int p = 0; p < cr.ExactCorners.Length; p++)
                        {
                            float absX = Math.Clamp(roi.Box.X + cr.ExactCorners[p].X + offsetX, 0, imgW - 1);
                            float absY = Math.Clamp(roi.Box.Y + cr.ExactCorners[p].Y + offsetY, 0, imgH - 1);
                            cr.ExactCorners[p].X = absX - roi.Box.X;
                            cr.ExactCorners[p].Y = absY - roi.Box.Y;
                        }
                    }

                    // --- C. Remap & Clamp Bounding Box ---
                    int finalAbsX = Math.Clamp(roi.Box.X + cr.Box.X + offsetX, 0, imgW - 1);
                    int finalAbsY = Math.Clamp(roi.Box.Y + cr.Box.Y + offsetY, 0, imgH - 1);
                    int finalW = Math.Min(cr.Box.Width, imgW - finalAbsX);
                    int finalH = Math.Min(cr.Box.Height, imgH - finalAbsY);

                    cr.Box = new Rect(finalAbsX - roi.Box.X, finalAbsY - roi.Box.Y, finalW, finalH);

                    roi.CharResults[i] = cr;
                }

            }
            finally
            {
                if (tempMorphedImage != null && !tempMorphedImage.IsDisposed) tempMorphedImage.Dispose();
                if (deskewedCrop != null && !deskewedCrop.IsDisposed) deskewedCrop.Dispose();
                if (rotatedCrop != null && rotatedCrop != crop && !rotatedCrop.IsDisposed) rotatedCrop.Dispose();
            }
        }

        /// <summary>
        /// post processing
        /// </summary>
        /// <param name="value"></param>
        /// <param name="actualHeight"></param>
        /// <param name="minHeight"></param>
        /// <returns></returns>
        private static string CharPostProcessing(string? value,double actualHeight,double minHeight) 
        {
            // --- DYNAMIC HEIGHT CORRECTOR (o vs O) ---
            if (value == "O" || value == "o" || value == "0")
            {
                // Lowercase 'o' is usually 60-80% the height of a normal letter.
                if (actualHeight <= minHeight * 0.82)
                {
                    return "o";
                }
            }

            return value!;
        }


        //public static (char missingChar,int index) FinMissingChar(ReadOnlySpan<char> originalValue,ReadOnlySpan<char> observed)
        //{
        //    //1.store the length of original value
        //    int len = originalValue.Length;

        //    //2.return if original have novalue
        //    if (len == 0) return (default, -1);

        //    //3.start scanning
        //    int i = 0;
        //    while(i < len)
        //    {

        //    }
        //}

        //private static string IsResultPass(string expText, string decText)
        //{
        //    // 1. Quick exit if lengths don't match
        //    if (expText.Length != decText.Length) return "Fail";

        //    for (int i = 0; i < expText.Length; i++)
        //    {
        //        // ==========================================================
        //        // THE FIX: Use single quotes '*' to compare the raw char.
        //        // This allocates ZERO memory and is instantly processed by the CPU!
        //        // ==========================================================
        //        if (expText[i] == '*') continue;

        //        // If it's not a wildcard, and the characters don't match, fail instantly
        //        if (expText[i] != decText[i])
        //        {
        //            return "Fail";
        //        }
        //    }

        //    return "Pass";
        //}


        /// <summary>
        /// function for check expected result based on wildcards given
        /// </summary>
        /// <param name="expText">expected value</param>
        /// <param name="decText">detected value</param>
        /// <returns></returns>
        private static string IsResultPass(string expText, string decText)
        {
            // 1. Quick exit if lengths don't match
            if (expText.Length != decText.Length) return "Fail";

            for (int i = 0; i < expText.Length; i++)
            {
                char exp = expText[i];
                char dec = decText[i];

                // 2. Process Wildcards
                if (exp == '*') continue; // '*' means ANY character is a Pass

                if (exp == '#') // '#' means MUST BE A NUMBER
                {
                    if (!char.IsDigit(dec)) return "Fail";
                    continue;
                }

                if (exp == '@') // '@' means MUST BE A LETTER
                {
                    if (!char.IsLetter(dec)) return "Fail";
                    continue;
                }

                // 3. Exact match for normal characters
                if (exp != dec) return "Fail";
            }

            return "Pass";
        }

        private static void PerformFeatureMatching(RoiObject roi, Mat sceneImage)
        {
            roi.CharResults.Clear();

            if (TemplateManager.GlobalTemplates.Count == 0)
            {
                roi.DecodedText = "No Templates";
                roi.TmScore = 0;
                roi.TmPass = false;
                return;
            }

            using Mat sceneGray = new();
            if (sceneImage.Channels() == 3) Cv2.CvtColor(sceneImage, sceneGray, ColorConversionCodes.BGR2GRAY);
            else sceneImage.CopyTo(sceneGray);

            double bestScore = -1.0;
            Mat bestTemplateImg = null;
            OpenCvSharp.Point bestLoc = new OpenCvSharp.Point();

            foreach (var template in TemplateManager.GlobalTemplates)
            {
                if (template.Image == null || template.Image.Empty()) continue;

                using Mat tplGray = new();
                if (template.Image.Channels() == 3) Cv2.CvtColor(template.Image, tplGray, ColorConversionCodes.BGR2GRAY);
                else template.Image.CopyTo(tplGray);

                if (sceneGray.Width < tplGray.Width || sceneGray.Height < tplGray.Height) continue;

                using Mat res = new Mat();
                Cv2.MatchTemplate(sceneGray, tplGray, res, TemplateMatchModes.CCoeffNormed);
                Cv2.MinMaxLoc(res, out _, out double maxVal, out _, out OpenCvSharp.Point maxLoc);

                if (maxVal > bestScore)
                {
                    bestScore = maxVal;
                    bestLoc = maxLoc;

                    if (bestTemplateImg != null) bestTemplateImg.Dispose();
                    bestTemplateImg = tplGray.Clone();
                }
            }

            if (bestScore < 0) bestScore = 0;
            roi.TmScore = bestScore;

            if (bestScore >= roi.TmThreshold)
            {
                roi.TmPass = true;
                roi.DecodedText = "PASS";
                if (bestTemplateImg != null) bestTemplateImg.Dispose();
            }
            else
            {
                roi.TmPass = false;
                roi.DecodedText = "FAIL";

                if (bestTemplateImg != null)
                {
                    try
                    {
                        Rect matchRect = new Rect(bestLoc.X, bestLoc.Y, bestTemplateImg.Width, bestTemplateImg.Height);
                        using Mat matchedCrop = new Mat(sceneGray, matchRect);

                        using Mat binTpl = new Mat();
                        using Mat binScene = new Mat();
                        Cv2.Threshold(bestTemplateImg, binTpl, 0, 255, ThresholdTypes.BinaryInv | ThresholdTypes.Otsu);
                        Cv2.Threshold(matchedCrop, binScene, 0, 255, ThresholdTypes.BinaryInv | ThresholdTypes.Otsu);

                        using Mat tplForContours = new Mat();
                        Cv2.MorphologyEx(binTpl, tplForContours, MorphTypes.Close, Kernel5x5);

                        Cv2.FindContours(tplForContours, out var tplContours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
                        List<Rect> charBoxes = new List<Rect>();
                        foreach (var cnt in tplContours)
                        {
                            var rect = Cv2.BoundingRect(cnt);
                            if (rect.Width >= 4 && rect.Height >= 10) charBoxes.Add(rect);
                        }

                        using Mat fatTpl = new Mat();
                        using Mat fatScene = new Mat();
                        Cv2.MorphologyEx(binTpl, fatTpl, MorphTypes.Dilate, Kernel3x3);
                        Cv2.MorphologyEx(binScene, fatScene, MorphTypes.Dilate, Kernel3x3);

                        using Mat missing = new Mat();
                        using Mat extra = new Mat();
                        Cv2.Subtract(binTpl, fatScene, missing);
                        Cv2.Subtract(binScene, fatTpl, extra);

                        using Mat diff = new Mat();
                        Cv2.BitwiseOr(missing, extra, diff);
                        Cv2.MorphologyEx(diff, diff, MorphTypes.Open, Kernel2x2);

                        using Mat smudgeDiff = diff.Clone();

                        foreach (var box in charBoxes)
                        {
                            using Mat charTplMat = new Mat(binTpl, box);
                            int tplInkArea = Cv2.CountNonZero(charTplMat);

                            int checkPad = 3;
                            int x1 = Math.Max(0, box.X - checkPad);
                            int y1 = Math.Max(0, box.Y - checkPad);
                            int x2 = Math.Min(diff.Width, box.X + box.Width + checkPad);
                            int y2 = Math.Min(diff.Height, box.Y + box.Height + checkPad);
                            Rect checkRect = new Rect(x1, y1, x2 - x1, y2 - y1);

                            using Mat charDiff = new Mat(diff, checkRect);
                            int defectPixels = Cv2.CountNonZero(charDiff);

                            double maxAllowedDefects = Math.Max(15, tplInkArea * 0.25);

                            if (defectPixels > maxAllowedDefects)
                            {
                                int visPad = 2;
                                Rect uiBox = new Rect(box.X + bestLoc.X - visPad, box.Y + bestLoc.Y - visPad, box.Width + (visPad * 2), box.Height + (visPad * 2));
                                roi.CharResults.Add(new CharResult { Box = uiBox, Text = "?", Score = 0, IsGood = false, IsExpectedMatch = false });
                            }

                            Cv2.Rectangle(smudgeDiff, checkRect, Scalar.Black, -1);
                        }

                        Cv2.FindContours(smudgeDiff, out var strayContours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
                        foreach (var cnt in strayContours)
                        {
                            var rect = Cv2.BoundingRect(cnt);
                            if (rect.Width >= 8 && rect.Height >= 8)
                            {
                                Rect uiBox = new Rect(rect.X + bestLoc.X, rect.Y + bestLoc.Y, rect.Width, rect.Height);
                                roi.CharResults.Add(new CharResult { Box = uiBox, Text = "?", Score = 0, IsGood = false, IsExpectedMatch = false });
                            }
                        }
                    }
                    finally { bestTemplateImg.Dispose(); }
                }
            }
        }

        private struct PointXComparer : IComparer<Point2f> { public int Compare(Point2f a, Point2f b) => a.X.CompareTo(b.X); }


        public static Mat DeskewImage(Mat src, out double skewAngle)
        {
            skewAngle = 0;
            try
            {
                using Mat gray = src.Channels() == 3 ? src.CvtColor(ColorConversionCodes.BGR2GRAY) : src.Clone();
                using Mat bin = new Mat();
                Cv2.Threshold(gray, bin, 0, 255, ThresholdTypes.BinaryInv | ThresholdTypes.Otsu);

                Cv2.FindContours(bin, out var contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

                List<Rect> boxes = new List<Rect>();
                foreach (var cnt in contours)
                {
                    var r = Cv2.BoundingRect(cnt);
                    // Only use substantial characters to calculate angle, ignore tiny noise specks
                    if (r.Height >= 10 && r.Width >= 4) boxes.Add(r);
                }

                // STABILITY FIX 1: If we don't have at least 4 solid characters, 
                // it's impossible to calculate a reliable angle. Don't rotate.
                if (boxes.Count < 4) return src.Clone();

                boxes.Sort((a, b) => a.X.CompareTo(b.X));
                List<double> angles = new List<double>();

                for (int i = 0; i < boxes.Count - 1; i++)
                {
                    Rect b1 = boxes[i];
                    for (int j = i + 1; j < Math.Min(i + 4, boxes.Count); j++)
                    {
                        Rect b2 = boxes[j];
                        // Only compare boxes on the same horizontal line
                        if (Math.Abs(b1.Y - b2.Y) < b1.Height * 0.5)
                        {
                            double dx = b2.X - b1.X;
                            double dy = b2.Bottom - b1.Bottom;
                            if (dx > 20) // Only trust boxes that are far enough apart
                            {
                                angles.Add(Math.Atan2(dy, dx) * (180.0 / Math.PI));
                            }
                        }
                    }
                }

                if (angles.Count < 3) return src.Clone();

                angles.Sort();
                double medianAngle = angles[angles.Count / 2];

                // STABILITY FIX 2: "The Dead Zone"
                // If the angle is less than 1.5 degrees, it's basically straight. 
                // Do not rotate, as rotation makes the pixels blurry.
                if (Math.Abs(medianAngle) < 1.5) return src.Clone();

                // STABILITY FIX 3: "The Sanity Limit"
                // In an industrial line, text is never tilted 12 degrees. 
                // If the math says 12, it's probably noise. Limit to 5 degrees.
                //if (Math.Abs(medianAngle) > 5.0) return src.Clone();
                if (Math.Abs(medianAngle) > SkewAngle) return src.Clone();

                skewAngle = medianAngle;
                Mat deskewed = new Mat();
                using Mat rotMat = Cv2.GetRotationMatrix2D(new Point2f(src.Width / 2f, src.Height / 2f), skewAngle, 1.0);
                Cv2.WarpAffine(src, deskewed, rotMat, src.Size(), InterpolationFlags.Cubic, BorderTypes.Constant, Scalar.White);

                return deskewed;
            }
            catch { return src.Clone(); }
        }

        public static Mat DeskewImage_Old(Mat src, out double skewAngle)
        {
            skewAngle = 0;
            try
            {
                using Mat gray = src.Channels() == 3 ? src.CvtColor(ColorConversionCodes.BGR2GRAY) : src.Clone();
                Cv2.Normalize(gray, gray, 0, 255, NormTypes.MinMax);
                using Mat bin = new Mat();
                Cv2.Threshold(gray, bin, 0, 255, ThresholdTypes.BinaryInv | ThresholdTypes.Otsu);

                // =====================================================================
                // NEW BULLETPROOF DESKEW: No Dilation. No smearing.
                // We find the raw letters, and calculate the angle of their bottom edges.
                // Slashes (/) and background noise cannot break this.
                // =====================================================================
                Cv2.FindContours(bin, out var contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

                List<Rect> boxes = new List<Rect>();
                foreach (var cnt in contours)
                {
                    var r = Cv2.BoundingRect(cnt);
                    // Filter for things that look like standard letters
                    if (r.Height >= 10 && r.Height <= src.Height / 2 && r.Width >= 2)
                    {
                        boxes.Add(r);
                    }
                }

                if (boxes.Count < 2) return src.Clone();

                // Sort boxes Left to Right
                boxes.Sort((a, b) => a.X.CompareTo(b.X));

                List<double> angles = new List<double>();

                // Compare adjacent boxes that are on the same text line
                for (int i = 0; i < boxes.Count - 1; i++)
                {
                    Rect b1 = boxes[i];
                    for (int j = i + 1; j < Math.Min(i + 5, boxes.Count); j++) // check next few boxes
                    {
                        Rect b2 = boxes[j];

                        // If they are on the same line (Y difference is small)
                        if (Math.Abs(b1.Y - b2.Y) < b1.Height)
                        {
                            double dx = b2.X - b1.X;
                            double dy = b2.Bottom - b1.Bottom; // Use the BOTTOM baseline of the text

                            if (dx > 0)
                            {
                                double angle = Math.Atan2(dy, dx) * (180.0 / Math.PI);

                                // Only accept reasonable text angles (between 0.5 and 15 degrees)
                                if (Math.Abs(angle) >= 0.5 && Math.Abs(angle) <= SkewAngle)
                                {
                                    angles.Add(angle);
                                }
                            }
                        }
                    }
                }

                if (angles.Count == 0) return src.Clone();

                angles.Sort();
                skewAngle = angles[angles.Count / 2]; // Get the median angle

                // If the angle is incredibly small (< 0.5 degrees), it's already flat. 
                // Do not rotate! Rotating perfectly flat text just makes it blurry.
                if (Math.Abs(skewAngle) < 0.5)
                {
                    skewAngle = 0;
                    return src.Clone();
                }

                Mat deskewed = new Mat();
                using Mat rotMat = Cv2.GetRotationMatrix2D(new Point2f(src.Width / 2f, src.Height / 2f), skewAngle, 1.0);
                Cv2.WarpAffine(src, deskewed, rotMat, src.Size(), InterpolationFlags.Cubic, BorderTypes.Constant, Scalar.White);

                return deskewed;
            }
            catch { return src.Clone(); }
        }

        //private static string ApplySmartHeuristics(string label, Rect box, Mat charImg, int medianLineHeight)
        //{
        //    double aspectRatio = (double)box.Width / box.Height;
        //    double heightRatio = (double)box.Height / medianLineHeight;

        //    // RULE 1: The "O vs o vs 0" Resolver
        //    if (label.Equals("O", StringComparison.OrdinalIgnoreCase) || label == "0" || label == "D")
        //    {
        //        if (aspectRatio < 0.75) return "0";
        //        if (heightRatio < 0.75) return "o";
        //        return "O";
        //    }

        //    // =================================================================
        //    // THE FIX: Ensure we have a 1-channel image for CountNonZero
        //    // =================================================================
        //    using Mat binImg = new Mat();
        //    if (charImg.Channels() == 3)
        //    {
        //        Cv2.CvtColor(charImg, binImg, ColorConversionCodes.BGR2GRAY);
        //        // Binarize it so ink is White (255) and background is Black (0)
        //        Cv2.Threshold(binImg, binImg, 0, 255, ThresholdTypes.BinaryInv | ThresholdTypes.Otsu);
        //    }
        //    else
        //    {
        //        // If it's already 1 channel, we must ensure it is BinaryInv (White Ink)
        //        Cv2.Threshold(charImg, binImg, 0, 255, ThresholdTypes.BinaryInv | ThresholdTypes.Otsu);
        //    }

        //    // RULE 2: The "8 vs B" Resolver
        //    if (label == "8" || label == "B")
        //    {
        //        using Mat leftEdge = new Mat(binImg, new Rect(0, 0, (int)(binImg.Width * 0.25), binImg.Height));
        //        double leftDensity = (double)Cv2.CountNonZero(leftEdge) / (leftEdge.Width * leftEdge.Height);

        //        return leftDensity > 0.70 ? "B" : "8";
        //    }

        //    // RULE 3: The "t vs 7" Resolver
        //    if (label == "t" || label == "7")
        //    {
        //        using Mat topLeft = new Mat(binImg, new Rect(0, 0, binImg.Width / 2, binImg.Height / 3));
        //        double topLeftDensity = (double)Cv2.CountNonZero(topLeft) / (topLeft.Width * topLeft.Height);

        //        return topLeftDensity < 0.25 ? "7" : "t";
        //    }

        //    // RULE 4: The "1 vs I vs l" Resolver
        //    if (label == "1" || label == "I" || label == "l")
        //    {
        //        if (aspectRatio > 0.35)
        //        {
        //            using Mat topLeft = new Mat(binImg, new Rect(0, 0, binImg.Width / 2, binImg.Height / 3));
        //            double topLeftDensity = (double)Cv2.CountNonZero(topLeft) / (topLeft.Width * topLeft.Height);

        //            if (topLeftDensity > 0.15) return "1";
        //        }

        //        if (heightRatio < 0.85) return "l";

        //        return "I";
        //    }

        //    // RULE 5: The "S vs 5" Resolver
        //    if (label == "S" || label == "5")
        //    {
        //        using Mat topEdge = new Mat(binImg, new Rect(0, 0, binImg.Width, binImg.Height / 5));
        //        double topDensity = (double)Cv2.CountNonZero(topEdge) / (topEdge.Width * topEdge.Height);

        //        if (topDensity > 0.60) return "5";
        //        return "S";
        //    }

        //    return label;
        //}

        private static string ApplySmartHeuristics(string label, Rect box, Mat charImg, int medianLineHeight)
        {
            double aspectRatio = (double)box.Width / box.Height;
            double heightRatio = (double)box.Height / medianLineHeight;

            // RULE 1: The "O vs o vs 0" Resolver
            if (label.Equals("O", StringComparison.OrdinalIgnoreCase) || label == "0" || label == "D" || label == "Q")
            {
                // A 'Q' usually has a tail dropping below the baseline, making it artificially taller 
                // or having ink in the bottom right corner.
                if (label == "Q") return "Q";

                if (aspectRatio < 0.70) return "0";
                if (heightRatio < 0.75) return "o";
                return "O";
            }

            using Mat binImg = new Mat();
            if (charImg.Channels() == 3)
            {
                Cv2.CvtColor(charImg, binImg, ColorConversionCodes.BGR2GRAY);
                Cv2.Threshold(binImg, binImg, 0, 255, ThresholdTypes.BinaryInv | ThresholdTypes.Otsu);
            }
            else
            {
                Cv2.Threshold(charImg, binImg, 0, 255, ThresholdTypes.BinaryInv | ThresholdTypes.Otsu);
            }

            // RULE 2: The "8 vs B" Resolver
            if (label == "8" || label == "B")
            {
                using Mat leftEdge = new Mat(binImg, new Rect(0, 0, (int)(binImg.Width * 0.25), binImg.Height));
                double leftDensity = (double)Cv2.CountNonZero(leftEdge) / (leftEdge.Width * leftEdge.Height);
                return leftDensity > 0.70 ? "B" : "8";
            }

            // RULE 3: The "t vs 7" Resolver
            if (label == "t" || label == "7")
            {
                using Mat topLeft = new Mat(binImg, new Rect(0, 0, binImg.Width / 2, binImg.Height / 3));
                double topLeftDensity = (double)Cv2.CountNonZero(topLeft) / (topLeft.Width * topLeft.Height);
                return topLeftDensity < 0.25 ? "7" : "t";
            }

            //// =================================================================
            //// THE FIX: RULE 4: The "1 vs I vs l" Symmetry Resolver
            //// =================================================================
            //if (label == "1" || label == "I" || label == "l")
            //{
            //    // 1. If it is incredibly skinny, it's almost always a lowercase 'l'.
            //    if (aspectRatio < 0.25) return "l";

            //    // 2. We split the image down the middle vertically.
            //    int halfWidth = binImg.Width / 2;
            //    using Mat leftHalf = new Mat(binImg, new Rect(0, 0, halfWidth, binImg.Height));
            //    using Mat rightHalf = new Mat(binImg, new Rect(halfWidth, 0, binImg.Width - halfWidth, binImg.Height));

            //    int leftPixels = Cv2.CountNonZero(leftHalf);
            //    int rightPixels = Cv2.CountNonZero(rightHalf);

            //    // 3. A '1' has the main vertical bar on the right side, and mostly empty space under the left flag.
            //    //    An 'I' (even with serifs) is perfectly balanced left and right.
            //    if (rightPixels > (leftPixels * 1.5))
            //    {
            //        return "1"; // Highly asymmetrical to the right
            //    }

            //    // 4. Default to Capital 'I' in industrial environments
            //    return "I";
            //}

            //// RULE 5: The "S vs 5" Resolver
            //if (label == "S" || label == "5")
            //{
            //    using Mat topEdge = new Mat(binImg, new Rect(0, 0, binImg.Width, binImg.Height / 5));
            //    double topDensity = (double)Cv2.CountNonZero(topEdge) / (topEdge.Width * topEdge.Height);
            //    if (topDensity > 0.60) return "5";
            //    return "S";
            //}

            //// RULE 6: The "2 vs Z" Resolver
            //if (label == "2" || label == "Z")
            //{
            //    // A '2' has a curved top right (empty space). A 'Z' has a hard right angle (ink).
            //    using Mat topRight = new Mat(binImg, new Rect(binImg.Width / 2, 0, binImg.Width / 2, binImg.Height / 3));
            //    double trDensity = (double)Cv2.CountNonZero(topRight) / (topRight.Width * topRight.Height);
            //    return trDensity > 0.40 ? "Z" : "2";
            //}

            return label;
        }

        public static Rect UnrotateBox(Rect box, int origW, int origH, RotationAngles angle)
        {
            if (angle == RotationAngles.Zero) return box;
            int x = box.X, y = box.Y, w = box.Width, h = box.Height;

            return angle switch
            {
                RotationAngles.Ninety => new Rect(origW - y - h, x, h, w),
                RotationAngles.OneEighty => new Rect(origW - x - w, origH - y - h, w, h),
                RotationAngles.TwoSeventy => new Rect(y, origH - x - w, h, w),
                _ => box
            };
        }

        public static OpenCvSharp.Point UnrotatePoint(OpenCvSharp.Point pt, int origW, int origH, RotationAngles angle)
        {
            if (angle == RotationAngles.Zero) return pt;
            int x = pt.X, y = pt.Y;
            return angle switch
            {
                RotationAngles.Ninety => new OpenCvSharp.Point(origW - y, x),
                RotationAngles.OneEighty => new OpenCvSharp.Point(origW - x, origH - y),
                RotationAngles.TwoSeventy => new OpenCvSharp.Point(y, origH - x),
                _ => pt
            };
        }

        private static Rect InverseDeskewRect(Rect box, double angle, float cx, float cy)
        {
            using Mat rotMat = Cv2.GetRotationMatrix2D(new Point2f(cx, cy), -angle, 1.0);
            double m00 = rotMat.At<double>(0, 0), m01 = rotMat.At<double>(0, 1), m02 = rotMat.At<double>(0, 2);
            double m10 = rotMat.At<double>(1, 0), m11 = rotMat.At<double>(1, 1), m12 = rotMat.At<double>(1, 2);

            float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;
            void Check(double px, double py)
            {
                double nx = m00 * px + m01 * py + m02, ny = m10 * px + m11 * py + m12;
                if (nx < minX) minX = (float)nx; if (nx > maxX) maxX = (float)nx;
                if (ny < minY) minY = (float)ny; if (ny > maxY) maxY = (float)ny;
            }

            Check(box.Left, box.Top); Check(box.Right, box.Top);
            Check(box.Right, box.Bottom); Check(box.Left, box.Bottom);

            return new Rect((int)Math.Round(minX), (int)Math.Round(minY), (int)Math.Round(maxX - minX), (int)Math.Round(maxY - minY));
        }

        // =========================================================================
        // FULLY RESTORED: ANCHOR AND PREVIEW FUNCTIONS
        // =========================================================================
        public static Mat GenerateRoiControlPreview(Mat currentFullImage, RoiObject roi)
        {
            CvRect safeBox = roi.Box.Intersect(new CvRect(0, 0, currentFullImage.Width, currentFullImage.Height));
            if (safeBox.Width <= 0 || safeBox.Height <= 0) return null;

            using Mat crop = new Mat(currentFullImage, safeBox);
            Mat rotatedCrop = new Mat();

            try
            {
                RotateImage(crop, out rotatedCrop, roi.RotationAngle);

                if (roi.MorphOp == MorphOperation.None) return rotatedCrop.Clone();

                using Mat gray = new Mat();
                if (rotatedCrop.Channels() == 4) Cv2.CvtColor(rotatedCrop, gray, ColorConversionCodes.BGRA2GRAY);
                else if (rotatedCrop.Channels() == 3) Cv2.CvtColor(rotatedCrop, gray, ColorConversionCodes.BGR2GRAY);
                else rotatedCrop.CopyTo(gray);

                Mat processedRoi = new Mat();
                ProcessImageForMode(gray, processedRoi, roi.SegmentationMode);

                if (roi.MorphKernelWidth > 0 && roi.MorphKernelHeight > 0 && roi.MorphIterations > 0)
                {
                    MorphologyProcessor.Apply(processedRoi, roi.MorphOp, roi.MorphKernelWidth, roi.MorphKernelHeight, roi.MorphIterations);
                }

                return processedRoi;
            }
            finally
            {
                if (rotatedCrop != null && !rotatedCrop.IsDisposed) rotatedCrop.Dispose();
            }
        }

        public static OpenCvSharp.Point FindAnyBarcodeCenter(Mat fullImage, Rect searchRect)
        {
            try
            {
                using Mat crop = new Mat(fullImage, searchRect);
                var options = new ReaderOptions { Formats = BarcodeFormats.Any, TryRotate = true, TryHarder = true };

                using Mat gray = new Mat();
                if (crop.Channels() == 3) Cv2.CvtColor(crop, gray, ColorConversionCodes.BGR2GRAY);
                else crop.CopyTo(gray);

                int pad = 20;
                using Mat paddedGray = new Mat();
                Cv2.CopyMakeBorder(gray, paddedGray, pad, pad, pad, pad, BorderTypes.Constant, Scalar.White);

                var center = TryGetCenterZx(paddedGray, options, -pad, -pad);
                if (center.X != -1) return center;

                using Mat binary = new Mat();
                Cv2.Threshold(paddedGray, binary, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);
                center = TryGetCenterZx(binary, options, -pad, -pad);
                if (center.X != -1) return center;

                float scale = 2.0f;
                using Mat scaled = new Mat();
                Cv2.Resize(binary, scaled, new OpenCvSharp.Size(0, 0), scale, scale, InterpolationFlags.Nearest);

                center = TryGetCenterZx(scaled, options, -pad, -pad, scale);
                if (center.X != -1) return center;

                return new OpenCvSharp.Point(-1, -1);
            }
            catch { return new OpenCvSharp.Point(-1, -1); }
        }

        private static OpenCvSharp.Point TryGetCenterZx(Mat img, ReaderOptions options, int offsetX, int offsetY, float scaleDivisor = 1.0f)
        {
            try
            {
                var iv = new ImageView(img.Data, img.Width, img.Height, ImageFormat.Lum);
                var results = BarcodeReader.Read(iv, options);

                if (results.Length > 0)
                {
                    var p = results[0].Position;
                    long sumX = p.TopLeft.X + p.TopRight.X + p.BottomRight.X + p.BottomLeft.X;
                    long sumY = p.TopLeft.Y + p.TopRight.Y + p.BottomRight.Y + p.BottomLeft.Y;

                    float avgX = sumX / 4.0f;
                    float avgY = sumY / 4.0f;

                    avgX /= scaleDivisor; avgY /= scaleDivisor;
                    avgX += offsetX; avgY += offsetY;

                    return new OpenCvSharp.Point((int)avgX, (int)avgY);
                }
            }
            catch { }
            return new OpenCvSharp.Point(-1, -1);
        }

        public static OpenCvSharp.Point FindMatchLocation(Mat fullImage, Mat template, Rect searchRect, bool useEdge)
        {
            try
            {
                using Mat searchArea = new Mat(fullImage, searchRect);

                using Mat procSearch = new Mat();
                using Mat procTemplate = new Mat();
                using Mat graySearch = new Mat();
                using Mat grayTemplate = new Mat();

                if (searchArea.Channels() == 3) Cv2.CvtColor(searchArea, graySearch, ColorConversionCodes.BGR2GRAY);
                else searchArea.CopyTo(graySearch);

                if (template.Channels() == 3) Cv2.CvtColor(template, grayTemplate, ColorConversionCodes.BGR2GRAY);
                else template.CopyTo(grayTemplate);

                Cv2.EqualizeHist(graySearch, graySearch);
                Cv2.EqualizeHist(grayTemplate, grayTemplate);

                if (useEdge)
                {
                    Cv2.Canny(graySearch, procSearch, 50, 150);
                    Cv2.Canny(grayTemplate, procTemplate, 50, 150);
                }
                else
                {
                    graySearch.CopyTo(procSearch);
                    grayTemplate.CopyTo(procTemplate);
                }

                using Mat res = new Mat();
                Cv2.MatchTemplate(procSearch, procTemplate, res, TemplateMatchModes.CCoeffNormed);
                Cv2.MinMaxLoc(res, out double minVal, out double maxVal, out OpenCvSharp.Point minLoc, out OpenCvSharp.Point maxLoc);

                //decrese the thersold to increase the accurcy
                //double threshold = useEdge ? 0.35 : 0.50;

                double threshold = useEdge ? AnchorConfidence.Min : AnchorConfidence.Max;

                if (maxVal >= threshold) return maxLoc;

                using var orb = ORB.Create(500);
                using Mat des1 = new Mat();
                using Mat des2 = new Mat();

                orb.DetectAndCompute(grayTemplate, null, out KeyPoint[] kpTemplate, des1);
                orb.DetectAndCompute(graySearch, null, out KeyPoint[] kpSearch, des2);

                if (des1.Empty() || des2.Empty() || kpTemplate.Length < 5 || kpSearch.Length < 5)
                    return new OpenCvSharp.Point(-1, -1);

                using var matcher = new BFMatcher(NormTypes.Hamming, crossCheck: true);
                var matches = matcher.Match(des1, des2);
                var goodMatches = matches.Where(m => m.Distance < 50).OrderBy(m => m.Distance).ToList();

                if (goodMatches.Count > 10) goodMatches = goodMatches.Take(goodMatches.Count / 2).ToList();
                if (goodMatches.Count < 3) return new OpenCvSharp.Point(-1, -1);

                List<int> dxs = new List<int>();
                List<int> dys = new List<int>();

                foreach (var m in goodMatches)
                {
                    var ptTemplate = kpTemplate[m.QueryIdx].Pt;
                    var ptSearch = kpSearch[m.TrainIdx].Pt;

                    dxs.Add((int)(ptSearch.X - ptTemplate.X));
                    dys.Add((int)(ptSearch.Y - ptTemplate.Y));
                }

                dxs.Sort(); dys.Sort();
                return new OpenCvSharp.Point(dxs[dxs.Count / 2], dys[dys.Count / 2]);
            }
            catch { return new OpenCvSharp.Point(-1, -1); }
        }
    }
}