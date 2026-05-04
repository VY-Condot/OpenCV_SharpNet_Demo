using OpenCV_SharpNet.Enums;
using OpenCV_SharpNet.Models;
using OpenCV_SharpNet.Services;
using OpenCvSharp;
using System.Text.Json.Serialization;

namespace OpenCV_SharpNet.Models
{
    public class RoiDataTransfer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Flatten Rect into 4 integers
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }

        public string Type { get; set; } // Save Enum as String
        public bool ShowOverlay { get; set; }

        // Settings
        public string ExpectedText { get; set; }
        public double Threshold { get; set; }
        public int MinBlobW { get; set; }
        public int MinBlobH { get; set; }
        public int MaxBlobW { get; set; }
        public int MaxBlobH { get; set; }

        // Anchor Config
        public bool IsAnchor { get; set; }
        public bool IsUseEdgeMatching { get; set; }
        public int AnchorTop { get; set; }
        public int AnchorBottom { get; set; }
        public int AnchorLeft { get; set; }
        public int AnchorRight { get; set; }

        // --- MAGIC PART: Save Image as Byte Array ---
        public byte[]? AnchorPatternData { get; set; }

        //=========================================================================================================
        // NEW ADDITION : ADD THIS PROPERTY FOR DEFINE ROTATION ANGLE OF IMAGE IN DEGREES SO OCR GIVE PROPER RESULT
        // ADDITION DATE : 05-02-2026
        //=========================================================================================================
        public RotationAngles RotationAngle { get; set; }

        //=========================================================================================================
        // NEW ADDITION : ADD THIS PROPERTY FLAG FOR KEEP TRACKING THE NAME CHANGES OF ROI
        // ADDITION DATE : 09-02-2026
        //=========================================================================================================
        public bool IsNameChanged { get; set; }

        //=========================================================================================================
        // NEW ADDITION : ADD THIS PROPERTY FOR STORING AND REJECTING ROI RESULT BASED ON THRESHOLD VALUE
        // ADDITION DATE : 10-02-2026
        //=========================================================================================================
        public double OverallThreshold { get; set; }

        //=========================================================================================================
        // NEW ADDITION : ADD THIS PROPERTY CHOOSE THE BARCODE TYPE FOR DECODING
        // ADDITION DATE : 04-03-2026
        //=========================================================================================================
        //public string BarCodeFormat { get; set; } = "Auto";
        public string BarCodeFormat { get; set; }

        // --- NEW: MORPHOLOGICAL SETTINGS ---
        public MorphOperation MorphOp { get; set; }

        // Default to 3x3 which is the industry standard baseline
        public int MorphKernelWidth { get; set; }
        public int MorphKernelHeight { get; set; }

        // How many times to apply the effect (1 is usually enough, 2 is aggressive)
        public int MorphIterations { get; set; }

        // Default to Balanced
        public SegmentationMode SegmentationMode { get; set; } = SegmentationMode.Balanced;

        //==============================================================================================
        //ADD NEW PROPETY FOR REFERENCE AND ROI REFERENCE ID FOR FIND DYANMIC ROI
        //ADD ON : 08-04-2026
        //==============================================================================================
        public bool IsUseRoiReference { get; set; } = false;
        public int ReferenceRoiID { get; set; } = -1;
    }
}