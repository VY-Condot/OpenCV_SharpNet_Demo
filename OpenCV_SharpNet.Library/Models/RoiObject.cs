using CsplCam.Library.Enums;
using CsplCam.Library.Models;
using CsplCam.Library.Models.GS1_QC;
using CsplCam.Library.Services;
using OpenCvSharp;
using System.Collections.Generic;
using System.Diagnostics;
using ZXingCpp;

namespace CsplCam.Library.Models
{
    [DebuggerStepThrough]
    public class RoiObject
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Rect Box { get; set; }
        public RoiType Type { get; set; }
        // Add this new property
        public bool ShowOverlay { get; set; } = true; // Default to true (visible)

        // --- NEW TEMPORARY FLAG ---
        // This tracks if we touched this ROI in this session. 
        // It defaults to FALSE when app starts.
        public bool JustTrained { get; set; } = false;


        // --- Properties for Right Panel ---
        public string ExpectedText { get; set; } = string.Empty;
        public double Threshold { get; set; } = 0.50;
        public double RoiScore { get; set; } = 0.00;

        public int MinBlobW { get; set; } = 8;
        public int MinBlobH { get; set; } = 18;
        public int MaxBlobW { get; set; } = 250;
        public int MaxBlobH { get; set; } = 250;

        public bool IsAnchor { get; set; } = false;
        public bool IsUseEdgeMatching { get; set; } = false;

        public bool IsHighPrecisionMode { get; set; } = true;

        //=========================================================================================================
        // NEW ADDITION : ADD THIS PROPERTY FOR DEFINE ROTATION ANGLE OF IMAGE IN DEGREES SO OCR GIVE PROPER RESULT
        // ADDITION DATE : 05-02-2026
        //=========================================================================================================
        public RotationAngles RotationAngle { get; set; } = RotationAngles.Zero;

        //=========================================================================================================
        // NEW ADDITION : ADD THIS PROPERTY FLAG FOR KEEP TRACKING THE NAME CHANGES OF ROI
        // ADDITION DATE : 09-02-2026
        //=========================================================================================================
        public bool IsNameChanged { get; set; } = false;

        //=========================================================================================================
        // NEW ADDITION : ADD THIS PROPERTY for DISPLAYING OVER ALL RESULT AT ONCE
        // ADDITION DATE : 04-04-2026
        //=========================================================================================================
        public string? OverAllResult { get; set; } = string.Empty;

        //=========================================================================================================
        // NEW ADDITION : ADD THIS PROPERTY FOR STORING AND REJECTING ROI RESULT BASED ON THRESHOLD VALUE
        // ADDITION DATE : 10-02-2026
        //=========================================================================================================
        //public double OverallThreshold { get; set; } = 0.80;

        //=========================================================================================================
        // NEW ADDITION : ADD THIS PROPERTY FOR CLONING THE ROI OBJECT 
        // ADDITION DATE : 11-02-2026
        //=========================================================================================================
        public RoiObject Clone
        {
            get
            {
                return GetClone();
            }
        }

        //=========================================================================================================
        // NEW ADDITION : ADD THIS PROPERTY FOR ENABLE MULTIPLE SELECTION
        // ADDITION DATE : 12-02-2026
        //=========================================================================================================
        [System.Text.Json.Serialization.JsonIgnore]
        public bool IsSelected { get; set; } = false;

        //=========================================================================================================
        // NEW ADDITION : ADD THIS PROPERTY TO STORE TIME OF DECODED TEXT
        // ADDITION DATE : 10-02-2026
        //=========================================================================================================
        public TimeSpan TimeTakenForDecoding { get; internal set; } = TimeSpan.Zero;


        //=========================================================================================================
        // NEW ADDITION : ADD THIS PROPERTYS FOR TEMPLATE MATCHING ROIS
        // ADDITION DATE : 13-02-2026
        //=========================================================================================================
        
        // --- TM (Template Matching) SPECIFIC PROPERTIES ---
        [System.Text.Json.Serialization.JsonIgnore]
        public Mat TmTemplate { get; set; } // The "Golden" image
        public bool  TmPass{ get; set; } // Pass/Fail result
        public double  TmScore{ get; set; }// The result score

        public double TmThreshold { get; set; } = 0.70; // 0.0 to 1.0 (70% match)

        //=========================================================================================================
        //NEW ADDITION : ADD THIS PROPERTYS FOR TEMPLATE MATCHING ROIS 
        //ADDITION DATE : 13-02-2026

        // THIS PROP DEFINE THE SEGMENTATION MODE FOR OCR PROCESSING IN TEXT ROIS. THIS ALLOWS USERS TO CHOOSE DIFFERENT PRE-PROCESSING STRATEGIES BASED ON THEIR SPECIFIC USE CASES, SUCH AS STANDARD TEXT, PUNCTUATION-SENSITIVE TEXT, NOISY BACKGROUNDS, HIGH-ACCURACY REQUIREMENTS, OR INDUSTRIAL APPLICATIONS LIKE DOT-PEEN OR INKJET PRINTING.
        //=========================================================================================================

        // Default to Balanced
        public SegmentationMode SegmentationMode { get; set; } = SegmentationMode.Balanced;

        //=========================================================================================================


        #region MORPHOLOGICAL OPERATIONS
        //=========================================================================================================
        //NEW ADDITION : ADD THIS PROPERTYS FOR HOLDING AND STORING THE MORPHOLOGICAL SETTINGS(MODE) FOR OCR PROCESSING IN TEXT ROIS
        //ADDITION DATE : 24-02-2026

        // THIS PROP DO MORPHOLOGICAL OPERATIONS LIKE EROSION AND DILATION ON THE BASIS OF KEREL SIZE AND ITERATION NUMBERS DEFINE BY THE USER. THIS ALLOWS USERS TO ENHANCE THE TEXT FEATURES IN THE ROI BEFORE OCR PROCESSING, WHICH CAN BE PARTICULARLY USEFUL FOR IMPROVING RECOGNITION ACCURACY IN CHALLENGING CONDITIONS SUCH AS NOISY BACKGROUNDS, LOW-CONTRAST TEXT, OR SPECIFIC FONT STYLES. BY ADJUSTING THE MORPHOLOGICAL SETTINGS, USERS CAN TAILOR THE PRE-PROCESSING TO BETTER SUIT THEIR PARTICULAR USE CASES AND ACHIEVE BETTER OCR RESULTS.


        // --- NEW: MORPHOLOGICAL SETTINGS ---
        public MorphOperation MorphOp { get; set; } = MorphOperation.None;

        public OpenCvSharp.MorphShapes MorphShape { get; set; } = OpenCvSharp.MorphShapes.Rect;

        // Default to 3x3 which is the industry standard baseline
        public int MorphKernelWidth { get; set; } = 3;
        public int MorphKernelHeight { get; set; } = 3;

        // How many times to apply the effect (1 is usually enough, 2 is aggressive)
        public int MorphIterations { get; set; } = 1;

        // Inside RoiObject.cs
        [System.Text.Json.Serialization.JsonIgnore] // Don't save this to JSON
        public System.Drawing.Bitmap? PreviewBitmap { get; set; }

        //=========================================================================================================
        #endregion

        // Anchor Settings ---
        public int AnchorTop { get; set; } = 800;
        public int AnchorBottom { get; set; } = 800;
        public int AnchorLeft { get; set; } = 800;
        public int AnchorRight { get; set; } = 800;
        
        ///// <summary>
        ///// set the min and max confidence of the anchor character (the character with the highest anchor confidence in the image).
        ///// </summary>
        //public struct AnchorConfidence
        //{
        //    public static double Min { get; set; } = 0.35;
        //    public static double Max { get; set; } = 0.50;
        //}

        // Visual pattern (Not saved to JSON)
        [System.Text.Json.Serialization.JsonIgnore]
        public Mat? AnchorTemplate { get; set; }

        // --- Results ---
        public string DecodedText { get; set; } = string.Empty;
        public List<CharResult> CharResults { get; set; } = [];

        //=========================================================================================================
        // NEW ADDITION : ADD THIS PROPERTY CHOOSE THE BARCODE TYPE FOR DECODING
        // ADDITION DATE : 04-03-2026
        //=========================================================================================================
        //public string BarCodeFormat { get; set; } = "Auto";
        public string BarCodeFormat { get; set; } = string.Empty;

        //=========================================================================================================
        // NEW ADDITION : ADD THIS PROPERTY CHOOSE THE BARCODE TYPE FOR DECODING
        // ADDITION DATE : 06-03-2026
        //=========================================================================================================
        public bool isBarCodeFormatAuto { get; set; } = true;


        //=========================================================================================================
        // NEW ADDITION : ADD THIS PROPERTY FOR GETTING GS1 BARCODE QC CHECK RESULT
        // ADDITION DATE : 30-03-2026
        //=========================================================================================================
        public GS1_QC_CheckResult? Gs1QcResult { get; set; }
        public bool IsRunGS1QcCheck { get; set; } = false;

        //==============================================================================================
        //ADD NEW PROPETY FOR REFERENCE AND ROI REFERENCE ID FOR FIND DYANMIC ROI
        //ADD ON : 08-04-2026
        //==============================================================================================
        public bool IsUseRoiReference { get; set; } = false;
        public int ReferenceRoiID { get; set; } = -1;

        //==============================================================================================
        //ADD NEW PROPETY FOR GETTING AND SHOWING THE ANGLE OF ROTATION
        //ADD ON : 09-04-2026
        //==============================================================================================
        public double SkewAngleOfRoi { get; set; } = 0.0;

        //====================================================================
        //NEW ENUM CLASS FOR CONTEXT MASKING IN OCR TO IMPROVE ACCURACY BY LIMITING CHARACTER SET
        //ADDED ON : 10-04-2026
        //====================================================================
        public OcrMaskType MaskType { get; set; } = OcrMaskType.Any;

        //====================================================================
        //new prop for enableing ai mode of application based on user need
        //ADDED ON : 23-04-2026
        //====================================================================
        public bool UseAiFallback { get; set; } = false;
        public bool UseSuperResolution { get; set; } = false;
        public bool UseBruteForceGridRecovery { get; set; } = false;

        //==============================================================================================
        //ADD NEW PROPETY FUNCTION FOR CLOING ROI OBJECT 
        //ADD ON : 11-02-2026
        //==============================================================================================
        private RoiObject GetClone()
        {
            return new RoiObject()
            {
                Id = this.Id,
                Name = this.Name,
                Box = this.Box,
                Type = this.Type,
                ShowOverlay = this.ShowOverlay,
                JustTrained = this.JustTrained,
                ExpectedText = this.ExpectedText,
                Threshold = this.Threshold,
                RoiScore = this.RoiScore,
                MinBlobW = this.MinBlobW,
                MinBlobH = this.MinBlobH,
                MaxBlobW = this.MaxBlobW,
                MaxBlobH = this.MaxBlobH,
                IsAnchor = this.IsAnchor,
                IsUseEdgeMatching = this.IsUseEdgeMatching,
                IsHighPrecisionMode = this.IsHighPrecisionMode,
                RotationAngle = this.RotationAngle,
                IsNameChanged = this.IsNameChanged,
                AnchorBottom = this.AnchorBottom,
                SegmentationMode = this.SegmentationMode,
                AnchorLeft = this.AnchorLeft,
                AnchorRight = this.AnchorRight,
                AnchorTop = this.AnchorTop,
                //OverallThreshold = this.OverallThreshold,
            };
        }
    }

    //public class CharResult
    //{
    //    public Rect Box { get; set; }
    //    public string? Text { get; set; }
    //    public double Score { get; set; }
    //    public bool IsGood { get; set; }

    //    // --- NEW PROPERTY ---
    //    public bool IsExpectedMatch { get; set; } // Does it match the expected character?
    //}

    //public class CharTemplate
    //{
    //    public Mat Vector { get; set; }      // The 30x30 visual shape
    //    public double AspectRatio { get; set; } // Width / Height
    //    public double FillDensity { get; set; } // How much ink? (0.0 to 1.0)
    //}
}