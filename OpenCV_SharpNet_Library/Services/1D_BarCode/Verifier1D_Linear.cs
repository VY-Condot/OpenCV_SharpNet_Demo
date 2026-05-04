using OpenCV_SharpNet.Interfaces;
using OpenCV_SharpNet.Models.GS1_QC;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCV_SharpNet.Services._1D_BarCode
{
    [DebuggerStepThrough]
    public class Verifier1D_Linear : IBarcodeVerifier
    {
        public GS1_QC_CheckResult EvaluateQuality(Mat rawGrayCrop, Rect boundingBox, bool isDecoded, string barcodeFormat)
        {
            try
            {
                int pad = Math.Max(15, boundingBox.Width / 10);
                int x = Math.Max(0, boundingBox.X - pad);
                int y = Math.Max(0, boundingBox.Y - pad);
                int w = Math.Min(rawGrayCrop.Width - x, boundingBox.Width + (pad * 2));
                int h = Math.Min(rawGrayCrop.Height - y, boundingBox.Height + (pad * 2));
                if (w < 15 || h < 15) return new GS1_QC_CheckResult();

                using Mat symbolRoi = new Mat(rawGrayCrop, new Rect(x, y, w, h));
                string[] letters = { "F", "D", "C", "B", "A" };
                int gradeDecode = isDecoded ? 4 : 0;

                // 1. Symbol Contrast
                using Mat blurred = new Mat();
                Cv2.MedianBlur(symbolRoi, blurred, 3);
                Cv2.MinMaxLoc(blurred, out double rMin, out double rMax);

                double contrast = ((rMax - rMin) / 255.0) * 100.0;
                int gradeSC = contrast >= 70 ? 4 : contrast >= 55 ? 3 : contrast >= 40 ? 2 : contrast >= 20 ? 1 : 0;

                // 2. Minimum Reflectance (Page 9)
                double rMinPercent = rMin / 255.0;
                double rMaxPercent = rMax / 255.0;
                int gradeMinReflect = (rMinPercent <= 0.5 * rMaxPercent) ? 4 : 0;

                // 3. Modulation / Edge Contrast
                using Mat mean = new Mat();
                using Mat stdDev = new Mat();
                Cv2.MeanStdDev(symbolRoi, mean, stdDev);
                double modVariance = stdDev.At<double>(0) / 255.0;
                int gradeMOD = modVariance >= 0.25 ? 4 : modVariance >= 0.20 ? 3 : modVariance >= 0.15 ? 2 : modVariance >= 0.10 ? 1 : 0;
                int gradeECmin = gradeMOD; // Correlated approximation

                // 4. Defects (Spots and Voids) (Page 9)
                using Mat bin = new Mat();
                Cv2.Threshold(symbolRoi, bin, 0, 255, ThresholdTypes.BinaryInv | ThresholdTypes.Otsu);

                using Mat cleanBin = new Mat();
                using Mat kernel1D = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(3, 3));
                Cv2.MorphologyEx(bin, cleanBin, MorphTypes.Open, kernel1D);
                Cv2.MorphologyEx(cleanBin, cleanBin, MorphTypes.Close, kernel1D);

                using Mat diff = new Mat();
                Cv2.Absdiff(bin, cleanBin, diff);
                double defectRatio = (double)Cv2.CountNonZero(diff) / (w * h);
                int gradeDefects = defectRatio <= 0.05 ? 4 : defectRatio <= 0.10 ? 3 : defectRatio <= 0.15 ? 2 : defectRatio <= 0.20 ? 1 : 0;

                // 5. Decodability (Page 9)
                int gradeDecodability = isDecoded ? Math.Min(4, gradeMOD + 1) : 0;

                int[] all1DGrades = { gradeDecode, gradeSC, gradeMinReflect, gradeECmin, gradeMOD, gradeDefects, gradeDecodability };
                int finalGrade = all1DGrades.Min();

                string typeLabel = string.IsNullOrEmpty(barcodeFormat) ? "1D Linear Barcode" : $"{barcodeFormat} (1D ISO 15416)";

                return new GS1_QC_CheckResult(
                    BarcodeType: typeLabel, Decode: letters[gradeDecode], SC: letters[gradeSC], MOD: letters[gradeMOD],
                    MinReflectance: letters[gradeMinReflect], MinEdgeContrast: letters[gradeECmin],
                    Defects: letters[gradeDefects], Decodability: letters[gradeDecodability],
                    OverAll: $"{letters[finalGrade]} ({finalGrade}.0)"
                );
            }
            catch { return new GS1_QC_CheckResult(); }
        }
    }
}
