namespace OpenCV_SharpNet.UI.UserControls
{
    partial class GradingView
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GradingView));
            TblPNlMain = new TableLayoutPanel();
            TblPnlBottom = new TableLayoutPanel();
            btnSaveGradingSetting = new Button();
            GrpGradeSetting = new GroupBox();
            tblGrades = new TableLayoutPanel();
            lblIncludeThisGrade = new Label();
            lblGradeType = new Label();
            lblGradeTypeName = new Label();
            chkInCludeGrade = new CheckBox();
            tblPnlData = new TableLayoutPanel();
            flowpnlGrades = new FlowLayoutPanel();
            pnlgradeA = new Panel();
            cmbGradeA = new ComboBox();
            lblGradeA = new Label();
            numPadGradeMaxValueA = new NumericUpDown();
            pnlGradeB = new Panel();
            cmbGradeB = new ComboBox();
            lblGradeB = new Label();
            numPadGradeMaxValueB = new NumericUpDown();
            pnlGradeC = new Panel();
            cmbGradeC = new ComboBox();
            lblGradeC = new Label();
            numPadGradeMaxValueC = new NumericUpDown();
            pnlGradeD = new Panel();
            cmbGradeD = new ComboBox();
            lblGradeD = new Label();
            numPadGradeMaxValueD = new NumericUpDown();
            pnlGradeF = new Panel();
            cmbGradeF = new ComboBox();
            lblGradeF = new Label();
            numPadGradeMaxValueF = new NumericUpDown();
            tableLayoutPanel1 = new TableLayoutPanel();
            lblOptions = new Label();
            lblGradeNames = new Label();
            lblMaxValue = new Label();
            errInfoProvider = new ErrorProvider(components);
            TblPNlMain.SuspendLayout();
            TblPnlBottom.SuspendLayout();
            GrpGradeSetting.SuspendLayout();
            tblGrades.SuspendLayout();
            tblPnlData.SuspendLayout();
            flowpnlGrades.SuspendLayout();
            pnlgradeA.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPadGradeMaxValueA).BeginInit();
            pnlGradeB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPadGradeMaxValueB).BeginInit();
            pnlGradeC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPadGradeMaxValueC).BeginInit();
            pnlGradeD.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPadGradeMaxValueD).BeginInit();
            pnlGradeF.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPadGradeMaxValueF).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errInfoProvider).BeginInit();
            SuspendLayout();
            // 
            // TblPNlMain
            // 
            TblPNlMain.ColumnCount = 1;
            TblPNlMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TblPNlMain.Controls.Add(TblPnlBottom, 0, 1);
            TblPNlMain.Controls.Add(GrpGradeSetting, 0, 0);
            TblPNlMain.Dock = DockStyle.Fill;
            TblPNlMain.Location = new Point(0, 0);
            TblPNlMain.Name = "TblPNlMain";
            TblPNlMain.RowCount = 3;
            TblPNlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 418F));
            TblPNlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 57F));
            TblPNlMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblPNlMain.Size = new Size(658, 483);
            TblPNlMain.TabIndex = 2;
            // 
            // TblPnlBottom
            // 
            TblPnlBottom.ColumnCount = 3;
            TblPnlBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            TblPnlBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 171F));
            TblPnlBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            TblPnlBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            TblPnlBottom.Controls.Add(btnSaveGradingSetting, 1, 0);
            TblPnlBottom.Dock = DockStyle.Fill;
            TblPnlBottom.Location = new Point(3, 421);
            TblPnlBottom.Name = "TblPnlBottom";
            TblPnlBottom.RowCount = 1;
            TblPnlBottom.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblPnlBottom.Size = new Size(652, 51);
            TblPnlBottom.TabIndex = 3;
            // 
            // btnSaveGradingSetting
            // 
            btnSaveGradingSetting.BackColor = Color.Red;
            btnSaveGradingSetting.Dock = DockStyle.Fill;
            btnSaveGradingSetting.Font = new Font("Calibri", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSaveGradingSetting.ForeColor = Color.Transparent;
            btnSaveGradingSetting.Image = (Image)resources.GetObject("btnSaveGradingSetting.Image");
            btnSaveGradingSetting.ImageAlign = ContentAlignment.MiddleLeft;
            btnSaveGradingSetting.Location = new Point(243, 3);
            btnSaveGradingSetting.Name = "btnSaveGradingSetting";
            btnSaveGradingSetting.Padding = new Padding(10, 0, 0, 0);
            btnSaveGradingSetting.Size = new Size(165, 45);
            btnSaveGradingSetting.TabIndex = 2;
            btnSaveGradingSetting.Text = "Apply Setting";
            btnSaveGradingSetting.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnSaveGradingSetting.UseCompatibleTextRendering = true;
            btnSaveGradingSetting.UseVisualStyleBackColor = false;
            btnSaveGradingSetting.Click += BtnSaveGradingSetting_Click;
            // 
            // GrpGradeSetting
            // 
            GrpGradeSetting.BackColor = Color.Transparent;
            GrpGradeSetting.Controls.Add(tblGrades);
            GrpGradeSetting.Dock = DockStyle.Fill;
            GrpGradeSetting.Location = new Point(3, 3);
            GrpGradeSetting.Name = "GrpGradeSetting";
            GrpGradeSetting.Size = new Size(652, 412);
            GrpGradeSetting.TabIndex = 0;
            GrpGradeSetting.TabStop = false;
            GrpGradeSetting.Text = "Grade Setting";
            GrpGradeSetting.UseCompatibleTextRendering = true;
            // 
            // tblGrades
            // 
            tblGrades.ColumnCount = 3;
            tblGrades.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tblGrades.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 191F));
            tblGrades.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 199F));
            tblGrades.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tblGrades.Controls.Add(lblIncludeThisGrade, 2, 0);
            tblGrades.Controls.Add(lblGradeType, 1, 0);
            tblGrades.Controls.Add(lblGradeTypeName, 1, 1);
            tblGrades.Controls.Add(chkInCludeGrade, 2, 1);
            tblGrades.Controls.Add(tblPnlData, 1, 2);
            tblGrades.Dock = DockStyle.Fill;
            tblGrades.Location = new Point(3, 23);
            tblGrades.Name = "tblGrades";
            tblGrades.RowCount = 4;
            tblGrades.RowStyles.Add(new RowStyle(SizeType.Absolute, 31F));
            tblGrades.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tblGrades.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tblGrades.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            tblGrades.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tblGrades.Size = new Size(646, 386);
            tblGrades.TabIndex = 0;
            // 
            // lblIncludeThisGrade
            // 
            lblIncludeThisGrade.AutoSize = true;
            lblIncludeThisGrade.Dock = DockStyle.Fill;
            lblIncludeThisGrade.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblIncludeThisGrade.Location = new Point(214, 3);
            lblIncludeThisGrade.Margin = new Padding(3);
            lblIncludeThisGrade.Name = "lblIncludeThisGrade";
            lblIncludeThisGrade.Size = new Size(429, 25);
            lblIncludeThisGrade.TabIndex = 4;
            lblIncludeThisGrade.Text = "Include :";
            lblIncludeThisGrade.TextAlign = ContentAlignment.MiddleLeft;
            lblIncludeThisGrade.UseCompatibleTextRendering = true;
            // 
            // lblGradeType
            // 
            lblGradeType.AutoSize = true;
            lblGradeType.Dock = DockStyle.Fill;
            lblGradeType.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblGradeType.Location = new Point(23, 3);
            lblGradeType.Margin = new Padding(3);
            lblGradeType.Name = "lblGradeType";
            lblGradeType.Size = new Size(185, 25);
            lblGradeType.TabIndex = 0;
            lblGradeType.Text = "Grade Type :";
            lblGradeType.TextAlign = ContentAlignment.MiddleLeft;
            lblGradeType.UseCompatibleTextRendering = true;
            // 
            // lblGradeTypeName
            // 
            lblGradeTypeName.AutoSize = true;
            lblGradeTypeName.Dock = DockStyle.Fill;
            lblGradeTypeName.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblGradeTypeName.Location = new Point(23, 34);
            lblGradeTypeName.Margin = new Padding(3);
            lblGradeTypeName.Name = "lblGradeTypeName";
            lblGradeTypeName.Size = new Size(185, 28);
            lblGradeTypeName.TabIndex = 2;
            lblGradeTypeName.Text = "Grade Type ";
            lblGradeTypeName.TextAlign = ContentAlignment.MiddleLeft;
            lblGradeTypeName.UseCompatibleTextRendering = true;
            // 
            // chkInCludeGrade
            // 
            chkInCludeGrade.AutoSize = true;
            chkInCludeGrade.Dock = DockStyle.Fill;
            chkInCludeGrade.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkInCludeGrade.Location = new Point(214, 34);
            chkInCludeGrade.Name = "chkInCludeGrade";
            chkInCludeGrade.Size = new Size(429, 28);
            chkInCludeGrade.TabIndex = 5;
            chkInCludeGrade.Text = "Include Grade";
            chkInCludeGrade.UseCompatibleTextRendering = true;
            chkInCludeGrade.UseVisualStyleBackColor = true;
            // 
            // tblPnlData
            // 
            tblPnlData.ColumnCount = 1;
            tblGrades.SetColumnSpan(tblPnlData, 2);
            tblPnlData.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblPnlData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tblPnlData.Controls.Add(flowpnlGrades, 0, 1);
            tblPnlData.Controls.Add(tableLayoutPanel1, 0, 0);
            tblPnlData.Dock = DockStyle.Fill;
            tblPnlData.Location = new Point(23, 68);
            tblPnlData.Name = "tblPnlData";
            tblPnlData.RowCount = 2;
            tblGrades.SetRowSpan(tblPnlData, 2);
            tblPnlData.RowStyles.Add(new RowStyle(SizeType.Percent, 16.1904755F));
            tblPnlData.RowStyles.Add(new RowStyle(SizeType.Percent, 83.8095245F));
            tblPnlData.Size = new Size(620, 315);
            tblPnlData.TabIndex = 9;
            // 
            // flowpnlGrades
            // 
            flowpnlGrades.Controls.Add(pnlgradeA);
            flowpnlGrades.Controls.Add(pnlGradeB);
            flowpnlGrades.Controls.Add(pnlGradeC);
            flowpnlGrades.Controls.Add(pnlGradeD);
            flowpnlGrades.Controls.Add(pnlGradeF);
            flowpnlGrades.FlowDirection = FlowDirection.TopDown;
            flowpnlGrades.Location = new Point(3, 54);
            flowpnlGrades.Name = "flowpnlGrades";
            flowpnlGrades.Size = new Size(606, 254);
            flowpnlGrades.TabIndex = 8;
            // 
            // pnlgradeA
            // 
            pnlgradeA.Controls.Add(cmbGradeA);
            pnlgradeA.Controls.Add(lblGradeA);
            pnlgradeA.Controls.Add(numPadGradeMaxValueA);
            pnlgradeA.Location = new Point(3, 3);
            pnlgradeA.Name = "pnlgradeA";
            pnlgradeA.Size = new Size(584, 44);
            pnlgradeA.TabIndex = 0;
            // 
            // cmbGradeA
            // 
            cmbGradeA.FormattingEnabled = true;
            cmbGradeA.Location = new Point(358, 7);
            cmbGradeA.Name = "cmbGradeA";
            cmbGradeA.Size = new Size(211, 28);
            cmbGradeA.TabIndex = 9;
            // 
            // lblGradeA
            // 
            lblGradeA.AutoSize = true;
            lblGradeA.Location = new Point(57, 15);
            lblGradeA.Name = "lblGradeA";
            lblGradeA.Size = new Size(63, 20);
            lblGradeA.TabIndex = 8;
            lblGradeA.Text = "Grade A";
            // 
            // numPadGradeMaxValueA
            // 
            numPadGradeMaxValueA.DecimalPlaces = 2;
            numPadGradeMaxValueA.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            numPadGradeMaxValueA.Location = new Point(209, 8);
            numPadGradeMaxValueA.Margin = new Padding(3, 8, 3, 3);
            numPadGradeMaxValueA.Name = "numPadGradeMaxValueA";
            numPadGradeMaxValueA.Size = new Size(84, 27);
            numPadGradeMaxValueA.TabIndex = 6;
            // 
            // pnlGradeB
            // 
            pnlGradeB.Controls.Add(cmbGradeB);
            pnlGradeB.Controls.Add(lblGradeB);
            pnlGradeB.Controls.Add(numPadGradeMaxValueB);
            pnlGradeB.Location = new Point(3, 53);
            pnlGradeB.Name = "pnlGradeB";
            pnlGradeB.Size = new Size(584, 44);
            pnlGradeB.TabIndex = 1;
            // 
            // cmbGradeB
            // 
            cmbGradeB.FormattingEnabled = true;
            cmbGradeB.Location = new Point(358, 7);
            cmbGradeB.Name = "cmbGradeB";
            cmbGradeB.Size = new Size(211, 28);
            cmbGradeB.TabIndex = 10;
            // 
            // lblGradeB
            // 
            lblGradeB.AutoSize = true;
            lblGradeB.Location = new Point(56, 13);
            lblGradeB.Name = "lblGradeB";
            lblGradeB.Size = new Size(62, 20);
            lblGradeB.TabIndex = 8;
            lblGradeB.Text = "Grade B";
            // 
            // numPadGradeMaxValueB
            // 
            numPadGradeMaxValueB.DecimalPlaces = 2;
            numPadGradeMaxValueB.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            numPadGradeMaxValueB.Location = new Point(209, 8);
            numPadGradeMaxValueB.Margin = new Padding(3, 8, 3, 3);
            numPadGradeMaxValueB.Name = "numPadGradeMaxValueB";
            numPadGradeMaxValueB.Size = new Size(84, 27);
            numPadGradeMaxValueB.TabIndex = 6;
            // 
            // pnlGradeC
            // 
            pnlGradeC.Controls.Add(cmbGradeC);
            pnlGradeC.Controls.Add(lblGradeC);
            pnlGradeC.Controls.Add(numPadGradeMaxValueC);
            pnlGradeC.Location = new Point(3, 103);
            pnlGradeC.Name = "pnlGradeC";
            pnlGradeC.Size = new Size(584, 44);
            pnlGradeC.TabIndex = 9;
            // 
            // cmbGradeC
            // 
            cmbGradeC.FormattingEnabled = true;
            cmbGradeC.Location = new Point(358, 7);
            cmbGradeC.Name = "cmbGradeC";
            cmbGradeC.Size = new Size(211, 28);
            cmbGradeC.TabIndex = 10;
            // 
            // lblGradeC
            // 
            lblGradeC.AutoSize = true;
            lblGradeC.Location = new Point(56, 13);
            lblGradeC.Name = "lblGradeC";
            lblGradeC.Size = new Size(62, 20);
            lblGradeC.TabIndex = 8;
            lblGradeC.Text = "Grade C";
            // 
            // numPadGradeMaxValueC
            // 
            numPadGradeMaxValueC.DecimalPlaces = 2;
            numPadGradeMaxValueC.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            numPadGradeMaxValueC.Location = new Point(209, 8);
            numPadGradeMaxValueC.Margin = new Padding(3, 8, 3, 3);
            numPadGradeMaxValueC.Name = "numPadGradeMaxValueC";
            numPadGradeMaxValueC.Size = new Size(84, 27);
            numPadGradeMaxValueC.TabIndex = 6;
            // 
            // pnlGradeD
            // 
            pnlGradeD.Controls.Add(cmbGradeD);
            pnlGradeD.Controls.Add(lblGradeD);
            pnlGradeD.Controls.Add(numPadGradeMaxValueD);
            pnlGradeD.Location = new Point(3, 153);
            pnlGradeD.Name = "pnlGradeD";
            pnlGradeD.Size = new Size(584, 44);
            pnlGradeD.TabIndex = 9;
            // 
            // cmbGradeD
            // 
            cmbGradeD.FormattingEnabled = true;
            cmbGradeD.Location = new Point(358, 8);
            cmbGradeD.Name = "cmbGradeD";
            cmbGradeD.Size = new Size(211, 28);
            cmbGradeD.TabIndex = 10;
            // 
            // lblGradeD
            // 
            lblGradeD.AutoSize = true;
            lblGradeD.Location = new Point(56, 13);
            lblGradeD.Name = "lblGradeD";
            lblGradeD.Size = new Size(64, 20);
            lblGradeD.TabIndex = 8;
            lblGradeD.Text = "Grade D";
            // 
            // numPadGradeMaxValueD
            // 
            numPadGradeMaxValueD.DecimalPlaces = 2;
            numPadGradeMaxValueD.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            numPadGradeMaxValueD.Location = new Point(209, 8);
            numPadGradeMaxValueD.Margin = new Padding(3, 8, 3, 3);
            numPadGradeMaxValueD.Name = "numPadGradeMaxValueD";
            numPadGradeMaxValueD.Size = new Size(84, 27);
            numPadGradeMaxValueD.TabIndex = 6;
            // 
            // pnlGradeF
            // 
            pnlGradeF.Controls.Add(cmbGradeF);
            pnlGradeF.Controls.Add(lblGradeF);
            pnlGradeF.Controls.Add(numPadGradeMaxValueF);
            pnlGradeF.Location = new Point(3, 203);
            pnlGradeF.Name = "pnlGradeF";
            pnlGradeF.Size = new Size(584, 44);
            pnlGradeF.TabIndex = 10;
            // 
            // cmbGradeF
            // 
            cmbGradeF.FormattingEnabled = true;
            cmbGradeF.Location = new Point(358, 7);
            cmbGradeF.Name = "cmbGradeF";
            cmbGradeF.Size = new Size(211, 28);
            cmbGradeF.TabIndex = 10;
            // 
            // lblGradeF
            // 
            lblGradeF.AutoSize = true;
            lblGradeF.Location = new Point(56, 13);
            lblGradeF.Name = "lblGradeF";
            lblGradeF.Size = new Size(60, 20);
            lblGradeF.TabIndex = 8;
            lblGradeF.Text = "Grade F";
            // 
            // numPadGradeMaxValueF
            // 
            numPadGradeMaxValueF.DecimalPlaces = 2;
            numPadGradeMaxValueF.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            numPadGradeMaxValueF.Location = new Point(209, 8);
            numPadGradeMaxValueF.Margin = new Padding(3, 8, 3, 3);
            numPadGradeMaxValueF.Name = "numPadGradeMaxValueF";
            numPadGradeMaxValueF.Size = new Size(84, 27);
            numPadGradeMaxValueF.TabIndex = 6;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 186F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 172F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 159F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(lblOptions, 2, 0);
            tableLayoutPanel1.Controls.Add(lblGradeNames, 0, 0);
            tableLayoutPanel1.Controls.Add(lblMaxValue, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(614, 45);
            tableLayoutPanel1.TabIndex = 9;
            // 
            // lblOptions
            // 
            lblOptions.AutoSize = true;
            lblOptions.Dock = DockStyle.Left;
            lblOptions.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblOptions.Location = new Point(361, 3);
            lblOptions.Margin = new Padding(3);
            lblOptions.Name = "lblOptions";
            lblOptions.Size = new Size(86, 39);
            lblOptions.TabIndex = 8;
            lblOptions.Text = "Operators :";
            lblOptions.TextAlign = ContentAlignment.MiddleLeft;
            lblOptions.UseCompatibleTextRendering = true;
            // 
            // lblGradeNames
            // 
            lblGradeNames.AutoSize = true;
            lblGradeNames.Dock = DockStyle.Fill;
            lblGradeNames.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblGradeNames.Location = new Point(3, 3);
            lblGradeNames.Margin = new Padding(3);
            lblGradeNames.Name = "lblGradeNames";
            lblGradeNames.Size = new Size(180, 39);
            lblGradeNames.TabIndex = 7;
            lblGradeNames.Text = "Grades :";
            lblGradeNames.TextAlign = ContentAlignment.MiddleCenter;
            lblGradeNames.UseCompatibleTextRendering = true;
            // 
            // lblMaxValue
            // 
            lblMaxValue.AutoSize = true;
            lblMaxValue.Dock = DockStyle.Left;
            lblMaxValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMaxValue.Location = new Point(189, 3);
            lblMaxValue.Margin = new Padding(3);
            lblMaxValue.Name = "lblMaxValue";
            lblMaxValue.Size = new Size(138, 39);
            lblMaxValue.TabIndex = 3;
            lblMaxValue.Text = "Grade Max Value :";
            lblMaxValue.TextAlign = ContentAlignment.MiddleLeft;
            lblMaxValue.UseCompatibleTextRendering = true;
            // 
            // errInfoProvider
            // 
            errInfoProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errInfoProvider.ContainerControl = this;
            errInfoProvider.Icon = (Icon)resources.GetObject("errInfoProvider.Icon");
            // 
            // GradingView
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(TblPNlMain);
            Name = "GradingView";
            Size = new Size(658, 483);
            Load += GradingView_Load;
            TblPNlMain.ResumeLayout(false);
            TblPnlBottom.ResumeLayout(false);
            GrpGradeSetting.ResumeLayout(false);
            tblGrades.ResumeLayout(false);
            tblGrades.PerformLayout();
            tblPnlData.ResumeLayout(false);
            flowpnlGrades.ResumeLayout(false);
            pnlgradeA.ResumeLayout(false);
            pnlgradeA.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numPadGradeMaxValueA).EndInit();
            pnlGradeB.ResumeLayout(false);
            pnlGradeB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numPadGradeMaxValueB).EndInit();
            pnlGradeC.ResumeLayout(false);
            pnlGradeC.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numPadGradeMaxValueC).EndInit();
            pnlGradeD.ResumeLayout(false);
            pnlGradeD.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numPadGradeMaxValueD).EndInit();
            pnlGradeF.ResumeLayout(false);
            pnlGradeF.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numPadGradeMaxValueF).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)errInfoProvider).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel TblPNlMain;
        private TableLayoutPanel TblPnlBottom;
        private Button btnSaveGradingSetting;
        private GroupBox GrpGradeSetting;
        private TableLayoutPanel tblGrades;
        private Label lblIncludeThisGrade;
        private Label lblGradeType;
        private Label lblGradeTypeName;
        private Label lblMaxValue;
        private CheckBox chkInCludeGrade;
        private Label lblGradeNames;
        private FlowLayoutPanel flowpnlGrades;
        private Panel pnlgradeA;
        private Label lblGradeA;
        private Panel pnlGradeB;
        private Label lblGradeB;
        private Panel pnlGradeC;
        private Label lblGradeC;
        private Panel pnlGradeD;
        private Label lblGradeD;
        private Panel pnlGradeF;
        private Label lblGradeF;
        public NumericUpDown numPadGradeMaxValueA;
        public NumericUpDown numPadGradeMaxValueB;
        public NumericUpDown numPadGradeMaxValueC;
        public NumericUpDown numPadGradeMaxValueD;
        public NumericUpDown numPadGradeMaxValueF;
        private TableLayoutPanel tblPnlData;
        private TableLayoutPanel tableLayoutPanel1;
        private Label lblOptions;
        private ComboBox cmbGradeA;
        private ComboBox cmbGradeB;
        private ComboBox cmbGradeC;
        private ComboBox cmbGradeD;
        private ComboBox cmbGradeF;
        private ErrorProvider errInfoProvider;
    }
}
