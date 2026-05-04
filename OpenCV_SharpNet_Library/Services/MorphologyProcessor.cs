using OpenCV_SharpNet.Enums;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenCV_SharpNet.Services
{
    [DebuggerStepThrough]
    public static class MorphologyProcessor
    {
        [ThreadStatic]
        private static Dictionary<int, Mat> _kernelCache;

        public static void Apply(Mat image, MorphOperation op, int kernelW, int kernelH, int iterations)
        {
            if (op == MorphOperation.None || iterations <= 0 || kernelW <= 0 || kernelH <= 0)
                return;

            if (_kernelCache == null) _kernelCache = new Dictionary<int, Mat>();

            // Unique key to cache different sizes (e.g., 3x3 becomes 30003)
            int key = (kernelW * 10000) + kernelH;

            if (!_kernelCache.TryGetValue(key, out Mat kernel))
            {
                //kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(kernelW, kernelH));

                // TO THIS:
                kernel = Cv2.GetStructuringElement(MorphShapes.Cross, new OpenCvSharp.Size(kernelW, kernelH));

                //FOLLOWED BY MORPH OPERATION
                //Cv2.MorphologyEx(image, image, MorphTypes.Close, kernel);

                _kernelCache[key] = kernel;
            }

            if (op == MorphOperation.Erode)
                Cv2.Erode(image, image, kernel, null, iterations);
            else if (op == MorphOperation.Dilate)
                Cv2.Dilate(image, image, kernel, null, iterations);
        }
    }
}