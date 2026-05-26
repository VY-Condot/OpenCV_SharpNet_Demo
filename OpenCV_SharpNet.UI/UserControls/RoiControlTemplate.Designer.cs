namespace OpenCV_SharpNet.UserControls
{
    partial class RoiControlTemplate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RoiControlTemplate));
            TblPNlMain = new TableLayoutPanel();
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
            GrpRoiData = new GroupBox();
            TblPnlRoiOcrData = new TableLayoutPanel();
            LblExpectedThresold = new Label();
            TxtExpectedThr = new TextBox();
            LblDecodedThresold = new Label();
            LblDecodedThr = new Label();
            LblDecoded = new Label();
            TxtDecoded = new TextBox();
            pzPreview = new PanZoomViewer();
            TblPNlMain.SuspendLayout();
            GrpBoxMorph.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)NumPadMorphIteration).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumPadMorphKeranalWidth).BeginInit();
            tblPnlMorphMode.SuspendLayout();
            TblPnlRotationAngle.SuspendLayout();
            TblPnlBottom.SuspendLayout();
            GrpRoiData.SuspendLayout();
            TblPnlRoiOcrData.SuspendLayout();
            SuspendLayout();
            // 
            // TblPNlMain
            // 
            TblPNlMain.ColumnCount = 1;
            TblPNlMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TblPNlMain.Controls.Add(GrpBoxMorph, 0, 2);
            TblPNlMain.Controls.Add(TblPnlRotationAngle, 0, 1);
            TblPNlMain.Controls.Add(TblPnlBottom, 0, 4);
            TblPNlMain.Controls.Add(GrpRoiData, 0, 0);
            TblPNlMain.Controls.Add(pzPreview, 0, 3);
            TblPNlMain.Dock = DockStyle.Fill;
            TblPNlMain.Location = new Point(0, 0);
            TblPNlMain.Name = "TblPNlMain";
            TblPNlMain.RowCount = 3;
            TblPNlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 71F));
            TblPNlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            TblPNlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 108F));
            TblPNlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 118F));
            TblPNlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 8F));
            TblPNlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            TblPNlMain.Size = new Size(597, 397);
            TblPNlMain.TabIndex = 1;
            // 
            // GrpBoxMorph
            // 
            GrpBoxMorph.Controls.Add(tableLayoutPanel1);
            GrpBoxMorph.Dock = DockStyle.Fill;
            GrpBoxMorph.Location = new Point(3, 120);
            GrpBoxMorph.Name = "GrpBoxMorph";
            GrpBoxMorph.Size = new Size(591, 102);
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
            tableLayoutPanel1.Location = new Point(3, 23);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 53.3333321F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 46.6666679F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(585, 76);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // NumPadMorphIteration
            // 
            NumPadMorphIteration.Dock = DockStyle.Fill;
            NumPadMorphIteration.Enabled = false;
            NumPadMorphIteration.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            NumPadMorphIteration.Location = new Point(316, 43);
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
            lblMorphMode.Size = new Size(110, 34);
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
            lblMorphIternation.Location = new Point(181, 43);
            lblMorphIternation.Margin = new Padding(3);
            lblMorphIternation.Name = "lblMorphIternation";
            lblMorphIternation.Size = new Size(129, 30);
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
            lblMorphKeranalWidth.Location = new Point(3, 43);
            lblMorphKeranalWidth.Margin = new Padding(3);
            lblMorphKeranalWidth.Name = "lblMorphKeranalWidth";
            lblMorphKeranalWidth.Size = new Size(110, 30);
            lblMorphKeranalWidth.TabIndex = 0;
            lblMorphKeranalWidth.Text = "Kernel Size :";
            lblMorphKeranalWidth.TextAlign = ContentAlignment.MiddleLeft;
            lblMorphKeranalWidth.UseCompatibleTextRendering = true;
            // 
            // NumPadMorphKeranalWidth
            // 
            NumPadMorphKeranalWidth.Dock = DockStyle.Fill;
            NumPadMorphKeranalWidth.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            NumPadMorphKeranalWidth.Location = new Point(119, 43);
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
            tblPnlMorphMode.Size = new Size(432, 34);
            tblPnlMorphMode.TabIndex = 14;
            // 
            // rdMorphModeDilate
            // 
            rdMorphModeDilate.AutoSize = true;
            rdMorphModeDilate.Dock = DockStyle.Fill;
            rdMorphModeDilate.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rdMorphModeDilate.Location = new Point(152, 3);
            rdMorphModeDilate.Name = "rdMorphModeDilate";
            rdMorphModeDilate.Size = new Size(277, 28);
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
            rdMorphModeErode.Size = new Size(68, 28);
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
            rdMorphModeNone.Size = new Size(69, 28);
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
            TblPnlRotationAngle.Location = new Point(3, 74);
            TblPnlRotationAngle.Name = "TblPnlRotationAngle";
            TblPnlRotationAngle.RowCount = 1;
            TblPnlRotationAngle.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblPnlRotationAngle.Size = new Size(591, 40);
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
            LblRotationAngle.Size = new Size(84, 34);
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
            chkAnchor.Size = new Size(143, 34);
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
            chkIsUseReferance.Size = new Size(149, 34);
            chkIsUseReferance.TabIndex = 10;
            chkIsUseReferance.Text = "Use Ref. Roi";
            chkIsUseReferance.CheckedChanged += ChkIsUseReferance_CheckedChanged;
            // 
            // TblPnlBottom
            // 
            TblPnlBottom.ColumnCount = 3;
            TblPnlBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            TblPnlBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 159F));
            TblPnlBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            TblPnlBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            TblPnlBottom.Controls.Add(BtnDecodeROI, 1, 0);
            TblPnlBottom.Dock = DockStyle.Fill;
            TblPnlBottom.Location = new Point(3, 346);
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
            BtnDecodeROI.Location = new Point(219, 3);
            BtnDecodeROI.Name = "BtnDecodeROI";
            BtnDecodeROI.Padding = new Padding(10, 0, 0, 0);
            BtnDecodeROI.Size = new Size(153, 42);
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
            GrpRoiData.Size = new Size(591, 65);
            GrpRoiData.TabIndex = 0;
            GrpRoiData.TabStop = false;
            GrpRoiData.Text = "ROI Name";
            GrpRoiData.UseCompatibleTextRendering = true;
            // 
            // TblPnlRoiOcrData
            // 
            TblPnlRoiOcrData.ColumnCount = 7;
            TblPnlRoiOcrData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 89F));
            TblPnlRoiOcrData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 49F));
            TblPnlRoiOcrData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 55F));
            TblPnlRoiOcrData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            TblPnlRoiOcrData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 66F));
            TblPnlRoiOcrData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 263F));
            TblPnlRoiOcrData.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TblPnlRoiOcrData.Controls.Add(LblExpectedThresold, 0, 0);
            TblPnlRoiOcrData.Controls.Add(TxtExpectedThr, 1, 0);
            TblPnlRoiOcrData.Controls.Add(LblDecodedThresold, 2, 0);
            TblPnlRoiOcrData.Controls.Add(LblDecodedThr, 3, 0);
            TblPnlRoiOcrData.Controls.Add(LblDecoded, 4, 0);
            TblPnlRoiOcrData.Controls.Add(TxtDecoded, 5, 0);
            TblPnlRoiOcrData.Dock = DockStyle.Fill;
            TblPnlRoiOcrData.Location = new Point(3, 23);
            TblPnlRoiOcrData.Name = "TblPnlRoiOcrData";
            TblPnlRoiOcrData.RowCount = 1;
            TblPnlRoiOcrData.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblPnlRoiOcrData.Size = new Size(585, 39);
            TblPnlRoiOcrData.TabIndex = 0;
            // 
            // LblExpectedThresold
            // 
            LblExpectedThresold.AutoSize = true;
            LblExpectedThresold.Dock = DockStyle.Fill;
            LblExpectedThresold.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblExpectedThresold.Location = new Point(3, 3);
            LblExpectedThresold.Margin = new Padding(3);
            LblExpectedThresold.Name = "LblExpectedThresold";
            LblExpectedThresold.Size = new Size(83, 33);
            LblExpectedThresold.TabIndex = 4;
            LblExpectedThresold.Text = "Temp Thr :";
            LblExpectedThresold.TextAlign = ContentAlignment.MiddleLeft;
            LblExpectedThresold.UseCompatibleTextRendering = true;
            // 
            // TxtExpectedThr
            // 
            TxtExpectedThr.BorderStyle = BorderStyle.FixedSingle;
            TxtExpectedThr.Dock = DockStyle.Fill;
            TxtExpectedThr.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            TxtExpectedThr.Location = new Point(92, 3);
            TxtExpectedThr.Multiline = true;
            TxtExpectedThr.Name = "TxtExpectedThr";
            TxtExpectedThr.Size = new Size(43, 33);
            TxtExpectedThr.TabIndex = 7;
            TxtExpectedThr.Text = "0.5";
            TxtExpectedThr.TextChanged += TxtExpectedThr_TextChanged;
            // 
            // LblDecodedThresold
            // 
            LblDecodedThresold.AutoSize = true;
            LblDecodedThresold.Dock = DockStyle.Fill;
            LblDecodedThresold.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblDecodedThresold.Location = new Point(141, 3);
            LblDecodedThresold.Margin = new Padding(3);
            LblDecodedThresold.Name = "LblDecodedThresold";
            LblDecodedThresold.Size = new Size(49, 33);
            LblDecodedThresold.TabIndex = 5;
            LblDecodedThresold.Text = "Score: ";
            LblDecodedThresold.TextAlign = ContentAlignment.MiddleLeft;
            LblDecodedThresold.UseCompatibleTextRendering = true;
            // 
            // LblDecodedThr
            // 
            LblDecodedThr.BackColor = Color.White;
            LblDecodedThr.BorderStyle = BorderStyle.FixedSingle;
            LblDecodedThr.Dock = DockStyle.Fill;
            LblDecodedThr.FlatStyle = FlatStyle.Flat;
            LblDecodedThr.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblDecodedThr.Location = new Point(196, 3);
            LblDecodedThr.Margin = new Padding(3);
            LblDecodedThr.Name = "LblDecodedThr";
            LblDecodedThr.Padding = new Padding(2, 0, 0, 0);
            LblDecodedThr.Size = new Size(44, 33);
            LblDecodedThr.TabIndex = 6;
            LblDecodedThr.Text = "0.8";
            LblDecodedThr.TextAlign = ContentAlignment.MiddleLeft;
            LblDecodedThr.UseCompatibleTextRendering = true;
            // 
            // LblDecoded
            // 
            LblDecoded.AutoSize = true;
            LblDecoded.Dock = DockStyle.Fill;
            LblDecoded.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblDecoded.Location = new Point(246, 3);
            LblDecoded.Margin = new Padding(3);
            LblDecoded.Name = "LblDecoded";
            LblDecoded.Size = new Size(60, 33);
            LblDecoded.TabIndex = 1;
            LblDecoded.Text = "Result : ";
            LblDecoded.TextAlign = ContentAlignment.MiddleLeft;
            LblDecoded.UseCompatibleTextRendering = true;
            // 
            // TxtDecoded
            // 
            TxtDecoded.BackColor = Color.White;
            TxtDecoded.BorderStyle = BorderStyle.FixedSingle;
            TxtDecoded.Dock = DockStyle.Fill;
            TxtDecoded.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            TxtDecoded.Location = new Point(312, 3);
            TxtDecoded.Multiline = true;
            TxtDecoded.Name = "TxtDecoded";
            TxtDecoded.ReadOnly = true;
            TxtDecoded.Size = new Size(257, 33);
            TxtDecoded.TabIndex = 3;
            // 
            // pzPreview
            // 
            pzPreview.Dock = DockStyle.Fill;
            pzPreview.Image = null;
            pzPreview.Interpolation = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            pzPreview.Location = new Point(3, 228);
            pzPreview.Name = "pzPreview";
            pzPreview.Size = new Size(591, 112);
            pzPreview.TabIndex = 7;
            // 
            // RoiControlTemplate
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(TblPNlMain);
            Name = "RoiControlTemplate";
            Size = new Size(597, 397);
            TblPNlMain.ResumeLayout(false);
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
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel TblPNlMain;
        private GroupBox GrpBoxMorph;
        private TableLayoutPanel tableLayoutPanel1;
        private NumericUpDown NumPadMorphIteration;
        private Label lblMorphMode;
        private Label lblMorphIternation;
        private Label lblMorphKeranalWidth;
        private NumericUpDown NumPadMorphKeranalWidth;
        private TableLayoutPanel tblPnlMorphMode;
        private RadioButton rdMorphModeDilate;
        private RadioButton rdMorphModeErode;
        private RadioButton rdMorphModeNone;
        private TableLayoutPanel TblPnlRotationAngle;
        private Label LblRotationAngle;
        private ComboBox cmbRotationAngle;
        private CheckBox chkAnchor;
        private CheckBox chkIsUseReferance;
        private TableLayoutPanel TblPnlBottom;
        private Button BtnDecodeROI;
        private GroupBox GrpRoiData;
        private TableLayoutPanel TblPnlRoiOcrData;
        private TextBox TxtExpectedThr;
        private TextBox TxtDecoded;
        private Label LblDecoded;
        private Label LblDecodedThresold;
        private Label LblDecodedThr;
        private Label LblExpectedThresold;
        private PanZoomViewer pzPreview;
    }
}
