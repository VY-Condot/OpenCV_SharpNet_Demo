using CsplCam.Library.Enums;
using CsplCam.Library.Interfaces;
using CsplCam.Library.Models;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using ZXingCpp;

namespace OpenCV_SharpNet.UserControls
{
    public partial class ROIControlBarCode : UserControl, IRoiControl
    {
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

        string[] strBarCodeFormats = new string[] { string.Empty, "Pharma" };

        public ROIControlBarCode()
        {
            InitializeComponent();

            // =================================================================
            // OPTIMIZATION: ENABLE DEEP DOUBLE BUFFERING TO KILL FLICKERING
            // =================================================================
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            UpdateStyles();
            EnableDoubleBuffering(TblPNlMain);
            EnableDoubleBuffering(TblPnlRoiOcrData);
            EnableDoubleBuffering(TblImageAndRepo);

            cmbRotationAngle.DataSource = Enum.GetValues(typeof(RotationAngles));

            //load dynamic controls
            var formats = Enum.GetValues(typeof(BarcodeFormats)).Cast<BarcodeFormats>().Where(f => f != BarcodeFormats.None && f != BarcodeFormats.Any).Select(P => P.ToString()).ToList();
            formats.AddRange(strBarCodeFormats);
            formats = formats.OrderBy(P => P).ToList();

            cmbBarcodeFormat.DataSource = formats;

            //click events
            Click += (sender, args) => SelectionClick?.Invoke(this, EventArgs.Empty);
            GrpRoiData.Click += (sender, args) => SelectionClick?.Invoke(this, EventArgs.Empty);

            ControlSize = new Size(Width, Height);
        }

        // =================================================================
        // OPTIMIZATION HELPER: Forces TableLayoutPanels to render smoothly
        // =================================================================
        private void EnableDoubleBuffering(Control control)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

        private void CmbBarcodeFormat_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_isBinding || BoundedROI == null) return;

            //set barcode format selected by user
            BoundedROI.BarCodeFormat = cmbBarcodeFormat.SelectedItem?.ToString() ?? string.Empty;

            chkBarcodeFormatAuto.Checked = string.IsNullOrWhiteSpace(BoundedROI.BarCodeFormat);
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

                // =========================================================
                // OPTIMIZATION: Suspend Layout during heavy UI shifts
                // =========================================================
                TblPNlMain.SuspendLayout();

                if (cmbBarcodeFormat.Items.Count > 0 && !cmbBarcodeFormat.Focused && cmbBarcodeFormat.SelectedItem?.ToString() != roi.BarCodeFormat)
                {
                    cmbBarcodeFormat.SelectedItem = roi.BarCodeFormat;
                }

                if (!chkBarcodeFormatAuto.Focused && chkBarcodeFormatAuto.Checked != roi.isBarCodeFormatAuto)
                    chkBarcodeFormatAuto.Checked = roi.isBarCodeFormatAuto;

                if (!chkAnchor.Focused && chkAnchor.Checked != roi.IsAnchor)
                    chkAnchor.Checked = roi.IsAnchor;

                GenerateReport();

                if (cmbRotationAngle.Items.Count > 0 && !cmbRotationAngle.Focused && (RotationAngles)cmbRotationAngle.SelectedItem != roi.RotationAngle)
                {
                    cmbRotationAngle.SelectedItem = roi.RotationAngle;
                }

                //SetNum(NumPadMorphKeranalWidth, roi.MorphKernelWidth);
                //SetNum(NumPadMorphIteration, roi.MorphIterations);

                CheckedRadioButtonState(roi.MorphOp);

                TblPNlMain.ResumeLayout(true);
            }
            finally
            {
                _isBinding = false; //resume listening
            }
        }

        private void CheckedRadioButtonState(MorphOperation morphOperation)
        {
            //switch (morphOperation)
            //{
            //    case MorphOperation.None:
            //        if (!rdMorphModeNone.Checked) rdMorphModeNone.Checked = true;
            //        break;
            //    case MorphOperation.Erode:
            //        if (!rdMorphModeErode.Checked) rdMorphModeErode.Checked = true;
            //        break;
            //    case MorphOperation.Dilate:
            //        if (!rdMorphModeDilate.Checked) rdMorphModeDilate.Checked = true;
            //        break;
            //}
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

        private void TxtExpected_TextChanged(object sender, EventArgs e)
        {
            if (BoundedROI is null || _isBinding) return;
            BoundedROI.ExpectedText = TxtExpected.Text;
        }

        private void NumPadMaxWidth_ValueChanged(object sender, EventArgs e)
        {
            if (BoundedROI is null || _isBinding) return;
            BoundedROI.MaxBlobW = (int)NumPadMaxWidth.Value;
        }

        private void TxtExpectedThr_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(TxtExpectedThr.Text, out double val))
            {
                if (BoundedROI.Type == RoiType.TemplateMatch) BoundedROI.TmThreshold = val;
                else BoundedROI.Threshold = val;
            }
        }

        private void NumPadMinWidth_ValueChanged(object sender, EventArgs e)
        {
            if (BoundedROI is null || _isBinding) return;
            BoundedROI.MinBlobW = (int)NumPadMinWidth.Value;
        }

        private void NumPadMinHeight_ValueChanged(object sender, EventArgs e)
        {
            if (BoundedROI is null || _isBinding) return;
            BoundedROI.MinBlobH = (int)NumPadMinHeight.Value;
        }

        private void NumPadMaxHeight_ValueChanged(object sender, EventArgs e)
        {
            if (BoundedROI is null || _isBinding) return;
            BoundedROI.MaxBlobH = (int)NumPadMaxHeight.Value;
        }

        private void chkDoOCR_CheckedChanged(object sender, EventArgs e)
        {
            //BoundedROI.ShowOverlay = chkDoOCR.Checked;
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
            //if (BoundedROI is null || _isBinding) return;
            //BoundedROI.MorphKernelWidth = BoundedROI.MorphKernelHeight = (int)NumPadMorphKeranalWidth.Value;
            //SettingsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void NumPadMorphIteration_ValueChanged(object sender, EventArgs e) { }

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

        void GenerateReport()
        {
            if (BoundedROI.Gs1QcResult is null || string.IsNullOrWhiteSpace(BoundedROI.Gs1QcResult.OverAll))
            {
                lstGS1_Repo.Items.Clear();

                lstGS1_Repo.Items.Add($"Over All: No data found");
                return;
            }

            // =================================================================
            // OPTIMIZATION: Suspend ListBox drawing while adding 15 items
            // =================================================================
            lstGS1_Repo.BeginUpdate();
            lstGS1_Repo.Items.Clear();

            if (BoundedROI.BarCodeFormat.Equals("datamatrix", StringComparison.OrdinalIgnoreCase) || BoundedROI.BarCodeFormat.Equals("QRCode", StringComparison.OrdinalIgnoreCase) || BoundedROI.BarCodeFormat.Equals("Aztec", StringComparison.OrdinalIgnoreCase) || BoundedROI.BarCodeFormat.Equals("PDF", StringComparison.OrdinalIgnoreCase))
            {
                lstGS1_Repo.Items.Add($"Over All: {BoundedROI.Gs1QcResult.OverAll}");
                lstGS1_Repo.Items.Add(string.Empty);

                lstGS1_Repo.Items.Add($"Decode: {BoundedROI.Gs1QcResult.Decode}");
                lstGS1_Repo.Items.Add($"Symbol Contrast: {BoundedROI.Gs1QcResult.SC}");
                lstGS1_Repo.Items.Add($"Auxiliary Nonuniformity: {BoundedROI.Gs1QcResult.AN}");
                lstGS1_Repo.Items.Add($"Grid Nonuniformity: {BoundedROI.Gs1QcResult.GN}");
                lstGS1_Repo.Items.Add($"Modulation: {BoundedROI.Gs1QcResult.MOD}");
                lstGS1_Repo.Items.Add($"Fixed Pattern Damage: {BoundedROI.Gs1QcResult.FPD}");
                lstGS1_Repo.Items.Add($"Unused Error Correction: {BoundedROI.Gs1QcResult.UEC}");
                lstGS1_Repo.Items.Add($"Print Growth (Info): {BoundedROI.Gs1QcResult.PG}");
                lstGS1_Repo.Items.Add(string.Empty);
                lstGS1_Repo.Items.Add($"AngleOf Distortion: {BoundedROI.Gs1QcResult.AS9132_Distortion}");
                lstGS1_Repo.Items.Add($"Quiet Zone: {BoundedROI.Gs1QcResult.AS9132_QuietZone}");
                lstGS1_Repo.Items.Add($"Elongation: {BoundedROI.Gs1QcResult.AS9132_Elongation}");
            }
            else
            {
                //lstGS1_Repo.Items.Add($"Decode: {BoundedROI.Gs1QcResult.Decode}");
                //lstGS1_Repo.Items.Add($"Symbol Contrast: {BoundedROI.Gs1QcResult.SC}");
                //lstGS1_Repo.Items.Add($"Modulation: {BoundedROI.Gs1QcResult.MOD}");
                //lstGS1_Repo.Items.Add($"Minimum Reflectance: {BoundedROI.Gs1QcResult.MinReflectance}");
                //lstGS1_Repo.Items.Add($"Minimum Edge Contrast: {BoundedROI.Gs1QcResult.MinEdgeContrast}");
                //lstGS1_Repo.Items.Add($"Defects(Spots / Voids):: {BoundedROI.Gs1QcResult.Defects}");
                //lstGS1_Repo.Items.Add($"Decodability: {BoundedROI.Gs1QcResult.Defects}");
                //lstGS1_Repo.Items.Add(string.Empty);
                //lstGS1_Repo.Items.Add($"Over All: {BoundedROI.Gs1QcResult.OverAll}");

                lstGS1_Repo.Items.Add($"Over All: No data found");
            }
            lstGS1_Repo.EndUpdate();
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

        private void ChkBarcodeAdvancedMode_Click(object sender, EventArgs e)
        {
            if (BoundedROI is null || _isBinding) return;

            BoundedROI.UseBruteForceGridRecovery = chkBarcodeAdvancedMode.Checked;
        }

        private void ChkBarcodeFormatAuto_Click(object sender, EventArgs e)
        {
            if (_isBinding || BoundedROI == null) return;

            BoundedROI.isBarCodeFormatAuto = chkBarcodeFormatAuto.Checked;

            //SET COMBOX VALUES BASED ON CHECKBOX STATE
            if (chkBarcodeFormatAuto.Checked && cmbBarcodeFormat.Items.Count > 0)
            {
                cmbBarcodeFormat.SelectedItem = string.Empty;
            }
        }

        private void ChkGradingRepo_CheckedChanged(object sender, EventArgs e)
        {
            if (BoundedROI is null || _isBinding) return;

            BoundedROI.IsRunGS1QcCheck = chkGradingRepo.Checked;
        }
    }
}
