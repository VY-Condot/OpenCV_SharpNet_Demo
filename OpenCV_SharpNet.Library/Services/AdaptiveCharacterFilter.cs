using CsplCam.Library.Models;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsplCam.Library.Services
{
    /// <summary>
    /// Smart Character Filter - Preserves ALL valid characters while removing ONLY garbage.
    /// 
    /// Philosophy: LENIENT BY DEFAULT
    /// - If it looks remotely like a character, KEEP IT
    /// - Only reject if it's clearly noise (empty, fragmented, uniform, etc.)
    /// 
    /// Works with ANY MinBlobW/MinBlobH - no arbitrary limits imposed.
    /// </summary>
    [DebuggerStepThrough]
    public static class SmartCharacterFilter
    {
        /// <summary>
        /// Filter contours to remove ONLY obvious garbage.
        /// Preserves all characters that user's MinBlobW/MinBlobH constraints allow.
        /// </summary>
        public static List<Rect> FilterOnlyObviousGarbage(
            Mat binaryImage,
            List<Rect> allBoxes,
            RoiObject roi)
        {
            if (allBoxes.Count == 0) return allBoxes;

            var filtered = new List<Rect>();

            foreach (var box in allBoxes)
            {
                // User already filtered by MinBlobW/H and MaxBlobW/H - respect that
                if (box.Width < roi.MinBlobW || box.Height < roi.MinBlobH ||
                    box.Width > roi.MaxBlobW || box.Height > roi.MaxBlobH)
                    continue;

                // ===================================================================
                // GARBAGE DETECTION RULES (Very Lenient)
                // Only reject if there's CLEAR EVIDENCE of garbage
                // ===================================================================

                // RULE 1: Completely Empty Box (0% ink)
                using (Mat region = new Mat(binaryImage, box))
                {
                    int nonZeroCount = Cv2.CountNonZero(region);
                    double fillPercentage = (double)nonZeroCount / (box.Width * box.Height);

                    if (fillPercentage < 0.01)  // Less than 1% ink = definitely empty
                    {
                        continue;  // SKIP THIS - It's garbage
                    }

                    // RULE 2: Pathologically thin boxes (single pixel lines)
                    if (box.Width <= 1 || box.Height <= 1)
                    {
                        continue;  // SKIP THIS - It's a line artifact
                    }

                    // RULE 3: Box is 95%+ ink (solid block = not a character)
                    if (fillPercentage > 0.95)
                    {
                        continue;  // SKIP THIS - It's a solid block
                    }

                    // RULE 4: Extremely low fill AND extreme aspect ratio (noise)
                    double aspectRatio = (double)box.Width / box.Height;
                    if (fillPercentage < 0.05 && (aspectRatio < 0.1 || aspectRatio > 10))
                    {
                        continue;  // SKIP THIS - Sparse noise
                    }
                }

                // If we reach here, it passed all garbage checks - KEEP IT!
                filtered.Add(box);
            }

            return filtered;
        }

        /// <summary>
        /// Optional: Remove border artifacts if user enables it.
        /// </summary>
        public static List<Rect> RemoveBorderArtifacts(
            List<Rect> boxes,
            int imageWidth,
            int imageHeight,
            int borderMargin = 5)
        {
            return boxes
                .Where(b => b.X > borderMargin &&
                            b.Y > borderMargin &&
                            b.Right < imageWidth - borderMargin &&
                            b.Bottom < imageHeight - borderMargin)
                .ToList();
        }

        /// <summary>
        /// Diagnose why characters might be missing.
        /// Returns statistics for debugging.
        /// </summary>
        public static Dictionary<string, object> GetDiagnostics(
            Mat binaryImage,
            List<Rect> allBoxes,
            RoiObject roi)
        {
            var stats = new Dictionary<string, object>
            {
                { "Total Contours Detected", allBoxes.Count },
                { "Empty Boxes (< 1% ink)", 0 },
                { "Solid Blocks (> 95% ink)", 0 },
                { "Single-Pixel Lines", 0 },
                { "Sparse Noise", 0 },
                { "Below MinBlobW", 0 },
                { "Below MinBlobH", 0 },
                { "Above MaxBlobW", 0 },
                { "Above MaxBlobH", 0 },
                { "Final Accepted", 0 }
            };

            foreach (var box in allBoxes)
            {
                if (box.Width < roi.MinBlobW)
                {
                    stats["Below MinBlobW"] = (int)stats["Below MinBlobW"] + 1;
                    continue;
                }
                if (box.Height < roi.MinBlobH)
                {
                    stats["Below MinBlobH"] = (int)stats["Below MinBlobH"] + 1;
                    continue;
                }
                if (box.Width > roi.MaxBlobW)
                {
                    stats["Above MaxBlobW"] = (int)stats["Above MaxBlobW"] + 1;
                    continue;
                }
                if (box.Height > roi.MaxBlobH)
                {
                    stats["Above MaxBlobH"] = (int)stats["Above MaxBlobH"] + 1;
                    continue;
                }

                using (Mat region = new Mat(binaryImage, box))
                {
                    int nonZeroCount = Cv2.CountNonZero(region);
                    double fillPercentage = (double)nonZeroCount / (box.Width * box.Height);

                    if (fillPercentage < 0.01)
                    {
                        stats["Empty Boxes (< 1% ink)"] = (int)stats["Empty Boxes (< 1% ink)"] + 1;
                    }
                    else if (fillPercentage > 0.95)
                    {
                        stats["Solid Blocks (> 95% ink)"] = (int)stats["Solid Blocks (> 95% ink)"] + 1;
                    }
                    else if (box.Width <= 1 || box.Height <= 1)
                    {
                        stats["Single-Pixel Lines"] = (int)stats["Single-Pixel Lines"] + 1;
                    }
                    else if (fillPercentage < 0.05 && ((double)box.Width / box.Height < 0.1 || (double)box.Width / box.Height > 10))
                    {
                        stats["Sparse Noise"] = (int)stats["Sparse Noise"] + 1;
                    }
                    else
                    {
                        stats["Final Accepted"] = (int)stats["Final Accepted"] + 1;
                    }
                }
            }

            return stats;
        }
    }
}
