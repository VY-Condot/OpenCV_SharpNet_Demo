using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CsplCam.Library.Models.GS1_QC
{
    /// <summary>
    /// set min and Max for grading
    /// </summary>
    /// <param name="Min"></param>
    /// <param name="Max"></param>
    public readonly record struct GradingRange
    {
        //public GradingSystems GradingSystem { get; init; }
        public double GradeValue { get; init; }
        public Grades Grades { get; init; }

        //public bool IsEnbled { get; init; }

        public ComparisonOperators ComparisonOperators { get; init; }

        //creat private constructor
        private GradingRange(Grades grades,double gradeValue, ComparisonOperators operators)
        {
            Grades = grades;
            GradeValue = gradeValue;
            ComparisonOperators = operators;
        }

        //create function for create instance of class
        public static GradingRange Create(Grades grades, double gradeValue, ComparisonOperators operators)
        {
            return new GradingRange(grades, gradeValue, operators);
        }

        //check is value in in grading range or not 
        public bool IsWithinRange(double value,ComparisonOperators operators)
        {
            return operators switch
            {
                ComparisonOperators.LessThanEqualTo => value <= GradeValue,
                ComparisonOperators.GreaterThanEqualTo => value >= GradeValue,
                _ => false
            };
        }

        //safe factory for default or empty constructor
        public static GradingRange Empty => new GradingRange(Grades.F, 0, ComparisonOperators.LessThanEqualTo);
    }

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
        PrintGrowth,
        AngleOfDistortion,
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
        public GradingRange PrintGrowth { get; set; }
        public GradingRange AngleOfDistortion{ get; set; }
        public GradingRange QuietZone{ get; set; }
    }

    //public class UserConfigerdGrading
    //{
    //    //public List<GradingRange> AxialNonuniformity { get; set; } = new();
    //    //public List<GradingRange> GridNonuniformity { get; set; } = new();
    //    //public List<GradingRange> UnusedErrorCorrection { get; set; } = new();
    //    //public List<GradingRange> FixedPatternDamage { get; set; } = new();
    //    //public List<GradingRange> Modulation { get; set; } = new();
    //    //public List<GradingRange> Decode { get; set; } = new();
    //    //public List<GradingRange> SymbolContrast { get; set; } = new();
    //    //public List<GradingRange> PrintGrowth { get; set; } = new();
    //    //public List<GradingRange> AngleOfDistortion { get; set; } = new();
    //    //public List<GradingRange> QuietZone { get; set; } = new();

    //    public AxialNonuniformity AxialNonuniformity { get; set; }
    //}

    //public class AxialNonuniformity
    //{
    //    public GradingSystems GradingSystem { get; init; }

    //    public bool IsEnbled { get; init; }

    //    public List<GradingRange> GradingData { get; set; } = new();
    //}



    /// <summary>
    /// A single reusable class to hold configuration metadata and rules for ANY metric.
    /// </summary>
    public class GradingMetricConfig
    {
        public GradingSystems GradingSystem { get; init; }
        public bool IsEnabled { get; set; } = true; // Fixed spelling from IsEnbled
        public List<GradingRange> GradingData { get; set; } = new();

        public GradingMetricConfig(GradingSystems system)
        {
            GradingSystem = system;
        }
    }

    /// <summary>
    /// Your main user configuration model. Clean, structured, and easy to maintain!
    /// </summary>
    public class UserConfiguredGrading
    {
        public GradingMetricConfig AxialNonuniformity { get; set; } = new(GradingSystems.AxialNonuniformity);
        public GradingMetricConfig GridNonuniformity { get; set; } = new(GradingSystems.GridNonuniformity);
        public GradingMetricConfig UnusedErrorCorrection { get; set; } = new(GradingSystems.UnusedErrorCorrection);
        public GradingMetricConfig FixedPatternDamage { get; set; } = new(GradingSystems.FixedPatternDamage);
        public GradingMetricConfig Modulation { get; set; } = new(GradingSystems.Modulation);
        public GradingMetricConfig Decode { get; set; } = new(GradingSystems.Decode);
        public GradingMetricConfig SymbolContrast { get; set; } = new(GradingSystems.SymbolContrast);
        public GradingMetricConfig PrintGrowth { get; set; } = new(GradingSystems.PrintGrowth);
        public GradingMetricConfig AngleOfDistortion { get; set; } = new(GradingSystems.AngleOfDistortion);
        public GradingMetricConfig QuietZone { get; set; } = new(GradingSystems.QuietZone);

        /// <summary>
        /// Reusable helper to dynamically find the correct metric config by its enum name.
        /// </summary>
        public GradingMetricConfig GetMetric(GradingSystems system) => system switch
        {
            GradingSystems.AxialNonuniformity => AxialNonuniformity,
            GradingSystems.GridNonuniformity => GridNonuniformity,
            GradingSystems.UnusedErrorCorrection => UnusedErrorCorrection,
            GradingSystems.FixedPatternDamage => FixedPatternDamage,
            GradingSystems.Modulation => Modulation,
            GradingSystems.Decode => Decode,
            GradingSystems.SymbolContrast => SymbolContrast,
            GradingSystems.PrintGrowth => PrintGrowth,
            GradingSystems.AngleOfDistortion => AngleOfDistortion,
            GradingSystems.QuietZone => QuietZone,
            _ => throw new ArgumentOutOfRangeException(nameof(system))
        };
    }
}
