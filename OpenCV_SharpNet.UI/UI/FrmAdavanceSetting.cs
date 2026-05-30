using CsplCam.Library.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenCV_SharpNet.UI.UI
{
    public partial class FrmAdavanceSetting : Form
    {
        public FrmAdavanceSetting()
        {
            InitializeComponent();
        }

        private void FrmAdavanceSetting_Load(object sender, EventArgs e)
        {
            numPadAspectRatioDifferenceMultiplier.Value = (decimal)OcrEngine.AspectRatioDifferenceMultiplier;
            numPadAspectRatioDifferenceThreshold.Value = (decimal)OcrEngine.AspectRatioDifferenceThreshold;
            numPadAspectRatioPenaltyValue.Value = (decimal)OcrEngine.AspectRatioPenaltyValue;
            numPadCharXaxisDifferenceMultiplier.Value = (decimal)OcrEngine.CharXaxisDifferenceMultiplier;
            numPadCharYaxisDifferenceMultiplier.Value = (decimal)OcrEngine.CharYaxisDifferenceMultiplier;
            numPadOcvTargetMatchConfidence.Value = (decimal)OcrEngine.OcvTargetMatchConfidence;
            numPadSkewAngle.Value = (decimal)OcrEngine.SkewAngle;

            numPadDensityDifferenceMultiplier.Value = (decimal)OcrEngine.DensityDifferenceMultiplier;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            OcrEngine.AspectRatioDifferenceMultiplier = (double)numPadAspectRatioDifferenceMultiplier.Value;

            OcrEngine.AspectRatioDifferenceThreshold = (double)numPadAspectRatioDifferenceThreshold.Value;

            OcrEngine.AspectRatioPenaltyValue = (double)numPadAspectRatioPenaltyValue.Value;

            OcrEngine.CharXaxisDifferenceMultiplier = (double)numPadCharXaxisDifferenceMultiplier.Value;

           OcrEngine.CharYaxisDifferenceMultiplier = (double)numPadCharYaxisDifferenceMultiplier.Value;

            OcrEngine.OcvTargetMatchConfidence = (double)numPadOcvTargetMatchConfidence.Value;
            OcrEngine.SkewAngle = (double)numPadSkewAngle.Value;

            OcrEngine.DensityDifferenceMultiplier = (double)numPadDensityDifferenceMultiplier.Value;

        }
    }
}
