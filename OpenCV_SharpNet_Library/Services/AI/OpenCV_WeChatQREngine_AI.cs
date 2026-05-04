using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCV_SharpNet.Services.AI
{
    public class OpenCV_WeChatQREngine_AI
    {
        private static WeChatQRCode _detector;
        private static bool _isInitialized = false;

        // Call this ONCE in MainForm_Load
        public static void Initialize(string modelsFolderPath)
        {
            try
            {
                string detProto = Path.Combine(modelsFolderPath, "detect.prototxt");
                string detModel = Path.Combine(modelsFolderPath, "detect.caffemodel");
                string srProto = Path.Combine(modelsFolderPath, "sr.prototxt");
                string srModel = Path.Combine(modelsFolderPath, "sr.caffemodel");

                if (File.Exists(detProto) && File.Exists(detModel))
                {
                    _detector = WeChatQRCode.Create(detProto, detModel, srProto, srModel);
                    _isInitialized = true;
                    System.Diagnostics.Debug.WriteLine("AI WeChat QR Engine Initialized.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("AI WeChat QR Init Failed: " + ex.Message);
            }
        }

        // The Fallback Method
        public static List<(string Text, Point2f[] Corners)> DetectAndDecode(Mat inputImage)
        {
            var results = new List<(string Text, Point2f[] Corners)>();
            if (!_isInitialized || inputImage.Empty()) return results;

            try
            {
                // The CNN does detection, super-resolution, and decoding internally
                Mat[] points;
                _detector.DetectAndDecode(inputImage, out points, out string[] decodedTexts);

                for (int i = 0; i < decodedTexts.Length; i++)
                {
                    if (points.Length > i && points[i] != null && !points[i].Empty())
                    {
                        // Extract the 4 corners from the CNN's output matrix
                        Point2f[] corners = new Point2f[4];
                        for (int p = 0; p < 4; p++)
                        {
                            corners[p] = new Point2f(points[i].At<float>(p, 0), points[i].At<float>(p, 1));
                        }

                        results.Add((decodedTexts[i], corners));
                        points[i].Dispose();
                    }
                }
            }
            catch { }

            return results;
        }
    }
}
