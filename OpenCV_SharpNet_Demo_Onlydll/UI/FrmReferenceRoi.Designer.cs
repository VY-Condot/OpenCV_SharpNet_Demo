namespace OpenCV_SharpNet_Demo.UI
{
    partial class FrmReferenceRoi
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReferenceRoi));
            lstReference = new ListBox();
            btnOK = new Button();
            tblMain = new TableLayoutPanel();
            tblBtnBottom = new TableLayoutPanel();
            tblMain.SuspendLayout();
            tblBtnBottom.SuspendLayout();
            SuspendLayout();
            // 
            // lstReference
            // 
            lstReference.Dock = DockStyle.Fill;
            lstReference.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lstReference.FormattingEnabled = true;
            lstReference.Location = new Point(3, 3);
            lstReference.Name = "lstReference";
            lstReference.Size = new Size(322, 285);
            lstReference.TabIndex = 0;
            lstReference.SelectedIndexChanged += LstReference_SelectedIndexChanged;
            // 
            // btnOK
            // 
            btnOK.DialogResult = DialogResult.OK;
            btnOK.Dock = DockStyle.Fill;
            btnOK.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnOK.Location = new Point(125, 3);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(71, 33);
            btnOK.TabIndex = 1;
            btnOK.Text = "O&K";
            btnOK.UseCompatibleTextRendering = true;
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += BtnOK_Click;
            // 
            // tblMain
            // 
            tblMain.ColumnCount = 1;
            tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tblMain.Controls.Add(lstReference, 0, 0);
            tblMain.Controls.Add(tblBtnBottom, 0, 1);
            tblMain.Dock = DockStyle.Fill;
            tblMain.Location = new Point(0, 0);
            tblMain.Name = "tblMain";
            tblMain.RowCount = 2;
            tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tblMain.Size = new Size(328, 336);
            tblMain.TabIndex = 2;
            // 
            // tblBtnBottom
            // 
            tblBtnBottom.ColumnCount = 3;
            tblBtnBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblBtnBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 77F));
            tblBtnBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblBtnBottom.Controls.Add(btnOK, 1, 0);
            tblBtnBottom.Dock = DockStyle.Fill;
            tblBtnBottom.Location = new Point(3, 294);
            tblBtnBottom.Name = "tblBtnBottom";
            tblBtnBottom.RowCount = 1;
            tblBtnBottom.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblBtnBottom.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tblBtnBottom.Size = new Size(322, 39);
            tblBtnBottom.TabIndex = 1;
            // 
            // FrmReferenceRoi
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(328, 336);
            Controls.Add(tblMain);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmReferenceRoi";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ReferenceRoi Window";
            Load += FrmReferenceRoi_Load;
            tblMain.ResumeLayout(false);
            tblBtnBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ListBox lstReference;
        private Button btnOK;
        private TableLayoutPanel tblMain;
        private TableLayoutPanel tblBtnBottom;
    }
}