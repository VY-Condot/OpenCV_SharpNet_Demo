using OpenCV_SharpNet.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenCV_SharpNet_Ver_3_0_1.UI
{
    public partial class FrmBarCodeReadingMode : Form
    {
        public RoiObject _roiObject { get; set; }
        public FrmBarCodeReadingMode(RoiObject roiObject)
        {
            InitializeComponent();
            _roiObject = roiObject;

            rdBruteForceRecoveryMode.Checked = _roiObject.UseBruteForceGridRecovery;
            rdSuperResolutionMode.Checked = _roiObject.UseSuperResolution;
        }

        private void RdBruteForceRecoveryMode_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void RdBruteForceRecoveryMode_Click(object sender, EventArgs e)
        {
            if (_roiObject == null)
            {
                _roiObject.UseBruteForceGridRecovery = rdBruteForceRecoveryMode.Checked;
            }
        }

        private void RdSuperResolutionMode_Click(object sender, EventArgs e)
        {
            if (_roiObject == null)
            {
                _roiObject.UseSuperResolution = rdSuperResolutionMode.Checked;
            }
        }

        public RoiObject GetUpdatedRoi()
        {
            return _roiObject;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //resetting this 
            _roiObject.UseBruteForceGridRecovery = false;
            _roiObject.UseSuperResolution = false;

            if (rdSuperResolutionMode.Checked)
                _roiObject.UseSuperResolution = rdSuperResolutionMode.Checked;
            else if (rdBruteForceRecoveryMode.Checked)
                _roiObject.UseBruteForceGridRecovery = rdBruteForceRecoveryMode.Checked;


            DialogResult = DialogResult.OK;
            Close();
        }

        private void FrmBarCodeReadingMode_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
