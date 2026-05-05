namespace OpenCV_SharpNet.UI
{
    partial class ROI_RenameForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ROI_RenameForm));
            TblPnlMain = new TableLayoutPanel();
            TblPnlBottom = new TableLayoutPanel();
            BtnExit = new Button();
            BtnOk = new Button();
            LblRenameLbl = new Label();
            TblPnlRename = new TableLayoutPanel();
            LblRenameText = new Label();
            TxtROINewName = new TextBox();
            TblPnlMain.SuspendLayout();
            TblPnlBottom.SuspendLayout();
            TblPnlRename.SuspendLayout();
            SuspendLayout();
            // 
            // TblPnlMain
            // 
            TblPnlMain.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            TblPnlMain.ColumnCount = 1;
            TblPnlMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TblPnlMain.Controls.Add(TblPnlBottom, 0, 2);
            TblPnlMain.Controls.Add(LblRenameLbl, 0, 0);
            TblPnlMain.Controls.Add(TblPnlRename, 0, 1);
            TblPnlMain.Dock = DockStyle.Fill;
            TblPnlMain.Location = new Point(0, 0);
            TblPnlMain.Name = "TblPnlMain";
            TblPnlMain.RowCount = 3;
            TblPnlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            TblPnlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 49F));
            TblPnlMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 8F));
            TblPnlMain.Size = new Size(543, 145);
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
            TblPnlBottom.Location = new Point(4, 93);
            TblPnlBottom.Name = "TblPnlBottom";
            TblPnlBottom.RowCount = 1;
            TblPnlBottom.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblPnlBottom.Size = new Size(535, 48);
            TblPnlBottom.TabIndex = 2;
            // 
            // BtnExit
            // 
            BtnExit.BackColor = Color.DimGray;
            BtnExit.DialogResult = DialogResult.Cancel;
            BtnExit.Dock = DockStyle.Fill;
            BtnExit.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnExit.ForeColor = Color.Linen;
            BtnExit.Location = new Point(255, 3);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(100, 42);
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
            BtnOk.Location = new Point(180, 3);
            BtnOk.Name = "BtnOk";
            BtnOk.Size = new Size(69, 42);
            BtnOk.TabIndex = 2;
            BtnOk.Text = "O&K";
            BtnOk.UseCompatibleTextRendering = true;
            BtnOk.UseVisualStyleBackColor = false;
            BtnOk.Click += BtnOk_Click;
            // 
            // LblRenameLbl
            // 
            LblRenameLbl.AutoSize = true;
            LblRenameLbl.Dock = DockStyle.Fill;
            LblRenameLbl.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblRenameLbl.Location = new Point(4, 4);
            LblRenameLbl.Margin = new Padding(3);
            LblRenameLbl.Name = "LblRenameLbl";
            LblRenameLbl.Size = new Size(535, 32);
            LblRenameLbl.TabIndex = 0;
            LblRenameLbl.Text = "Renaming : ";
            LblRenameLbl.TextAlign = ContentAlignment.MiddleLeft;
            LblRenameLbl.UseCompatibleTextRendering = true;
            // 
            // TblPnlRename
            // 
            TblPnlRename.ColumnCount = 2;
            TblPnlRename.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 167F));
            TblPnlRename.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TblPnlRename.Controls.Add(LblRenameText, 0, 0);
            TblPnlRename.Controls.Add(TxtROINewName, 1, 0);
            TblPnlRename.Dock = DockStyle.Fill;
            TblPnlRename.Location = new Point(4, 43);
            TblPnlRename.Name = "TblPnlRename";
            TblPnlRename.RowCount = 1;
            TblPnlRename.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TblPnlRename.Size = new Size(535, 43);
            TblPnlRename.TabIndex = 1;
            // 
            // LblRenameText
            // 
            LblRenameText.AutoSize = true;
            LblRenameText.Dock = DockStyle.Fill;
            LblRenameText.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblRenameText.Location = new Point(3, 3);
            LblRenameText.Margin = new Padding(3);
            LblRenameText.Name = "LblRenameText";
            LblRenameText.Size = new Size(161, 37);
            LblRenameText.TabIndex = 1;
            LblRenameText.Text = "Enter New Name :";
            LblRenameText.TextAlign = ContentAlignment.MiddleLeft;
            LblRenameText.UseCompatibleTextRendering = true;
            // 
            // TxtROINewName
            // 
            TxtROINewName.BorderStyle = BorderStyle.FixedSingle;
            TxtROINewName.Dock = DockStyle.Fill;
            TxtROINewName.Location = new Point(170, 3);
            TxtROINewName.Multiline = true;
            TxtROINewName.Name = "TxtROINewName";
            TxtROINewName.Size = new Size(362, 37);
            TxtROINewName.TabIndex = 2;
            // 
            // ROI_RenameForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(543, 145);
            Controls.Add(TblPnlMain);
            Font = new Font("Palatino Linotype", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ROI_RenameForm";
            Opacity = 0.9D;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Rename ROI";
            Load += ROI_RenameForm_Load;
            TblPnlMain.ResumeLayout(false);
            TblPnlMain.PerformLayout();
            TblPnlBottom.ResumeLayout(false);
            TblPnlRename.ResumeLayout(false);
            TblPnlRename.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel TblPnlMain;
        private Label LblRenameLbl;
        private TableLayoutPanel TblPnlRename;
        private TableLayoutPanel TblPnlBottom;
        private Label LblRenameText;
        private TextBox TxtROINewName;
        private Button BtnExit;
        private Button BtnOk;
    }
}