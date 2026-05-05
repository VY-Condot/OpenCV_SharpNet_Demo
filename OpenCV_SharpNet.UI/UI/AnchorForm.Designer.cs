namespace OpenCV_SharpNet.UI
{
    partial class AnchorForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnchorForm));
            TblPnlMain = new TableLayoutPanel();
            TblPnlBottom = new TableLayoutPanel();
            BtnExit = new Button();
            BtnOk = new Button();
            GrpRoiPreview = new GroupBox();
            Pb_RoiPreview = new PictureBox();
            GrpSearchLimit = new GroupBox();
            TblPnlSetting = new TableLayoutPanel();
            NumPadRight = new NumericUpDown();
            NumPadLeft = new NumericUpDown();
            NumPadBottom = new NumericUpDown();
            LblBottom = new Label();
            LblTop = new Label();
            LblLeft = new Label();
            LblRight = new Label();
            NumPadTop = new NumericUpDown();
            TblPnlMain.SuspendLayout();
            TblPnlBottom.SuspendLayout();
            GrpRoiPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Pb_RoiPreview).BeginInit();
            GrpSearchLimit.SuspendLayout();
            TblPnlSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)NumPadRight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumPadLeft).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumPadBottom).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumPadTop).BeginInit();
            SuspendLayout();
            // 
            // TblPnlMain
            // 
            TblPnlMain.ColumnCount = 1;
            TblPnlMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TblPnlMain.Controls.Add(TblPnlBottom, 0, 2);
            TblPnlMain.Controls.Add(GrpRoiPreview, 0, 0);
            TblPnlMain.Controls.Add(GrpSearchLimit, 0, 1);
            TblPnlMain.Dock = DockStyle.Fill;
            TblPnlMain.Location = new Point(0, 0);
            TblPnlMain.Name = "TblPnlMain";
            TblPnlMain.RowCount = 3;
            TblPnlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 126F));
            TblPnlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 108F));
            TblPnlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 8F));
            TblPnlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            TblPnlMain.Size = new Size(385, 289);
            TblPnlMain.TabIndex = 0;
            // 
            // TblPnlBottom
            // 
            TblPnlBottom.ColumnCount = 4;
            TblPnlBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            TblPnlBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 75F));
            TblPnlBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 106F));
            TblPnlBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            TblPnlBottom.Controls.Add(BtnExit, 2, 0);
            TblPnlBottom.Controls.Add(BtnOk, 1, 0);
            TblPnlBottom.Dock = DockStyle.Fill;
            TblPnlBottom.Location = new Point(3, 237);
            TblPnlBottom.Name = "TblPnlBottom";
            TblPnlBottom.RowCount = 1;
            TblPnlBottom.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblPnlBottom.Size = new Size(379, 49);
            TblPnlBottom.TabIndex = 3;
            // 
            // BtnExit
            // 
            BtnExit.BackColor = Color.DimGray;
            BtnExit.DialogResult = DialogResult.Cancel;
            BtnExit.Dock = DockStyle.Fill;
            BtnExit.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnExit.ForeColor = Color.Linen;
            BtnExit.Location = new Point(177, 3);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(100, 43);
            BtnExit.TabIndex = 3;
            BtnExit.Text = "&Cancel";
            BtnExit.UseCompatibleTextRendering = true;
            BtnExit.UseVisualStyleBackColor = false;
            BtnExit.Click += BtnExit_Click;
            // 
            // BtnOk
            // 
            BtnOk.BackColor = Color.OrangeRed;
            BtnOk.DialogResult = DialogResult.OK;
            BtnOk.Dock = DockStyle.Fill;
            BtnOk.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnOk.ForeColor = Color.Black;
            BtnOk.Location = new Point(102, 3);
            BtnOk.Name = "BtnOk";
            BtnOk.Size = new Size(69, 43);
            BtnOk.TabIndex = 2;
            BtnOk.Text = "O&K";
            BtnOk.UseCompatibleTextRendering = true;
            BtnOk.UseVisualStyleBackColor = false;
            BtnOk.Click += BtnOk_Click;
            // 
            // GrpRoiPreview
            // 
            GrpRoiPreview.Controls.Add(Pb_RoiPreview);
            GrpRoiPreview.Dock = DockStyle.Fill;
            GrpRoiPreview.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            GrpRoiPreview.Location = new Point(3, 3);
            GrpRoiPreview.Name = "GrpRoiPreview";
            GrpRoiPreview.Size = new Size(379, 120);
            GrpRoiPreview.TabIndex = 0;
            GrpRoiPreview.TabStop = false;
            GrpRoiPreview.Text = "Anchor ROI Preview";
            GrpRoiPreview.UseCompatibleTextRendering = true;
            // 
            // Pb_RoiPreview
            // 
            Pb_RoiPreview.Dock = DockStyle.Fill;
            Pb_RoiPreview.Location = new Point(3, 24);
            Pb_RoiPreview.Name = "Pb_RoiPreview";
            Pb_RoiPreview.Size = new Size(373, 93);
            Pb_RoiPreview.SizeMode = PictureBoxSizeMode.StretchImage;
            Pb_RoiPreview.TabIndex = 0;
            Pb_RoiPreview.TabStop = false;
            // 
            // GrpSearchLimit
            // 
            GrpSearchLimit.Controls.Add(TblPnlSetting);
            GrpSearchLimit.Dock = DockStyle.Fill;
            GrpSearchLimit.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            GrpSearchLimit.Location = new Point(3, 129);
            GrpSearchLimit.Name = "GrpSearchLimit";
            GrpSearchLimit.Size = new Size(379, 102);
            GrpSearchLimit.TabIndex = 1;
            GrpSearchLimit.TabStop = false;
            GrpSearchLimit.Text = "Search Limits (pixels)";
            GrpSearchLimit.UseCompatibleTextRendering = true;
            // 
            // TblPnlSetting
            // 
            TblPnlSetting.ColumnCount = 6;
            TblPnlSetting.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            TblPnlSetting.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 51F));
            TblPnlSetting.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 71F));
            TblPnlSetting.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 69F));
            TblPnlSetting.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 69F));
            TblPnlSetting.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            TblPnlSetting.Controls.Add(NumPadRight, 4, 1);
            TblPnlSetting.Controls.Add(NumPadLeft, 2, 1);
            TblPnlSetting.Controls.Add(NumPadBottom, 4, 0);
            TblPnlSetting.Controls.Add(LblBottom, 3, 0);
            TblPnlSetting.Controls.Add(LblTop, 1, 0);
            TblPnlSetting.Controls.Add(LblLeft, 1, 1);
            TblPnlSetting.Controls.Add(LblRight, 3, 1);
            TblPnlSetting.Controls.Add(NumPadTop, 2, 0);
            TblPnlSetting.Dock = DockStyle.Fill;
            TblPnlSetting.Location = new Point(3, 24);
            TblPnlSetting.Name = "TblPnlSetting";
            TblPnlSetting.RowCount = 2;
            TblPnlSetting.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            TblPnlSetting.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            TblPnlSetting.Size = new Size(373, 75);
            TblPnlSetting.TabIndex = 0;
            // 
            // NumPadRight
            // 
            NumPadRight.Dock = DockStyle.Fill;
            NumPadRight.Location = new Point(250, 40);
            NumPadRight.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            NumPadRight.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            NumPadRight.Name = "NumPadRight";
            NumPadRight.Size = new Size(63, 28);
            NumPadRight.TabIndex = 7;
            NumPadRight.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // NumPadLeft
            // 
            NumPadLeft.Dock = DockStyle.Fill;
            NumPadLeft.Location = new Point(110, 40);
            NumPadLeft.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            NumPadLeft.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            NumPadLeft.Name = "NumPadLeft";
            NumPadLeft.Size = new Size(65, 28);
            NumPadLeft.TabIndex = 6;
            NumPadLeft.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // NumPadBottom
            // 
            NumPadBottom.Dock = DockStyle.Fill;
            NumPadBottom.Location = new Point(250, 3);
            NumPadBottom.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            NumPadBottom.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            NumPadBottom.Name = "NumPadBottom";
            NumPadBottom.Size = new Size(63, 28);
            NumPadBottom.TabIndex = 5;
            NumPadBottom.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // LblBottom
            // 
            LblBottom.AutoSize = true;
            LblBottom.Dock = DockStyle.Fill;
            LblBottom.Location = new Point(181, 3);
            LblBottom.Margin = new Padding(3);
            LblBottom.Name = "LblBottom";
            LblBottom.Size = new Size(63, 31);
            LblBottom.TabIndex = 2;
            LblBottom.Text = "Bottom";
            LblBottom.TextAlign = ContentAlignment.MiddleLeft;
            LblBottom.UseCompatibleTextRendering = true;
            // 
            // LblTop
            // 
            LblTop.AutoSize = true;
            LblTop.Dock = DockStyle.Fill;
            LblTop.Location = new Point(59, 3);
            LblTop.Margin = new Padding(3);
            LblTop.Name = "LblTop";
            LblTop.Size = new Size(45, 31);
            LblTop.TabIndex = 0;
            LblTop.Text = "Top";
            LblTop.TextAlign = ContentAlignment.MiddleLeft;
            LblTop.UseCompatibleTextRendering = true;
            // 
            // LblLeft
            // 
            LblLeft.AutoSize = true;
            LblLeft.Dock = DockStyle.Fill;
            LblLeft.Location = new Point(59, 40);
            LblLeft.Margin = new Padding(3);
            LblLeft.Name = "LblLeft";
            LblLeft.Size = new Size(45, 32);
            LblLeft.TabIndex = 1;
            LblLeft.Text = "Left";
            LblLeft.TextAlign = ContentAlignment.MiddleLeft;
            LblLeft.UseCompatibleTextRendering = true;
            // 
            // LblRight
            // 
            LblRight.AutoSize = true;
            LblRight.Dock = DockStyle.Fill;
            LblRight.Location = new Point(181, 40);
            LblRight.Margin = new Padding(3);
            LblRight.Name = "LblRight";
            LblRight.Size = new Size(63, 32);
            LblRight.TabIndex = 3;
            LblRight.Text = "Right";
            LblRight.TextAlign = ContentAlignment.MiddleLeft;
            LblRight.UseCompatibleTextRendering = true;
            // 
            // NumPadTop
            // 
            NumPadTop.Dock = DockStyle.Fill;
            NumPadTop.Location = new Point(110, 3);
            NumPadTop.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            NumPadTop.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            NumPadTop.Name = "NumPadTop";
            NumPadTop.Size = new Size(65, 28);
            NumPadTop.TabIndex = 4;
            NumPadTop.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // AnchorForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(385, 289);
            Controls.Add(TblPnlMain);
            Font = new Font("Palatino Linotype", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "AnchorForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AnchorForm";
            Load += AnchorForm_Load;
            TblPnlMain.ResumeLayout(false);
            TblPnlBottom.ResumeLayout(false);
            GrpRoiPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Pb_RoiPreview).EndInit();
            GrpSearchLimit.ResumeLayout(false);
            TblPnlSetting.ResumeLayout(false);
            TblPnlSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)NumPadRight).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumPadLeft).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumPadBottom).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumPadTop).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel TblPnlMain;
        private GroupBox GrpRoiPreview;
        private PictureBox Pb_RoiPreview;
        private GroupBox GrpSearchLimit;
        private TableLayoutPanel TblPnlSetting;
        private Label LblRight;
        private Label LblBottom;
        private Label LblLeft;
        private Label LblTop;
        private NumericUpDown NumPadRight;
        private NumericUpDown NumPadLeft;
        private NumericUpDown NumPadBottom;
        private NumericUpDown NumPadTop;
        private TableLayoutPanel TblPnlBottom;
        private Button BtnExit;
        private Button BtnOk;
    }
}