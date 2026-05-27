using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsplCam.Library.Models.GS1_QC
{
    /// <summary>
    /// Your main user configuration model. Clean, structured, and easy to maintain!
    /// </summary>
    public class UserConfiguredGrading
    {
        public GradingMetricConfig AxialNonuniformity { get; set; } = new(GradingSystems.AxialNonuniformity) { MinValue = 0 , MaxValue = 1,Increment = 0.01 };
        public GradingMetricConfig GridNonuniformity { get; set; } = new(GradingSystems.GridNonuniformity) 
        { MinValue = 0, MaxValue = 1, Increment = 0.01 };
        public GradingMetricConfig UnusedErrorCorrection { get; set; } = new(GradingSystems.UnusedErrorCorrection) { MinValue = 0, MaxValue = 3, Increment = 0.01 };
        public GradingMetricConfig FixedPatternDamage { get; set; } = new(GradingSystems.FixedPatternDamage)
        { MinValue = 0, MaxValue = 5, Increment = 1 };
        public GradingMetricConfig Modulation { get; set; } = new(GradingSystems.Modulation)
        { MinValue = 0, MaxValue = 1, Increment = 0.01 };
        public GradingMetricConfig Decode { get; set; } = new(GradingSystems.Decode) 
        { MinValue = 0, MaxValue = 5, Increment = 1 };
        public GradingMetricConfig SymbolContrast { get; set; } = new(GradingSystems.SymbolContrast)
        { MinValue = 0, MaxValue = 100, Increment = 1 };
        //public GradingMetricConfig PrintGrowth { get; set; } = new(GradingSystems.PrintGrowth);
        //public GradingMetricConfig AngleOfDistortion { get; set; } = new(GradingSystems.AngleOfDistortion);
        public GradingMetricConfig QuietZone { get; set; } = new(GradingSystems.QuietZone) 
        { MinValue = 0, MaxValue = 1, Increment = 0.01 };

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
            //GradingSystems.PrintGrowth => PrintGrowth,
            //GradingSystems.AngleOfDistortion => AngleOfDistortion,
            GradingSystems.QuietZone => QuietZone,
            _ => throw new ArgumentOutOfRangeException(nameof(system))
        };
    }
}
