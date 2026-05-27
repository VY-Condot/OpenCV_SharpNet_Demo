using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsplCam.Library.Models.GS1_QC
{
    /// <summary>
    /// A single reusable class to hold configuration metadata and rules for ANY metric.
    /// </summary>
    public class GradingMetricConfig
    {
        public GradingSystems GradingSystem { get; init; }
        public bool IsEnabled { get; set; } = true; // Fixed spelling from IsEnbled

        public double MinValue { get; set; } = 0;
        public double MaxValue { get; set; } = 1;
        public double Increment { get; set; } = 0.01;

        //public List<GradingRange> GradingData { get; set; } = new();
        public List<GradingRange> GradingData;

        public GradingMetricConfig(GradingSystems system)   
        {
            GradingSystem = system;
            GradingData = GetGradingRanges(GradingSystem);
        }

        private List<GradingRange> GetGradingRanges(GradingSystems gradingSystems)
        {
            List<GradingRange> gradingRanges = new List<GradingRange>();

            switch (gradingSystems)
            {
                case GradingSystems.AxialNonuniformity:
                    gradingRanges = new List<GradingRange>
                    {
                        GradingRange.Create(Grades.A, 0.06, ComparisonOperators.LessThanEqualTo),
                        GradingRange.Create(Grades.B, 0.08, ComparisonOperators.LessThanEqualTo),
                        GradingRange.Create(Grades.C, 0.10, ComparisonOperators.LessThanEqualTo),
                        GradingRange.Create(Grades.D, 0.12, ComparisonOperators.LessThanEqualTo),
                        GradingRange.Create(Grades.F, 0.13, ComparisonOperators.GreaterThanEqualTo)
                    };
                    break;
                case GradingSystems.GridNonuniformity:
                    gradingRanges = new List<GradingRange>
                    {
                        GradingRange.Create(Grades.A, 0.38, ComparisonOperators.LessThanEqualTo),
                        GradingRange.Create(Grades.B, 0.50, ComparisonOperators.LessThanEqualTo),
                        GradingRange.Create(Grades.C, 0.63, ComparisonOperators.LessThanEqualTo),
                        GradingRange.Create(Grades.D, 0.75, ComparisonOperators.LessThanEqualTo),
                        GradingRange.Create(Grades.F, 0.76, ComparisonOperators.GreaterThanEqualTo)
                    };
                    break;
                case GradingSystems.UnusedErrorCorrection:
                    gradingRanges = new List<GradingRange>
                    {
                        GradingRange.Create(Grades.A, 2, ComparisonOperators.LessThanEqualTo),
                        GradingRange.Create(Grades.B, 2, ComparisonOperators.LessThanEqualTo),
                        GradingRange.Create(Grades.C, 2, ComparisonOperators.LessThanEqualTo),
                        GradingRange.Create(Grades.D, 2, ComparisonOperators.LessThanEqualTo),
                        GradingRange.Create(Grades.F, 3, ComparisonOperators.GreaterThanEqualTo)
                    };
                    break;
                case GradingSystems.FixedPatternDamage:
                    gradingRanges = new List<GradingRange>
                    {
                        GradingRange.Create(Grades.A, 0, ComparisonOperators.LessThanEqualTo),
                        GradingRange.Create(Grades.B, 1, ComparisonOperators.LessThanEqualTo),
                        GradingRange.Create(Grades.C, 2, ComparisonOperators.LessThanEqualTo),
                        GradingRange.Create(Grades.D, 3, ComparisonOperators.LessThanEqualTo),
                        GradingRange.Create(Grades.F, 4, ComparisonOperators.GreaterThanEqualTo)
                    };
                    break;
                case GradingSystems.Modulation:
                    gradingRanges = new List<GradingRange>
                    {
                        GradingRange.Create(Grades.A, 0.40, ComparisonOperators.GreaterThanEqualTo),
                        GradingRange.Create(Grades.B, 0.30, ComparisonOperators.GreaterThanEqualTo),
                        GradingRange.Create(Grades.C, 0.20, ComparisonOperators.GreaterThanEqualTo),
                        GradingRange.Create(Grades.D, 0.10, ComparisonOperators.GreaterThanEqualTo),
                        GradingRange.Create(Grades.F, 0.09, ComparisonOperators.LessThanEqualTo)
                    };
                    break;
                case GradingSystems.Decode:
                    gradingRanges = new List<GradingRange>
                    {
                        GradingRange.Create(Grades.A, 0, ComparisonOperators.LessThanEqualTo),
                        GradingRange.Create(Grades.B, 0, ComparisonOperators.LessThanEqualTo),
                        GradingRange.Create(Grades.C, 0, ComparisonOperators.LessThanEqualTo),
                        GradingRange.Create(Grades.D, 0, ComparisonOperators.LessThanEqualTo),
                        GradingRange.Create(Grades.F, 0, ComparisonOperators.GreaterThanEqualTo)
                    };
                    break;
                case GradingSystems.SymbolContrast:
                    gradingRanges = new List<GradingRange>
                    {
                        GradingRange.Create(Grades.A, 70, ComparisonOperators.GreaterThanEqualTo),
                        GradingRange.Create(Grades.B, 55, ComparisonOperators.GreaterThanEqualTo),
                        GradingRange.Create(Grades.C, 40, ComparisonOperators.GreaterThanEqualTo),
                        GradingRange.Create(Grades.D, 20, ComparisonOperators.GreaterThanEqualTo),
                        GradingRange.Create(Grades.F, 19, ComparisonOperators.LessThanEqualTo)
                    };
                    break;
                //case GradingSystems.PrintGrowth:
                //    gradingRanges = new List<GradingRange>
                //    {
                //        GradingRange.Create(Grades.A, 0.06, ComparisonOperators.LessThanEqualTo),
                //        GradingRange.Create(Grades.B, 0.08, ComparisonOperators.LessThanEqualTo),
                //        GradingRange.Create(Grades.C, 0.10, ComparisonOperators.LessThanEqualTo),
                //        GradingRange.Create(Grades.D, 0.12, ComparisonOperators.LessThanEqualTo),
                //        GradingRange.Create(Grades.F, 0.13, ComparisonOperators.GreaterThanEqualTo)
                //    };
                //    break;
                //case GradingSystems.AngleOfDistortion:
                //    gradingRanges = new List<GradingRange>
                //    {
                //        GradingRange.Create(Grades.A, 0.06, ComparisonOperators.LessThanEqualTo),
                //        GradingRange.Create(Grades.B, 0.08, ComparisonOperators.LessThanEqualTo),
                //        GradingRange.Create(Grades.C, 0.10, ComparisonOperators.LessThanEqualTo),
                //        GradingRange.Create(Grades.D, 0.12, ComparisonOperators.LessThanEqualTo),
                //        GradingRange.Create(Grades.F, 0.13, ComparisonOperators.GreaterThanEqualTo)
                //    };
                //    break;
                case GradingSystems.QuietZone:
                    gradingRanges = new List<GradingRange>
                    {
                        GradingRange.Create(Grades.A, 0.4, ComparisonOperators.GreaterThanEqualTo),
                        GradingRange.Create(Grades.B, 0.4, ComparisonOperators.GreaterThanEqualTo),
                        GradingRange.Create(Grades.C, 0.4, ComparisonOperators.GreaterThanEqualTo),
                        GradingRange.Create(Grades.D, 0.4, ComparisonOperators.GreaterThanEqualTo),
                        GradingRange.Create(Grades.F, 0.4, ComparisonOperators.LessThanEqualTo)
                    };
                    break;
                default:
                    gradingRanges = new List<GradingRange>
                    {
                        GradingRange.Create(Grades.A, 0, ComparisonOperators.LessThanEqualTo),
                        GradingRange.Create(Grades.B, 0, ComparisonOperators.LessThanEqualTo),
                        GradingRange.Create(Grades.C, 0, ComparisonOperators.LessThanEqualTo),
                        GradingRange.Create(Grades.D, 0, ComparisonOperators.LessThanEqualTo),
                        GradingRange.Create(Grades.F, 0, ComparisonOperators.LessThanEqualTo)
                    };
                    break;
            }
            return gradingRanges;
        }
    }
}
