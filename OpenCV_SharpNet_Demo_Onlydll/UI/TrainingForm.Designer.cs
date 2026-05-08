namespace OpenCV_SharpNet_Demo
{
    partial class TrainingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrainingForm));
            TblPnlMain = new TableLayoutPanel();
            TblPnlBottom = new TableLayoutPanel();
            BtnExit = new Button();
            BtnSave = new Button();
            LblHeading = new Label();
            FLowPnlTrainedChars = new FlowLayoutPanel();
            TblPnlMain.SuspendLayout();
            TblPnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // TblPnlMain
            // 
            TblPnlMain.ColumnCount = 1;
            TblPnlMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TblPnlMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 22F));
            TblPnlMain.Controls.Add(TblPnlBottom, 0, 2);
            TblPnlMain.Controls.Add(LblHeading, 0, 0);
            TblPnlMain.Controls.Add(FLowPnlTrainedChars, 0, 1);
            TblPnlMain.Dock = DockStyle.Fill;
            TblPnlMain.Location = new Point(0, 0);
            TblPnlMain.Name = "TblPnlMain";
            TblPnlMain.RowCount = 3;
            TblPnlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            TblPnlMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblPnlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
            TblPnlMain.Size = new Size(900, 450);
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
            TblPnlBottom.Controls.Add(BtnSave, 1, 0);
            TblPnlBottom.Dock = DockStyle.Fill;
            TblPnlBottom.Location = new Point(3, 398);
            TblPnlBottom.Name = "TblPnlBottom";
            TblPnlBottom.RowCount = 1;
            TblPnlBottom.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblPnlBottom.Size = new Size(894, 49);
            TblPnlBottom.TabIndex = 4;
            // 
            // BtnExit
            // 
            BtnExit.BackColor = Color.DimGray;
            BtnExit.DialogResult = DialogResult.Cancel;
            BtnExit.Dock = DockStyle.Fill;
            BtnExit.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnExit.ForeColor = Color.Linen;
            BtnExit.Location = new Point(434, 3);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(100, 43);
            BtnExit.TabIndex = 3;
            BtnExit.Text = "&Cancel";
            BtnExit.UseCompatibleTextRendering = true;
            BtnExit.UseVisualStyleBackColor = false;
            BtnExit.Click += BtnExit_Click;
            // 
            // BtnSave
            // 
            BtnSave.BackColor = Color.OrangeRed;
            BtnSave.Dock = DockStyle.Fill;
            BtnSave.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnSave.ForeColor = Color.Black;
            BtnSave.Location = new Point(359, 3);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(69, 43);
            BtnSave.TabIndex = 2;
            BtnSave.Text = "Sa&ve";
            BtnSave.UseCompatibleTextRendering = true;
            BtnSave.UseVisualStyleBackColor = false;
            BtnSave.Click += BtnSave_Click;
            // 
            // LblHeading
            // 
            LblHeading.AutoSize = true;
            LblHeading.Dock = DockStyle.Fill;
            LblHeading.Font = new Font("Palatino Linotype", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblHeading.Location = new Point(3, 3);
            LblHeading.Margin = new Padding(3);
            LblHeading.Name = "LblHeading";
            LblHeading.Size = new Size(894, 34);
            LblHeading.TabIndex = 0;
            LblHeading.Text = "Total Char To Be Train";
            LblHeading.TextAlign = ContentAlignment.MiddleCenter;
            LblHeading.UseCompatibleTextRendering = true;
            // 
            // FLowPnlTrainedChars
            // 
            FLowPnlTrainedChars.AutoScroll = true;
            FLowPnlTrainedChars.Dock = DockStyle.Fill;
            FLowPnlTrainedChars.Location = new Point(3, 43);
            FLowPnlTrainedChars.Name = "FLowPnlTrainedChars";
            FLowPnlTrainedChars.Size = new Size(894, 349);
            FLowPnlTrainedChars.TabIndex = 1;
            // 
            // TrainingForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 450);
            Controls.Add(TblPnlMain);
            Font = new Font("Palatino Linotype", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "TrainingForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Teach Font";
            Load += TrainingForm_Load;
            TblPnlMain.ResumeLayout(false);
            TblPnlMain.PerformLayout();
            TblPnlBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel TblPnlMain;
        private Label LblHeading;
        private FlowLayoutPanel FLowPnlTrainedChars;
        private TableLayoutPanel TblPnlBottom;
        private Button BtnExit;
        private Button BtnSave;
    }
}