using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsplCam.Library.Models.GS1_QC
{
    #region public properties for user value for grading systems including all grades

    public enum Grades
    {
        A = 4,
        B = 3,
        C = 2,
        D = 1,
        F = 0
    }

    public enum ComparisonOperators
    {
        LessThanEqualTo,
        GreaterThanEqualTo
    }

    public enum GradingSystems
    {
        AxialNonuniformity,
        GridNonuniformity,
        UnusedErrorCorrection,
        FixedPatternDamage,
        Modulation,
        Decode,
        SymbolContrast,
        //PrintGrowth,
        //AngleOfDistortion,
        QuietZone
    }

    #endregion

    public class Gradings
    {
        public GradingRange AxialNonuniformity { get; set; }
        public GradingRange GridNonuniformity { get; set; }
        public GradingRange UnusedErrorCorrection { get; set; }
        public GradingRange FixedPatternDamage { get; set; }
        public GradingRange Modulation { get; set; }
        public GradingRange Decode { get; set; }
        public GradingRange SymbolContrast { get; set; }
        //public GradingRange PrintGrowth { get; set; }
        //public GradingRange AngleOfDistortion { get; set; }
        public GradingRange QuietZone { get; set; }
    }
}
