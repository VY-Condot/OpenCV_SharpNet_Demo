using System.Diagnostics;

namespace OpenCV_SharpNet.Models.GS1_QC
{
    [DebuggerStepThrough]
    public record GS1_QC_CheckResult
    (
        string BarcodeType = null,
        string OverAll = null,
        string Decode = null,
        string SC = null,
        string MOD = null,

        // 2D & DATAMATRIX (ISO 15415)
        string AN = null,
        string GN = null,
        string FPD = null,
        string UEC = null,
        string PG = null,

        // AS 9132 & AIM DPM (Aerospace / DPM Standards)
        string AS9132_Distortion = null,
        string AS9132_QuietZone = null,
        string AS9132_Elongation = null,
        string DPM_Rmin = null,

        // 1D LINEAR SPECIFIC (ISO 15416)
        string MinReflectance = null,
        string MinEdgeContrast = null,
        string Defects = null,
        string Decodability = null
    );
}
