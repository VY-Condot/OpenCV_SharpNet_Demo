using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace OpenCV_SharpNet_Demo.UserControls
{
    partial class ROIControlBarCode
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ROIControlBarCode));
            TblPNlMain = new TableLayoutPanel();
            grpQC_Result = new GroupBox();
            tblPnlgs1Repo = new TableLayoutPanel();
            lstGS1_Repo = new ListBox();
            chkGradingRepo = new CheckBox();
            TblPnlRotationAngle = new TableLayoutPanel();
            LblRotationAngle = new Label();
            cmbRotationAngle = new ComboBox();
            chkAnchor = new CheckBox();
            chkIsUseReferance = new CheckBox();
            TblPnlBottom = new TableLayoutPanel();
            BtnDecodeROI = new Button();
            GrpRoiData = new GroupBox();
            TblPnlRoiOcrData = new TableLayoutPanel();
            chkBarcodeFormatAuto = new CheckBox();
            cmbBarcodeFormat = new ComboBox();
            lblBarcodeFormat = new Label();
            LblDecoded = new Label();
            TxtDecoded = new TextBox();
            TblImageAndRepo = new TableLayoutPanel();
            pzPreview = new PanZoomViewer();
            grpBoxAdvancedMode = new GroupBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            chkBarcodeAdvancedMode = new CheckBox();
            TblPNlMain.SuspendLayout();
            grpQC_Result.SuspendLayout();
            tblPnlgs1Repo.SuspendLayout();
            TblPnlRotationAngle.SuspendLayout();
            TblPnlBottom.SuspendLayout();
            GrpRoiData.SuspendLayout();
            TblPnlRoiOcrData.SuspendLayout();
            TblImageAndRepo.SuspendLayout();
            grpBoxAdvancedMode.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // TblPNlMain
            // 
            TblPNlMain.ColumnCount = 1;
            TblPNlMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TblPNlMain.Controls.Add(grpQC_Result, 0, 4);
            TblPNlMain.Controls.Add(TblPnlRotationAngle, 0, 2);
            TblPNlMain.Controls.Add(TblPnlBottom, 0, 5);
            TblPNlMain.Controls.Add(GrpRoiData, 0, 0);
            TblPNlMain.Controls.Add(TblImageAndRepo, 0, 3);
            TblPNlMain.Controls.Add(grpBoxAdvancedMode, 0, 1);
            TblPNlMain.Dock = DockStyle.Fill;
            TblPNlMain.Location = new Point(0, 0);
            TblPNlMain.Name = "TblPNlMain";
            TblPNlMain.RowCount = 7;
            TblPNlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 176F));
            TblPNlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 65F));
            TblPNlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 51F));
            TblPNlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 105F));
            TblPNlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 308F));
            TblPNlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            TblPNlMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblPNlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            TblPNlMain.Size = new Size(597, 765);
            TblPNlMain.TabIndex = 0;
            // 
            // grpQC_Result
            // 
            grpQC_Result.Controls.Add(tblPnlgs1Repo);
            grpQC_Result.Dock = DockStyle.Fill;
            grpQC_Result.Location = new Point(3, 400);
            grpQC_Result.Name = "grpQC_Result";
            grpQC_Result.Size = new Size(591, 302);
            grpQC_Result.TabIndex = 10;
            grpQC_Result.TabStop = false;
            grpQC_Result.Text = "QC Report";
            grpQC_Result.UseCompatibleTextRendering = true;
            // 
            // tblPnlgs1Repo
            // 
            tblPnlgs1Repo.ColumnCount = 1;
            tblPnlgs1Repo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblPnlgs1Repo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tblPnlgs1Repo.Controls.Add(lstGS1_Repo, 0, 1);
            tblPnlgs1Repo.Controls.Add(chkGradingRepo, 0, 0);
            tblPnlgs1Repo.Dock = DockStyle.Fill;
            tblPnlgs1Repo.Location = new Point(3, 24);
            tblPnlgs1Repo.Name = "tblPnlgs1Repo";
            tblPnlgs1Repo.RowCount = 2;
            tblPnlgs1Repo.RowStyles.Add(new RowStyle(SizeType.Absolute, 43F));
            tblPnlgs1Repo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblPnlgs1Repo.Size = new Size(585, 275);
            tblPnlgs1Repo.TabIndex = 0;
            // 
            // lstGS1_Repo
            // 
            lstGS1_Repo.Dock = DockStyle.Fill;
            lstGS1_Repo.Font = new Font("Palatino Linotype", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lstGS1_Repo.FormattingEnabled = true;
            lstGS1_Repo.Location = new Point(3, 46);
            lstGS1_Repo.Name = "lstGS1_Repo";
            lstGS1_Repo.Size = new Size(579, 226);
            lstGS1_Repo.TabIndex = 8;
            // 
            // chkGradingRepo
            // 
            chkGradingRepo.AutoSize = true;
            chkGradingRepo.Dock = DockStyle.Fill;
            chkGradingRepo.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold);
            chkGradingRepo.Location = new Point(3, 3);
            chkGradingRepo.Name = "chkGradingRepo";
            chkGradingRepo.Padding = new Padding(5, 0, 0, 0);
            chkGradingRepo.Size = new Size(579, 37);
            chkGradingRepo.TabIndex = 9;
            chkGradingRepo.Text = "Generate Grading Report";
            chkGradingRepo.UseCompatibleTextRendering = true;
            chkGradingRepo.UseVisualStyleBackColor = true;
            chkGradingRepo.CheckedChanged += ChkGradingRepo_CheckedChanged;
            // 
            // TblPnlRotationAngle
            // 
            TblPnlRotationAngle.ColumnCount = 5;
            TblPnlRotationAngle.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            TblPnlRotationAngle.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 188F));
            TblPnlRotationAngle.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 9F));
            TblPnlRotationAngle.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 149F));
            TblPnlRotationAngle.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TblPnlRotationAngle.Controls.Add(LblRotationAngle, 0, 0);
            TblPnlRotationAngle.Controls.Add(cmbRotationAngle, 1, 0);
            TblPnlRotationAngle.Controls.Add(chkAnchor, 3, 0);
            TblPnlRotationAngle.Controls.Add(chkIsUseReferance, 4, 0);
            TblPnlRotationAngle.Dock = DockStyle.Fill;
            TblPnlRotationAngle.Location = new Point(3, 244);
            TblPnlRotationAngle.Name = "TblPnlRotationAngle";
            TblPnlRotationAngle.RowCount = 1;
            TblPnlRotationAngle.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblPnlRotationAngle.Size = new Size(591, 45);
            TblPnlRotationAngle.TabIndex = 5;
            // 
            // LblRotationAngle
            // 
            LblRotationAngle.AutoSize = true;
            LblRotationAngle.Dock = DockStyle.Fill;
            LblRotationAngle.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold);
            LblRotationAngle.Location = new Point(3, 3);
            LblRotationAngle.Margin = new Padding(3);
            LblRotationAngle.Name = "LblRotationAngle";
            LblRotationAngle.Size = new Size(84, 39);
            LblRotationAngle.TabIndex = 2;
            LblRotationAngle.Text = " Rotation : ";
            LblRotationAngle.TextAlign = ContentAlignment.MiddleLeft;
            LblRotationAngle.UseCompatibleTextRendering = true;
            // 
            // cmbRotationAngle
            // 
            cmbRotationAngle.FormattingEnabled = true;
            cmbRotationAngle.Location = new Point(93, 8);
            cmbRotationAngle.Margin = new Padding(3, 8, 3, 3);
            cmbRotationAngle.Name = "cmbRotationAngle";
            cmbRotationAngle.Size = new Size(182, 28);
            cmbRotationAngle.TabIndex = 3;
            cmbRotationAngle.SelectedIndexChanged += CmbRotationAngle_SelectedIndexChanged;
            cmbRotationAngle.Validating += CmbRotationAngle_Validating;
            // 
            // chkAnchor
            // 
            chkAnchor.AutoSize = true;
            chkAnchor.Dock = DockStyle.Fill;
            chkAnchor.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkAnchor.Location = new Point(290, 3);
            chkAnchor.Name = "chkAnchor";
            chkAnchor.Size = new Size(143, 39);
            chkAnchor.TabIndex = 9;
            chkAnchor.Text = "Enable Anchor";
            chkAnchor.UseCompatibleTextRendering = true;
            chkAnchor.UseVisualStyleBackColor = true;
            chkAnchor.CheckedChanged += ChkAnchor_CheckedChanged;
            // 
            // chkIsUseReferance
            // 
            chkIsUseReferance.AutoSize = true;
            chkIsUseReferance.Dock = DockStyle.Fill;
            chkIsUseReferance.Enabled = false;
            chkIsUseReferance.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkIsUseReferance.Location = new Point(439, 3);
            chkIsUseReferance.Name = "chkIsUseReferance";
            chkIsUseReferance.Size = new Size(149, 39);
            chkIsUseReferance.TabIndex = 10;
            chkIsUseReferance.Text = "Use ROI Ref.";
            chkIsUseReferance.UseCompatibleTextRendering = true;
            chkIsUseReferance.UseVisualStyleBackColor = true;
            chkIsUseReferance.CheckedChanged += ChkIsUseReferance_CheckedChanged;
            // 
            // TblPnlBottom
            // 
            TblPnlBottom.ColumnCount = 3;
            TblPnlBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            TblPnlBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 158F));
            TblPnlBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            TblPnlBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            TblPnlBottom.Controls.Add(BtnDecodeROI, 1, 0);
            TblPnlBottom.Dock = DockStyle.Fill;
            TblPnlBottom.Location = new Point(3, 708);
            TblPnlBottom.Name = "TblPnlBottom";
            TblPnlBottom.RowCount = 1;
            TblPnlBottom.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblPnlBottom.Size = new Size(591, 46);
            TblPnlBottom.TabIndex = 3;
            // 
            // BtnDecodeROI
            // 
            BtnDecodeROI.BackColor = Color.Red;
            BtnDecodeROI.Dock = DockStyle.Fill;
            BtnDecodeROI.Font = new Font("Calibri", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnDecodeROI.ForeColor = Color.Transparent;
            BtnDecodeROI.Image = (Image)resources.GetObject("BtnDecodeROI.Image");
            BtnDecodeROI.ImageAlign = ContentAlignment.MiddleLeft;
            BtnDecodeROI.Location = new Point(219, 3);
            BtnDecodeROI.Name = "BtnDecodeROI";
            BtnDecodeROI.Padding = new Padding(10, 0, 0, 0);
            BtnDecodeROI.Size = new Size(152, 40);
            BtnDecodeROI.TabIndex = 2;
            BtnDecodeROI.Text = "Decode ROI";
            BtnDecodeROI.TextImageRelation = TextImageRelation.ImageBeforeText;
            BtnDecodeROI.UseCompatibleTextRendering = true;
            BtnDecodeROI.UseVisualStyleBackColor = false;
            BtnDecodeROI.Click += BtnDecodeROI_Click;
            // 
            // GrpRoiData
            // 
            GrpRoiData.BackColor = Color.Transparent;
            GrpRoiData.Controls.Add(TblPnlRoiOcrData);
            GrpRoiData.Dock = DockStyle.Fill;
            GrpRoiData.Location = new Point(3, 3);
            GrpRoiData.Name = "GrpRoiData";
            GrpRoiData.Size = new Size(591, 170);
            GrpRoiData.TabIndex = 0;
            GrpRoiData.TabStop = false;
            GrpRoiData.Text = "ROI Name";
            GrpRoiData.UseCompatibleTextRendering = true;
            // 
            // TblPnlRoiOcrData
            // 
            TblPnlRoiOcrData.ColumnCount = 3;
            TblPnlRoiOcrData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 87F));
            TblPnlRoiOcrData.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TblPnlRoiOcrData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 235F));
            TblPnlRoiOcrData.Controls.Add(chkBarcodeFormatAuto, 2, 0);
            TblPnlRoiOcrData.Controls.Add(cmbBarcodeFormat, 1, 0);
            TblPnlRoiOcrData.Controls.Add(lblBarcodeFormat, 0, 0);
            TblPnlRoiOcrData.Controls.Add(LblDecoded, 0, 1);
            TblPnlRoiOcrData.Controls.Add(TxtDecoded, 1, 1);
            TblPnlRoiOcrData.Dock = DockStyle.Fill;
            TblPnlRoiOcrData.Location = new Point(3, 24);
            TblPnlRoiOcrData.Name = "TblPnlRoiOcrData";
            TblPnlRoiOcrData.RowCount = 2;
            TblPnlRoiOcrData.RowStyles.Add(new RowStyle(SizeType.Percent, 28.67133F));
            TblPnlRoiOcrData.RowStyles.Add(new RowStyle(SizeType.Percent, 71.3286743F));
            TblPnlRoiOcrData.Size = new Size(585, 143);
            TblPnlRoiOcrData.TabIndex = 0;
            // 
            // chkBarcodeFormatAuto
            // 
            chkBarcodeFormatAuto.AutoSize = true;
            chkBarcodeFormatAuto.Dock = DockStyle.Fill;
            chkBarcodeFormatAuto.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkBarcodeFormatAuto.Location = new Point(353, 3);
            chkBarcodeFormatAuto.Name = "chkBarcodeFormatAuto";
            chkBarcodeFormatAuto.Padding = new Padding(5, 0, 0, 0);
            chkBarcodeFormatAuto.Size = new Size(229, 35);
            chkBarcodeFormatAuto.TabIndex = 13;
            chkBarcodeFormatAuto.Text = "Auto";
            chkBarcodeFormatAuto.UseCompatibleTextRendering = true;
            chkBarcodeFormatAuto.UseVisualStyleBackColor = true;
            chkBarcodeFormatAuto.Click += ChkBarcodeFormatAuto_Click;
            // 
            // cmbBarcodeFormat
            // 
            cmbBarcodeFormat.Dock = DockStyle.Fill;
            cmbBarcodeFormat.FormattingEnabled = true;
            cmbBarcodeFormat.Location = new Point(90, 8);
            cmbBarcodeFormat.Margin = new Padding(3, 8, 3, 3);
            cmbBarcodeFormat.Name = "cmbBarcodeFormat";
            cmbBarcodeFormat.Size = new Size(257, 28);
            cmbBarcodeFormat.TabIndex = 4;
            cmbBarcodeFormat.SelectedIndexChanged += CmbBarcodeFormat_SelectedIndexChanged;
            // 
            // lblBarcodeFormat
            // 
            lblBarcodeFormat.AutoSize = true;
            lblBarcodeFormat.Dock = DockStyle.Fill;
            lblBarcodeFormat.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBarcodeFormat.Location = new Point(3, 3);
            lblBarcodeFormat.Margin = new Padding(3);
            lblBarcodeFormat.Name = "lblBarcodeFormat";
            lblBarcodeFormat.Size = new Size(81, 35);
            lblBarcodeFormat.TabIndex = 0;
            lblBarcodeFormat.Text = "Format : ";
            lblBarcodeFormat.TextAlign = ContentAlignment.MiddleLeft;
            lblBarcodeFormat.UseCompatibleTextRendering = true;
            // 
            // LblDecoded
            // 
            LblDecoded.AutoSize = true;
            LblDecoded.Dock = DockStyle.Fill;
            LblDecoded.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblDecoded.Location = new Point(3, 44);
            LblDecoded.Margin = new Padding(3);
            LblDecoded.Name = "LblDecoded";
            LblDecoded.Size = new Size(81, 96);
            LblDecoded.TabIndex = 1;
            LblDecoded.Text = "Decoded : ";
            LblDecoded.TextAlign = ContentAlignment.MiddleLeft;
            LblDecoded.UseCompatibleTextRendering = true;
            // 
            // TxtDecoded
            // 
            TxtDecoded.BackColor = Color.White;
            TxtDecoded.BorderStyle = BorderStyle.FixedSingle;
            TblPnlRoiOcrData.SetColumnSpan(TxtDecoded, 2);
            TxtDecoded.Dock = DockStyle.Fill;
            TxtDecoded.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            TxtDecoded.Location = new Point(90, 44);
            TxtDecoded.Multiline = true;
            TxtDecoded.Name = "TxtDecoded";
            TxtDecoded.ReadOnly = true;
            TxtDecoded.Size = new Size(492, 96);
            TxtDecoded.TabIndex = 3;
            // 
            // TblImageAndRepo
            // 
            TblImageAndRepo.ColumnCount = 1;
            TblImageAndRepo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TblImageAndRepo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            TblImageAndRepo.Controls.Add(pzPreview, 0, 0);
            TblImageAndRepo.Dock = DockStyle.Fill;
            TblImageAndRepo.Location = new Point(3, 295);
            TblImageAndRepo.Name = "TblImageAndRepo";
            TblImageAndRepo.RowCount = 1;
            TblImageAndRepo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblImageAndRepo.Size = new Size(591, 99);
            TblImageAndRepo.TabIndex = 9;
            // 
            // pzPreview
            // 
            pzPreview.Dock = DockStyle.Fill;
            pzPreview.Image = null;
            pzPreview.Interpolation = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            pzPreview.Location = new Point(3, 3);
            pzPreview.Name = "pzPreview";
            pzPreview.Size = new Size(585, 93);
            pzPreview.TabIndex = 0;
            // 
            // grpBoxAdvancedMode
            // 
            grpBoxAdvancedMode.Controls.Add(tableLayoutPanel2);
            grpBoxAdvancedMode.Dock = DockStyle.Fill;
            grpBoxAdvancedMode.Location = new Point(3, 179);
            grpBoxAdvancedMode.Name = "grpBoxAdvancedMode";
            grpBoxAdvancedMode.Size = new Size(591, 59);
            grpBoxAdvancedMode.TabIndex = 11;
            grpBoxAdvancedMode.TabStop = false;
            grpBoxAdvancedMode.Text = "Processing Mode";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 173F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Controls.Add(chkBarcodeAdvancedMode, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 24);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(585, 32);
            tableLayoutPanel2.TabIndex = 6;
            // 
            // chkBarcodeAdvancedMode
            // 
            chkBarcodeAdvancedMode.AutoSize = true;
            chkBarcodeAdvancedMode.Dock = DockStyle.Fill;
            chkBarcodeAdvancedMode.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkBarcodeAdvancedMode.Location = new Point(3, 3);
            chkBarcodeAdvancedMode.Name = "chkBarcodeAdvancedMode";
            chkBarcodeAdvancedMode.Padding = new Padding(5, 0, 0, 0);
            chkBarcodeAdvancedMode.Size = new Size(167, 26);
            chkBarcodeAdvancedMode.TabIndex = 12;
            chkBarcodeAdvancedMode.Text = "Advanced Mode";
            chkBarcodeAdvancedMode.UseCompatibleTextRendering = true;
            chkBarcodeAdvancedMode.UseVisualStyleBackColor = true;
            chkBarcodeAdvancedMode.Click += ChkBarcodeAdvancedMode_Click;
            // 
            // ROIControlBarCode
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(TblPNlMain);
            DoubleBuffered = true;
            Font = new Font("Palatino Linotype", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "ROIControlBarCode";
            Size = new Size(597, 765);
            TblPNlMain.ResumeLayout(false);
            grpQC_Result.ResumeLayout(false);
            tblPnlgs1Repo.ResumeLayout(false);
            tblPnlgs1Repo.PerformLayout();
            TblPnlRotationAngle.ResumeLayout(false);
            TblPnlRotationAngle.PerformLayout();
            TblPnlBottom.ResumeLayout(false);
            GrpRoiData.ResumeLayout(false);
            TblPnlRoiOcrData.ResumeLayout(false);
            TblPnlRoiOcrData.PerformLayout();
            TblImageAndRepo.ResumeLayout(false);
            grpBoxAdvancedMode.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel TblPNlMain;
        private GroupBox GrpRoiData;
        private TableLayoutPanel TblPnlRoiOcrData;
        private Label LblExpected;
        private Label LblExpectedThresold;
        private Label LblDecoded;
        private TextBox TxtExpected;
        private TextBox TxtDecoded;
        private Label LblDecodedThresold;
        private TextBox TxtExpectedThr;
        private Label LblDecodedThr;
        //private CheckBox chkDoOCR;
        private TableLayoutPanel TblPnlBlobFil;
        private Label LblBlobMaxWidth;
        private Label LblBlobMinWidth;
        private Label LblBlobMinHeight;
        private Label LblBlobMaxHeight;
        private NumericUpDown NumPadMaxWidth;
        private NumericUpDown NumPadMinWidth;
        private NumericUpDown NumPadMinHeight;
        private NumericUpDown NumPadMaxHeight;
        private TableLayoutPanel TblPnlBottom;
        private Button BtnDecodeROI;
        private GroupBox GrpBlobFilter;
        private TableLayoutPanel TblPnlRotationAngle;
        private Label LblRotationAngle;
        private ListBox lstGS1_Repo;
        private TableLayoutPanel TblImageAndRepo;
        private GroupBox grpQC_Result;
        private Label lblOverAllResult;
        private Button btnCharResult;
        private ComboBox cmbRotationAngle;
        private CheckBox chkAnchor;
        private CheckBox chkIsUseReferance;
        private ComboBox cmbBarcodeFormat;
        private Label lblBarcodeFormat;
        private GroupBox grpBoxAdvancedMode;
        private TableLayoutPanel tableLayoutPanel2;
        //private CheckBox chkIsBarcodeFormatAuto;
        private CheckBox chkBarcodeAdvancedMode;
        private CheckBox chkBarcodeFormatAuto;
        private TableLayoutPanel tblPnlgs1Repo;
        private CheckBox chkGradingRepo;
        private PanZoomViewer pzPreview;
    }
}
