using CsplCam.Library.Enums;
using CsplCam.Library.Interfaces;
using CsplCam.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXingCpp;

namespace OpenCV_SharpNet_Demo.UserControls
{
    public partial class RoiControlTemplate : UserControl, IRoiControl
    {
        public RoiControlTemplate()
        {
            InitializeComponent();

            // =================================================================
            // OPTIMIZATION: ENABLE DEEP DOUBLE BUFFERING TO KILL FLICKERING
            // =================================================================
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            UpdateStyles();
            EnableDoubleBuffering(TblPNlMain);
            EnableDoubleBuffering(TblPnlRoiOcrData);

            cmbRotationAngle.DataSource = Enum.GetValues(typeof(RotationAngles));

            //click events
            Click += (sender, args) => SelectionClick?.Invoke(this, EventArgs.Empty);
            GrpRoiData.Click += (sender, args) => SelectionClick?.Invoke(this, EventArgs.Empty);

            ControlSize = new Size(Width, Height);
        }


        //GET ROI OBJECT
        public RoiObject BoundedROI { get; private set; }

        public Size? ControlSize { get; set; }

        // --- FLAG ---
        private bool _isBinding = false;

        //create eventhandler for roi changed
        public event EventHandler SelectionClick;
        public event EventHandler SettingsChanged;
        public event EventHandler DecodeRequested;

        //new event for open the anchor setting window
        public event EventHandler OpenAnchorSettingsWindow;

        //new event for open the roi anchor reference window
        public event EventHandler OpenRoiReferenceWindow;

        ////new event for open the barcode reading and detecting mode
        //public event EventHandler OpenBarCodeDetectionMode;

        string[] strBarCodeFormats = new string[] { string.Empty, "Pharma" };

        // =================================================================
        // OPTIMIZATION HELPER: Forces TableLayoutPanels to render smoothly
        // =================================================================
        private void EnableDoubleBuffering(Control control)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

        public void BindData(RoiObject roi, bool isSelected)
        {
            _isBinding = true; //stop listening

            try
            {
                BoundedROI = roi;

                // 1. Visual Selection State
                //Color bg = isSelected ? Color.LemonChiffon : Color.WhiteSmoke;
                //if (this.BackColor != bg) this.BackColor = bg;

                Color bg = isSelected ? Color.SeaShell : Color.White;
                if (this.BackColor != bg) this.BackColor = bg;

                // 2. Common Data
                string title = $"{roi.Name} ({roi.Type})";
                if (GrpRoiData.Text != title) GrpRoiData.Text = title;

                if (!TxtDecoded.Focused && roi.DecodedText != TxtDecoded.Text)
                {
                    TxtDecoded.Text = roi.DecodedText;
                }

                if (!chkAnchor.Focused && chkAnchor.Checked != roi.IsAnchor)
                    chkAnchor.Checked = roi.IsAnchor;

                if (chkIsUseReferance.Checked != roi.IsUseRoiReference)
                    chkIsUseReferance.Checked = roi.IsUseRoiReference;

                string tmThr = Math.Round(roi.TmThreshold, 2).ToString();
                if (TxtExpectedThr.Text != tmThr) TxtExpectedThr.Text = tmThr;

                string tmScore = Math.Round(roi.TmScore, 2).ToString();
                if (LblDecodedThr.Text != tmScore) LblDecodedThr.Text = tmScore;

                TblPNlMain.ResumeLayout(true);

                if (cmbRotationAngle.Items.Count > 0 && !cmbRotationAngle.Focused && (RotationAngles)cmbRotationAngle.SelectedItem != roi.RotationAngle)
                {
                    cmbRotationAngle.SelectedItem = roi.RotationAngle;
                }

                SetNum(NumPadMorphKeranalWidth, roi.MorphKernelWidth);
                SetNum(NumPadMorphIteration, roi.MorphIterations);

                CheckedRadioButtonState(roi.MorphOp);

                // =========================================================
                // OPTIMIZATION: Suspend Layout during heavy UI shifts
                // =========================================================
                TblPNlMain.SuspendLayout();
            }
            finally
            {
                _isBinding = false; //resume listening
            }
        }

        private void CheckedRadioButtonState(MorphOperation morphOperation)
        {
            switch (morphOperation)
            {
                case MorphOperation.None:
                    if (!rdMorphModeNone.Checked) rdMorphModeNone.Checked = true;
                    break;
                case MorphOperation.Erode:
                    if (!rdMorphModeErode.Checked) rdMorphModeErode.Checked = true;
                    break;
                case MorphOperation.Dilate:
                    if (!rdMorphModeDilate.Checked) rdMorphModeDilate.Checked = true;
                    break;
            }
        }

        private void SetNum(NumericUpDown num, int val)
        {
            if (!num.Focused)
            {
                int safeVal = Math.Max((int)num.Minimum, Math.Min((int)num.Maximum, val));
                if (num.Value != safeVal) num.Value = safeVal;
            }
        }

        private void BtnDecodeROI_Click(object sender, EventArgs e) { DecodeRequested?.Invoke(this, EventArgs.Empty); }

        private void TxtExpectedThr_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(TxtExpectedThr.Text, out double val))
            {
                if (BoundedROI.Type == RoiType.TemplateMatch) BoundedROI.TmThreshold = val;
                else BoundedROI.Threshold = val;
            }
        }

        public void SetSelectionState(bool isSelected)
        {
            Color bg = isSelected ? Color.LemonChiffon : Color.WhiteSmoke;
            if (this.BackColor != bg) this.BackColor = bg;
        }

        private void CmbRotationAngle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isBinding || BoundedROI == null) return;
            BoundedROI.RotationAngle = (RotationAngles)cmbRotationAngle.SelectedItem;
            SettingsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void NumPadMorphKeranalWidth_ValueChanged(object sender, EventArgs e)
        {
            if (BoundedROI is null || _isBinding) return;
            BoundedROI.MorphKernelWidth = BoundedROI.MorphKernelHeight = (int)NumPadMorphKeranalWidth.Value;
            SettingsChanged?.Invoke(this, EventArgs.Empty);
        }

        // =================================================================
        // OPTIMIZATION: Safe Image Disposing to prevent GDI Crashes
        // =================================================================
        public void SetPreviewImage(System.Drawing.Bitmap newImage)
        {
            if (newImage == null) return;

            var oldImage = pzPreview.Image;
            bool sizeChanged = oldImage == null || oldImage.Width != newImage.Width || oldImage.Height != newImage.Height;

            // Assign new before disposing old to prevent UI red-X crash
            pzPreview.Image = newImage;

            if (sizeChanged)
            {
                pzPreview.AutoFit();
            }

            if (oldImage != null)
            {
                oldImage.Dispose();
            }
        }

        private void MorphModeSelection(string StrMorphMode)
        {
            if (string.IsNullOrWhiteSpace(StrMorphMode)) return;
            Enum.TryParse(typeof(MorphOperation), StrMorphMode, true, out var mode);
            if (mode is null || _isBinding || BoundedROI == null) return;
            BoundedROI.MorphOp = (MorphOperation)mode;
            SettingsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void RdMorphModeNone_Click(object sender, EventArgs e)
        {
            var d = (RadioButton)sender;
            MorphModeSelection(d.Text);
        }

        private void RdMorphModeErode_Click(object sender, EventArgs e)
        {
            var d = (RadioButton)sender;
            MorphModeSelection(d.Text);
        }

        private void RdMorphModeDilate_Click(object sender, EventArgs e)
        {
            var d = (RadioButton)sender;
            MorphModeSelection(d.Text);
        }

        private void CmbRotationAngle_Validating(object sender, CancelEventArgs e)
        {
            var strAngle = cmbRotationAngle.SelectedItem;
            if (!cmbRotationAngle.Items.Contains(strAngle) && cmbRotationAngle.Focused)
            {
                MessageBox.Show("Please select correct angle from dropdown.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }
        }

        private void ChkAnchor_CheckedChanged(object sender, EventArgs e)
        {
            if (BoundedROI is null || _isBinding) return;

            BoundedROI.IsAnchor = chkAnchor.Checked;
            chkIsUseReferance.Enabled = chkAnchor.Checked;

            OpenAnchorSettingsWindow?.Invoke(this, EventArgs.Empty);
        }

        private void ChkIsUseReferance_CheckedChanged(object sender, EventArgs e)
        {
            if (BoundedROI is null || _isBinding) return;

            BoundedROI.IsUseRoiReference = chkIsUseReferance.Checked;
            OpenRoiReferenceWindow?.Invoke(this, EventArgs.Empty);
        }
    }
}
