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
        public GradingMetricConfig AxialNonuniformity { get; set; } = new(GradingSystems.AxialNonuniformity);
        public GradingMetricConfig GridNonuniformity { get; set; } = new(GradingSystems.GridNonuniformity);
        public GradingMetricConfig UnusedErrorCorrection { get; set; } = new(GradingSystems.UnusedErrorCorrection);
        public GradingMetricConfig FixedPatternDamage { get; set; } = new(GradingSystems.FixedPatternDamage);
        public GradingMetricConfig Modulation { get; set; } = new(GradingSystems.Modulation);
        public GradingMetricConfig Decode { get; set; } = new(GradingSystems.Decode);
        public GradingMetricConfig SymbolContrast { get; set; } = new(GradingSystems.SymbolContrast);
        //public GradingMetricConfig PrintGrowth { get; set; } = new(GradingSystems.PrintGrowth);
        //public GradingMetricConfig AngleOfDistortion { get; set; } = new(GradingSystems.AngleOfDistortion);
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
            //GradingSystems.PrintGrowth => PrintGrowth,
            //GradingSystems.AngleOfDistortion => AngleOfDistortion,
            GradingSystems.QuietZone => QuietZone,
            _ => throw new ArgumentOutOfRangeException(nameof(system))
        };
    }
}
