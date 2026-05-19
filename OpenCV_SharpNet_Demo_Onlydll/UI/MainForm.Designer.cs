namespace OpenCV_SharpNet_Demo
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
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
            TblPnlImageList = new TableLayoutPanel();
            BtnLoadImage = new Button();
            LstImageList = new ListBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            BtnNextImage = new Button();
            BtnPreviousImage = new Button();
            StatusStripMenu = new StatusStrip();
            toolStripMenu = new ToolStripStatusLabel();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            tsslUserManual = new ToolStripStatusLabel();
            toolStripVersion = new ToolStripStatusLabel();
            msAppMenu = new MenuStrip();
            pictureStorageToolStripMenuItem = new ToolStripMenuItem();
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
            TblPnlImageList.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            StatusStripMenu.SuspendLayout();
            msAppMenu.SuspendLayout();
            SuspendLayout();
            // 
            // TblMain
            // 
            TblMain.BackColor = Color.Transparent;
            TblMain.ColumnCount = 1;
            TblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TblMain.Controls.Add(TblBodyMain, 0, 1);
            TblMain.Controls.Add(TblPnlImageList, 0, 0);
            TblMain.Controls.Add(StatusStripMenu, 0, 2);
            TblMain.Dock = DockStyle.Fill;
            TblMain.Location = new Point(0, 28);
            TblMain.Name = "TblMain";
            TblMain.RowCount = 3;
            TblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 117F));
            TblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            TblMain.Size = new Size(1321, 732);
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
            TblBodyMain.Location = new Point(3, 120);
            TblBodyMain.Name = "TblBodyMain";
            TblBodyMain.RowCount = 1;
            TblBodyMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblBodyMain.Size = new Size(1315, 579);
            TblBodyMain.TabIndex = 1;
            // 
            // SplitConImageViewAndButton
            // 
            SplitConImageViewAndButton.BackColor = Color.Transparent;
            SplitConImageViewAndButton.BorderStyle = BorderStyle.FixedSingle;
            SplitConImageViewAndButton.Dock = DockStyle.Fill;
            SplitConImageViewAndButton.Location = new Point(4, 4);
            SplitConImageViewAndButton.Name = "SplitConImageViewAndButton";
            // 
            // SplitConImageViewAndButton.Panel1
            // 
            SplitConImageViewAndButton.Panel1.BackColor = Color.Transparent;
            SplitConImageViewAndButton.Panel1.Controls.Add(TblPnlButtons);
            // 
            // SplitConImageViewAndButton.Panel2
            // 
            SplitConImageViewAndButton.Panel2.Controls.Add(splitConBodyMain);
            SplitConImageViewAndButton.Size = new Size(1307, 571);
            SplitConImageViewAndButton.SplitterDistance = 131;
            SplitConImageViewAndButton.TabIndex = 0;
            SplitConImageViewAndButton.SplitterMoved += SplitConImageViewAndButton_SplitterMoved;
            // 
            // TblPnlButtons
            // 
            TblPnlButtons.AutoScroll = true;
            TblPnlButtons.BackColor = Color.Transparent;
            TblPnlButtons.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            TblPnlButtons.ColumnCount = 1;
            TblPnlButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TblPnlButtons.Controls.Add(grpConfigs, 0, 2);
            TblPnlButtons.Controls.Add(grpTrainAndDecode, 0, 1);
            TblPnlButtons.Controls.Add(GrpRoiTypes, 0, 0);
            TblPnlButtons.Dock = DockStyle.Fill;
            TblPnlButtons.Location = new Point(0, 0);
            TblPnlButtons.Name = "TblPnlButtons";
            TblPnlButtons.RowCount = 4;
            TblPnlButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 192F));
            TblPnlButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 141F));
            TblPnlButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 189F));
            TblPnlButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblPnlButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            TblPnlButtons.Size = new Size(129, 569);
            TblPnlButtons.TabIndex = 0;
            // 
            // grpConfigs
            // 
            grpConfigs.BackColor = Color.Transparent;
            grpConfigs.Controls.Add(flowPnlConfig);
            grpConfigs.Dock = DockStyle.Fill;
            grpConfigs.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpConfigs.ForeColor = Color.FromArgb(80, 80, 80);
            grpConfigs.Location = new Point(4, 339);
            grpConfigs.Name = "grpConfigs";
            grpConfigs.Size = new Size(121, 183);
            grpConfigs.TabIndex = 3;
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
            flowPnlConfig.Size = new Size(115, 157);
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
            BtnSaveRoi.Padding = new Padding(9, 0, 0, 0);
            BtnSaveRoi.Size = new Size(156, 44);
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
            BtnLoadRoi.Padding = new Padding(9, 0, 0, 0);
            BtnLoadRoi.Size = new Size(156, 44);
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
            BtnShowTamplate.Padding = new Padding(9, 0, 0, 0);
            BtnShowTamplate.Size = new Size(156, 44);
            BtnShowTamplate.TabIndex = 7;
            BtnShowTamplate.Text = "&All Tamplates";
            BtnShowTamplate.TextImageRelation = TextImageRelation.ImageBeforeText;
            BtnShowTamplate.UseCompatibleTextRendering = true;
            BtnShowTamplate.UseVisualStyleBackColor = false;
            BtnShowTamplate.Click += BtnShowTamplate_Click;
            // 
            // grpTrainAndDecode
            // 
            grpTrainAndDecode.BackColor = Color.Transparent;
            grpTrainAndDecode.Controls.Add(flowPnlTrainAndDecode);
            grpTrainAndDecode.Dock = DockStyle.Fill;
            grpTrainAndDecode.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpTrainAndDecode.ForeColor = Color.FromArgb(80, 80, 80);
            grpTrainAndDecode.Location = new Point(4, 197);
            grpTrainAndDecode.Name = "grpTrainAndDecode";
            grpTrainAndDecode.Size = new Size(121, 135);
            grpTrainAndDecode.TabIndex = 2;
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
            flowPnlTrainAndDecode.Size = new Size(115, 109);
            flowPnlTrainAndDecode.TabIndex = 0;
            // 
            // BtnTrainSelROI
            // 
            BtnTrainSelROI.BackColor = Color.LightCyan;
            BtnTrainSelROI.FlatStyle = FlatStyle.Flat;
            BtnTrainSelROI.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            BtnTrainSelROI.ForeColor = Color.FromArgb(32, 32, 32);
            BtnTrainSelROI.Image = (Image)resources.GetObject("BtnTrainSelROI.Image");
            BtnTrainSelROI.ImageAlign = ContentAlignment.MiddleLeft;
            BtnTrainSelROI.Location = new Point(3, 3);
            BtnTrainSelROI.Name = "BtnTrainSelROI";
            BtnTrainSelROI.Padding = new Padding(9, 0, 0, 0);
            BtnTrainSelROI.Size = new Size(156, 44);
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
            BtnDecodeAllROI.ForeColor = Color.FromArgb(32, 32, 32);
            BtnDecodeAllROI.Image = (Image)resources.GetObject("BtnDecodeAllROI.Image");
            BtnDecodeAllROI.ImageAlign = ContentAlignment.MiddleLeft;
            BtnDecodeAllROI.Location = new Point(3, 53);
            BtnDecodeAllROI.Name = "BtnDecodeAllROI";
            BtnDecodeAllROI.Padding = new Padding(9, 0, 0, 0);
            BtnDecodeAllROI.Size = new Size(156, 44);
            BtnDecodeAllROI.TabIndex = 6;
            BtnDecodeAllROI.Text = "Decode &All ";
            BtnDecodeAllROI.TextImageRelation = TextImageRelation.ImageBeforeText;
            BtnDecodeAllROI.UseCompatibleTextRendering = true;
            BtnDecodeAllROI.UseVisualStyleBackColor = false;
            BtnDecodeAllROI.Click += BtnDecodeAllROI_Click;
            // 
            // GrpRoiTypes
            // 
            GrpRoiTypes.BackColor = Color.Transparent;
            GrpRoiTypes.Controls.Add(flowLayOutPnlRoiTypes);
            GrpRoiTypes.Dock = DockStyle.Fill;
            GrpRoiTypes.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            GrpRoiTypes.ForeColor = Color.FromArgb(80, 80, 80);
            GrpRoiTypes.Location = new Point(4, 4);
            GrpRoiTypes.Name = "GrpRoiTypes";
            GrpRoiTypes.Size = new Size(121, 186);
            GrpRoiTypes.TabIndex = 1;
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
            flowLayOutPnlRoiTypes.Size = new Size(115, 160);
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
            BtnAddRoi.Padding = new Padding(9, 0, 0, 0);
            BtnAddRoi.Size = new Size(156, 44);
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
            BtnAddBarCodeRoi.Padding = new Padding(9, 0, 0, 0);
            BtnAddBarCodeRoi.Size = new Size(156, 44);
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
            BtnAddTempateROI.Padding = new Padding(9, 0, 0, 0);
            BtnAddTempateROI.Size = new Size(156, 44);
            BtnAddTempateROI.TabIndex = 11;
            BtnAddTempateROI.Text = "TM ROI";
            BtnAddTempateROI.TextImageRelation = TextImageRelation.ImageBeforeText;
            BtnAddTempateROI.UseCompatibleTextRendering = true;
            BtnAddTempateROI.UseVisualStyleBackColor = false;
            BtnAddTempateROI.Click += BtnAddTempateROI_Click;
            // 
            // splitConBodyMain
            // 
            splitConBodyMain.BackColor = Color.Transparent;
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
            splitConBodyMain.Size = new Size(1172, 571);
            splitConBodyMain.SplitterDistance = 818;
            splitConBodyMain.TabIndex = 2;
            // 
            // SplitContainerImageView
            // 
            SplitContainerImageView.BackColor = Color.Transparent;
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
            SplitContainerImageView.Size = new Size(818, 571);
            SplitContainerImageView.SplitterDistance = 454;
            SplitContainerImageView.TabIndex = 0;
            // 
            // ImageCanvas
            // 
            ImageCanvas.BackColor = SystemColors.InactiveBorder;
            ImageCanvas.Dock = DockStyle.Fill;
            ImageCanvas.Location = new Point(0, 0);
            ImageCanvas.Name = "ImageCanvas";
            ImageCanvas.Size = new Size(816, 452);
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
            dgvDecodeTextRec.BorderStyle = BorderStyle.None;
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
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(32, 32, 32);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(229, 241, 251);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(32, 32, 32);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvDecodeTextRec.DefaultCellStyle = dataGridViewCellStyle2;
            dgvDecodeTextRec.Dock = DockStyle.Fill;
            dgvDecodeTextRec.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvDecodeTextRec.EnableHeadersVisualStyles = false;
            dgvDecodeTextRec.GridColor = Color.FromArgb(224, 224, 224);
            dgvDecodeTextRec.Location = new Point(0, 0);
            dgvDecodeTextRec.Name = "dgvDecodeTextRec";
            dgvDecodeTextRec.RowHeadersVisible = false;
            dgvDecodeTextRec.RowHeadersWidth = 51;
            dgvDecodeTextRec.Size = new Size(816, 111);
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
            TblPnlSettingAndRoiData.Controls.Add(FlowPnlRoiData, 0, 0);
            TblPnlSettingAndRoiData.Dock = DockStyle.Fill;
            TblPnlSettingAndRoiData.Location = new Point(0, 0);
            TblPnlSettingAndRoiData.Name = "TblPnlSettingAndRoiData";
            TblPnlSettingAndRoiData.RowCount = 1;
            TblPnlSettingAndRoiData.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblPnlSettingAndRoiData.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            TblPnlSettingAndRoiData.Size = new Size(348, 569);
            TblPnlSettingAndRoiData.TabIndex = 1;
            // 
            // FlowPnlRoiData
            // 
            FlowPnlRoiData.AutoScroll = true;
            FlowPnlRoiData.Dock = DockStyle.Fill;
            FlowPnlRoiData.Location = new Point(3, 3);
            FlowPnlRoiData.Name = "FlowPnlRoiData";
            FlowPnlRoiData.Size = new Size(342, 563);
            FlowPnlRoiData.TabIndex = 0;
            // 
            // TblPnlImageList
            // 
            TblPnlImageList.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            TblPnlImageList.ColumnCount = 3;
            TblPnlImageList.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 126F));
            TblPnlImageList.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 118F));
            TblPnlImageList.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TblPnlImageList.Controls.Add(BtnLoadImage, 0, 0);
            TblPnlImageList.Controls.Add(LstImageList, 2, 0);
            TblPnlImageList.Controls.Add(tableLayoutPanel1, 1, 0);
            TblPnlImageList.Dock = DockStyle.Fill;
            TblPnlImageList.Location = new Point(3, 3);
            TblPnlImageList.Name = "TblPnlImageList";
            TblPnlImageList.RowCount = 1;
            TblPnlImageList.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblPnlImageList.Size = new Size(1315, 111);
            TblPnlImageList.TabIndex = 2;
            // 
            // BtnLoadImage
            // 
            BtnLoadImage.BackColor = SystemColors.Window;
            BtnLoadImage.Dock = DockStyle.Fill;
            BtnLoadImage.FlatStyle = FlatStyle.Flat;
            BtnLoadImage.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnLoadImage.Image = (Image)resources.GetObject("BtnLoadImage.Image");
            BtnLoadImage.Location = new Point(4, 4);
            BtnLoadImage.Name = "BtnLoadImage";
            BtnLoadImage.Size = new Size(120, 103);
            BtnLoadImage.TabIndex = 0;
            BtnLoadImage.Text = "&Browse Image";
            BtnLoadImage.TextImageRelation = TextImageRelation.ImageAboveText;
            BtnLoadImage.UseCompatibleTextRendering = true;
            BtnLoadImage.UseVisualStyleBackColor = false;
            BtnLoadImage.Click += BtnLoadImage_Click;
            // 
            // LstImageList
            // 
            LstImageList.BackColor = Color.White;
            LstImageList.BorderStyle = BorderStyle.None;
            LstImageList.Dock = DockStyle.Fill;
            LstImageList.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LstImageList.ForeColor = Color.FromArgb(32, 32, 32);
            LstImageList.FormattingEnabled = true;
            LstImageList.Location = new Point(250, 4);
            LstImageList.Name = "LstImageList";
            LstImageList.Size = new Size(1061, 103);
            LstImageList.TabIndex = 0;
            LstImageList.SelectedIndexChanged += LstImageList_SelectedIndexChanged;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(BtnNextImage, 0, 1);
            tableLayoutPanel1.Controls.Add(BtnPreviousImage, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(131, 4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(112, 103);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // BtnNextImage
            // 
            BtnNextImage.BackColor = SystemColors.InactiveCaption;
            BtnNextImage.BackgroundImageLayout = ImageLayout.Center;
            BtnNextImage.Dock = DockStyle.Fill;
            BtnNextImage.FlatStyle = FlatStyle.Flat;
            BtnNextImage.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            BtnNextImage.Image = (Image)resources.GetObject("BtnNextImage.Image");
            BtnNextImage.ImageAlign = ContentAlignment.MiddleLeft;
            BtnNextImage.Location = new Point(3, 9);
            BtnNextImage.Name = "BtnNextImage";
            BtnNextImage.Size = new Size(106, 39);
            BtnNextImage.TabIndex = 1;
            BtnNextImage.Text = "&Next";
            BtnNextImage.TextImageRelation = TextImageRelation.ImageBeforeText;
            BtnNextImage.UseCompatibleTextRendering = true;
            BtnNextImage.UseVisualStyleBackColor = false;
            BtnNextImage.Click += BtnNextImage_Click;
            // 
            // BtnPreviousImage
            // 
            BtnPreviousImage.BackColor = SystemColors.InactiveCaption;
            BtnPreviousImage.BackgroundImageLayout = ImageLayout.Center;
            BtnPreviousImage.Dock = DockStyle.Fill;
            BtnPreviousImage.FlatStyle = FlatStyle.Flat;
            BtnPreviousImage.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            BtnPreviousImage.Image = (Image)resources.GetObject("BtnPreviousImage.Image");
            BtnPreviousImage.ImageAlign = ContentAlignment.MiddleLeft;
            BtnPreviousImage.Location = new Point(3, 54);
            BtnPreviousImage.Name = "BtnPreviousImage";
            BtnPreviousImage.Size = new Size(106, 39);
            BtnPreviousImage.TabIndex = 2;
            BtnPreviousImage.Text = "&Previous";
            BtnPreviousImage.TextImageRelation = TextImageRelation.ImageBeforeText;
            BtnPreviousImage.UseCompatibleTextRendering = true;
            BtnPreviousImage.UseVisualStyleBackColor = false;
            BtnPreviousImage.Click += BtnPreviousImage_Click;
            // 
            // StatusStripMenu
            // 
            StatusStripMenu.BackColor = Color.Transparent;
            StatusStripMenu.Dock = DockStyle.Fill;
            StatusStripMenu.Font = new Font("Palatino Linotype", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            StatusStripMenu.ImageScalingSize = new Size(20, 20);
            StatusStripMenu.Items.AddRange(new ToolStripItem[] { toolStripMenu, toolStripStatusLabel1, tsslUserManual, toolStripVersion });
            StatusStripMenu.Location = new Point(0, 702);
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
            // msAppMenu
            // 
            msAppMenu.BackColor = Color.FloralWhite;
            msAppMenu.ImageScalingSize = new Size(20, 20);
            msAppMenu.Items.AddRange(new ToolStripItem[] { pictureStorageToolStripMenuItem });
            msAppMenu.Location = new Point(0, 0);
            msAppMenu.Name = "msAppMenu";
            msAppMenu.Size = new Size(1321, 28);
            msAppMenu.TabIndex = 3;
            msAppMenu.Text = "menuStrip1";
            // 
            // pictureStorageToolStripMenuItem
            // 
            pictureStorageToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { hideMenuoolStripMenuItem, hideResultWinToolStripMenuItem, hideGridRecToolStripMenuItem });
            pictureStorageToolStripMenuItem.Name = "pictureStorageToolStripMenuItem";
            pictureStorageToolStripMenuItem.Size = new Size(94, 24);
            pictureStorageToolStripMenuItem.Text = "UI Settings";
            pictureStorageToolStripMenuItem.Click += PictureStorageToolStripMenuItem_Click;
            // 
            // hideMenuoolStripMenuItem
            // 
            hideMenuoolStripMenuItem.Name = "hideMenuoolStripMenuItem";
            hideMenuoolStripMenuItem.Size = new Size(227, 26);
            hideMenuoolStripMenuItem.Text = "Hide Menu";
            hideMenuoolStripMenuItem.Click += HideMenuoolStripMenuItem_Click;
            // 
            // hideResultWinToolStripMenuItem
            // 
            hideResultWinToolStripMenuItem.Name = "hideResultWinToolStripMenuItem";
            hideResultWinToolStripMenuItem.Size = new Size(227, 26);
            hideResultWinToolStripMenuItem.Text = "Hide Result Window";
            hideResultWinToolStripMenuItem.Click += HideResultWinToolStripMenuItem_Click;
            // 
            // hideGridRecToolStripMenuItem
            // 
            hideGridRecToolStripMenuItem.Name = "hideGridRecToolStripMenuItem";
            hideGridRecToolStripMenuItem.Size = new Size(227, 26);
            hideGridRecToolStripMenuItem.Text = "Hide Grid Records";
            hideGridRecToolStripMenuItem.Click += HideGridRecToolStripMenuItem_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.AliceBlue;
            ClientSize = new Size(1321, 760);
            Controls.Add(TblMain);
            Controls.Add(msAppMenu);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Main Form";
            WindowState = FormWindowState.Maximized;
            FormClosing += MainForm_FormClosing;
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
            TblPnlImageList.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            StatusStripMenu.ResumeLayout(false);
            StatusStripMenu.PerformLayout();
            msAppMenu.ResumeLayout(false);
            msAppMenu.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel TblMain;
        private ListBox LstImageList;
        private TableLayoutPanel TblBodyMain;
        private SplitContainer SplitConImageViewAndButton;
        private Button BtnLoadImage;
        private TableLayoutPanel TblPnlButtons;
        private Button BtnTrainSelROI;
        private Button BtnAddRoi;
        private Button BtnPreviousImage;
        private Button BtnNextImage;
        private Button BtnShowTamplate;
        private Button BtnDecodeAllROI;
        private Button BtnAddBarCodeRoi;
        private TableLayoutPanel TblPnlImageList;
        private Button BtnLoadRoi;
        private Button BtnSaveRoi;
        private StatusStrip StatusStripMenu;
        private ToolStripStatusLabel toolStripMenu;
        private ToolStripStatusLabel toolStripVersion;
        private Button BtnAddTempateROI;
        private SplitContainer splitConBodyMain;
        private SplitContainer SplitContainerImageView;
        private PictureBox ImageCanvas;
        private DataGridView dgvDecodeTextRec;
        private TableLayoutPanel TblPnlSettingAndRoiData;
        private FlowLayoutPanel FlowPnlRoiData;
        private DataGridViewTextBoxColumn DisRoiType;
        private DataGridViewTextBoxColumn DecodedText;
        private DataGridViewTextBoxColumn Result;
        private DataGridViewTextBoxColumn ThresoldValue;
        private DataGridViewTextBoxColumn RoiAngle;
        private DataGridViewTextBoxColumn ExecutionTime;
        private DataGridViewTextBoxColumn DecodeTime;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel tsslUserManual;
        private GroupBox GrpRoiTypes;
        private FlowLayoutPanel flowLayOutPnlRoiTypes;
        private GroupBox grpTrainAndDecode;
        private FlowLayoutPanel flowPnlTrainAndDecode;
        private GroupBox grpConfigs;
        private FlowLayoutPanel flowPnlConfig;
        private TableLayoutPanel tableLayoutPanel1;
        private MenuStrip msAppMenu;
        private ToolStripMenuItem pictureStorageToolStripMenuItem;
        private ToolStripMenuItem hideMenuoolStripMenuItem;
        private ToolStripMenuItem hideResultWinToolStripMenuItem;
        private ToolStripMenuItem hideGridRecToolStripMenuItem;
    }
}
