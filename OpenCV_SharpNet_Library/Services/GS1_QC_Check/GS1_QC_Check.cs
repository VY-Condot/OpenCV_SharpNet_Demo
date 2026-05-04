using OpenCV_SharpNet.Interfaces;
using OpenCV_SharpNet.Models.GS1_QC;
using OpenCvSharp;
using System;
using System.Configuration;
using System.Linq;
using ZXingCpp;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace OpenCV_SharpNet.Services.GS1_QC_Check
{
    public class GS1_QC_Check : IGS1_QC_Check
    {
        public GS1_QC_CheckResult EvaluateISO15415Quality(Mat rawGrayCrop, Rect boundingBox, Point2f[] zxingCorners, bool isDecoded)
        {
            try
            {
                if (zxingCorners == null || zxingCorners.Length != 4) return new GS1_QC_CheckResult();

                // =========================================================================
                // 1. CLOCKWISE SORTING & GEOMETRY METRICS (AN, GN)
                // =========================================================================
                var cw = SortCornersClockwiseFast(zxingCorners);

                double wTop = PointDistance(cw[0], cw[1]);
                double wBot = PointDistance(cw[3], cw[2]);
                double hLeft = PointDistance(cw[0], cw[3]);
                double hRight = PointDistance(cw[1], cw[2]);

                double avgWidth = (wTop + wBot) / 2.0;
                double avgHeight = (hLeft + hRight) / 2.0;

                double axialNonUniformity = Math.Abs((avgWidth / Math.Max(1.0, avgHeight)) - 1.0);
                int gradeAN = axialNonUniformity <= 0.06 ? 4 : axialNonUniformity <= 0.08 ? 3 : axialNonUniformity <= 0.10 ? 2 : axialNonUniformity <= 0.12 ? 1 : 0;

                double d1 = PointDistance(cw[0], cw[2]);
                double d2 = PointDistance(cw[1], cw[3]);

                // =========================================================================
                // 2. EXTRACTION FIX (Isolate the barcode to eliminate text)
                // =========================================================================
                int warpSize = 400;
                Point2f[] dstPts = { new Point2f(0, 0), new Point2f(warpSize, 0), new Point2f(warpSize, warpSize), new Point2f(0, warpSize) };

                using Mat cleanBarcode = new Mat();
                using Mat transform = Cv2.GetPerspectiveTransform(cw, dstPts);
                Cv2.WarpPerspective(rawGrayCrop, cleanBarcode, transform, new Size(warpSize, warpSize));

                double globalThreshold = Cv2.Threshold(cleanBarcode, new Mat(), 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

                // =========================================================================
                // 3. ESTIMATE GRID SIZE ON CLEAN IMAGE
                // =========================================================================
                int transitions = CountTransitionsFast(cleanBarcode, new Point2f(5, 5), new Point2f(warpSize - 5, 5), globalThreshold);
                int estSize = transitions + 1;
                int gridSize = GetClosestStandardGridSize(estSize);

                double gnCells = Math.Abs((d1 / Math.Max(1.0, d2)) - 1.0) * gridSize;
                int gradeGN = gnCells <= 0.38 ? 4 : gnCells <= 0.50 ? 3 : gnCells <= 0.63 ? 2 : gnCells <= 0.75 ? 1 : 0;

                // =========================================================================
                // 4. OPTIMIZED SAMPLING & VIRTUAL ROTATION 
                // =========================================================================
                double cellSize = (double)warpSize / gridSize;
                int maskRadius = Math.Max(1, (int)Math.Round((cellSize * 0.8) / 2.0));

                // Sample the grid exactly ONCE, saving massive CPU and RAM
                double[,] baseGridR = new double[gridSize, gridSize];
                for (int r = 0; r < gridSize; r++)
                {
                    int cy = Math.Clamp((int)((r + 0.5) * cellSize), 0, warpSize - 1);
                    for (int c = 0; c < gridSize; c++)
                    {
                        int cx = Math.Clamp((int)((c + 0.5) * cellSize), 0, warpSize - 1);
                        baseGridR[r, c] = GetFastCircularMean(cleanBarcode, cx, cy, maskRadius);
                    }
                }

                int bestFpdErrors = int.MaxValue;
                double[,] bestCellR = new double[gridSize, gridSize];

                // Evaluate 4 virtual rotations without rotating the actual image matrices
                for (int i = 0; i < 4; i++)
                {
                    double[,] cellR = new double[gridSize, gridSize];
                    int fpdErrors = 0;

                    for (int r = 0; r < gridSize; r++)
                    {
                        for (int c = 0; c < gridSize; c++)
                        {
                            // Map indices to simulate rotation mathematically
                            double val = i switch
                            {
                                0 => baseGridR[r, c],                                        // 0 deg
                                1 => baseGridR[gridSize - 1 - c, r],                         // 90 deg CW
                                2 => baseGridR[gridSize - 1 - r, gridSize - 1 - c],          // 180 deg
                                _ => baseGridR[c, gridSize - 1 - r]                          // 270 deg CW
                            };

                            cellR[r, c] = val;

                            // Dynamic FPD logic
                            if (c == 0 || r == gridSize - 1)
                            { // Left & Bottom L-Pattern
                                if (val > globalThreshold) fpdErrors++;
                            }
                            else if (r == 0)
                            { // Top Clock Track
                                if ((val <= globalThreshold) != (c % 2 == 0)) fpdErrors++;
                            }
                            else if (c == gridSize - 1)
                            { // Right Clock Track
                                if ((val <= globalThreshold) != (r % 2 != 0)) fpdErrors++;
                            }
                        }
                    }

                    if (fpdErrors < bestFpdErrors)
                    {
                        bestFpdErrors = fpdErrors;
                        bestCellR = cellR;
                    }
                }

                // =========================================================================
                // 5. SC & MODULATION MATH
                // =========================================================================
                double rMax = 0, rMin = 255;
                foreach (double val in bestCellR) { if (val > rMax) rMax = val; if (val < rMin) rMin = val; }

                double scPercent = ((rMax - rMin) / 255.0) * 100.0;
                int gradeSC = scPercent >= 70 ? 4 : scPercent >= 55 ? 3 : scPercent >= 40 ? 2 : scPercent >= 20 ? 1 : 0;

                double symbolContrast = Math.Max(1.0, rMax - rMin);
                List<double> modValues = new List<double>((gridSize - 2) * (gridSize - 2));
                int visualErrors = 0;

                for (int r = 1; r < gridSize - 1; r++)
                {
                    for (int c = 1; c < gridSize - 1; c++)
                    {
                        double v = bestCellR[r, c];
                        double reflectanceMargin = v <= globalThreshold ? globalThreshold - v : v - globalThreshold;
                        double mod = reflectanceMargin / symbolContrast;
                        modValues.Add(mod);

                        if (mod < 0.15) visualErrors++;
                    }
                }

                modValues.Sort();
                int ecCapacityIndex = (int)(modValues.Count * 0.15);
                double worstMod = modValues.Count > ecCapacityIndex ? modValues[ecCapacityIndex] : 0;
                int gradeMOD = worstMod >= 0.40 ? 4 : worstMod >= 0.30 ? 3 : worstMod >= 0.20 ? 2 : worstMod >= 0.10 ? 1 : 0;

                // =========================================================================
                // 6. FINAL GRADES
                // =========================================================================
                int gradeFPD = bestFpdErrors == 0 ? 4 : bestFpdErrors <= 1 ? 3 : bestFpdErrors <= 2 ? 2 : bestFpdErrors <= 3 ? 1 : 0;
                int gradeDecode = isDecoded ? 4 : 0;
                int gradeUEC = visualErrors <= 2 ? 4 : 2;

                int finalGrade = Math.Min(Math.Min(Math.Min(gradeDecode, gradeSC), Math.Min(gradeAN, gradeGN)), Math.Min(Math.Min(gradeMOD, gradeFPD), gradeUEC));
                string[] letters = { "F", "D", "C", "B", "A" };

                // =========================================================================
                // 7. ASNI / AS9132
                // =========================================================================
                int pad = Math.Max(15, boundingBox.Width / 10);
                int x = Math.Max(0, boundingBox.X - pad);
                int y = Math.Max(0, boundingBox.Y - pad);
                int w = Math.Min(rawGrayCrop.Width - x, boundingBox.Width + (pad * 2));
                int h = Math.Min(rawGrayCrop.Height - y, boundingBox.Height + (pad * 2));
                if (w < 15 || h < 15) return new GS1_QC_CheckResult();

                using Mat symbolRoi = new Mat(rawGrayCrop, new Rect(x, y, w, h));

                Point2f ptTL = new Point2f(), ptTR = new Point2f(), ptBR = new Point2f(), ptBL = new Point2f();
                bool cornersFound = false;

                if (isDecoded)
                {
                    try
                    {
                        // Optimization: reuse the Step property efficiently
                        var iv = new ImageView(symbolRoi.Data, symbolRoi.Width, symbolRoi.Height, ImageFormat.Lum, (int)symbolRoi.Step());
                        var results = BarcodeReader.Read(iv, new ReaderOptions { Formats = BarcodeFormats.Any, TryHarder = false });
                        if (results.Length > 0)
                        {
                            var p = results[0].Position;
                            ptTL = new Point2f(p.TopLeft.X, p.TopLeft.Y);
                            ptTR = new Point2f(p.TopRight.X, p.TopRight.Y);
                            ptBR = new Point2f(p.BottomRight.X, p.BottomRight.Y);
                            ptBL = new Point2f(p.BottomLeft.X, p.BottomLeft.Y);
                            cornersFound = true;
                        }
                    }
                    catch { }
                }

                if (!cornersFound)
                {
                    using Mat binCrop = new Mat();
                    Cv2.Threshold(symbolRoi, binCrop, 0, 255, ThresholdTypes.BinaryInv | ThresholdTypes.Otsu);
                    using Mat fused = new Mat();
                    using Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(5, 5));
                    Cv2.MorphologyEx(binCrop, fused, MorphTypes.Close, kernel);
                    Cv2.FindContours(fused, out var contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

                    if (contours.Length > 0)
                    {
                        // Fast manual loop replaces heavy LINQ OrderBy
                        Point[] hull = Cv2.ConvexHull(contours.OrderByDescending(c => Cv2.ContourArea(c)).First());
                        ptTL = GetExtremeCorner(hull, 1, 1);   // X + Y Min
                        ptBR = GetExtremeCorner(hull, -1, -1); // X + Y Max
                        ptTR = GetExtremeCorner(hull, -1, 1);  // X - Y Max
                        ptBL = GetExtremeCorner(hull, 1, -1);  // X - Y Min
                    }
                }

                // Perspective unwarp for ASNI
                int warpSize_Q = 300;
                Point2f[] srcPts = { ptTL, ptTR, ptBR, ptBL };
                Point2f[] dstPts_Q = { new Point2f(0, 0), new Point2f(warpSize_Q - 1, 0), new Point2f(warpSize_Q - 1, warpSize_Q - 1), new Point2f(0, warpSize_Q - 1) };

                using Mat transform_Q = Cv2.GetPerspectiveTransform(srcPts, dstPts_Q);
                using Mat warped = new Mat();
                Cv2.WarpPerspective(symbolRoi, warped, transform_Q, new OpenCvSharp.Size(warpSize_Q, warpSize_Q));

                double globalThr = Cv2.Threshold(warped, new Mat(), 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);
                int gridSize_Q = EstimateGridSizeFast(warped, globalThr);

                // AS9132 Calculations
                double physWidth = PointDistance(ptTL, ptTR);
                double physCellSize = gridSize_Q > 0 ? physWidth / gridSize_Q : 0;

                // Replaced slow array allocations and LINQ `.Min()` / `.Max()`
                double minX = Math.Min(Math.Min(ptTL.X, ptTR.X), Math.Min(ptBR.X, ptBL.X));
                double maxX = Math.Max(Math.Max(ptTL.X, ptTR.X), Math.Max(ptBR.X, ptBL.X));
                double minY = Math.Min(Math.Min(ptTL.Y, ptTR.Y), Math.Min(ptBR.Y, ptBL.Y));
                double maxY = Math.Max(Math.Max(ptTL.Y, ptTR.Y), Math.Max(ptBR.Y, ptBL.Y));

                double minClearancePx = Math.Min(Math.Min(minX, symbolRoi.Width - maxX), Math.Min(minY, symbolRoi.Height - maxY));
                double qzModules = physCellSize > 0 ? (minClearancePx / physCellSize) : 0;

                int gradeQZ = qzModules >= 0.4 ? 4 : 0;
                double qzPercent = Math.Min(100.0, qzModules * 100.0);
                double angleDistortion = GetAS9132Distortion(ptBL, ptTL, ptBR);

                return new GS1_QC_CheckResult
                (
                    Decode: letters[gradeDecode],
                    SC: $"{letters[gradeSC]} ({(scPercent):F2}%)",
                    AN: $"{letters[gradeAN]} ({axialNonUniformity:F2})",
                    GN: $"{letters[gradeGN]} ({gnCells:F2} cell)",
                    MOD: letters[gradeMOD],
                    FPD: letters[gradeFPD],
                    UEC: $"{letters[gradeUEC]}",
                    PG: $"R_Max: {(rMax / 255.0 * 100):F2}%, R_Min: {(rMin / 255.0 * 100):F2}%",
                    OverAll: $"{letters[finalGrade]}",
                    AS9132_Distortion: $"{angleDistortion:F2}°",
                    AS9132_QuietZone: $"{qzPercent:F2}% ({letters[gradeQZ]})",
                    AS9132_Elongation: $"{(axialNonUniformity * 100):F2}%",
                    DPM_Rmin: $"R_Min: {(rMin / 255.0 * 100):F2}%"
                );
            }
            catch { return new GS1_QC_CheckResult(); }
        }

        // =========================================================================================
        // HIGH-SPEED HELPER METHODS
        // =========================================================================================

        // Ultra-fast memory pointer access replacing Mat.Zeros + Cv2.Circle + Cv2.Mean
        private unsafe double GetFastCircularMean(Mat img, int cx, int cy, int radius)
        {
            int sum = 0, count = 0;
            byte* ptr = (byte*)img.DataPointer;
            int step = (int)img.Step();
            int r2 = radius * radius;

            int minX = Math.Max(0, cx - radius);
            int maxX = Math.Min(img.Width - 1, cx + radius);
            int minY = Math.Max(0, cy - radius);
            int maxY = Math.Min(img.Height - 1, cy + radius);

            for (int y = minY; y <= maxY; y++)
            {
                int dy = y - cy;
                int dy2 = dy * dy;
                byte* row = ptr + (y * step);

                for (int x = minX; x <= maxX; x++)
                {
                    int dx = x - cx;
                    if (dx * dx + dy2 <= r2)
                    {
                        sum += row[x];
                        count++;
                    }
                }
            }
            return count > 0 ? (double)sum / count : 0;
        }

        // Replaces LINQ sorting
        private Point2f[] SortCornersClockwiseFast(Point2f[] corners)
        {
            float cx = (corners[0].X + corners[1].X + corners[2].X + corners[3].X) / 4f;
            float cy = (corners[0].Y + corners[1].Y + corners[2].Y + corners[3].Y) / 4f;

            Array.Sort(corners, (a, b) =>
                Math.Atan2(a.Y - cy, a.X - cx).CompareTo(Math.Atan2(b.Y - cy, b.X - cx)));

            return corners;
        }

        // Replaces heavy LINQ OrderBy for Convex Hull Point searching
        private Point2f GetExtremeCorner(Point[] hull, int xMulti, int yMulti)
        {
            Point bestPoint = hull[0];
            int bestScore = (bestPoint.X * xMulti) + (bestPoint.Y * yMulti);

            for (int i = 1; i < hull.Length; i++)
            {
                int score = (hull[i].X * xMulti) + (hull[i].Y * yMulti);
                if (score < bestScore)
                {
                    bestScore = score;
                    bestPoint = hull[i];
                }
            }
            return new Point2f(bestPoint.X, bestPoint.Y);
        }

        // Direct iteration Array Search replacing LINQ
        private int GetClosestStandardGridSize(int estimated)
        {
            int[] stdSizes = { 10, 12, 14, 16, 18, 20, 22, 24, 26, 32, 36, 40, 44, 48, 52, 64, 72, 80, 88, 96, 104, 120, 132, 144 };
            int bestSize = stdSizes[0];
            int minDiff = int.MaxValue;

            foreach (int s in stdSizes)
            {
                int diff = Math.Abs(s - estimated);
                if (diff < minDiff)
                {
                    minDiff = diff;
                    bestSize = s;
                }
            }
            return bestSize;
        }

        private unsafe int EstimateGridSizeFast(Mat warped, double globalThr)
        {
            int size = warped.Width;
            int transitions = 0;

            byte* ptr = (byte*)warped.DataPointer;
            int step = (int)warped.Step();
            byte* middleRow = ptr + ((size / 2) * step);

            bool lastIsDark = middleRow[5] < globalThr;

            for (int x = 6; x < size - 5; x++)
            {
                bool isDark = middleRow[x] < globalThr;
                if (isDark != lastIsDark)
                {
                    transitions++;
                    lastIsDark = isDark;
                }
            }
            return GetClosestStandardGridSize(transitions + 1);
        }

        // Pointer-based rapid transition counting 
        private unsafe int CountTransitionsFast(Mat img, Point2f p1, Point2f p2, double thr)
        {
            int transitions = 0;
            int steps = (int)Math.Max(Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y));
            if (steps == 0) return 0;

            byte* ptr = (byte*)img.DataPointer;
            int step = (int)img.Step();
            int width = img.Width - 1;
            int height = img.Height - 1;

            bool lastIsDark = true;
            for (int i = 0; i <= steps; i++)
            {
                float t = (float)i / steps;
                int x = Math.Clamp((int)(p1.X + t * (p2.X - p1.X)), 0, width);
                int y = Math.Clamp((int)(p1.Y + t * (p2.Y - p1.Y)), 0, height);

                bool isDark = *(ptr + (y * step) + x) <= thr;
                if (i > 0 && isDark != lastIsDark) transitions++;
                lastIsDark = isDark;
            }
            return transitions;
        }

        private double PointDistance(Point2f p1, Point2f p2) => Math.Sqrt(((p1.X - p2.X) * (p1.X - p2.X)) + ((p1.Y - p2.Y) * (p1.Y - p2.Y)));

        private double GetAS9132Distortion(Point2f cornerBL, Point2f ptTL, Point2f ptBR)
        {
            double v1x = ptTL.X - cornerBL.X;
            double v1y = ptTL.Y - cornerBL.Y;
            double v2x = ptBR.X - cornerBL.X;
            double v2y = ptBR.Y - cornerBL.Y;

            double dotProduct = (v1x * v2x) + (v1y * v2y);
            double mag1 = Math.Sqrt(v1x * v1x + v1y * v1y);
            double mag2 = Math.Sqrt(v2x * v2x + v2y * v2y);

            if (mag1 == 0 || mag2 == 0) return 0;

            double angleInRadians = Math.Acos(dotProduct / (mag1 * mag2));
            return (angleInRadians * (180.0 / Math.PI)) - 90.0;
        }
    }
}