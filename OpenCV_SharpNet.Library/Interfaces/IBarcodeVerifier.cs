using CsplCam.Library.Models.GS1_QC;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsplCam.Library.Interfaces
{
    public interface IBarcodeVerifier
    {
        GS1_QC_CheckResult EvaluateQuality(Mat rawGrayCrop, Rect boundingBox, bool isDecoded, string barcodeFormat);
    }
}
