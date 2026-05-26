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

        public void BindData()
        {
            _isBinding = true;

            try
            {
                // 1. Get the current active metric configuration
                var activeMetric = Roi.UserConfiguredGrading.GetMetric(GradingSystems);

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
            }
            finally
            {
                _isBinding = false;
            }
        }

        //private void BtnSaveGradingSetting_Click(object sender, EventArgs e)
        //{
        //    // 1. Get the current active metric configuration
        //    var activeMetric = Roi.UserConfiguredGrading.GetMetric(GradingSystems);

        //    activeMetric.IsEnabled = chkInCludeGrade.Checked;

        //    // 3. Optional: Bind your IsEnabled property to a master UI Checkbox if you have one
        //    // chkMetricEnabled.Checked = activeMetric.IsEnabled;

        //    //// 4. Extract individual grade values from the underlying data list using LINQ
        //    //List<GradingRange> d = new List<GradingRange>();
        //    //foreach (var item in Controls.Cast<NumericUpDown>().ToList())
        //    //{
        //    //    var s = new GradingRange
        //    //    {
        //    //        ComparisonOperators = ComparisonOperators.GreaterThanEqualTo,
        //    //        Grades = (Grades)item.Tag,
        //    //        GradeValue = (double)item.Value
        //    //    };

        //    //    d.Add(s);
        //    //}

        //    var d = Controls.OfType<NumericUpDown>()
        //            .Where(n => n.Tag is Grades)
        //            .Select(n => new GradingRange
        //            {
        //                ComparisonOperators = ComparisonOperators.GreaterThanEqualTo,
        //                Grades = (Grades)n.Tag,
        //                GradeValue = (double)n.Value
        //            })
        //            .ToList();


        //    activeMetric.GradingData = d;
        //}


        private void BtnSaveGradingSetting_Click(object sender, EventArgs e)
        {
            // 1. Get the current active metric configuration
            var activeMetric = Roi.UserConfiguredGrading.GetMetric(GradingSystems);

            // 2. Map the activation status
            activeMetric.IsEnabled = chkInCludeGrade.Checked;

            // 3. Determine operator dynamically (Lower-is-better metrics use LessThanEqualTo)
            ComparisonOperators currentOp = DetermineOperatorForSystem(GradingSystems);

            // 4. Clean out the old rules
            activeMetric.GradingData.Clear();

            // 5. Build and add the brand new immutable validated structs directly using the factory
            activeMetric.GradingData.Add(GradingRange.Create(Grades.A, (double)numPadGradeMaxValueA.Value, currentOp));
            activeMetric.GradingData.Add(GradingRange.Create(Grades.B, (double)numPadGradeMaxValueB.Value, currentOp));
            activeMetric.GradingData.Add(GradingRange.Create(Grades.C, (double)numPadGradeMaxValueC.Value, currentOp));
            activeMetric.GradingData.Add(GradingRange.Create(Grades.D, (double)numPadGradeMaxValueD.Value, currentOp));
            activeMetric.GradingData.Add(GradingRange.Create(Grades.F, (double)numPadGradeMaxValueF.Value, currentOp));

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
    }
}
