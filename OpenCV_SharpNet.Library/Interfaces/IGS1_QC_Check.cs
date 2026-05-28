using CsplCam.Library.Models;
using CsplCam.Library.Models.GS1_QC;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsplCam.Library.Interfaces
{
    public interface IGS1_QC_Check
    {
        //string EvaluateISO15415Quality(Mat rawGrayCrop, Rect boundingBox);
        //GS1_QC_CheckResult EvaluateISO15415Quality(Mat rawGrayCrop, Rect boundingBox);
        //void ApplyGS1QualityCheck(Mat rawGrayCrop, Rect boundingBox);

        // ADD the 'bool isDecoded' parameter here:

        //GS1_QC_CheckResult EvaluateISO15415Quality(Mat rawGrayCrop, Rect boundingBox, Point2f[] zxingCorners, bool isDecoded);
        GS1_QC_CheckResult EvaluateISO15415Quality(Mat rawGrayCrop, RoiObject roiObject, Rect boundingBox, Point2f[] zxingCorners, bool isDecoded);
    }
}
