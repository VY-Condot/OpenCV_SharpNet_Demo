using OpenCV_SharpNet.Interfaces;
using OpenCV_SharpNet.Models.GS1_QC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenCV_SharpNet_Demo.UI.GS1_QC
{
    public partial class GS1_QC_Report : Form
    {
        //private readonly IGS1_QC_Check _QC_Check;
        //public GS1_QC_Report(IGS1_QC_Check qC_Check)
        //{
        //    if (qC_Check == null) throw new ArgumentNullException(nameof(qC_Check));
        //    _QC_Check = qC_Check;

        //    InitializeComponent();
        //}

        private readonly GS1_QC_CheckResult gS1_QC_CheckResult;
        public GS1_QC_Report(GS1_QC_CheckResult result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));
            gS1_QC_CheckResult = result;

            InitializeComponent();
        }

        private void GS1_QC_Report_Load(object sender, EventArgs e)
        {
            //listBox1.Items.Add($"Overall Grade: {gS1_QC_CheckResult.Decode}");
            //listBox1.Items.Add($"Symbol Contrast: {gS1_QC_CheckResult.SC}");
            //listBox1.Items.Add($"Auxiliary Nonuniformity: {gS1_QC_CheckResult.AN}");
            //listBox1.Items.Add($"Grid Nonuniformity: {gS1_QC_CheckResult.GN}");
            //listBox1.Items.Add($"Modulation: {gS1_QC_CheckResult.MOD}");
            //listBox1.Items.Add($"Fixed Pattern Damage: {gS1_QC_CheckResult.FPD}");
            //listBox1.Items.Add($"Unused Error Correction: {gS1_QC_CheckResult.UEC}");




            listBox1.Items.Add($"Decode: {gS1_QC_CheckResult.Decode}");
            listBox1.Items.Add($"Symbol Contrast: {gS1_QC_CheckResult.SC}");
            listBox1.Items.Add($"Auxiliary Nonuniformity: {gS1_QC_CheckResult.AN}");
            listBox1.Items.Add($"Grid Nonuniformity: {gS1_QC_CheckResult.GN}");
            listBox1.Items.Add($"Modulation: {gS1_QC_CheckResult.MOD}");
            listBox1.Items.Add($"Fixed Pattern Damage: {gS1_QC_CheckResult.FPD}");
            listBox1.Items.Add($"Unused Error Correction: {gS1_QC_CheckResult.UEC}");
            listBox1.Items.Add($"Print Growth (Info): {gS1_QC_CheckResult.PG}");
            listBox1.Items.Add(string.Empty);
            listBox1.Items.Add($"AngleOf Distortion: {gS1_QC_CheckResult.AS9132_Distortion}");
            listBox1.Items.Add($"Quiet Zone: {gS1_QC_CheckResult.AS9132_QuietZone}");
            listBox1.Items.Add($"Elongation: {gS1_QC_CheckResult.AS9132_Elongation}");
            //listBox1.Items.Add($"DPM Rmin: {gS1_QC_CheckResult.DPM_Rmin}");
            listBox1.Items.Add(string.Empty);
            listBox1.Items.Add($"Over All: {gS1_QC_CheckResult.OverAll}");
        }
    }
}
