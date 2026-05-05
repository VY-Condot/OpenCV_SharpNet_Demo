namespace OpenCV_SharpNet.UI
{
    partial class TamplatesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TamplatesForm));
            TblPnlMain = new TableLayoutPanel();
            SplitConCharTamplates = new SplitContainer();
            LstCharacters = new ListBox();
            FlowPnlTamplates = new FlowLayoutPanel();
            TblPnlLabel = new TableLayoutPanel();
            LblTotalTamplates = new Label();
            LblImageName = new Label();
            LblCharacters = new Label();
            TblPnlButtons = new TableLayoutPanel();
            BtnDeleteTamplates = new Button();
            BtnExit = new Button();
            Ss_Footer = new StatusStrip();
            TblPnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SplitConCharTamplates).BeginInit();
            SplitConCharTamplates.Panel1.SuspendLayout();
            SplitConCharTamplates.Panel2.SuspendLayout();
            SplitConCharTamplates.SuspendLayout();
            TblPnlLabel.SuspendLayout();
            TblPnlButtons.SuspendLayout();
            SuspendLayout();
            // 
            // TblPnlMain
            // 
            TblPnlMain.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            TblPnlMain.ColumnCount = 1;
            TblPnlMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TblPnlMain.Controls.Add(SplitConCharTamplates, 0, 1);
            TblPnlMain.Controls.Add(TblPnlLabel, 0, 0);
            TblPnlMain.Controls.Add(TblPnlButtons, 0, 2);
            TblPnlMain.Dock = DockStyle.Fill;
            TblPnlMain.Location = new Point(0, 0);
            TblPnlMain.Name = "TblPnlMain";
            TblPnlMain.RowCount = 3;
            TblPnlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            TblPnlMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblPnlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 51F));
            TblPnlMain.Size = new Size(1047, 587);
            TblPnlMain.TabIndex = 0;
            // 
            // SplitConCharTamplates
            // 
            SplitConCharTamplates.BorderStyle = BorderStyle.FixedSingle;
            SplitConCharTamplates.Dock = DockStyle.Fill;
            SplitConCharTamplates.Location = new Point(4, 59);
            SplitConCharTamplates.Name = "SplitConCharTamplates";
            // 
            // SplitConCharTamplates.Panel1
            // 
            SplitConCharTamplates.Panel1.Controls.Add(LstCharacters);
            // 
            // SplitConCharTamplates.Panel2
            // 
            SplitConCharTamplates.Panel2.Controls.Add(FlowPnlTamplates);
            SplitConCharTamplates.Size = new Size(1039, 472);
            SplitConCharTamplates.SplitterDistance = 226;
            SplitConCharTamplates.TabIndex = 0;
            // 
            // LstCharacters
            // 
            LstCharacters.Dock = DockStyle.Fill;
            LstCharacters.Font = new Font("Palatino Linotype", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LstCharacters.FormattingEnabled = true;
            LstCharacters.ItemHeight = 24;
            LstCharacters.Location = new Point(0, 0);
            LstCharacters.Name = "LstCharacters";
            LstCharacters.Size = new Size(224, 470);
            LstCharacters.TabIndex = 1;
            LstCharacters.SelectedValueChanged += LstCharacters_SelectedValueChanged;
            // 
            // FlowPnlTamplates
            // 
            FlowPnlTamplates.AutoScroll = true;
            FlowPnlTamplates.Dock = DockStyle.Fill;
            FlowPnlTamplates.Location = new Point(0, 0);
            FlowPnlTamplates.Name = "FlowPnlTamplates";
            FlowPnlTamplates.Size = new Size(807, 470);
            FlowPnlTamplates.TabIndex = 0;
            // 
            // TblPnlLabel
            // 
            TblPnlLabel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            TblPnlLabel.ColumnCount = 3;
            TblPnlLabel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 221F));
            TblPnlLabel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 203F));
            TblPnlLabel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TblPnlLabel.Controls.Add(LblTotalTamplates, 1, 0);
            TblPnlLabel.Controls.Add(LblImageName, 2, 0);
            TblPnlLabel.Controls.Add(LblCharacters, 0, 0);
            TblPnlLabel.Dock = DockStyle.Fill;
            TblPnlLabel.Location = new Point(4, 4);
            TblPnlLabel.Name = "TblPnlLabel";
            TblPnlLabel.RowCount = 1;
            TblPnlLabel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblPnlLabel.Size = new Size(1039, 48);
            TblPnlLabel.TabIndex = 2;
            // 
            // LblTotalTamplates
            // 
            LblTotalTamplates.Dock = DockStyle.Fill;
            LblTotalTamplates.Font = new Font("Palatino Linotype", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblTotalTamplates.Location = new Point(226, 4);
            LblTotalTamplates.Margin = new Padding(3);
            LblTotalTamplates.Name = "LblTotalTamplates";
            LblTotalTamplates.Size = new Size(197, 40);
            LblTotalTamplates.TabIndex = 2;
            LblTotalTamplates.Text = "Tamplates Count : ";
            LblTotalTamplates.TextAlign = ContentAlignment.MiddleLeft;
            LblTotalTamplates.UseCompatibleTextRendering = true;
            // 
            // LblImageName
            // 
            LblImageName.Dock = DockStyle.Fill;
            LblImageName.Font = new Font("Palatino Linotype", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblImageName.Location = new Point(430, 4);
            LblImageName.Margin = new Padding(3);
            LblImageName.Name = "LblImageName";
            LblImageName.Size = new Size(605, 40);
            LblImageName.TabIndex = 1;
            LblImageName.Text = "ImageName";
            LblImageName.TextAlign = ContentAlignment.MiddleCenter;
            LblImageName.UseCompatibleTextRendering = true;
            // 
            // LblCharacters
            // 
            LblCharacters.Dock = DockStyle.Fill;
            LblCharacters.Font = new Font("Palatino Linotype", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblCharacters.ForeColor = Color.Red;
            LblCharacters.Location = new Point(4, 4);
            LblCharacters.Margin = new Padding(3);
            LblCharacters.Name = "LblCharacters";
            LblCharacters.Size = new Size(215, 40);
            LblCharacters.TabIndex = 0;
            LblCharacters.Text = "Trained Chars";
            LblCharacters.TextAlign = ContentAlignment.MiddleLeft;
            LblCharacters.UseCompatibleTextRendering = true;
            // 
            // TblPnlButtons
            // 
            TblPnlButtons.ColumnCount = 4;
            TblPnlButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            TblPnlButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 173F));
            TblPnlButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            TblPnlButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            TblPnlButtons.Controls.Add(BtnDeleteTamplates, 1, 0);
            TblPnlButtons.Controls.Add(BtnExit, 2, 0);
            TblPnlButtons.Dock = DockStyle.Fill;
            TblPnlButtons.Location = new Point(4, 538);
            TblPnlButtons.Name = "TblPnlButtons";
            TblPnlButtons.RowCount = 1;
            TblPnlButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblPnlButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            TblPnlButtons.Size = new Size(1039, 45);
            TblPnlButtons.TabIndex = 3;
            // 
            // BtnDeleteTamplates
            // 
            BtnDeleteTamplates.BackColor = Color.Red;
            BtnDeleteTamplates.Dock = DockStyle.Fill;
            BtnDeleteTamplates.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnDeleteTamplates.ForeColor = Color.White;
            BtnDeleteTamplates.Location = new Point(381, 3);
            BtnDeleteTamplates.Name = "BtnDeleteTamplates";
            BtnDeleteTamplates.Size = new Size(167, 39);
            BtnDeleteTamplates.TabIndex = 1;
            BtnDeleteTamplates.Text = "&Delete Tamplate";
            BtnDeleteTamplates.UseCompatibleTextRendering = true;
            BtnDeleteTamplates.UseVisualStyleBackColor = false;
            BtnDeleteTamplates.Click += BtnDeleteTamplates_Click;
            // 
            // BtnExit
            // 
            BtnExit.BackColor = Color.DimGray;
            BtnExit.Dock = DockStyle.Fill;
            BtnExit.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnExit.ForeColor = Color.Linen;
            BtnExit.Location = new Point(554, 3);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(104, 39);
            BtnExit.TabIndex = 2;
            BtnExit.Text = "E&xit";
            BtnExit.UseCompatibleTextRendering = true;
            BtnExit.UseVisualStyleBackColor = false;
            BtnExit.Click += BtnExit_Click;
            // 
            // Ss_Footer
            // 
            Ss_Footer.ImageScalingSize = new Size(20, 20);
            Ss_Footer.Location = new Point(0, 587);
            Ss_Footer.Name = "Ss_Footer";
            Ss_Footer.Padding = new Padding(1, 0, 16, 0);
            Ss_Footer.Size = new Size(1047, 22);
            Ss_Footer.TabIndex = 1;
            Ss_Footer.Text = "statusStrip1";
            // 
            // TamplatesForm
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            ClientSize = new Size(1047, 609);
            Controls.Add(TblPnlMain);
            Controls.Add(Ss_Footer);
            Font = new Font("Palatino Linotype", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "TamplatesForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Templates";
            Load += TamplatesForm_Load;
            SizeChanged += TamplatesForm_SizeChanged;
            TblPnlMain.ResumeLayout(false);
            SplitConCharTamplates.Panel1.ResumeLayout(false);
            SplitConCharTamplates.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SplitConCharTamplates).EndInit();
            SplitConCharTamplates.ResumeLayout(false);
            TblPnlLabel.ResumeLayout(false);
            TblPnlButtons.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel TblPnlMain;
        private StatusStrip Ss_Footer;
        private SplitContainer SplitConCharTamplates;
        private Label LblCharacters;
        private ListBox LstCharacters;
        private FlowLayoutPanel FlowPnlTamplates;
        private Button BtnDeleteTamplates;
        private TableLayoutPanel TblPnlLabel;
        private Label LblImageName;
        private TableLayoutPanel TblPnlButtons;
        private Button BtnExit;
        private Label LblTotalTamplates;
    }
}