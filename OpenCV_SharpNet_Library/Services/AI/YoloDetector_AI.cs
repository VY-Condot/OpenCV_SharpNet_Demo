using OpenCvSharp;
using OpenCvSharp.Dnn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Size = OpenCvSharp.Size;

namespace OpenCV_SharpNet.Services.AI
{
    public class YoloDetector_AI
    {
        private static Net _yoloNet;
        private static bool _isInitialized = false;

        // Call this ONCE in MainForm_Load
        public static void Initialize(string onnxModelPath)
        {
            try
            {
                if (File.Exists(onnxModelPath))
                {
                    _yoloNet = CvDnn.ReadNetFromOnnx(onnxModelPath);

                    // =================================================================
                    // THE FIX: Use the global OpenCvSharp.Dnn enums
                    // =================================================================

                    // Standard CPU approach (Safe, works on every computer):
                    _yoloNet.SetPreferableBackend(Backend.OPENCV);
                    _yoloNet.SetPreferableTarget(Target.CPU);

                    /*
                    // OpenVINO approach (If you have OpenVINO binaries installed for Intel CPUs)
                    _yoloNet.SetPreferableBackend(OpenCvSharp.Dnn.Backend.INFERENCE_ENGINE);
                    _yoloNet.SetPreferableTarget(OpenCvSharp.Dnn.Target.CPU); 
                    */

                    _isInitialized = true;
                }
            }
            catch { }
        }

        public static List<Rect> DetectBarcodes(Mat frame, float confThreshold = 0.5f)
        {
            var detectedBoxes = new List<Rect>();
            if (!_isInitialized || frame.Empty()) return detectedBoxes;

            // 1. Prepare the image for YOLOv8 (640x640 is standard)
            using Mat blob = CvDnn.BlobFromImage(frame, 1 / 255.0, new Size(640, 640), new Scalar(0, 0, 0), true, false);
            _yoloNet.SetInput(blob);

            // 2. Run Forward Pass (This takes ~50ms - 150ms on a CPU)
            using Mat output = _yoloNet.Forward();

            // 3. Parse the YOLOv8 Tensor [1, 84, 8400]
            using Mat reshaped = output.Reshape(1, output.Size(1)); // [84, 8400]
            using Mat transposed = reshaped.T(); // [8400, 84]

            float scaleX = frame.Width / 640f;
            float scaleY = frame.Height / 640f;

            for (int i = 0; i < transposed.Rows; i++)
            {
                float confidence = transposed.At<float>(i, 4); // Get Object Score
                if (confidence > confThreshold)
                {
                    float cx = transposed.At<float>(i, 0);
                    float cy = transposed.At<float>(i, 1);
                    float w = transposed.At<float>(i, 2);
                    float h = transposed.At<float>(i, 3);

                    // Map back to original image size
                    int left = (int)((cx - w / 2) * scaleX);
                    int top = (int)((cy - h / 2) * scaleY);
                    int width = (int)(w * scaleX);
                    int height = (int)(h * scaleY);

                    detectedBoxes.Add(new Rect(left, top, width, height));
                }
            }

            // Note: In production, apply Non-Maximum Suppression (NMS) here to remove duplicate overlapping boxes.
            return detectedBoxes;
        }
    }
}
