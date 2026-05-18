namespace OpenCV_SharpNet_Demo
{
    partial class CameraDemo
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CameraDemo));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            TblMain = new TableLayoutPanel();
            TblBodyMain = new TableLayoutPanel();
            SplitConImageViewAndButton = new SplitContainer();
            TblPnlButtons = new TableLayoutPanel();
            grpConfigs = new GroupBox();
            flowPnlConfig = new FlowLayoutPanel();
            BtnSaveRoi = new Button();
            BtnLoadRoi = new Button();
            BtnShowTamplate = new Button();
            grpTrainAndDecode = new GroupBox();
            flowPnlTrainAndDecode = new FlowLayoutPanel();
            BtnTrainSelROI = new Button();
            BtnDecodeAllROI = new Button();
            GrpRoiTypes = new GroupBox();
            flowLayOutPnlRoiTypes = new FlowLayoutPanel();
            BtnAddRoi = new Button();
            BtnAddBarCodeRoi = new Button();
            BtnAddTempateROI = new Button();
            splitConBodyMain = new SplitContainer();
            SplitContainerImageView = new SplitContainer();
            ImageCanvas = new PictureBox();
            dgvDecodeTextRec = new DataGridView();
            DisRoiType = new DataGridViewTextBoxColumn();
            DecodedText = new DataGridViewTextBoxColumn();
            Result = new DataGridViewTextBoxColumn();
            ThresoldValue = new DataGridViewTextBoxColumn();
            RoiAngle = new DataGridViewTextBoxColumn();
            ExecutionTime = new DataGridViewTextBoxColumn();
            DecodeTime = new DataGridViewTextBoxColumn();
            TblPnlSettingAndRoiData = new TableLayoutPanel();
            FlowPnlRoiData = new FlowLayoutPanel();
            TblBlobSettings = new TableLayoutPanel();
            GrpBoxSegMentMode = new GroupBox();
            CmbSegments = new ComboBox();
            TblPnlImageList = new TableLayoutPanel();
            TblDeviceConfig = new TableLayoutPanel();
            GrpSetParam = new GroupBox();
            TblParameter = new TableLayoutPanel();
            LblExposerTime = new Label();
            tbExposure = new TextBox();
            LblGain = new Label();
            tbGain = new TextBox();
            LblFrameRate = new Label();
            tbFrameRate = new TextBox();
            bnGetParam = new Button();
            bnSetParam = new Button();
            GrpImageAq = new GroupBox();
            TblImageAq = new TableLayoutPanel();
            bnTriggerMode = new RadioButton();
            bnContinuesMode = new RadioButton();
            bnStopGrab = new Button();
            bnStartGrab = new Button();
            cbSoftTrigger = new CheckBox();
            bnTriggerExec = new Button();
            grpDeviceConnection = new GroupBox();
            TblSearchDevice = new TableLayoutPanel();
            cbDeviceList = new ComboBox();
            TblDeviceSearch = new TableLayoutPanel();
            bnEnum = new Button();
            bnOpen = new Button();
            bnClose = new Button();
            StatusStripMenu = new StatusStrip();
            toolStripMenu = new ToolStripStatusLabel();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            tsslUserManual = new ToolStripStatusLabel();
            toolStripVersion = new ToolStripStatusLabel();
            listBoxResult1 = new ListBox();
            msAppMenu = new MenuStrip();
            pictureStorageToolStripMenuItem = new ToolStripMenuItem();
            saveAsBMPToolStripMenuItem = new ToolStripMenuItem();
            saveAsJPGToolStripMenuItem = new ToolStripMenuItem();
            saveAsTIFFToolStripMenuItem = new ToolStripMenuItem();
            saveAsPNGToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripMenuItem();
            hideMenuoolStripMenuItem = new ToolStripMenuItem();
            hideResultWinToolStripMenuItem = new ToolStripMenuItem();
            hideGridRecToolStripMenuItem = new ToolStripMenuItem();
            TblMain.SuspendLayout();
            TblBodyMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SplitConImageViewAndButton).BeginInit();
            SplitConImageViewAndButton.Panel1.SuspendLayout();
            SplitConImageViewAndButton.Panel2.SuspendLayout();
            SplitConImageViewAndButton.SuspendLayout();
            TblPnlButtons.SuspendLayout();
            grpConfigs.SuspendLayout();
            flowPnlConfig.SuspendLayout();
            grpTrainAndDecode.SuspendLayout();
            flowPnlTrainAndDecode.SuspendLayout();
            GrpRoiTypes.SuspendLayout();
            flowLayOutPnlRoiTypes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitConBodyMain).BeginInit();
            splitConBodyMain.Panel1.SuspendLayout();
            splitConBodyMain.Panel2.SuspendLayout();
            splitConBodyMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SplitContainerImageView).BeginInit();
            SplitContainerImageView.Panel1.SuspendLayout();
            SplitContainerImageView.Panel2.SuspendLayout();
            SplitContainerImageView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ImageCanvas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvDecodeTextRec).BeginInit();
            TblPnlSettingAndRoiData.SuspendLayout();
            TblBlobSettings.SuspendLayout();
            GrpBoxSegMentMode.SuspendLayout();
            TblPnlImageList.SuspendLayout();
            TblDeviceConfig.SuspendLayout();
            GrpSetParam.SuspendLayout();
            TblParameter.SuspendLayout();
            GrpImageAq.SuspendLayout();
            TblImageAq.SuspendLayout();
            grpDeviceConnection.SuspendLayout();
            TblSearchDevice.SuspendLayout();
            TblDeviceSearch.SuspendLayout();
            StatusStripMenu.SuspendLayout();
            msAppMenu.SuspendLayout();
            SuspendLayout();
            // 
            // TblMain
            // 
            TblMain.ColumnCount = 1;
            TblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TblMain.Controls.Add(TblBodyMain, 0, 1);
            TblMain.Controls.Add(TblPnlImageList, 0, 0);
            TblMain.Controls.Add(StatusStripMenu, 0, 2);
            TblMain.Dock = DockStyle.Fill;
            TblMain.Location = new Point(0, 28);
            TblMain.Name = "TblMain";
            TblMain.RowCount = 3;
            TblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 153F));
            TblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            TblMain.Size = new Size(1321, 772);
            TblMain.TabIndex = 0;
            // 
            // TblBodyMain
            // 
            TblBodyMain.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            TblBodyMain.ColumnCount = 1;
            TblBodyMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TblBodyMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 18F));
            TblBodyMain.Controls.Add(SplitConImageViewAndButton, 0, 0);
            TblBodyMain.Dock = DockStyle.Fill;
            TblBodyMain.Location = new Point(3, 156);
            TblBodyMain.Name = "TblBodyMain";
            TblBodyMain.RowCount = 1;
            TblBodyMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblBodyMain.Size = new Size(1315, 583);
            TblBodyMain.TabIndex = 1;
            // 
            // SplitConImageViewAndButton
            // 
            SplitConImageViewAndButton.BorderStyle = BorderStyle.FixedSingle;
            SplitConImageViewAndButton.Dock = DockStyle.Fill;
            SplitConImageViewAndButton.Location = new Point(4, 4);
            SplitConImageViewAndButton.Name = "SplitConImageViewAndButton";
            // 
            // SplitConImageViewAndButton.Panel1
            // 
            SplitConImageViewAndButton.Panel1.Controls.Add(TblPnlButtons);
            // 
            // SplitConImageViewAndButton.Panel2
            // 
            SplitConImageViewAndButton.Panel2.Controls.Add(splitConBodyMain);
            SplitConImageViewAndButton.Size = new Size(1307, 575);
            SplitConImageViewAndButton.SplitterDistance = 137;
            SplitConImageViewAndButton.TabIndex = 0;
            // 
            // TblPnlButtons
            // 
            TblPnlButtons.AutoScroll = true;
            TblPnlButtons.ColumnCount = 1;
            TblPnlButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TblPnlButtons.Controls.Add(grpConfigs, 0, 2);
            TblPnlButtons.Controls.Add(grpTrainAndDecode, 0, 1);
            TblPnlButtons.Controls.Add(GrpRoiTypes, 0, 0);
            TblPnlButtons.Dock = DockStyle.Fill;
            TblPnlButtons.Location = new Point(0, 0);
            TblPnlButtons.Name = "TblPnlButtons";
            TblPnlButtons.RowCount = 4;
            TblPnlButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 193F));
            TblPnlButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 141F));
            TblPnlButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 190F));
            TblPnlButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblPnlButtons.Size = new Size(135, 573);
            TblPnlButtons.TabIndex = 0;
            // 
            // grpConfigs
            // 
            grpConfigs.Controls.Add(flowPnlConfig);
            grpConfigs.Dock = DockStyle.Fill;
            grpConfigs.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpConfigs.Location = new Point(3, 337);
            grpConfigs.Name = "grpConfigs";
            grpConfigs.Size = new Size(129, 184);
            grpConfigs.TabIndex = 2;
            grpConfigs.TabStop = false;
            grpConfigs.Text = "Config";
            grpConfigs.UseCompatibleTextRendering = true;
            // 
            // flowPnlConfig
            // 
            flowPnlConfig.Controls.Add(BtnSaveRoi);
            flowPnlConfig.Controls.Add(BtnLoadRoi);
            flowPnlConfig.Controls.Add(BtnShowTamplate);
            flowPnlConfig.Dock = DockStyle.Fill;
            flowPnlConfig.Location = new Point(3, 23);
            flowPnlConfig.Name = "flowPnlConfig";
            flowPnlConfig.Size = new Size(123, 158);
            flowPnlConfig.TabIndex = 0;
            // 
            // BtnSaveRoi
            // 
            BtnSaveRoi.BackColor = SystemColors.InactiveCaption;
            BtnSaveRoi.FlatAppearance.BorderColor = Color.FromArgb(208, 208, 208);
            BtnSaveRoi.FlatStyle = FlatStyle.Flat;
            BtnSaveRoi.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            BtnSaveRoi.ForeColor = Color.FromArgb(32, 32, 32);
            BtnSaveRoi.Image = (Image)resources.GetObject("BtnSaveRoi.Image");
            BtnSaveRoi.ImageAlign = ContentAlignment.MiddleLeft;
            BtnSaveRoi.Location = new Point(3, 3);
            BtnSaveRoi.Name = "BtnSaveRoi";
            BtnSaveRoi.Padding = new Padding(10, 0, 0, 0);
            BtnSaveRoi.Size = new Size(153, 44);
            BtnSaveRoi.TabIndex = 9;
            BtnSaveRoi.Text = "Save ROI";
            BtnSaveRoi.TextImageRelation = TextImageRelation.ImageBeforeText;
            BtnSaveRoi.UseCompatibleTextRendering = true;
            BtnSaveRoi.UseVisualStyleBackColor = false;
            BtnSaveRoi.Click += BtnSaveRoi_Click;
            // 
            // BtnLoadRoi
            // 
            BtnLoadRoi.BackColor = SystemColors.InactiveCaption;
            BtnLoadRoi.FlatAppearance.BorderColor = Color.FromArgb(208, 208, 208);
            BtnLoadRoi.FlatStyle = FlatStyle.Flat;
            BtnLoadRoi.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            BtnLoadRoi.ForeColor = Color.FromArgb(32, 32, 32);
            BtnLoadRoi.Image = (Image)resources.GetObject("BtnLoadRoi.Image");
            BtnLoadRoi.ImageAlign = ContentAlignment.MiddleLeft;
            BtnLoadRoi.Location = new Point(3, 53);
            BtnLoadRoi.Name = "BtnLoadRoi";
            BtnLoadRoi.Padding = new Padding(10, 0, 0, 0);
            BtnLoadRoi.Size = new Size(153, 44);
            BtnLoadRoi.TabIndex = 10;
            BtnLoadRoi.Text = "Load ROI";
            BtnLoadRoi.TextImageRelation = TextImageRelation.ImageBeforeText;
            BtnLoadRoi.UseCompatibleTextRendering = true;
            BtnLoadRoi.UseVisualStyleBackColor = false;
            BtnLoadRoi.Click += BtnLoadRoi_Click;
            // 
            // BtnShowTamplate
            // 
            BtnShowTamplate.BackColor = SystemColors.InactiveCaption;
            BtnShowTamplate.FlatAppearance.BorderColor = Color.FromArgb(208, 208, 208);
            BtnShowTamplate.FlatStyle = FlatStyle.Flat;
            BtnShowTamplate.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            BtnShowTamplate.ForeColor = Color.FromArgb(32, 32, 32);
            BtnShowTamplate.Image = (Image)resources.GetObject("BtnShowTamplate.Image");
            BtnShowTamplate.ImageAlign = ContentAlignment.MiddleLeft;
            BtnShowTamplate.Location = new Point(3, 103);
            BtnShowTamplate.Name = "BtnShowTamplate";
            BtnShowTamplate.Padding = new Padding(10, 0, 0, 0);
            BtnShowTamplate.Size = new Size(153, 44);
            BtnShowTamplate.TabIndex = 7;
            BtnShowTamplate.Text = "&All Tamplates";
            BtnShowTamplate.TextImageRelation = TextImageRelation.ImageBeforeText;
            BtnShowTamplate.UseCompatibleTextRendering = true;
            BtnShowTamplate.UseVisualStyleBackColor = false;
            BtnShowTamplate.Click += BtnShowTamplate_Click;
            // 
            // grpTrainAndDecode
            // 
            grpTrainAndDecode.Controls.Add(flowPnlTrainAndDecode);
            grpTrainAndDecode.Dock = DockStyle.Fill;
            grpTrainAndDecode.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpTrainAndDecode.Location = new Point(3, 196);
            grpTrainAndDecode.Name = "grpTrainAndDecode";
            grpTrainAndDecode.Size = new Size(129, 135);
            grpTrainAndDecode.TabIndex = 1;
            grpTrainAndDecode.TabStop = false;
            grpTrainAndDecode.Text = "Train && Decode";
            grpTrainAndDecode.UseCompatibleTextRendering = true;
            // 
            // flowPnlTrainAndDecode
            // 
            flowPnlTrainAndDecode.Controls.Add(BtnTrainSelROI);
            flowPnlTrainAndDecode.Controls.Add(BtnDecodeAllROI);
            flowPnlTrainAndDecode.Dock = DockStyle.Fill;
            flowPnlTrainAndDecode.Location = new Point(3, 23);
            flowPnlTrainAndDecode.Name = "flowPnlTrainAndDecode";
            flowPnlTrainAndDecode.Size = new Size(123, 109);
            flowPnlTrainAndDecode.TabIndex = 0;
            // 
            // BtnTrainSelROI
            // 
            BtnTrainSelROI.BackColor = Color.LightCyan;
            BtnTrainSelROI.FlatStyle = FlatStyle.Flat;
            BtnTrainSelROI.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            BtnTrainSelROI.Image = (Image)resources.GetObject("BtnTrainSelROI.Image");
            BtnTrainSelROI.ImageAlign = ContentAlignment.MiddleLeft;
            BtnTrainSelROI.Location = new Point(3, 3);
            BtnTrainSelROI.Name = "BtnTrainSelROI";
            BtnTrainSelROI.Padding = new Padding(10, 0, 0, 0);
            BtnTrainSelROI.Size = new Size(153, 44);
            BtnTrainSelROI.TabIndex = 4;
            BtnTrainSelROI.Text = "&Train ROI";
            BtnTrainSelROI.TextImageRelation = TextImageRelation.ImageBeforeText;
            BtnTrainSelROI.UseCompatibleTextRendering = true;
            BtnTrainSelROI.UseVisualStyleBackColor = false;
            BtnTrainSelROI.Click += BtnTrainSelROI_Click;
            // 
            // BtnDecodeAllROI
            // 
            BtnDecodeAllROI.BackColor = Color.LightCyan;
            BtnDecodeAllROI.FlatStyle = FlatStyle.Flat;
            BtnDecodeAllROI.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            BtnDecodeAllROI.Image = (Image)resources.GetObject("BtnDecodeAllROI.Image");
            BtnDecodeAllROI.ImageAlign = ContentAlignment.MiddleLeft;
            BtnDecodeAllROI.Location = new Point(3, 53);
            BtnDecodeAllROI.Name = "BtnDecodeAllROI";
            BtnDecodeAllROI.Padding = new Padding(10, 0, 0, 0);
            BtnDecodeAllROI.Size = new Size(153, 44);
            BtnDecodeAllROI.TabIndex = 6;
            BtnDecodeAllROI.Text = "Decode &All";
            BtnDecodeAllROI.TextImageRelation = TextImageRelation.ImageBeforeText;
            BtnDecodeAllROI.UseCompatibleTextRendering = true;
            BtnDecodeAllROI.UseVisualStyleBackColor = false;
            BtnDecodeAllROI.Click += BtnDecodeAllROI_Click;
            // 
            // GrpRoiTypes
            // 
            GrpRoiTypes.Controls.Add(flowLayOutPnlRoiTypes);
            GrpRoiTypes.Dock = DockStyle.Fill;
            GrpRoiTypes.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            GrpRoiTypes.Location = new Point(3, 3);
            GrpRoiTypes.Name = "GrpRoiTypes";
            GrpRoiTypes.Size = new Size(129, 187);
            GrpRoiTypes.TabIndex = 0;
            GrpRoiTypes.TabStop = false;
            GrpRoiTypes.Text = "ROI Types";
            GrpRoiTypes.UseCompatibleTextRendering = true;
            // 
            // flowLayOutPnlRoiTypes
            // 
            flowLayOutPnlRoiTypes.Controls.Add(BtnAddRoi);
            flowLayOutPnlRoiTypes.Controls.Add(BtnAddBarCodeRoi);
            flowLayOutPnlRoiTypes.Controls.Add(BtnAddTempateROI);
            flowLayOutPnlRoiTypes.Dock = DockStyle.Fill;
            flowLayOutPnlRoiTypes.Location = new Point(3, 23);
            flowLayOutPnlRoiTypes.Name = "flowLayOutPnlRoiTypes";
            flowLayOutPnlRoiTypes.Size = new Size(123, 161);
            flowLayOutPnlRoiTypes.TabIndex = 0;
            // 
            // BtnAddRoi
            // 
            BtnAddRoi.BackColor = SystemColors.InactiveCaption;
            BtnAddRoi.FlatAppearance.BorderColor = Color.FromArgb(208, 208, 208);
            BtnAddRoi.FlatStyle = FlatStyle.Flat;
            BtnAddRoi.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            BtnAddRoi.ForeColor = Color.FromArgb(32, 32, 32);
            BtnAddRoi.Image = (Image)resources.GetObject("BtnAddRoi.Image");
            BtnAddRoi.ImageAlign = ContentAlignment.MiddleLeft;
            BtnAddRoi.Location = new Point(3, 3);
            BtnAddRoi.Name = "BtnAddRoi";
            BtnAddRoi.Padding = new Padding(10, 0, 0, 0);
            BtnAddRoi.Size = new Size(153, 44);
            BtnAddRoi.TabIndex = 3;
            BtnAddRoi.Text = "Text &ROI";
            BtnAddRoi.TextImageRelation = TextImageRelation.ImageBeforeText;
            BtnAddRoi.UseCompatibleTextRendering = true;
            BtnAddRoi.UseVisualStyleBackColor = false;
            BtnAddRoi.Click += BtnAddRoi_Click;
            // 
            // BtnAddBarCodeRoi
            // 
            BtnAddBarCodeRoi.BackColor = SystemColors.InactiveCaption;
            BtnAddBarCodeRoi.FlatAppearance.BorderColor = Color.FromArgb(208, 208, 208);
            BtnAddBarCodeRoi.FlatStyle = FlatStyle.Flat;
            BtnAddBarCodeRoi.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            BtnAddBarCodeRoi.ForeColor = Color.FromArgb(32, 32, 32);
            BtnAddBarCodeRoi.Image = (Image)resources.GetObject("BtnAddBarCodeRoi.Image");
            BtnAddBarCodeRoi.ImageAlign = ContentAlignment.MiddleLeft;
            BtnAddBarCodeRoi.Location = new Point(3, 53);
            BtnAddBarCodeRoi.Name = "BtnAddBarCodeRoi";
            BtnAddBarCodeRoi.Padding = new Padding(10, 0, 0, 0);
            BtnAddBarCodeRoi.Size = new Size(153, 44);
            BtnAddBarCodeRoi.TabIndex = 8;
            BtnAddBarCodeRoi.Text = "Bar&Code ROI";
            BtnAddBarCodeRoi.TextImageRelation = TextImageRelation.ImageBeforeText;
            BtnAddBarCodeRoi.UseCompatibleTextRendering = true;
            BtnAddBarCodeRoi.UseVisualStyleBackColor = false;
            BtnAddBarCodeRoi.Click += BtnAddBarCodeRoi_Click;
            // 
            // BtnAddTempateROI
            // 
            BtnAddTempateROI.BackColor = SystemColors.InactiveCaption;
            BtnAddTempateROI.FlatAppearance.BorderColor = Color.FromArgb(208, 208, 208);
            BtnAddTempateROI.FlatStyle = FlatStyle.Flat;
            BtnAddTempateROI.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            BtnAddTempateROI.ForeColor = Color.FromArgb(32, 32, 32);
            BtnAddTempateROI.Image = (Image)resources.GetObject("BtnAddTempateROI.Image");
            BtnAddTempateROI.ImageAlign = ContentAlignment.MiddleLeft;
            BtnAddTempateROI.Location = new Point(3, 103);
            BtnAddTempateROI.Name = "BtnAddTempateROI";
            BtnAddTempateROI.Padding = new Padding(10, 0, 0, 0);
            BtnAddTempateROI.Size = new Size(153, 44);
            BtnAddTempateROI.TabIndex = 11;
            BtnAddTempateROI.Text = "TM ROI";
            BtnAddTempateROI.TextImageRelation = TextImageRelation.ImageBeforeText;
            BtnAddTempateROI.UseCompatibleTextRendering = true;
            BtnAddTempateROI.UseVisualStyleBackColor = false;
            BtnAddTempateROI.Click += BtnAddTempateROI_Click;
            // 
            // splitConBodyMain
            // 
            splitConBodyMain.BorderStyle = BorderStyle.FixedSingle;
            splitConBodyMain.Dock = DockStyle.Fill;
            splitConBodyMain.Location = new Point(0, 0);
            splitConBodyMain.Name = "splitConBodyMain";
            // 
            // splitConBodyMain.Panel1
            // 
            splitConBodyMain.Panel1.Controls.Add(SplitContainerImageView);
            // 
            // splitConBodyMain.Panel2
            // 
            splitConBodyMain.Panel2.Controls.Add(TblPnlSettingAndRoiData);
            splitConBodyMain.Size = new Size(1166, 575);
            splitConBodyMain.SplitterDistance = 799;
            splitConBodyMain.TabIndex = 1;
            // 
            // SplitContainerImageView
            // 
            SplitContainerImageView.BorderStyle = BorderStyle.FixedSingle;
            SplitContainerImageView.Dock = DockStyle.Fill;
            SplitContainerImageView.Location = new Point(0, 0);
            SplitContainerImageView.Name = "SplitContainerImageView";
            SplitContainerImageView.Orientation = Orientation.Horizontal;
            // 
            // SplitContainerImageView.Panel1
            // 
            SplitContainerImageView.Panel1.Controls.Add(ImageCanvas);
            // 
            // SplitContainerImageView.Panel2
            // 
            SplitContainerImageView.Panel2.Controls.Add(dgvDecodeTextRec);
            SplitContainerImageView.Size = new Size(799, 575);
            SplitContainerImageView.SplitterDistance = 449;
            SplitContainerImageView.TabIndex = 0;
            // 
            // ImageCanvas
            // 
            ImageCanvas.BackColor = SystemColors.InactiveBorder;
            ImageCanvas.Dock = DockStyle.Fill;
            ImageCanvas.Location = new Point(0, 0);
            ImageCanvas.Name = "ImageCanvas";
            ImageCanvas.Size = new Size(797, 447);
            ImageCanvas.TabIndex = 0;
            ImageCanvas.TabStop = false;
            ImageCanvas.Paint += ImageCanvas_Paint;
            ImageCanvas.MouseDown += ImageCanvas_MouseDown;
            ImageCanvas.MouseMove += ImageCanvas_MouseMove;
            ImageCanvas.MouseUp += ImageCanvas_MouseUp;
            // 
            // dgvDecodeTextRec
            // 
            dgvDecodeTextRec.AllowUserToAddRows = false;
            dgvDecodeTextRec.AllowUserToDeleteRows = false;
            dgvDecodeTextRec.AllowUserToResizeRows = false;
            dgvDecodeTextRec.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgvDecodeTextRec.BackgroundColor = Color.White;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(243, 244, 246);
            dataGridViewCellStyle1.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(32, 32, 32);
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvDecodeTextRec.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvDecodeTextRec.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDecodeTextRec.Columns.AddRange(new DataGridViewColumn[] { DisRoiType, DecodedText, Result, ThresoldValue, RoiAngle, ExecutionTime, DecodeTime });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(32, 32, 32);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(229, 241, 251);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(32, 32, 32);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvDecodeTextRec.DefaultCellStyle = dataGridViewCellStyle2;
            dgvDecodeTextRec.Dock = DockStyle.Fill;
            dgvDecodeTextRec.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvDecodeTextRec.GridColor = Color.FromArgb(224, 224, 224);
            dgvDecodeTextRec.Location = new Point(0, 0);
            dgvDecodeTextRec.Name = "dgvDecodeTextRec";
            dgvDecodeTextRec.RowHeadersVisible = false;
            dgvDecodeTextRec.RowHeadersWidth = 51;
            dgvDecodeTextRec.Size = new Size(797, 120);
            dgvDecodeTextRec.TabIndex = 1;
            // 
            // DisRoiType
            // 
            DisRoiType.HeaderText = "Roi Type";
            DisRoiType.MinimumWidth = 6;
            DisRoiType.Name = "DisRoiType";
            DisRoiType.ReadOnly = true;
            DisRoiType.Width = 96;
            // 
            // DecodedText
            // 
            DecodedText.HeaderText = "Decoded Text";
            DecodedText.MinimumWidth = 6;
            DecodedText.Name = "DecodedText";
            DecodedText.ReadOnly = true;
            DecodedText.Width = 131;
            // 
            // Result
            // 
            Result.HeaderText = "Result";
            Result.MinimumWidth = 6;
            Result.Name = "Result";
            Result.ReadOnly = true;
            Result.Width = 79;
            // 
            // ThresoldValue
            // 
            ThresoldValue.HeaderText = "Min Thresold";
            ThresoldValue.MinimumWidth = 6;
            ThresoldValue.Name = "ThresoldValue";
            ThresoldValue.ReadOnly = true;
            ThresoldValue.Width = 128;
            // 
            // RoiAngle
            // 
            RoiAngle.HeaderText = "RoiAngle";
            RoiAngle.MinimumWidth = 6;
            RoiAngle.Name = "RoiAngle";
            RoiAngle.ReadOnly = true;
            // 
            // ExecutionTime
            // 
            ExecutionTime.HeaderText = "Execution Time";
            ExecutionTime.MinimumWidth = 6;
            ExecutionTime.Name = "ExecutionTime";
            ExecutionTime.ReadOnly = true;
            ExecutionTime.Width = 142;
            // 
            // DecodeTime
            // 
            DecodeTime.HeaderText = "Decoded Time";
            DecodeTime.MinimumWidth = 6;
            DecodeTime.Name = "DecodeTime";
            DecodeTime.ReadOnly = true;
            DecodeTime.Width = 136;
            // 
            // TblPnlSettingAndRoiData
            // 
            TblPnlSettingAndRoiData.ColumnCount = 1;
            TblPnlSettingAndRoiData.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TblPnlSettingAndRoiData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 18F));
            TblPnlSettingAndRoiData.Controls.Add(FlowPnlRoiData, 0, 1);
            TblPnlSettingAndRoiData.Controls.Add(TblBlobSettings, 0, 0);
            TblPnlSettingAndRoiData.Dock = DockStyle.Fill;
            TblPnlSettingAndRoiData.Location = new Point(0, 0);
            TblPnlSettingAndRoiData.Name = "TblPnlSettingAndRoiData";
            TblPnlSettingAndRoiData.RowCount = 2;
            TblPnlSettingAndRoiData.RowStyles.Add(new RowStyle(SizeType.Absolute, 83F));
            TblPnlSettingAndRoiData.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblPnlSettingAndRoiData.Size = new Size(361, 573);
            TblPnlSettingAndRoiData.TabIndex = 1;
            // 
            // FlowPnlRoiData
            // 
            FlowPnlRoiData.AutoScroll = true;
            FlowPnlRoiData.Dock = DockStyle.Fill;
            FlowPnlRoiData.Location = new Point(3, 86);
            FlowPnlRoiData.Name = "FlowPnlRoiData";
            FlowPnlRoiData.Size = new Size(355, 484);
            FlowPnlRoiData.TabIndex = 0;
            // 
            // TblBlobSettings
            // 
            TblBlobSettings.ColumnCount = 1;
            TblBlobSettings.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TblBlobSettings.Controls.Add(GrpBoxSegMentMode, 0, 0);
            TblBlobSettings.Dock = DockStyle.Fill;
            TblBlobSettings.Location = new Point(3, 3);
            TblBlobSettings.Name = "TblBlobSettings";
            TblBlobSettings.RowCount = 1;
            TblBlobSettings.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblBlobSettings.RowStyles.Add(new RowStyle(SizeType.Absolute, 77F));
            TblBlobSettings.Size = new Size(355, 77);
            TblBlobSettings.TabIndex = 1;
            TblBlobSettings.Visible = false;
            // 
            // GrpBoxSegMentMode
            // 
            GrpBoxSegMentMode.Controls.Add(CmbSegments);
            GrpBoxSegMentMode.Dock = DockStyle.Fill;
            GrpBoxSegMentMode.Location = new Point(3, 3);
            GrpBoxSegMentMode.Name = "GrpBoxSegMentMode";
            GrpBoxSegMentMode.Size = new Size(349, 71);
            GrpBoxSegMentMode.TabIndex = 0;
            GrpBoxSegMentMode.TabStop = false;
            GrpBoxSegMentMode.Text = "Segmentation Mode";
            // 
            // CmbSegments
            // 
            CmbSegments.Font = new Font("Segoe UI", 9F);
            CmbSegments.FormattingEnabled = true;
            CmbSegments.Location = new Point(6, 31);
            CmbSegments.Name = "CmbSegments";
            CmbSegments.Size = new Size(321, 28);
            CmbSegments.TabIndex = 0;
            CmbSegments.SelectedIndexChanged += CmbSegments_SelectedIndexChanged;
            // 
            // TblPnlImageList
            // 
            TblPnlImageList.BackColor = Color.White;
            TblPnlImageList.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            TblPnlImageList.ColumnCount = 2;
            TblPnlImageList.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 313F));
            TblPnlImageList.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TblPnlImageList.Controls.Add(TblDeviceConfig, 1, 0);
            TblPnlImageList.Controls.Add(grpDeviceConnection, 0, 0);
            TblPnlImageList.Dock = DockStyle.Fill;
            TblPnlImageList.Location = new Point(3, 3);
            TblPnlImageList.Name = "TblPnlImageList";
            TblPnlImageList.RowCount = 1;
            TblPnlImageList.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblPnlImageList.Size = new Size(1315, 147);
            TblPnlImageList.TabIndex = 2;
            // 
            // TblDeviceConfig
            // 
            TblDeviceConfig.ColumnCount = 3;
            TblDeviceConfig.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 371F));
            TblDeviceConfig.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 569F));
            TblDeviceConfig.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 8F));
            TblDeviceConfig.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            TblDeviceConfig.Controls.Add(GrpSetParam, 1, 0);
            TblDeviceConfig.Controls.Add(GrpImageAq, 0, 0);
            TblDeviceConfig.Dock = DockStyle.Fill;
            TblDeviceConfig.Location = new Point(318, 4);
            TblDeviceConfig.Name = "TblDeviceConfig";
            TblDeviceConfig.RowCount = 1;
            TblDeviceConfig.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblDeviceConfig.Size = new Size(993, 139);
            TblDeviceConfig.TabIndex = 1;
            // 
            // GrpSetParam
            // 
            GrpSetParam.Controls.Add(TblParameter);
            GrpSetParam.Dock = DockStyle.Fill;
            GrpSetParam.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            GrpSetParam.Location = new Point(374, 3);
            GrpSetParam.Name = "GrpSetParam";
            GrpSetParam.Size = new Size(563, 133);
            GrpSetParam.TabIndex = 2;
            GrpSetParam.TabStop = false;
            GrpSetParam.Text = "Parameter";
            // 
            // TblParameter
            // 
            TblParameter.ColumnCount = 7;
            TblParameter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 114F));
            TblParameter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            TblParameter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 43F));
            TblParameter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            TblParameter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 91F));
            TblParameter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            TblParameter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 48F));
            TblParameter.Controls.Add(LblExposerTime, 0, 0);
            TblParameter.Controls.Add(tbExposure, 1, 0);
            TblParameter.Controls.Add(LblGain, 2, 0);
            TblParameter.Controls.Add(tbGain, 3, 0);
            TblParameter.Controls.Add(LblFrameRate, 4, 0);
            TblParameter.Controls.Add(tbFrameRate, 5, 0);
            TblParameter.Controls.Add(bnGetParam, 1, 2);
            TblParameter.Controls.Add(bnSetParam, 4, 2);
            TblParameter.Dock = DockStyle.Fill;
            TblParameter.Location = new Point(3, 23);
            TblParameter.Name = "TblParameter";
            TblParameter.RowCount = 3;
            TblParameter.RowStyles.Add(new RowStyle(SizeType.Percent, 41.48936F));
            TblParameter.RowStyles.Add(new RowStyle(SizeType.Percent, 14.8936167F));
            TblParameter.RowStyles.Add(new RowStyle(SizeType.Percent, 43.61702F));
            TblParameter.Size = new Size(557, 107);
            TblParameter.TabIndex = 0;
            // 
            // LblExposerTime
            // 
            LblExposerTime.AutoSize = true;
            LblExposerTime.Dock = DockStyle.Fill;
            LblExposerTime.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            LblExposerTime.Location = new Point(3, 3);
            LblExposerTime.Margin = new Padding(3);
            LblExposerTime.Name = "LblExposerTime";
            LblExposerTime.Size = new Size(108, 38);
            LblExposerTime.TabIndex = 0;
            LblExposerTime.Text = "Exposure Time";
            LblExposerTime.TextAlign = ContentAlignment.MiddleLeft;
            LblExposerTime.UseCompatibleTextRendering = true;
            // 
            // tbExposure
            // 
            tbExposure.BorderStyle = BorderStyle.FixedSingle;
            tbExposure.Dock = DockStyle.Fill;
            tbExposure.Font = new Font("Segoe UI", 9F);
            tbExposure.Location = new Point(117, 3);
            tbExposure.Multiline = true;
            tbExposure.Name = "tbExposure";
            tbExposure.Size = new Size(54, 38);
            tbExposure.TabIndex = 1;
            // 
            // LblGain
            // 
            LblGain.AutoSize = true;
            LblGain.Dock = DockStyle.Fill;
            LblGain.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            LblGain.Location = new Point(177, 3);
            LblGain.Margin = new Padding(3);
            LblGain.Name = "LblGain";
            LblGain.Size = new Size(37, 38);
            LblGain.TabIndex = 2;
            LblGain.Text = "Gain";
            LblGain.TextAlign = ContentAlignment.MiddleLeft;
            LblGain.UseCompatibleTextRendering = true;
            // 
            // tbGain
            // 
            tbGain.BorderStyle = BorderStyle.FixedSingle;
            tbGain.Dock = DockStyle.Fill;
            tbGain.Font = new Font("Segoe UI", 9F);
            tbGain.Location = new Point(220, 3);
            tbGain.Multiline = true;
            tbGain.Name = "tbGain";
            tbGain.Size = new Size(54, 38);
            tbGain.TabIndex = 4;
            // 
            // LblFrameRate
            // 
            LblFrameRate.AutoSize = true;
            LblFrameRate.Dock = DockStyle.Fill;
            LblFrameRate.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            LblFrameRate.Location = new Point(280, 3);
            LblFrameRate.Margin = new Padding(3);
            LblFrameRate.Name = "LblFrameRate";
            LblFrameRate.Size = new Size(85, 38);
            LblFrameRate.TabIndex = 3;
            LblFrameRate.Text = "Frame Rate";
            LblFrameRate.TextAlign = ContentAlignment.MiddleLeft;
            LblFrameRate.UseCompatibleTextRendering = true;
            // 
            // tbFrameRate
            // 
            tbFrameRate.BorderStyle = BorderStyle.FixedSingle;
            tbFrameRate.Dock = DockStyle.Fill;
            tbFrameRate.Font = new Font("Segoe UI", 9F);
            tbFrameRate.Location = new Point(371, 3);
            tbFrameRate.Multiline = true;
            tbFrameRate.Name = "tbFrameRate";
            tbFrameRate.Size = new Size(54, 38);
            tbFrameRate.TabIndex = 5;
            // 
            // bnGetParam
            // 
            bnGetParam.BackColor = SystemColors.InactiveCaption;
            TblParameter.SetColumnSpan(bnGetParam, 3);
            bnGetParam.Dock = DockStyle.Fill;
            bnGetParam.FlatStyle = FlatStyle.Flat;
            bnGetParam.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            bnGetParam.ForeColor = Color.FromArgb(32, 32, 32);
            bnGetParam.Location = new Point(117, 62);
            bnGetParam.Name = "bnGetParam";
            bnGetParam.Size = new Size(157, 42);
            bnGetParam.TabIndex = 3;
            bnGetParam.Text = "Get Parameter";
            bnGetParam.UseCompatibleTextRendering = true;
            bnGetParam.UseVisualStyleBackColor = false;
            bnGetParam.Click += BnGetParam_Click;
            // 
            // bnSetParam
            // 
            bnSetParam.BackColor = SystemColors.InactiveCaption;
            TblParameter.SetColumnSpan(bnSetParam, 2);
            bnSetParam.Dock = DockStyle.Fill;
            bnSetParam.FlatStyle = FlatStyle.Flat;
            bnSetParam.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            bnSetParam.ForeColor = Color.FromArgb(32, 32, 32);
            bnSetParam.Location = new Point(280, 62);
            bnSetParam.Name = "bnSetParam";
            bnSetParam.Size = new Size(145, 42);
            bnSetParam.TabIndex = 7;
            bnSetParam.Text = "Set Parameter";
            bnSetParam.UseCompatibleTextRendering = true;
            bnSetParam.UseVisualStyleBackColor = false;
            bnSetParam.Click += BnSetParam_Click;
            // 
            // GrpImageAq
            // 
            GrpImageAq.Controls.Add(TblImageAq);
            GrpImageAq.Dock = DockStyle.Fill;
            GrpImageAq.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            GrpImageAq.Location = new Point(3, 3);
            GrpImageAq.Name = "GrpImageAq";
            GrpImageAq.Size = new Size(365, 133);
            GrpImageAq.TabIndex = 0;
            GrpImageAq.TabStop = false;
            GrpImageAq.Text = "Image Acquisition";
            // 
            // TblImageAq
            // 
            TblImageAq.ColumnCount = 2;
            TblImageAq.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            TblImageAq.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            TblImageAq.Controls.Add(bnTriggerMode, 1, 0);
            TblImageAq.Controls.Add(bnContinuesMode, 0, 0);
            TblImageAq.Controls.Add(bnStopGrab, 1, 2);
            TblImageAq.Controls.Add(bnStartGrab, 0, 2);
            TblImageAq.Controls.Add(cbSoftTrigger, 0, 1);
            TblImageAq.Controls.Add(bnTriggerExec, 1, 1);
            TblImageAq.Dock = DockStyle.Fill;
            TblImageAq.Location = new Point(3, 23);
            TblImageAq.Name = "TblImageAq";
            TblImageAq.RowCount = 3;
            TblImageAq.RowStyles.Add(new RowStyle(SizeType.Percent, 30.0970879F));
            TblImageAq.RowStyles.Add(new RowStyle(SizeType.Percent, 30.8510647F));
            TblImageAq.RowStyles.Add(new RowStyle(SizeType.Percent, 39.361702F));
            TblImageAq.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            TblImageAq.Size = new Size(359, 107);
            TblImageAq.TabIndex = 0;
            // 
            // bnTriggerMode
            // 
            bnTriggerMode.AutoSize = true;
            bnTriggerMode.Dock = DockStyle.Fill;
            bnTriggerMode.Font = new Font("Segoe UI", 9F);
            bnTriggerMode.Location = new Point(182, 3);
            bnTriggerMode.Name = "bnTriggerMode";
            bnTriggerMode.Size = new Size(174, 26);
            bnTriggerMode.TabIndex = 1;
            bnTriggerMode.Text = "Trigger Mode";
            bnTriggerMode.UseVisualStyleBackColor = true;
            bnTriggerMode.CheckedChanged += BnTriggerMode_CheckedChanged;
            // 
            // bnContinuesMode
            // 
            bnContinuesMode.AutoSize = true;
            bnContinuesMode.Checked = true;
            bnContinuesMode.Dock = DockStyle.Fill;
            bnContinuesMode.Font = new Font("Segoe UI", 9F);
            bnContinuesMode.Location = new Point(3, 3);
            bnContinuesMode.Name = "bnContinuesMode";
            bnContinuesMode.Size = new Size(173, 26);
            bnContinuesMode.TabIndex = 0;
            bnContinuesMode.TabStop = true;
            bnContinuesMode.Text = "Continuous";
            bnContinuesMode.UseVisualStyleBackColor = true;
            bnContinuesMode.CheckedChanged += BnContinuesMode_CheckedChanged;
            // 
            // bnStopGrab
            // 
            bnStopGrab.BackColor = SystemColors.InactiveCaption;
            bnStopGrab.Dock = DockStyle.Left;
            bnStopGrab.FlatStyle = FlatStyle.Flat;
            bnStopGrab.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            bnStopGrab.ForeColor = Color.FromArgb(32, 32, 32);
            bnStopGrab.Image = (Image)resources.GetObject("bnStopGrab.Image");
            bnStopGrab.ImageAlign = ContentAlignment.MiddleLeft;
            bnStopGrab.Location = new Point(182, 67);
            bnStopGrab.Name = "bnStopGrab";
            bnStopGrab.Padding = new Padding(8, 0, 0, 0);
            bnStopGrab.Size = new Size(98, 37);
            bnStopGrab.TabIndex = 4;
            bnStopGrab.Text = "Stop";
            bnStopGrab.TextImageRelation = TextImageRelation.ImageBeforeText;
            bnStopGrab.UseCompatibleTextRendering = true;
            bnStopGrab.UseVisualStyleBackColor = false;
            bnStopGrab.Click += BnStopGrab_Click;
            // 
            // bnStartGrab
            // 
            bnStartGrab.BackColor = SystemColors.InactiveCaption;
            bnStartGrab.Dock = DockStyle.Right;
            bnStartGrab.FlatStyle = FlatStyle.Flat;
            bnStartGrab.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            bnStartGrab.ForeColor = Color.FromArgb(32, 32, 32);
            bnStartGrab.Image = (Image)resources.GetObject("bnStartGrab.Image");
            bnStartGrab.ImageAlign = ContentAlignment.MiddleLeft;
            bnStartGrab.Location = new Point(78, 67);
            bnStartGrab.Name = "bnStartGrab";
            bnStartGrab.Padding = new Padding(8, 0, 0, 0);
            bnStartGrab.Size = new Size(98, 37);
            bnStartGrab.TabIndex = 3;
            bnStartGrab.Text = "Start";
            bnStartGrab.TextImageRelation = TextImageRelation.ImageBeforeText;
            bnStartGrab.UseCompatibleTextRendering = true;
            bnStartGrab.UseVisualStyleBackColor = false;
            bnStartGrab.Click += BnStartGrab_Click;
            // 
            // cbSoftTrigger
            // 
            cbSoftTrigger.AutoSize = true;
            cbSoftTrigger.Font = new Font("Segoe UI", 9F);
            cbSoftTrigger.Location = new Point(3, 35);
            cbSoftTrigger.Name = "cbSoftTrigger";
            cbSoftTrigger.Size = new Size(161, 26);
            cbSoftTrigger.TabIndex = 5;
            cbSoftTrigger.Text = "Trigger by Software";
            cbSoftTrigger.UseCompatibleTextRendering = true;
            cbSoftTrigger.UseVisualStyleBackColor = true;
            cbSoftTrigger.CheckedChanged += CbSoftTrigger_CheckedChanged;
            // 
            // bnTriggerExec
            // 
            bnTriggerExec.BackColor = SystemColors.InactiveCaption;
            bnTriggerExec.Dock = DockStyle.Fill;
            bnTriggerExec.FlatStyle = FlatStyle.Flat;
            bnTriggerExec.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            bnTriggerExec.ForeColor = Color.FromArgb(32, 32, 32);
            bnTriggerExec.Location = new Point(182, 35);
            bnTriggerExec.Name = "bnTriggerExec";
            bnTriggerExec.Size = new Size(174, 26);
            bnTriggerExec.TabIndex = 6;
            bnTriggerExec.Text = "Trigger Once";
            bnTriggerExec.UseCompatibleTextRendering = true;
            bnTriggerExec.UseVisualStyleBackColor = false;
            bnTriggerExec.Click += BnTriggerExec_Click;
            // 
            // grpDeviceConnection
            // 
            grpDeviceConnection.Controls.Add(TblSearchDevice);
            grpDeviceConnection.Dock = DockStyle.Fill;
            grpDeviceConnection.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            grpDeviceConnection.Location = new Point(4, 4);
            grpDeviceConnection.Name = "grpDeviceConnection";
            grpDeviceConnection.Size = new Size(307, 139);
            grpDeviceConnection.TabIndex = 2;
            grpDeviceConnection.TabStop = false;
            grpDeviceConnection.Text = "Device Connection";
            grpDeviceConnection.UseCompatibleTextRendering = true;
            // 
            // TblSearchDevice
            // 
            TblSearchDevice.ColumnCount = 1;
            TblSearchDevice.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TblSearchDevice.Controls.Add(cbDeviceList, 0, 0);
            TblSearchDevice.Controls.Add(TblDeviceSearch, 0, 1);
            TblSearchDevice.Dock = DockStyle.Fill;
            TblSearchDevice.Location = new Point(3, 23);
            TblSearchDevice.Name = "TblSearchDevice";
            TblSearchDevice.RowCount = 3;
            TblSearchDevice.RowStyles.Add(new RowStyle(SizeType.Absolute, 53F));
            TblSearchDevice.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            TblSearchDevice.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblSearchDevice.Size = new Size(301, 113);
            TblSearchDevice.TabIndex = 0;
            // 
            // cbDeviceList
            // 
            cbDeviceList.Dock = DockStyle.Fill;
            cbDeviceList.Font = new Font("Segoe UI", 9F);
            cbDeviceList.FormattingEnabled = true;
            cbDeviceList.Location = new Point(3, 3);
            cbDeviceList.Name = "cbDeviceList";
            cbDeviceList.Size = new Size(295, 28);
            cbDeviceList.TabIndex = 0;
            // 
            // TblDeviceSearch
            // 
            TblDeviceSearch.ColumnCount = 3;
            TblDeviceSearch.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            TblDeviceSearch.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            TblDeviceSearch.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96F));
            TblDeviceSearch.Controls.Add(bnEnum, 0, 0);
            TblDeviceSearch.Controls.Add(bnOpen, 1, 0);
            TblDeviceSearch.Controls.Add(bnClose, 2, 0);
            TblDeviceSearch.Dock = DockStyle.Fill;
            TblDeviceSearch.Location = new Point(3, 56);
            TblDeviceSearch.Name = "TblDeviceSearch";
            TblDeviceSearch.RowCount = 1;
            TblDeviceSearch.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblDeviceSearch.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            TblDeviceSearch.Size = new Size(295, 46);
            TblDeviceSearch.TabIndex = 1;
            // 
            // bnEnum
            // 
            bnEnum.BackColor = SystemColors.InactiveCaption;
            bnEnum.Dock = DockStyle.Fill;
            bnEnum.FlatStyle = FlatStyle.Flat;
            bnEnum.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            bnEnum.ForeColor = Color.FromArgb(32, 32, 32);
            bnEnum.Image = (Image)resources.GetObject("bnEnum.Image");
            bnEnum.ImageAlign = ContentAlignment.MiddleLeft;
            bnEnum.Location = new Point(3, 3);
            bnEnum.Name = "bnEnum";
            bnEnum.Size = new Size(93, 40);
            bnEnum.TabIndex = 1;
            bnEnum.Text = "Search";
            bnEnum.TextImageRelation = TextImageRelation.ImageBeforeText;
            bnEnum.UseCompatibleTextRendering = true;
            bnEnum.UseVisualStyleBackColor = false;
            bnEnum.Click += BnEnum_Click;
            // 
            // bnOpen
            // 
            bnOpen.BackColor = SystemColors.InactiveCaption;
            bnOpen.Dock = DockStyle.Fill;
            bnOpen.FlatStyle = FlatStyle.Flat;
            bnOpen.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            bnOpen.ForeColor = Color.FromArgb(32, 32, 32);
            bnOpen.Image = (Image)resources.GetObject("bnOpen.Image");
            bnOpen.ImageAlign = ContentAlignment.MiddleLeft;
            bnOpen.Location = new Point(102, 3);
            bnOpen.Name = "bnOpen";
            bnOpen.Size = new Size(93, 40);
            bnOpen.TabIndex = 2;
            bnOpen.Text = "Open";
            bnOpen.TextImageRelation = TextImageRelation.ImageBeforeText;
            bnOpen.UseCompatibleTextRendering = true;
            bnOpen.UseVisualStyleBackColor = false;
            bnOpen.Click += BnOpen_Click;
            // 
            // bnClose
            // 
            bnClose.BackColor = SystemColors.InactiveCaption;
            bnClose.Dock = DockStyle.Fill;
            bnClose.FlatStyle = FlatStyle.Flat;
            bnClose.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            bnClose.ForeColor = Color.FromArgb(32, 32, 32);
            bnClose.Image = (Image)resources.GetObject("bnClose.Image");
            bnClose.ImageAlign = ContentAlignment.MiddleLeft;
            bnClose.Location = new Point(201, 3);
            bnClose.Name = "bnClose";
            bnClose.Size = new Size(91, 40);
            bnClose.TabIndex = 3;
            bnClose.Text = "Close";
            bnClose.TextImageRelation = TextImageRelation.ImageBeforeText;
            bnClose.UseCompatibleTextRendering = true;
            bnClose.UseVisualStyleBackColor = false;
            bnClose.Click += BnClose_Click;
            // 
            // StatusStripMenu
            // 
            StatusStripMenu.Dock = DockStyle.Fill;
            StatusStripMenu.Font = new Font("Palatino Linotype", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            StatusStripMenu.ImageScalingSize = new Size(20, 20);
            StatusStripMenu.Items.AddRange(new ToolStripItem[] { toolStripMenu, toolStripStatusLabel1, tsslUserManual, toolStripVersion });
            StatusStripMenu.Location = new Point(0, 742);
            StatusStripMenu.Name = "StatusStripMenu";
            StatusStripMenu.Padding = new Padding(1, 0, 12, 0);
            StatusStripMenu.Size = new Size(1321, 30);
            StatusStripMenu.TabIndex = 3;
            StatusStripMenu.Text = "Selection mode and info";
            // 
            // toolStripMenu
            // 
            toolStripMenu.Font = new Font("Palatino Linotype", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            toolStripMenu.Name = "toolStripMenu";
            toolStripMenu.Size = new Size(1249, 24);
            toolStripMenu.Spring = true;
            toolStripMenu.Text = "Selection Info";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.IsLink = true;
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(80, 24);
            toolStripStatusLabel1.Text = "Document";
            toolStripStatusLabel1.Visible = false;
            toolStripStatusLabel1.Click += ToolStripStatusLabel1_Click;
            // 
            // tsslUserManual
            // 
            tsslUserManual.Font = new Font("Palatino Linotype", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tsslUserManual.IsLink = true;
            tsslUserManual.Name = "tsslUserManual";
            tsslUserManual.Size = new Size(95, 24);
            tsslUserManual.Text = "User Manual";
            tsslUserManual.Visible = false;
            tsslUserManual.Click += TsslUserManual_Click;
            // 
            // toolStripVersion
            // 
            toolStripVersion.Font = new Font("Palatino Linotype", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            toolStripVersion.Name = "toolStripVersion";
            toolStripVersion.Size = new Size(59, 24);
            toolStripVersion.Text = "Version";
            // 
            // listBoxResult1
            // 
            listBoxResult1.FormattingEnabled = true;
            listBoxResult1.Location = new Point(1180, 468);
            listBoxResult1.Name = "listBoxResult1";
            listBoxResult1.Size = new Size(134, 104);
            listBoxResult1.TabIndex = 1;
            // 
            // msAppMenu
            // 
            msAppMenu.BackColor = Color.FloralWhite;
            msAppMenu.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            msAppMenu.ImageScalingSize = new Size(20, 20);
            msAppMenu.Items.AddRange(new ToolStripItem[] { pictureStorageToolStripMenuItem, toolStripMenuItem1 });
            msAppMenu.Location = new Point(0, 0);
            msAppMenu.Name = "msAppMenu";
            msAppMenu.Size = new Size(1321, 28);
            msAppMenu.TabIndex = 2;
            msAppMenu.Text = "menuStrip1";
            // 
            // pictureStorageToolStripMenuItem
            // 
            pictureStorageToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { saveAsBMPToolStripMenuItem, saveAsJPGToolStripMenuItem, saveAsTIFFToolStripMenuItem, saveAsPNGToolStripMenuItem });
            pictureStorageToolStripMenuItem.Name = "pictureStorageToolStripMenuItem";
            pictureStorageToolStripMenuItem.Size = new Size(128, 24);
            pictureStorageToolStripMenuItem.Text = "Picture Storage";
            // 
            // saveAsBMPToolStripMenuItem
            // 
            saveAsBMPToolStripMenuItem.Name = "saveAsBMPToolStripMenuItem";
            saveAsBMPToolStripMenuItem.Size = new Size(178, 26);
            saveAsBMPToolStripMenuItem.Text = "Save as BMP";
            saveAsBMPToolStripMenuItem.Click += SaveAsBMPToolStripMenuItem_Click;
            // 
            // saveAsJPGToolStripMenuItem
            // 
            saveAsJPGToolStripMenuItem.Name = "saveAsJPGToolStripMenuItem";
            saveAsJPGToolStripMenuItem.Size = new Size(178, 26);
            saveAsJPGToolStripMenuItem.Text = "Save as JPG";
            saveAsJPGToolStripMenuItem.Click += SaveAsJPGToolStripMenuItem_Click;
            // 
            // saveAsTIFFToolStripMenuItem
            // 
            saveAsTIFFToolStripMenuItem.Name = "saveAsTIFFToolStripMenuItem";
            saveAsTIFFToolStripMenuItem.Size = new Size(178, 26);
            saveAsTIFFToolStripMenuItem.Text = "Save as TIFF";
            saveAsTIFFToolStripMenuItem.Click += SaveAsTIFFToolStripMenuItem_Click;
            // 
            // saveAsPNGToolStripMenuItem
            // 
            saveAsPNGToolStripMenuItem.Name = "saveAsPNGToolStripMenuItem";
            saveAsPNGToolStripMenuItem.Size = new Size(178, 26);
            saveAsPNGToolStripMenuItem.Text = "Save as PNG";
            saveAsPNGToolStripMenuItem.Click += SaveAsPNGToolStripMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { hideMenuoolStripMenuItem, hideResultWinToolStripMenuItem, hideGridRecToolStripMenuItem });
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(96, 24);
            toolStripMenuItem1.Text = "UI Settings";
            // 
            // hideMenuoolStripMenuItem
            // 
            hideMenuoolStripMenuItem.Name = "hideMenuoolStripMenuItem";
            hideMenuoolStripMenuItem.Size = new Size(229, 26);
            hideMenuoolStripMenuItem.Text = "Hide Menu";
            hideMenuoolStripMenuItem.Click += HideMenuoolStripMenuItem_Click;
            // 
            // hideResultWinToolStripMenuItem
            // 
            hideResultWinToolStripMenuItem.Name = "hideResultWinToolStripMenuItem";
            hideResultWinToolStripMenuItem.Size = new Size(229, 26);
            hideResultWinToolStripMenuItem.Text = "Hide Result Window";
            hideResultWinToolStripMenuItem.Click += HideResultWinToolStripMenuItem_Click;
            // 
            // hideGridRecToolStripMenuItem
            // 
            hideGridRecToolStripMenuItem.Name = "hideGridRecToolStripMenuItem";
            hideGridRecToolStripMenuItem.Size = new Size(229, 26);
            hideGridRecToolStripMenuItem.Text = "Hide Grid Records";
            hideGridRecToolStripMenuItem.Click += HideGridRecToolStripMenuItem_Click;
            // 
            // CameraDemo
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.AliceBlue;
            ClientSize = new Size(1321, 800);
            Controls.Add(TblMain);
            Controls.Add(listBoxResult1);
            Controls.Add(msAppMenu);
            Font = new Font("Segoe UI", 9F);
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MainMenuStrip = msAppMenu;
            Name = "CameraDemo";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Main Form";
            WindowState = FormWindowState.Maximized;
            FormClosing += CameraDemo_FormClosing;
            Load += MainForm_Load;
            TblMain.ResumeLayout(false);
            TblMain.PerformLayout();
            TblBodyMain.ResumeLayout(false);
            SplitConImageViewAndButton.Panel1.ResumeLayout(false);
            SplitConImageViewAndButton.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SplitConImageViewAndButton).EndInit();
            SplitConImageViewAndButton.ResumeLayout(false);
            TblPnlButtons.ResumeLayout(false);
            grpConfigs.ResumeLayout(false);
            flowPnlConfig.ResumeLayout(false);
            grpTrainAndDecode.ResumeLayout(false);
            flowPnlTrainAndDecode.ResumeLayout(false);
            GrpRoiTypes.ResumeLayout(false);
            flowLayOutPnlRoiTypes.ResumeLayout(false);
            splitConBodyMain.Panel1.ResumeLayout(false);
            splitConBodyMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitConBodyMain).EndInit();
            splitConBodyMain.ResumeLayout(false);
            SplitContainerImageView.Panel1.ResumeLayout(false);
            SplitContainerImageView.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SplitContainerImageView).EndInit();
            SplitContainerImageView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)ImageCanvas).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvDecodeTextRec).EndInit();
            TblPnlSettingAndRoiData.ResumeLayout(false);
            TblBlobSettings.ResumeLayout(false);
            GrpBoxSegMentMode.ResumeLayout(false);
            TblPnlImageList.ResumeLayout(false);
            TblDeviceConfig.ResumeLayout(false);
            GrpSetParam.ResumeLayout(false);
            TblParameter.ResumeLayout(false);
            TblParameter.PerformLayout();
            GrpImageAq.ResumeLayout(false);
            TblImageAq.ResumeLayout(false);
            TblImageAq.PerformLayout();
            grpDeviceConnection.ResumeLayout(false);
            TblSearchDevice.ResumeLayout(false);
            TblDeviceSearch.ResumeLayout(false);
            StatusStripMenu.ResumeLayout(false);
            StatusStripMenu.PerformLayout();
            msAppMenu.ResumeLayout(false);
            msAppMenu.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel TblMain;
        private TableLayoutPanel TblBodyMain;
        private SplitContainer SplitConImageViewAndButton;
        private TableLayoutPanel TblPnlSettingAndRoiData;
        private FlowLayoutPanel FlowPnlRoiData;
        private TableLayoutPanel TblPnlButtons;
        private Button BtnTrainSelROI;
        private Button BtnAddRoi;
        private Button BtnShowTamplate;
        private Button BtnDecodeAllROI;
        private PictureBox ImageCanvas;
        private TableLayoutPanel TblBlobSettings;
        private GroupBox GrpBoxSegMentMode;
        private ComboBox CmbSegments;
        private Button BtnAddBarCodeRoi;
        private TableLayoutPanel TblPnlImageList;
        private Button BtnLoadRoi;
        private Button BtnSaveRoi;
        private StatusStrip StatusStripMenu;
        private ToolStripStatusLabel toolStripMenu;
        private ToolStripStatusLabel toolStripVersion;
        private Button BtnAddTempateROI;
        private TableLayoutPanel TblSearchDevice;
        private ComboBox cbDeviceList;
        private TableLayoutPanel TblDeviceSearch;
        private Button bnOpen;
        private Button bnEnum;
        private TableLayoutPanel TblDeviceConfig;
        private GroupBox GrpImageAq;
        private TableLayoutPanel TblImageAq;
        private Button bnStopGrab;
        private Button bnStartGrab;
        private RadioButton bnTriggerMode;
        private RadioButton bnContinuesMode;
        private CheckBox cbSoftTrigger;
        private Button bnTriggerExec;
        private GroupBox GrpSetParam;
        private TableLayoutPanel TblParameter;
        private TextBox tbFrameRate;
        private TextBox tbGain;
        private Label LblGain;
        private Label LblExposerTime;
        private TextBox tbExposure;
        private Label LblFrameRate;
        private Button bnSetParam;
        private Button bnGetParam;
        private ListBox listBoxResult1;
        private DataGridView dgvDecodeTextRec;
        private SplitContainer SplitContainerImageView;
        private SplitContainer splitConBodyMain;
        private DataGridViewTextBoxColumn DisRoiType;
        private DataGridViewTextBoxColumn DecodedText;
        private DataGridViewTextBoxColumn Result;
        private DataGridViewTextBoxColumn ThresoldValue;
        private DataGridViewTextBoxColumn RoiAngle;
        private DataGridViewTextBoxColumn ExecutionTime;
        private DataGridViewTextBoxColumn DecodeTime;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel tsslUserManual;
        private GroupBox grpDeviceConnection;
        private MenuStrip msAppMenu;
        private ToolStripMenuItem pictureStorageToolStripMenuItem;
        private ToolStripMenuItem saveAsBMPToolStripMenuItem;
        private ToolStripMenuItem saveAsJPGToolStripMenuItem;
        private ToolStripMenuItem saveAsTIFFToolStripMenuItem;
        private ToolStripMenuItem saveAsPNGToolStripMenuItem;
        private GroupBox grpTrainAndDecode;
        private FlowLayoutPanel flowPnlTrainAndDecode;
        private GroupBox GrpRoiTypes;
        private FlowLayoutPanel flowLayOutPnlRoiTypes;
        private GroupBox grpConfigs;
        private FlowLayoutPanel flowPnlConfig;
        private Button bnClose;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem hideMenuoolStripMenuItem;
        private ToolStripMenuItem hideResultWinToolStripMenuItem;
        private ToolStripMenuItem hideGridRecToolStripMenuItem;
    }
}
