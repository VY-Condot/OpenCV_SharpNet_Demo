using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace OpenCV_SharpNet_Demo.UserControls
{
    partial class ROIControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ROIControl));
            TblPNlMain = new TableLayoutPanel();
            GrpBoxSegMentMode = new GroupBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            CmbSegments = new ComboBox();
            GrpBoxMorph = new GroupBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            NumPadMorphIteration = new NumericUpDown();
            lblMorphMode = new Label();
            lblMorphIternation = new Label();
            lblMorphKeranalWidth = new Label();
            NumPadMorphKeranalWidth = new NumericUpDown();
            tblPnlMorphMode = new TableLayoutPanel();
            rdMorphModeDilate = new RadioButton();
            rdMorphModeErode = new RadioButton();
            rdMorphModeNone = new RadioButton();
            TblPnlRotationAngle = new TableLayoutPanel();
            LblRotationAngle = new Label();
            cmbRotationAngle = new ComboBox();
            chkAnchor = new CheckBox();
            chkIsUseReferance = new CheckBox();
            TblPnlBottom = new TableLayoutPanel();
            BtnDecodeROI = new Button();
            btnCharResult = new Button();
            GrpRoiData = new GroupBox();
            TblPnlRoiOcrData = new TableLayoutPanel();
            LblExpected = new Label();
            LblDecoded = new Label();
            lblOverAllResult = new Label();
            LblDecodedThr = new Label();
            LblDecodedThresold = new Label();
            TxtExpected = new TextBox();
            TxtDecoded = new TextBox();
            LblExpectedThresold = new Label();
            TxtExpectedThr = new TextBox();
            chkDoOCR = new CheckBox();
            pnlBtnAndAdavancedBarcode = new Panel();
            btnCopyDecodedText = new Button();
            GrpBlobFilter = new GroupBox();
            TblPnlBlobFil = new TableLayoutPanel();
            NumPadMaxWidth = new NumericUpDown();
            LblBlobMaxWidth = new Label();
            LblBlobMinWidth = new Label();
            LblBlobMinHeight = new Label();
            LblBlobMaxHeight = new Label();
            NumPadMinWidth = new NumericUpDown();
            NumPadMinHeight = new NumericUpDown();
            NumPadMaxHeight = new NumericUpDown();
            pzPreview = new PanZoomViewer();
            TblPNlMain.SuspendLayout();
            GrpBoxSegMentMode.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            GrpBoxMorph.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)NumPadMorphIteration).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumPadMorphKeranalWidth).BeginInit();
            tblPnlMorphMode.SuspendLayout();
            TblPnlRotationAngle.SuspendLayout();
            TblPnlBottom.SuspendLayout();
            GrpRoiData.SuspendLayout();
            TblPnlRoiOcrData.SuspendLayout();
            pnlBtnAndAdavancedBarcode.SuspendLayout();
            GrpBlobFilter.SuspendLayout();
            TblPnlBlobFil.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)NumPadMaxWidth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumPadMinWidth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumPadMinHeight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumPadMaxHeight).BeginInit();
            SuspendLayout();
            // 
            // TblPNlMain
            // 
            TblPNlMain.ColumnCount = 1;
            TblPNlMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TblPNlMain.Controls.Add(GrpBoxSegMentMode, 0, 0);
            TblPNlMain.Controls.Add(GrpBoxMorph, 0, 4);
            TblPNlMain.Controls.Add(TblPnlRotationAngle, 0, 3);
            TblPNlMain.Controls.Add(TblPnlBottom, 0, 6);
            TblPNlMain.Controls.Add(GrpRoiData, 0, 1);
            TblPNlMain.Controls.Add(GrpBlobFilter, 0, 2);
            TblPNlMain.Controls.Add(pzPreview, 0, 5);
            TblPNlMain.Dock = DockStyle.Fill;
            TblPNlMain.Location = new Point(0, 0);
            TblPNlMain.Name = "TblPNlMain";
            TblPNlMain.RowCount = 5;
            TblPNlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 78F));
            TblPNlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 241F));
            TblPNlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 102F));
            TblPNlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 49F));
            TblPNlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 112F));
            TblPNlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 101F));
            TblPNlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 8F));
            TblPNlMain.Size = new Size(597, 737);
            TblPNlMain.TabIndex = 0;
            // 
            // GrpBoxSegMentMode
            // 
            GrpBoxSegMentMode.Controls.Add(tableLayoutPanel2);
            GrpBoxSegMentMode.Dock = DockStyle.Fill;
            GrpBoxSegMentMode.Location = new Point(3, 3);
            GrpBoxSegMentMode.Name = "GrpBoxSegMentMode";
            GrpBoxSegMentMode.Size = new Size(591, 72);
            GrpBoxSegMentMode.TabIndex = 8;
            GrpBoxSegMentMode.TabStop = false;
            GrpBoxSegMentMode.Text = "Segmentation Mode";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 274F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Controls.Add(CmbSegments, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 24);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(585, 45);
            tableLayoutPanel2.TabIndex = 6;
            // 
            // CmbSegments
            // 
            CmbSegments.Dock = DockStyle.Fill;
            CmbSegments.FormattingEnabled = true;
            CmbSegments.Location = new Point(3, 8);
            CmbSegments.Margin = new Padding(3, 8, 3, 3);
            CmbSegments.Name = "CmbSegments";
            CmbSegments.Size = new Size(268, 28);
            CmbSegments.TabIndex = 3;
            CmbSegments.SelectedIndexChanged += CmbSegments_SelectedIndexChanged;
            // 
            // GrpBoxMorph
            // 
            GrpBoxMorph.Controls.Add(tableLayoutPanel1);
            GrpBoxMorph.Dock = DockStyle.Fill;
            GrpBoxMorph.Location = new Point(3, 473);
            GrpBoxMorph.Name = "GrpBoxMorph";
            GrpBoxMorph.Size = new Size(591, 106);
            GrpBoxMorph.TabIndex = 6;
            GrpBoxMorph.TabStop = false;
            GrpBoxMorph.Text = "Morphological operations";
            GrpBoxMorph.UseCompatibleTextRendering = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 7;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 116F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 62F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 135F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 59F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 122F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(NumPadMorphIteration, 3, 1);
            tableLayoutPanel1.Controls.Add(lblMorphMode, 0, 0);
            tableLayoutPanel1.Controls.Add(lblMorphIternation, 2, 1);
            tableLayoutPanel1.Controls.Add(lblMorphKeranalWidth, 0, 1);
            tableLayoutPanel1.Controls.Add(NumPadMorphKeranalWidth, 1, 1);
            tableLayoutPanel1.Controls.Add(tblPnlMorphMode, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 24);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 53.3333321F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 46.6666679F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(585, 79);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // NumPadMorphIteration
            // 
            NumPadMorphIteration.Dock = DockStyle.Fill;
            NumPadMorphIteration.Enabled = false;
            NumPadMorphIteration.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            NumPadMorphIteration.Location = new Point(316, 45);
            NumPadMorphIteration.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            NumPadMorphIteration.Name = "NumPadMorphIteration";
            NumPadMorphIteration.Size = new Size(53, 28);
            NumPadMorphIteration.TabIndex = 11;
            // 
            // lblMorphMode
            // 
            lblMorphMode.AutoSize = true;
            lblMorphMode.Dock = DockStyle.Fill;
            lblMorphMode.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold);
            lblMorphMode.Location = new Point(3, 3);
            lblMorphMode.Margin = new Padding(3);
            lblMorphMode.Name = "lblMorphMode";
            lblMorphMode.Size = new Size(110, 36);
            lblMorphMode.TabIndex = 12;
            lblMorphMode.Text = "Morph Mode: ";
            lblMorphMode.TextAlign = ContentAlignment.MiddleLeft;
            lblMorphMode.UseCompatibleTextRendering = true;
            // 
            // lblMorphIternation
            // 
            lblMorphIternation.AutoSize = true;
            lblMorphIternation.Dock = DockStyle.Fill;
            lblMorphIternation.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMorphIternation.Location = new Point(181, 45);
            lblMorphIternation.Margin = new Padding(3);
            lblMorphIternation.Name = "lblMorphIternation";
            lblMorphIternation.Size = new Size(129, 31);
            lblMorphIternation.TabIndex = 10;
            lblMorphIternation.Text = "Morph Iteration :";
            lblMorphIternation.TextAlign = ContentAlignment.MiddleLeft;
            lblMorphIternation.UseCompatibleTextRendering = true;
            // 
            // lblMorphKeranalWidth
            // 
            lblMorphKeranalWidth.AutoSize = true;
            lblMorphKeranalWidth.Dock = DockStyle.Fill;
            lblMorphKeranalWidth.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMorphKeranalWidth.Location = new Point(3, 45);
            lblMorphKeranalWidth.Margin = new Padding(3);
            lblMorphKeranalWidth.Name = "lblMorphKeranalWidth";
            lblMorphKeranalWidth.Size = new Size(110, 31);
            lblMorphKeranalWidth.TabIndex = 0;
            lblMorphKeranalWidth.Text = "Kernel Size :";
            lblMorphKeranalWidth.TextAlign = ContentAlignment.MiddleLeft;
            lblMorphKeranalWidth.UseCompatibleTextRendering = true;
            // 
            // NumPadMorphKeranalWidth
            // 
            NumPadMorphKeranalWidth.Dock = DockStyle.Fill;
            NumPadMorphKeranalWidth.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            NumPadMorphKeranalWidth.Location = new Point(119, 45);
            NumPadMorphKeranalWidth.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            NumPadMorphKeranalWidth.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            NumPadMorphKeranalWidth.Name = "NumPadMorphKeranalWidth";
            NumPadMorphKeranalWidth.Size = new Size(56, 28);
            NumPadMorphKeranalWidth.TabIndex = 6;
            NumPadMorphKeranalWidth.Value = new decimal(new int[] { 1, 0, 0, 0 });
            NumPadMorphKeranalWidth.ValueChanged += NumPadMorphKeranalWidth_ValueChanged;
            // 
            // tblPnlMorphMode
            // 
            tblPnlMorphMode.ColumnCount = 3;
            tableLayoutPanel1.SetColumnSpan(tblPnlMorphMode, 5);
            tblPnlMorphMode.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17.36111F));
            tblPnlMorphMode.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17.12963F));
            tblPnlMorphMode.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65.50926F));
            tblPnlMorphMode.Controls.Add(rdMorphModeDilate, 2, 0);
            tblPnlMorphMode.Controls.Add(rdMorphModeErode, 1, 0);
            tblPnlMorphMode.Controls.Add(rdMorphModeNone, 0, 0);
            tblPnlMorphMode.Dock = DockStyle.Fill;
            tblPnlMorphMode.Location = new Point(119, 3);
            tblPnlMorphMode.Name = "tblPnlMorphMode";
            tblPnlMorphMode.RowCount = 1;
            tblPnlMorphMode.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblPnlMorphMode.Size = new Size(432, 36);
            tblPnlMorphMode.TabIndex = 14;
            // 
            // rdMorphModeDilate
            // 
            rdMorphModeDilate.AutoSize = true;
            rdMorphModeDilate.Dock = DockStyle.Fill;
            rdMorphModeDilate.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdMorphModeDilate.Location = new Point(152, 3);
            rdMorphModeDilate.Name = "rdMorphModeDilate";
            rdMorphModeDilate.Size = new Size(277, 30);
            rdMorphModeDilate.TabIndex = 2;
            rdMorphModeDilate.TabStop = true;
            rdMorphModeDilate.Text = "Dilate";
            rdMorphModeDilate.UseCompatibleTextRendering = true;
            rdMorphModeDilate.UseVisualStyleBackColor = true;
            rdMorphModeDilate.Click += RdMorphModeDilate_Click;
            // 
            // rdMorphModeErode
            // 
            rdMorphModeErode.AutoSize = true;
            rdMorphModeErode.Dock = DockStyle.Fill;
            rdMorphModeErode.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdMorphModeErode.Location = new Point(78, 3);
            rdMorphModeErode.Name = "rdMorphModeErode";
            rdMorphModeErode.Size = new Size(68, 30);
            rdMorphModeErode.TabIndex = 1;
            rdMorphModeErode.TabStop = true;
            rdMorphModeErode.Text = "Erode";
            rdMorphModeErode.UseCompatibleTextRendering = true;
            rdMorphModeErode.UseVisualStyleBackColor = true;
            rdMorphModeErode.Click += RdMorphModeErode_Click;
            // 
            // rdMorphModeNone
            // 
            rdMorphModeNone.AutoSize = true;
            rdMorphModeNone.Dock = DockStyle.Fill;
            rdMorphModeNone.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdMorphModeNone.Location = new Point(3, 3);
            rdMorphModeNone.Name = "rdMorphModeNone";
            rdMorphModeNone.Size = new Size(69, 30);
            rdMorphModeNone.TabIndex = 0;
            rdMorphModeNone.TabStop = true;
            rdMorphModeNone.Text = "None";
            rdMorphModeNone.UseCompatibleTextRendering = true;
            rdMorphModeNone.UseVisualStyleBackColor = true;
            rdMorphModeNone.Click += RdMorphModeNone_Click;
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
            TblPnlRotationAngle.Location = new Point(3, 424);
            TblPnlRotationAngle.Name = "TblPnlRotationAngle";
            TblPnlRotationAngle.RowCount = 1;
            TblPnlRotationAngle.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblPnlRotationAngle.Size = new Size(591, 43);
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
            LblRotationAngle.Size = new Size(84, 37);
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
            chkAnchor.Size = new Size(143, 37);
            chkAnchor.TabIndex = 9;
            chkAnchor.Text = "Enable Anchor";
            chkAnchor.UseCompatibleTextRendering = true;
            chkAnchor.UseVisualStyleBackColor = true;
            chkAnchor.CheckedChanged += ChkAnchor_CheckedChanged;
            // 
            // chkIsUseReferance
            // 
            chkIsUseReferance.Dock = DockStyle.Fill;
            chkIsUseReferance.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold);
            chkIsUseReferance.Location = new Point(439, 3);
            chkIsUseReferance.Name = "chkIsUseReferance";
            chkIsUseReferance.Size = new Size(149, 37);
            chkIsUseReferance.TabIndex = 10;
            chkIsUseReferance.Text = "Use Ref. Roi";
            chkIsUseReferance.CheckedChanged += ChkIsUseReferance_CheckedChanged;
            // 
            // TblPnlBottom
            // 
            TblPnlBottom.ColumnCount = 4;
            TblPnlBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            TblPnlBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 163F));
            TblPnlBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 155F));
            TblPnlBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            TblPnlBottom.Controls.Add(BtnDecodeROI, 1, 0);
            TblPnlBottom.Controls.Add(btnCharResult, 2, 0);
            TblPnlBottom.Dock = DockStyle.Fill;
            TblPnlBottom.Location = new Point(3, 686);
            TblPnlBottom.Name = "TblPnlBottom";
            TblPnlBottom.RowCount = 1;
            TblPnlBottom.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblPnlBottom.Size = new Size(591, 48);
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
            BtnDecodeROI.Location = new Point(139, 3);
            BtnDecodeROI.Name = "BtnDecodeROI";
            BtnDecodeROI.Padding = new Padding(10, 0, 0, 0);
            BtnDecodeROI.Size = new Size(157, 42);
            BtnDecodeROI.TabIndex = 2;
            BtnDecodeROI.Text = "Decode ROI";
            BtnDecodeROI.TextImageRelation = TextImageRelation.ImageBeforeText;
            BtnDecodeROI.UseCompatibleTextRendering = true;
            BtnDecodeROI.UseVisualStyleBackColor = false;
            BtnDecodeROI.Click += BtnDecodeROI_Click;
            // 
            // btnCharResult
            // 
            btnCharResult.BackColor = Color.Red;
            btnCharResult.Dock = DockStyle.Fill;
            btnCharResult.Font = new Font("Calibri", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCharResult.ForeColor = Color.Transparent;
            btnCharResult.Image = (Image)resources.GetObject("btnCharResult.Image");
            btnCharResult.ImageAlign = ContentAlignment.MiddleLeft;
            btnCharResult.Location = new Point(302, 3);
            btnCharResult.Name = "btnCharResult";
            btnCharResult.Padding = new Padding(10, 0, 0, 0);
            btnCharResult.Size = new Size(149, 42);
            btnCharResult.TabIndex = 3;
            btnCharResult.Text = "Char Result";
            btnCharResult.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCharResult.UseCompatibleTextRendering = true;
            btnCharResult.UseVisualStyleBackColor = false;
            btnCharResult.Click += BtnCharResult_Click;
            // 
            // GrpRoiData
            // 
            GrpRoiData.BackColor = Color.Transparent;
            GrpRoiData.Controls.Add(TblPnlRoiOcrData);
            GrpRoiData.Dock = DockStyle.Fill;
            GrpRoiData.Location = new Point(3, 81);
            GrpRoiData.Name = "GrpRoiData";
            GrpRoiData.Size = new Size(591, 235);
            GrpRoiData.TabIndex = 0;
            GrpRoiData.TabStop = false;
            GrpRoiData.Text = "ROI Name";
            GrpRoiData.UseCompatibleTextRendering = true;
            // 
            // TblPnlRoiOcrData
            // 
            TblPnlRoiOcrData.ColumnCount = 5;
            TblPnlRoiOcrData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 87F));
            TblPnlRoiOcrData.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TblPnlRoiOcrData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 63F));
            TblPnlRoiOcrData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 46F));
            TblPnlRoiOcrData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 139F));
            TblPnlRoiOcrData.Controls.Add(LblExpected, 0, 0);
            TblPnlRoiOcrData.Controls.Add(LblDecoded, 0, 1);
            TblPnlRoiOcrData.Controls.Add(lblOverAllResult, 4, 1);
            TblPnlRoiOcrData.Controls.Add(LblDecodedThr, 3, 1);
            TblPnlRoiOcrData.Controls.Add(LblDecodedThresold, 2, 1);
            TblPnlRoiOcrData.Controls.Add(TxtExpected, 1, 0);
            TblPnlRoiOcrData.Controls.Add(TxtDecoded, 1, 1);
            TblPnlRoiOcrData.Controls.Add(LblExpectedThresold, 2, 0);
            TblPnlRoiOcrData.Controls.Add(TxtExpectedThr, 3, 0);
            TblPnlRoiOcrData.Controls.Add(chkDoOCR, 4, 0);
            TblPnlRoiOcrData.Controls.Add(pnlBtnAndAdavancedBarcode, 1, 2);
            TblPnlRoiOcrData.Dock = DockStyle.Fill;
            TblPnlRoiOcrData.Location = new Point(3, 24);
            TblPnlRoiOcrData.Name = "TblPnlRoiOcrData";
            TblPnlRoiOcrData.RowCount = 3;
            TblPnlRoiOcrData.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            TblPnlRoiOcrData.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            TblPnlRoiOcrData.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            TblPnlRoiOcrData.Size = new Size(585, 208);
            TblPnlRoiOcrData.TabIndex = 0;
            // 
            // LblExpected
            // 
            LblExpected.AutoSize = true;
            LblExpected.Dock = DockStyle.Fill;
            LblExpected.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblExpected.Location = new Point(3, 3);
            LblExpected.Margin = new Padding(3);
            LblExpected.Name = "LblExpected";
            LblExpected.Size = new Size(81, 78);
            LblExpected.TabIndex = 0;
            LblExpected.Text = "Expected : ";
            LblExpected.TextAlign = ContentAlignment.MiddleLeft;
            LblExpected.UseCompatibleTextRendering = true;
            // 
            // LblDecoded
            // 
            LblDecoded.AutoSize = true;
            LblDecoded.Dock = DockStyle.Fill;
            LblDecoded.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblDecoded.Location = new Point(3, 87);
            LblDecoded.Margin = new Padding(3);
            LblDecoded.Name = "LblDecoded";
            LblDecoded.Size = new Size(81, 78);
            LblDecoded.TabIndex = 1;
            LblDecoded.Text = "Decoded : ";
            LblDecoded.TextAlign = ContentAlignment.MiddleLeft;
            LblDecoded.UseCompatibleTextRendering = true;
            // 
            // lblOverAllResult
            // 
            lblOverAllResult.AutoSize = true;
            lblOverAllResult.Dock = DockStyle.Fill;
            lblOverAllResult.Font = new Font("Palatino Linotype", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblOverAllResult.Location = new Point(449, 87);
            lblOverAllResult.Margin = new Padding(3);
            lblOverAllResult.Name = "lblOverAllResult";
            lblOverAllResult.Size = new Size(133, 78);
            lblOverAllResult.TabIndex = 9;
            lblOverAllResult.Text = "Pass/Fail";
            lblOverAllResult.TextAlign = ContentAlignment.MiddleLeft;
            lblOverAllResult.UseCompatibleTextRendering = true;
            // 
            // LblDecodedThr
            // 
            LblDecodedThr.BackColor = Color.White;
            LblDecodedThr.BorderStyle = BorderStyle.FixedSingle;
            LblDecodedThr.Dock = DockStyle.Fill;
            LblDecodedThr.FlatStyle = FlatStyle.Flat;
            LblDecodedThr.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblDecodedThr.Location = new Point(403, 109);
            LblDecodedThr.Margin = new Padding(3, 25, 3, 25);
            LblDecodedThr.Name = "LblDecodedThr";
            LblDecodedThr.Padding = new Padding(2, 0, 0, 0);
            LblDecodedThr.Size = new Size(40, 34);
            LblDecodedThr.TabIndex = 6;
            LblDecodedThr.Text = "0.8";
            LblDecodedThr.TextAlign = ContentAlignment.MiddleLeft;
            LblDecodedThr.UseCompatibleTextRendering = true;
            // 
            // LblDecodedThresold
            // 
            LblDecodedThresold.AutoSize = true;
            LblDecodedThresold.Dock = DockStyle.Fill;
            LblDecodedThresold.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblDecodedThresold.Location = new Point(340, 87);
            LblDecodedThresold.Margin = new Padding(3);
            LblDecodedThresold.Name = "LblDecodedThresold";
            LblDecodedThresold.Size = new Size(57, 78);
            LblDecodedThresold.TabIndex = 5;
            LblDecodedThresold.Text = "Avg Score: ";
            LblDecodedThresold.TextAlign = ContentAlignment.MiddleLeft;
            LblDecodedThresold.UseCompatibleTextRendering = true;
            // 
            // TxtExpected
            // 
            TxtExpected.BorderStyle = BorderStyle.FixedSingle;
            TxtExpected.Dock = DockStyle.Fill;
            TxtExpected.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            TxtExpected.Location = new Point(90, 3);
            TxtExpected.Multiline = true;
            TxtExpected.Name = "TxtExpected";
            TxtExpected.Size = new Size(244, 78);
            TxtExpected.TabIndex = 2;
            TxtExpected.TextChanged += TxtExpected_TextChanged;
            TxtExpected.KeyDown += TxtExpected_KeyDown;
            // 
            // TxtDecoded
            // 
            TxtDecoded.BackColor = Color.White;
            TxtDecoded.BorderStyle = BorderStyle.FixedSingle;
            TxtDecoded.Dock = DockStyle.Fill;
            TxtDecoded.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            TxtDecoded.Location = new Point(90, 87);
            TxtDecoded.Multiline = true;
            TxtDecoded.Name = "TxtDecoded";
            TxtDecoded.ReadOnly = true;
            TxtDecoded.Size = new Size(244, 78);
            TxtDecoded.TabIndex = 3;
            // 
            // LblExpectedThresold
            // 
            LblExpectedThresold.AutoSize = true;
            LblExpectedThresold.Dock = DockStyle.Fill;
            LblExpectedThresold.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblExpectedThresold.Location = new Point(340, 3);
            LblExpectedThresold.Margin = new Padding(3);
            LblExpectedThresold.Name = "LblExpectedThresold";
            LblExpectedThresold.Size = new Size(57, 78);
            LblExpectedThresold.TabIndex = 4;
            LblExpectedThresold.Text = "Char Thr :";
            LblExpectedThresold.TextAlign = ContentAlignment.MiddleLeft;
            LblExpectedThresold.UseCompatibleTextRendering = true;
            // 
            // TxtExpectedThr
            // 
            TxtExpectedThr.BorderStyle = BorderStyle.FixedSingle;
            TxtExpectedThr.Dock = DockStyle.Fill;
            TxtExpectedThr.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            TxtExpectedThr.Location = new Point(403, 25);
            TxtExpectedThr.Margin = new Padding(3, 25, 3, 25);
            TxtExpectedThr.Multiline = true;
            TxtExpectedThr.Name = "TxtExpectedThr";
            TxtExpectedThr.Size = new Size(40, 34);
            TxtExpectedThr.TabIndex = 7;
            TxtExpectedThr.Text = "0.5";
            TxtExpectedThr.TextChanged += TxtExpectedThr_TextChanged;
            TxtExpectedThr.KeyDown += TxtExpectedThr_KeyDown;
            // 
            // chkDoOCR
            // 
            chkDoOCR.AutoSize = true;
            chkDoOCR.Dock = DockStyle.Fill;
            chkDoOCR.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkDoOCR.Location = new Point(449, 3);
            chkDoOCR.Name = "chkDoOCR";
            chkDoOCR.Size = new Size(133, 78);
            chkDoOCR.TabIndex = 8;
            chkDoOCR.Text = "OCR (No Exp.)";
            chkDoOCR.UseCompatibleTextRendering = true;
            chkDoOCR.UseVisualStyleBackColor = true;
            chkDoOCR.CheckedChanged += chkDoOCR_CheckedChanged;
            // 
            // pnlBtnAndAdavancedBarcode
            // 
            pnlBtnAndAdavancedBarcode.Controls.Add(btnCopyDecodedText);
            pnlBtnAndAdavancedBarcode.Dock = DockStyle.Fill;
            pnlBtnAndAdavancedBarcode.Location = new Point(87, 168);
            pnlBtnAndAdavancedBarcode.Margin = new Padding(0);
            pnlBtnAndAdavancedBarcode.Name = "pnlBtnAndAdavancedBarcode";
            pnlBtnAndAdavancedBarcode.Size = new Size(250, 40);
            pnlBtnAndAdavancedBarcode.TabIndex = 11;
            // 
            // btnCopyDecodedText
            // 
            btnCopyDecodedText.BackColor = Color.Red;
            btnCopyDecodedText.Dock = DockStyle.Fill;
            btnCopyDecodedText.Font = new Font("Calibri", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCopyDecodedText.ForeColor = Color.Transparent;
            btnCopyDecodedText.ImageAlign = ContentAlignment.MiddleLeft;
            btnCopyDecodedText.Location = new Point(0, 0);
            btnCopyDecodedText.Margin = new Padding(10, 0, 10, 0);
            btnCopyDecodedText.Name = "btnCopyDecodedText";
            btnCopyDecodedText.Padding = new Padding(10, 0, 0, 0);
            btnCopyDecodedText.Size = new Size(250, 40);
            btnCopyDecodedText.TabIndex = 10;
            btnCopyDecodedText.Text = "Copy Decoded Text";
            btnCopyDecodedText.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCopyDecodedText.UseCompatibleTextRendering = true;
            btnCopyDecodedText.UseVisualStyleBackColor = false;
            btnCopyDecodedText.Visible = false;
            btnCopyDecodedText.Click += BtnCopyDecodedText_Click;
            // 
            // GrpBlobFilter
            // 
            GrpBlobFilter.Controls.Add(TblPnlBlobFil);
            GrpBlobFilter.Dock = DockStyle.Fill;
            GrpBlobFilter.Location = new Point(3, 322);
            GrpBlobFilter.Name = "GrpBlobFilter";
            GrpBlobFilter.Size = new Size(591, 96);
            GrpBlobFilter.TabIndex = 4;
            GrpBlobFilter.TabStop = false;
            GrpBlobFilter.Text = "Blob Filter";
            GrpBlobFilter.UseCompatibleTextRendering = true;
            // 
            // TblPnlBlobFil
            // 
            TblPnlBlobFil.ColumnCount = 5;
            TblPnlBlobFil.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 104F));
            TblPnlBlobFil.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 77F));
            TblPnlBlobFil.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 104F));
            TblPnlBlobFil.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 79F));
            TblPnlBlobFil.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TblPnlBlobFil.Controls.Add(NumPadMaxWidth, 3, 0);
            TblPnlBlobFil.Controls.Add(LblBlobMaxWidth, 2, 0);
            TblPnlBlobFil.Controls.Add(LblBlobMinWidth, 0, 0);
            TblPnlBlobFil.Controls.Add(LblBlobMinHeight, 0, 1);
            TblPnlBlobFil.Controls.Add(LblBlobMaxHeight, 2, 1);
            TblPnlBlobFil.Controls.Add(NumPadMinWidth, 1, 0);
            TblPnlBlobFil.Controls.Add(NumPadMinHeight, 1, 1);
            TblPnlBlobFil.Controls.Add(NumPadMaxHeight, 3, 1);
            TblPnlBlobFil.Dock = DockStyle.Fill;
            TblPnlBlobFil.Location = new Point(3, 24);
            TblPnlBlobFil.Name = "TblPnlBlobFil";
            TblPnlBlobFil.RowCount = 2;
            TblPnlBlobFil.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            TblPnlBlobFil.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            TblPnlBlobFil.Size = new Size(585, 69);
            TblPnlBlobFil.TabIndex = 1;
            // 
            // NumPadMaxWidth
            // 
            NumPadMaxWidth.Dock = DockStyle.Fill;
            NumPadMaxWidth.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            NumPadMaxWidth.Location = new Point(288, 3);
            NumPadMaxWidth.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            NumPadMaxWidth.Name = "NumPadMaxWidth";
            NumPadMaxWidth.Size = new Size(73, 28);
            NumPadMaxWidth.TabIndex = 8;
            NumPadMaxWidth.ValueChanged += NumPadMaxWidth_ValueChanged;
            // 
            // LblBlobMaxWidth
            // 
            LblBlobMaxWidth.AutoSize = true;
            LblBlobMaxWidth.Dock = DockStyle.Fill;
            LblBlobMaxWidth.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblBlobMaxWidth.Location = new Point(184, 3);
            LblBlobMaxWidth.Margin = new Padding(3);
            LblBlobMaxWidth.Name = "LblBlobMaxWidth";
            LblBlobMaxWidth.Size = new Size(98, 28);
            LblBlobMaxWidth.TabIndex = 4;
            LblBlobMaxWidth.Text = "Max Width :";
            LblBlobMaxWidth.TextAlign = ContentAlignment.MiddleLeft;
            LblBlobMaxWidth.UseCompatibleTextRendering = true;
            // 
            // LblBlobMinWidth
            // 
            LblBlobMinWidth.AutoSize = true;
            LblBlobMinWidth.Dock = DockStyle.Fill;
            LblBlobMinWidth.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblBlobMinWidth.Location = new Point(3, 3);
            LblBlobMinWidth.Margin = new Padding(3);
            LblBlobMinWidth.Name = "LblBlobMinWidth";
            LblBlobMinWidth.Size = new Size(98, 28);
            LblBlobMinWidth.TabIndex = 0;
            LblBlobMinWidth.Text = "Min Width : ";
            LblBlobMinWidth.TextAlign = ContentAlignment.MiddleLeft;
            LblBlobMinWidth.UseCompatibleTextRendering = true;
            // 
            // LblBlobMinHeight
            // 
            LblBlobMinHeight.AutoSize = true;
            LblBlobMinHeight.Dock = DockStyle.Fill;
            LblBlobMinHeight.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblBlobMinHeight.Location = new Point(3, 37);
            LblBlobMinHeight.Margin = new Padding(3);
            LblBlobMinHeight.Name = "LblBlobMinHeight";
            LblBlobMinHeight.Size = new Size(98, 29);
            LblBlobMinHeight.TabIndex = 1;
            LblBlobMinHeight.Text = "Min Height : ";
            LblBlobMinHeight.TextAlign = ContentAlignment.MiddleLeft;
            LblBlobMinHeight.UseCompatibleTextRendering = true;
            // 
            // LblBlobMaxHeight
            // 
            LblBlobMaxHeight.AutoSize = true;
            LblBlobMaxHeight.Dock = DockStyle.Fill;
            LblBlobMaxHeight.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblBlobMaxHeight.Location = new Point(184, 37);
            LblBlobMaxHeight.Margin = new Padding(3);
            LblBlobMaxHeight.Name = "LblBlobMaxHeight";
            LblBlobMaxHeight.Size = new Size(98, 29);
            LblBlobMaxHeight.TabIndex = 5;
            LblBlobMaxHeight.Text = "Max Height : ";
            LblBlobMaxHeight.TextAlign = ContentAlignment.MiddleLeft;
            LblBlobMaxHeight.UseCompatibleTextRendering = true;
            // 
            // NumPadMinWidth
            // 
            NumPadMinWidth.Dock = DockStyle.Fill;
            NumPadMinWidth.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            NumPadMinWidth.Location = new Point(107, 3);
            NumPadMinWidth.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            NumPadMinWidth.Name = "NumPadMinWidth";
            NumPadMinWidth.Size = new Size(71, 28);
            NumPadMinWidth.TabIndex = 6;
            NumPadMinWidth.ValueChanged += NumPadMinWidth_ValueChanged;
            // 
            // NumPadMinHeight
            // 
            NumPadMinHeight.Dock = DockStyle.Fill;
            NumPadMinHeight.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            NumPadMinHeight.Location = new Point(107, 37);
            NumPadMinHeight.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            NumPadMinHeight.Name = "NumPadMinHeight";
            NumPadMinHeight.Size = new Size(71, 28);
            NumPadMinHeight.TabIndex = 7;
            NumPadMinHeight.ValueChanged += NumPadMinHeight_ValueChanged;
            // 
            // NumPadMaxHeight
            // 
            NumPadMaxHeight.Dock = DockStyle.Fill;
            NumPadMaxHeight.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            NumPadMaxHeight.Location = new Point(288, 37);
            NumPadMaxHeight.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            NumPadMaxHeight.Name = "NumPadMaxHeight";
            NumPadMaxHeight.Size = new Size(73, 28);
            NumPadMaxHeight.TabIndex = 9;
            NumPadMaxHeight.ValueChanged += NumPadMaxHeight_ValueChanged;
            // 
            // pzPreview
            // 
            pzPreview.Dock = DockStyle.Fill;
            pzPreview.Image = null;
            pzPreview.Interpolation = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            pzPreview.Location = new Point(3, 585);
            pzPreview.Name = "pzPreview";
            pzPreview.Size = new Size(591, 95);
            pzPreview.TabIndex = 7;
            // 
            // ROIControl
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(TblPNlMain);
            DoubleBuffered = true;
            Font = new Font("Palatino Linotype", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "ROIControl";
            Size = new Size(597, 737);
            TblPNlMain.ResumeLayout(false);
            GrpBoxSegMentMode.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            GrpBoxMorph.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)NumPadMorphIteration).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumPadMorphKeranalWidth).EndInit();
            tblPnlMorphMode.ResumeLayout(false);
            tblPnlMorphMode.PerformLayout();
            TblPnlRotationAngle.ResumeLayout(false);
            TblPnlRotationAngle.PerformLayout();
            TblPnlBottom.ResumeLayout(false);
            GrpRoiData.ResumeLayout(false);
            TblPnlRoiOcrData.ResumeLayout(false);
            TblPnlRoiOcrData.PerformLayout();
            pnlBtnAndAdavancedBarcode.ResumeLayout(false);
            GrpBlobFilter.ResumeLayout(false);
            TblPnlBlobFil.ResumeLayout(false);
            TblPnlBlobFil.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)NumPadMaxWidth).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumPadMinWidth).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumPadMinHeight).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumPadMaxHeight).EndInit();
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
        private CheckBox chkDoOCR;
        private TableLayoutPanel TblPnlBlobFil;
        private Label LblBlobMaxWidth;
        private Label LblBlobMinWidth;
        private Label LblBlobMinHeight;
        private Label LblBlobMaxHeight;
        private NumericUpDown NumPadMaxWidth;
        private NumericUpDown NumPadMinWidth;
        private NumericUpDown NumPadMinHeight;
        private NumericUpDown NumPadMaxHeight;
        private GroupBox GrpBlobFilter;
        private TableLayoutPanel TblPnlRotationAngle;
        private Label LblRotationAngle;
        private GroupBox GrpBoxMorph;
        private TableLayoutPanel tableLayoutPanel1;
        private Label lblMorphKeranalWidth;
        private NumericUpDown NumPadMorphKeranalWidth;
        private NumericUpDown NumPadMorphIteration;
        private Label lblMorphIternation;
        private Label lblMorphMode;
        private TableLayoutPanel tblPnlMorphMode;
        private RadioButton rdMorphModeDilate;
        private RadioButton rdMorphModeErode;
        private RadioButton rdMorphModeNone;
        private Label lblOverAllResult;
        private ComboBox cmbRotationAngle;
        private CheckBox chkAnchor;
        private CheckBox chkIsUseReferance;
        private Button btnCopyDecodedText;
        private Panel pnlBtnAndAdavancedBarcode;
        private TableLayoutPanel TblPnlBottom;
        private Button BtnDecodeROI;
        private Button btnCharResult;
        private PanZoomViewer pzPreview;
        private GroupBox GrpBoxSegMentMode;
        private TableLayoutPanel tableLayoutPanel2;
        private ComboBox CmbSegments;
    }
}
