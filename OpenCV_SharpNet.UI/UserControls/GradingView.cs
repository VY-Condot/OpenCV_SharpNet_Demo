using CsplCam.Library.Interfaces;
using CsplCam.Library.Models;
using CsplCam.Library.Models.GS1_QC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenCV_SharpNet.UI.UserControls
{
    public partial class GradingView : UserControl, IGradingSystem
    {
        public GradingView(RoiObject roiObject)
        {
            InitializeComponent();

            // =================================================================
            // OPTIMIZATION: ENABLE DEEP DOUBLE BUFFERING TO KILL FLICKERING
            // =================================================================
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            UpdateStyles();

            Roi = roiObject;

            //binding mapping 
            numPadGradeMaxValueA.Tag = "A";
            numPadGradeMaxValueB.Tag = "B";
            numPadGradeMaxValueC.Tag = "C";
            numPadGradeMaxValueD.Tag = "D";
            numPadGradeMaxValueF.Tag = "F";
        }

        private RoiObject Roi { get; set; }
        private bool _isBinding = false;
        public GradingSystems GradingSystems { get; set; }

        private void SettingNumericPad(GradingMetricConfig activeMetric)
        {
            NumericUpDown[] numPadArray = { numPadGradeMaxValueA, numPadGradeMaxValueB, numPadGradeMaxValueC, numPadGradeMaxValueD, numPadGradeMaxValueF };

            foreach (var item in numPadArray)
            {
                SetDataToNumericPad(item, activeMetric);
            }
        }

        private void SetDataToNumericPad(NumericUpDown numericUpDown, GradingMetricConfig activeMetric)
        {
            numericUpDown.Minimum = (decimal)activeMetric.MinValue;
            numericUpDown.Maximum = (decimal)activeMetric.MaxValue;
            numericUpDown.Increment = (decimal)activeMetric.Increment;
        }

        private GradingMetricConfig GetGradingMetricConfig(UserConfiguredGrading userConfiguredGrading)
        {
            if (userConfiguredGrading is null)
                throw new ArgumentNullException(nameof(userConfiguredGrading), "User Configured Grading is null");

            return userConfiguredGrading.GetMetric(GradingSystems);
        }

        public void BindData()
        {
            _isBinding = true;

            try
            {
                // 1. Get the current active metric configuration
                //var activeMetric = Roi.UserConfiguredGrading.GetMetric(GradingSystems);

                var activeMetric = GetGradingMetricConfig(Roi.UserConfiguredGrading);

                //setting numeric pad values
                //SettingNumericPad(activeMetric);

                // 2. Set the UI label header
                lblGradeTypeName.Text = activeMetric.GradingSystem.ToString();

                chkInCludeGrade.Checked = activeMetric.IsEnabled;

                // 3. Optional: Bind your IsEnabled property to a master UI Checkbox if you have one
                // chkMetricEnabled.Checked = activeMetric.IsEnabled;

                // 4. Extract individual grade values from the underlying data list using LINQ
                numPadGradeMaxValueA.Value = (decimal)activeMetric.GradingData.FirstOrDefault(r => r.Grades == Grades.A).GradeValue;
                numPadGradeMaxValueB.Value = (decimal)activeMetric.GradingData.FirstOrDefault(r => r.Grades == Grades.B).GradeValue;
                numPadGradeMaxValueC.Value = (decimal)activeMetric.GradingData.FirstOrDefault(r => r.Grades == Grades.C).GradeValue;
                numPadGradeMaxValueD.Value = (decimal)activeMetric.GradingData.FirstOrDefault(r => r.Grades == Grades.D).GradeValue;
                numPadGradeMaxValueF.Value = (decimal)activeMetric.GradingData.FirstOrDefault(r => r.Grades == Grades.F).GradeValue;

                cmbGradeA.SelectedItem = activeMetric.GradingData.FirstOrDefault(r => r.Grades == Grades.A).ComparisonOperators;
                cmbGradeB.SelectedItem = activeMetric.GradingData.FirstOrDefault(r => r.Grades == Grades.B).ComparisonOperators;
                cmbGradeC.SelectedItem = activeMetric.GradingData.FirstOrDefault(r => r.Grades == Grades.C).ComparisonOperators;
                cmbGradeD.SelectedItem = activeMetric.GradingData.FirstOrDefault(r => r.Grades == Grades.D).ComparisonOperators;
                cmbGradeF.SelectedItem = activeMetric.GradingData.FirstOrDefault(r => r.Grades == Grades.F).ComparisonOperators;


                //SetNum(numPadGradeMaxValueA, (int)activeMetric.GradingData.FirstOrDefault(r => r.Grades == Grades.A).GradeValue);
                //SetNum(numPadGradeMaxValueA, (int)activeMetric.GradingData.FirstOrDefault(r => r.Grades == Grades.A).GradeValue);
                //SetNum(numPadGradeMaxValueA, (int)activeMetric.GradingData.FirstOrDefault(r => r.Grades == Grades.A).GradeValue);
                //SetNum(numPadGradeMaxValueA, (int)activeMetric.GradingData.FirstOrDefault(r => r.Grades == Grades.A).GradeValue);
            }
            finally
            {
                _isBinding = false;
            }

            // Initial display
            UpdateDynamicDescriptions();
        }

        private void SetNum(NumericUpDown num, int val)
        {
            if (!num.Focused)
            {
                int safeVal = Math.Max((int)num.Minimum, Math.Min((int)num.Maximum, val));
                if (num.Value != safeVal) num.Value = safeVal;
            }
        }

        private void BtnSaveGradingSetting_Click(object sender, EventArgs e)
        {
            // 1. Get the current active metric configuration
            var activeMetric = Roi.UserConfiguredGrading.GetMetric(GradingSystems);

            // 2. Map the activation status
            activeMetric.IsEnabled = chkInCludeGrade.Checked;

            // 3. Determine operator dynamically (Lower-is-better metrics use LessThanEqualTo)
            //ComparisonOperators currentOp = DetermineOperatorForSystem(GradingSystems);

            //ComparisonOperators currentOp = ComparisonOperators(cm);

            // 4. Clean out the old rules
            activeMetric.GradingData.Clear();

            // 5. Build and add the brand new immutable validated structs directly using the factory
            //activeMetric.GradingData.Add(GradingRange.Create(Grades.A, (double)numPadGradeMaxValueA.Value, currentOp));
            //activeMetric.GradingData.Add(GradingRange.Create(Grades.B, (double)numPadGradeMaxValueB.Value, currentOp));
            //activeMetric.GradingData.Add(GradingRange.Create(Grades.C, (double)numPadGradeMaxValueC.Value, currentOp));
            //activeMetric.GradingData.Add(GradingRange.Create(Grades.D, (double)numPadGradeMaxValueD.Value, currentOp));
            //activeMetric.GradingData.Add(GradingRange.Create(Grades.F, (double)numPadGradeMaxValueF.Value, currentOp));

            activeMetric.GradingData.Add(GradingRange.Create(Grades.A, (double)numPadGradeMaxValueA.Value, (ComparisonOperators)cmbGradeA.SelectedItem));
            activeMetric.GradingData.Add(GradingRange.Create(Grades.B, (double)numPadGradeMaxValueB.Value, (ComparisonOperators)cmbGradeB.SelectedItem));
            activeMetric.GradingData.Add(GradingRange.Create(Grades.C, (double)numPadGradeMaxValueC.Value, (ComparisonOperators)cmbGradeC.SelectedItem));
            activeMetric.GradingData.Add(GradingRange.Create(Grades.D, (double)numPadGradeMaxValueD.Value, (ComparisonOperators)cmbGradeD.SelectedItem));
            activeMetric.GradingData.Add(GradingRange.Create(Grades.F, (double)numPadGradeMaxValueF.Value, (ComparisonOperators)cmbGradeF.SelectedItem));

            MessageBox.Show($"{activeMetric.GradingSystem} thresholds updated successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Helper to automatically assign the proper ISO logic threshold based on standard
        /// </summary>
        private ComparisonOperators DetermineOperatorForSystem(GradingSystems system)
        {
            return system switch
            {
                GradingSystems.SymbolContrast or
                GradingSystems.Modulation or
                GradingSystems.UnusedErrorCorrection or
                GradingSystems.Decode or
                GradingSystems.FixedPatternDamage => ComparisonOperators.GreaterThanEqualTo,

                _ => ComparisonOperators.LessThanEqualTo
            };
        }

        private void GradingView_Load(object sender, EventArgs e)
        {
            var activeMetric = GetGradingMetricConfig(Roi.UserConfiguredGrading);

            //setting numeric pad values
            SettingNumericPad(activeMetric);

            //binding operation data to combobox
            ComboBox[] cmbBoxs = { cmbGradeA, cmbGradeB, cmbGradeC, cmbGradeD, cmbGradeF };

            foreach (var comboBox in cmbBoxs) comboBox.DataSource = Enum.GetValues(typeof(ComparisonOperators));

            //errInfoProvider.SetIconPadding(lblMaxValue, 5);
            //errInfoProvider.SetIconPadding(lblOptions, 5);

            //// ===================================================================
            //// GENERATE DESCRIPTIVE TOOLTIPS FOR GRADE THRESHOLDS
            //// ===================================================================
            //string maxValueDescription = GetGradeThresholdDescription(GradingSystems, "A");
            //string operatorDescription = GetGradeOperatorDescription(GradingSystems);

            //errInfoProvider.SetError(lblMaxValue, maxValueDescription);
            //errInfoProvider.SetError(lblOptions, operatorDescription);

            // Initial display
            UpdateDynamicDescriptions();
        }




        ///// <summary>
        ///// Generates a human-readable description for grade thresholds based on the grading system.
        ///// Explains what each grade letter (A-F) means for this specific metric.
        ///// </summary>
        //private string GetGradeThresholdDescription(GradingSystems system, string selectedGrade = "A")
        //{
        //    return system switch
        //    {
        //        GradingSystems.SymbolContrast =>
        //            "Symbol Contrast (SC) - Image brightness difference\n" +
        //            "A: ≥70% | B: ≥55% | C: ≥40% | D: ≥20% | F: <20%",

        //        GradingSystems.Modulation =>
        //            "Modulation (MOD) - Cell-to-cell reflectance variation\n" +
        //            "A: ≥0.40 | B: ≥0.30 | C: ≥0.20 | D: ≥0.10 | F: <0.10",

        //        GradingSystems.AxialNonuniformity =>
        //            "Axial Non-Uniformity (AN) - Width/height aspect ratio deviation\n" +
        //            "A: ≤0.06 (6%) | B: ≤0.08 (8%) | C: ≤0.10 (10%) | D: ≤0.12 (12%) | F: >12%",

        //        GradingSystems.GridNonuniformity =>
        //            "Grid Non-Uniformity (GN) - Cell size consistency across matrix\n" +
        //            "A: ≤0.38 cells | B: ≤0.50 | C: ≤0.63 | D: ≤0.75 | F: >0.75 cells",

        //        GradingSystems.FixedPatternDamage =>
        //            "Fixed Pattern Damage (FPD) - Finder pattern integrity\n" +
        //            "A: 0 errors | B: ≤1 error | C: ≤2 errors | D: ≤3 errors | F: >3 errors",

        //        GradingSystems.UnusedErrorCorrection =>
        //            "Unused Error Correction (UEC) - Visual errors in data area\n" +
        //            "A: ≤2 errors | B: ≤4 errors | C: ≤6 errors | D: ≤8 errors | F: >8 errors",

        //        GradingSystems.Decode =>
        //            "Decodability - Successfully decoded barcode\n" +
        //            "A: Successfully decoded | F: Failed to decode",

        //        GradingSystems.QuietZone =>
        //            "Quiet Zone (QZ) - Clear space around barcode margins\n" +
        //            "A: ≥10 modules | B: ≥8 | C: ≥6 | D: ≥4 | F: <4 modules",

        //        _ => "Grade thresholds configuration"
        //    };
        //}

        ///// <summary>
        ///// Generates description for comparison operators (>= vs <=) based on metric type.
        ///// Explains whether higher or lower values indicate better quality.
        ///// </summary>
        //private string GetGradeOperatorDescription(GradingSystems system)
        //{
        //    bool isHigherBetter = IsHigherBetterMetric(system);

        //    string operatorInfo = isHigherBetter
        //        ? "Higher is Better (≥): A starts at highest threshold, grades descend"
        //        : "Lower is Better (≤): A starts at lowest threshold, grades ascend";

        //    string systemInfo = system switch
        //    {
        //        GradingSystems.SymbolContrast =>
        //            "Higher contrast = clearer distinction between bars and spaces",

        //        GradingSystems.Modulation =>
        //            "Higher modulation = better cell reflectance difference",

        //        GradingSystems.AxialNonuniformity =>
        //            "Lower deviation = more uniform barcode shape",

        //        GradingSystems.GridNonuniformity =>
        //            "Lower deviation = consistent cell sizing",

        //        GradingSystems.FixedPatternDamage =>
        //            "Fewer errors = better finder pattern integrity",

        //        GradingSystems.UnusedErrorCorrection =>
        //            "Fewer visual errors = cleaner data area",

        //        GradingSystems.Decode =>
        //            "Successfully decoded barcode is required for grade A",

        //        GradingSystems.QuietZone =>
        //            "Larger quiet zone = better barcode isolation",

        //        _ => "Quality metric configuration"
        //    };

        //    return $"{operatorInfo}\n\n{systemInfo}";
        //}

        ///// <summary>
        ///// Determines if higher values are better for a given metric.
        ///// Returns true for metrics where higher = better quality (SC, MOD, etc.)
        ///// Returns false for metrics where lower = better quality (AN, GN, etc.)
        ///// </summary>
        //private bool IsHigherBetterMetric(GradingSystems system)
        //{
        //    return system switch
        //    {
        //        GradingSystems.SymbolContrast or
        //        GradingSystems.Modulation or
        //        GradingSystems.UnusedErrorCorrection or
        //        GradingSystems.Decode or
        //        GradingSystems.FixedPatternDamage or
        //        GradingSystems.QuietZone => true,

        //        GradingSystems.AxialNonuniformity or
        //        GradingSystems.GridNonuniformity => false,

        //        _ => true
        //    };
        //}






        /// <summary>
        /// Updates error provider descriptions dynamically based on current numeric pad values and operator selections.
        /// This method is called whenever any numeric value or operator changes.
        /// </summary>
        private void UpdateDynamicDescriptions()
        {
            if (_isBinding) return; // Skip during data binding operations

            string maxValueDescription = GenerateDynamicThresholdDescription();
            string operatorDescription = GenerateDynamicOperatorDescription();

            errInfoProvider.SetError(lblMaxValue, maxValueDescription);
            errInfoProvider.SetError(lblOptions, operatorDescription);
        }

        /// <summary>
        /// Generates dynamic grade threshold description based on actual numeric pad values.
        /// Shows the user-configured thresholds for A, B, C, D, F grades.
        /// </summary>
        private string GenerateDynamicThresholdDescription()
        {
            decimal valueA = numPadGradeMaxValueA.Value;
            decimal valueB = numPadGradeMaxValueB.Value;
            decimal valueC = numPadGradeMaxValueC.Value;
            decimal valueD = numPadGradeMaxValueD.Value;
            decimal valueF = numPadGradeMaxValueF.Value;

            string metricName = GetMetricDisplayName(GradingSystems);
            string metricUnit = GetMetricUnit(GradingSystems);
            string unit = string.IsNullOrEmpty(metricUnit) ? "" : $" {metricUnit}";

            string thresholdLine = $"A: {valueA}{unit} | B: {valueB}{unit} | C: {valueC}{unit} | D: {valueD}{unit} | F: {valueF}{unit}";

            string baseDescription = GetGradeThresholdDescription(GradingSystems);

            return $"{baseDescription}\n\n📊 Current Configuration:\n{thresholdLine}";
        }

        /// <summary>
        /// Generates dynamic operator description based on actual operator selections per grade.
        /// Shows how each grade's condition is evaluated.
        /// </summary>
        private string GenerateDynamicOperatorDescription()
        {
            var operatorA = (ComparisonOperators)cmbGradeA.SelectedItem;
            var operatorB = (ComparisonOperators)cmbGradeB.SelectedItem;
            var operatorC = (ComparisonOperators)cmbGradeC.SelectedItem;
            var operatorD = (ComparisonOperators)cmbGradeD.SelectedItem;
            var operatorF = (ComparisonOperators)cmbGradeF.SelectedItem;

            decimal valueA = numPadGradeMaxValueA.Value;
            decimal valueB = numPadGradeMaxValueB.Value;
            decimal valueC = numPadGradeMaxValueC.Value;
            decimal valueD = numPadGradeMaxValueD.Value;
            decimal valueF = numPadGradeMaxValueF.Value;

            string unit = GetMetricUnit(GradingSystems);
            string unitStr = string.IsNullOrEmpty(unit) ? "" : $" {unit}";

            // Build readable conditions
            string conditionA = FormatCondition("Grade A", valueA, operatorA, unitStr);
            string conditionB = FormatCondition("Grade B", valueB, operatorB, unitStr);
            string conditionC = FormatCondition("Grade C", valueC, operatorC, unitStr);
            string conditionD = FormatCondition("Grade D", valueD, operatorD, unitStr);
            string conditionF = FormatCondition("Grade F", valueF, operatorF, unitStr);

            string systemInfo = GetGradeOperatorDescription(GradingSystems);

            return $"{systemInfo}\n\n🎯 Evaluation Rules:\n{conditionA}\n{conditionB}\n{conditionC}\n{conditionD}\n{conditionF}";
        }

        /// <summary>
        /// Formats a single grade condition into human-readable text.
        /// Example: "Grade A: value ≥ 70%"
        /// </summary>
        private string FormatCondition(string gradeName, decimal value, ComparisonOperators op, string unit)
        {
            string opSymbol = op == ComparisonOperators.GreaterThanEqualTo ? "≥" : "≤";
            return $"  {gradeName}: value {opSymbol} {value}{unit}";
        }

        /// <summary>
        /// Gets the metric unit suffix (%, cells, modules, etc.)
        /// </summary>
        private string GetMetricUnit(GradingSystems system)
        {
            return system switch
            {
                GradingSystems.SymbolContrast => "%",
                GradingSystems.Modulation => "",
                GradingSystems.AxialNonuniformity => "(ratio)",
                GradingSystems.GridNonuniformity => "cells",
                GradingSystems.FixedPatternDamage => "errors",
                GradingSystems.UnusedErrorCorrection => "errors",
                GradingSystems.Decode => "",
                GradingSystems.QuietZone => "modules",
                _ => ""
            };
        }

        /// <summary>
        /// Gets the full display name for each metric
        /// </summary>
        private string GetMetricDisplayName(GradingSystems system)
        {
            return system switch
            {
                GradingSystems.SymbolContrast => "Symbol Contrast (SC)",
                GradingSystems.Modulation => "Modulation (MOD)",
                GradingSystems.AxialNonuniformity => "Axial Non-Uniformity (AN)",
                GradingSystems.GridNonuniformity => "Grid Non-Uniformity (GN)",
                GradingSystems.FixedPatternDamage => "Fixed Pattern Damage (FPD)",
                GradingSystems.UnusedErrorCorrection => "Unused Error Correction (UEC)",
                GradingSystems.Decode => "Decodability",
                GradingSystems.QuietZone => "Quiet Zone (QZ)",
                _ => "Quality Metric"
            };
        }

        private string GetGradeThresholdDescription(GradingSystems system)
        {
            return system switch
            {
                GradingSystems.SymbolContrast =>
                    "Symbol Contrast (SC) - Image brightness difference\n" +
                    "Higher contrast indicates clearer distinction between bars and spaces.",

                GradingSystems.Modulation =>
                    "Modulation (MOD) - Cell-to-cell reflectance variation\n" +
                    "Higher modulation indicates better reflectance differences between modules.",

                GradingSystems.AxialNonuniformity =>
                    "Axial Non-Uniformity (AN) - Width/height aspect ratio deviation\n" +
                    "Lower deviation indicates more uniform barcode shape (less distortion).",

                GradingSystems.GridNonuniformity =>
                    "Grid Non-Uniformity (GN) - Cell size consistency across matrix\n" +
                    "Lower deviation indicates consistent cell sizing throughout barcode.",

                GradingSystems.FixedPatternDamage =>
                    "Fixed Pattern Damage (FPD) - Finder pattern integrity\n" +
                    "Fewer errors indicate better finder pattern structure.",

                GradingSystems.UnusedErrorCorrection =>
                    "Unused Error Correction (UEC) - Visual errors in data area\n" +
                    "Fewer errors indicate cleaner data area appearance.",

                GradingSystems.Decode =>
                    "Decodability - Successfully decoded barcode\n" +
                    "Successfully decoded barcode achieves grade A; failure results in grade F.",

                GradingSystems.QuietZone =>
                    "Quiet Zone (QZ) - Clear space around barcode margins\n" +
                    "Larger quiet zone indicates better barcode isolation from surrounding content.",

                _ => "Grade thresholds configuration"
            };
        }

        private string GetGradeOperatorDescription(GradingSystems system)
        {
            bool isHigherBetter = IsHigherBetterMetric(system);

            string operatorInfo = isHigherBetter
                ? "📈 Higher is Better (≥): Grades improve with higher values"
                : "📉 Lower is Better (≤): Grades improve with lower values";

            string systemInfo = system switch
            {
                GradingSystems.SymbolContrast =>
                    "Higher contrast = clearer distinction between bars and spaces",

                GradingSystems.Modulation =>
                    "Higher modulation = better cell reflectance difference",

                GradingSystems.AxialNonuniformity =>
                    "Lower deviation = more uniform barcode shape",

                GradingSystems.GridNonuniformity =>
                    "Lower deviation = consistent cell sizing",

                GradingSystems.FixedPatternDamage =>
                    "Fewer errors = better finder pattern integrity",

                GradingSystems.UnusedErrorCorrection =>
                    "Fewer visual errors = cleaner data area",

                GradingSystems.Decode =>
                    "Successfully decoded barcode is required for grade A",

                GradingSystems.QuietZone =>
                    "Larger quiet zone = better barcode isolation",

                _ => "Quality metric configuration"
            };

            return $"{operatorInfo}\n\n{systemInfo}";
        }

        private bool IsHigherBetterMetric(GradingSystems system)
        {
            return system switch
            {
                GradingSystems.SymbolContrast or
                GradingSystems.Modulation or
                GradingSystems.UnusedErrorCorrection or
                GradingSystems.Decode or
                GradingSystems.FixedPatternDamage or
                GradingSystems.QuietZone => true,

                GradingSystems.AxialNonuniformity or
                GradingSystems.GridNonuniformity => false,

                _ => true
            };
        }
    }
}
