using OpenCvSharp;
using OpenCvSharp.DnnSuperres;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCV_SharpNet.Services.AI
{
    [DebuggerStepThrough]
    public class SuperResolutionEngine_AI
    {
        private static DnnSuperResImpl _srEngine;
        private static bool _isInitialized = false;

        // Call this ONCE in MainForm_Load
        public static void Initialize(string modelFilePath)
        {
            try
            {
                if (File.Exists(modelFilePath))
                {
                    _srEngine = new DnnSuperResImpl();
                    _srEngine.ReadModel(modelFilePath);

                    // "espcn" is fast (10ms). "edsr" is highly accurate but slow (100ms).
                    // The '2' means 2x upscale. Must match the downloaded model (e.g. ESPCN_x2.pb)
                    _srEngine.SetModel("espcn", 2);

                    _isInitialized = true;
                    System.Diagnostics.Debug.WriteLine("AI Super Resolution Engine Initialized.");
                }
            }
            catch { }
        }

        // The Fallback Method
        public static Mat Upscale(Mat inputImage)
        {
            if (!_isInitialized || inputImage.Empty()) return inputImage.Clone();

            Mat upscaled = new Mat();
            try
            {
                _srEngine.Upsample(inputImage, upscaled);
            }
            catch
            {
                inputImage.CopyTo(upscaled); // Return original if AI fails
            }
            return upscaled;
        }
    }
}
