
namespace CsplCam.Library.Enums
{

    //====================================================================
    //NEW ENUM CLASS FOR ROTATION
    //====================================================================
    public enum RotationAngles
    {
        Zero = 0,
        Ninety = 90,
        OneEighty = 180,
        TwoSeventy = 270
    }

    //====================================================================
    //NEW ENUM CLASS FOR DisplayInfo
    //====================================================================
    public enum DisplayInfo
    {
        TextROISelection,
        BarcodeROISelection,
        TimeTaken,
        AllSelectionMode,
        SingleSelectionMode,
        ImageCounting,
        CurrentImageIndex,
        ImagePanning,
        ROISave,
        TemplateMatching,
        LoadROI,
        Default
    }

    //====================================================================
    //NEW ENUM CLASS FOR DEFINING ROI TYPES
    //====================================================================
    public enum RoiType { Text, Barcode, TemplateMatch }

    //====================================================================
    //NEW ENUM CLASS FOR DEFINING MOUSE POINTER MODES
    //====================================================================
    public enum MouseMode { None, PanningImage, MovingRoi, ResizingRoi }

    //====================================================================
    //NEW ENUM CLASS FOR DEFINING RESIZE HANDLER POSITIONS
    //====================================================================
    public enum ResizeHandle { None, TopLeft, TopRight, BottomLeft, BottomRight, Top, Bottom, Left, Right }

    //====================================================================
    //NEW ENUM CLASS FOR DEFINING SEGMENTATION MODES OR OCR MODES 
    //====================================================================
    public enum SegmentationMode
    {
        Balanced,       // Best for standard text
        Punctuation,    // Ultra-sensitive (keeps dots/commas)
        NoiseHeavy,     // Strong smoothing for dirty backgrounds
        HighAccuracy,   // For very clean, high-res text
        Industrial,      // For Dot-Peen or Inkjet (DOD/CIJ)
        //// --- NEW MODES ---
        //RawAdaptive, // Pure math, no blur (Best for clean text)
        //RawOtsu      // Pure contrast, no blur (Best for metal/shiny)
    }

    //====================================================================
    //NEW ENUM CLASS FOR DEFINING Morphological operations ON IMAGE
    //ADDED ON : 24-02-2026
    //====================================================================
    public enum MorphOperation
    {
        None = 0,
        Erode = 1,   // Thins text (removes noise)
        Dilate = 2   // Thickens text (connects dots)
    }


    //====================================================================
    //NEW ENUM CLASS FOR CONTEXT MASKING IN OCR TO IMPROVE ACCURACY BY LIMITING CHARACTER SET
    //ADDED ON : 10-04-2026
    //====================================================================
    [Flags]
    public enum OcrMaskType
    {
        Numeric = 1,           // 0-9
        AlphaUppercase = 2,    // A-Z
        AlphaLowercase = 4,    // a-z
        Punctuation = 8,       // - . , : / etc.
        Alpha = AlphaUppercase | AlphaLowercase,
        AlphaNumeric = Numeric | AlphaUppercase | AlphaLowercase,
        Any = Numeric | AlphaUppercase | AlphaLowercase | Punctuation
    }
}

