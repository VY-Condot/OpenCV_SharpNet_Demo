namespace OpenCV_SharpNet_Demo.UI
{
    partial class FrmRedirect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRedirect));
            tblMain = new TableLayoutPanel();
            lblInstrucation = new Label();
            lblDivider = new Label();
            lblSubHeading = new Label();
            btnImageMode = new Button();
            btnCameraMode = new Button();
            lblHeading = new Label();
            StatusStripMenu = new StatusStrip();
            toolStripMenu = new ToolStripStatusLabel();
            toolStripDocument = new ToolStripStatusLabel();
            toolStripVersion = new ToolStripStatusLabel();
            tsslUserManual = new ToolStripStatusLabel();
            tblMain.SuspendLayout();
            StatusStripMenu.SuspendLayout();
            SuspendLayout();
            // 
            // tblMain
            // 
            tblMain.ColumnCount = 1;
            tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblMain.Controls.Add(lblInstrucation, 0, 3);
            tblMain.Controls.Add(lblDivider, 0, 2);
            tblMain.Controls.Add(lblSubHeading, 0, 1);
            tblMain.Controls.Add(btnImageMode, 0, 4);
            tblMain.Controls.Add(btnCameraMode, 0, 5);
            tblMain.Controls.Add(lblHeading, 0, 0);
            tblMain.Dock = DockStyle.Fill;
            tblMain.Location = new Point(0, 0);
            tblMain.Name = "tblMain";
            tblMain.RowCount = 7;
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 37F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 8F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 130F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 130F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblMain.Size = new Size(553, 429);
            tblMain.TabIndex = 0;
            // 
            // lblInstrucation
            // 
            lblInstrucation.AutoEllipsis = true;
            lblInstrucation.Dock = DockStyle.Fill;
            lblInstrucation.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblInstrucation.ForeColor = Color.DimGray;
            lblInstrucation.Location = new Point(3, 98);
            lblInstrucation.Margin = new Padding(3);
            lblInstrucation.Name = "lblInstrucation";
            lblInstrucation.Size = new Size(547, 56);
            lblInstrucation.TabIndex = 5;
            lblInstrucation.Text = "Select an active workspace to begin processing.";
            lblInstrucation.TextAlign = ContentAlignment.MiddleCenter;
            lblInstrucation.UseCompatibleTextRendering = true;
            // 
            // lblDivider
            // 
            lblDivider.AutoEllipsis = true;
            lblDivider.BackColor = Color.LightSteelBlue;
            lblDivider.Dock = DockStyle.Fill;
            lblDivider.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDivider.ForeColor = Color.LightSteelBlue;
            lblDivider.Location = new Point(50, 90);
            lblDivider.Margin = new Padding(50, 3, 50, 3);
            lblDivider.Name = "lblDivider";
            lblDivider.Size = new Size(453, 2);
            lblDivider.TabIndex = 4;
            lblDivider.TextAlign = ContentAlignment.MiddleCenter;
            lblDivider.UseCompatibleTextRendering = true;
            // 
            // lblSubHeading
            // 
            lblSubHeading.AutoEllipsis = true;
            lblSubHeading.Dock = DockStyle.Fill;
            lblSubHeading.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSubHeading.ForeColor = Color.SteelBlue;
            lblSubHeading.Location = new Point(3, 53);
            lblSubHeading.Margin = new Padding(3);
            lblSubHeading.Name = "lblSubHeading";
            lblSubHeading.Size = new Size(547, 31);
            lblSubHeading.TabIndex = 3;
            lblSubHeading.Text = "Industrial OCR & Pattern Recognition Suite";
            lblSubHeading.TextAlign = ContentAlignment.TopCenter;
            lblSubHeading.UseCompatibleTextRendering = true;
            // 
            // btnImageMode
            // 
            btnImageMode.BackColor = Color.White;
            btnImageMode.Dock = DockStyle.Fill;
            btnImageMode.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnImageMode.Image = (Image)resources.GetObject("btnImageMode.Image");
            btnImageMode.Location = new Point(50, 160);
            btnImageMode.Margin = new Padding(50, 3, 50, 3);
            btnImageMode.Name = "btnImageMode";
            btnImageMode.Size = new Size(453, 124);
            btnImageMode.TabIndex = 0;
            btnImageMode.Text = "Image mode";
            btnImageMode.TextImageRelation = TextImageRelation.ImageAboveText;
            btnImageMode.UseCompatibleTextRendering = true;
            btnImageMode.UseVisualStyleBackColor = false;
            btnImageMode.Click += BtnImageMode_Click;
            btnImageMode.MouseEnter += BtnImageMode_MouseEnter;
            btnImageMode.MouseLeave += BtnImageMode_MouseLeave;
            // 
            // btnCameraMode
            // 
            btnCameraMode.BackColor = Color.White;
            btnCameraMode.Dock = DockStyle.Fill;
            btnCameraMode.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCameraMode.Image = (Image)resources.GetObject("btnCameraMode.Image");
            btnCameraMode.Location = new Point(50, 290);
            btnCameraMode.Margin = new Padding(50, 3, 50, 3);
            btnCameraMode.Name = "btnCameraMode";
            btnCameraMode.Size = new Size(453, 124);
            btnCameraMode.TabIndex = 1;
            btnCameraMode.Text = "Camera mode";
            btnCameraMode.TextImageRelation = TextImageRelation.ImageAboveText;
            btnCameraMode.UseCompatibleTextRendering = true;
            btnCameraMode.UseVisualStyleBackColor = false;
            btnCameraMode.Click += BtnCameraMode_Click;
            btnCameraMode.MouseEnter += BtnCameraMode_MouseEnter;
            btnCameraMode.MouseLeave += BtnCameraMode_MouseLeave;
            // 
            // lblHeading
            // 
            lblHeading.AutoEllipsis = true;
            lblHeading.Dock = DockStyle.Fill;
            lblHeading.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblHeading.ForeColor = Color.MidnightBlue;
            lblHeading.Location = new Point(3, 3);
            lblHeading.Margin = new Padding(3);
            lblHeading.Name = "lblHeading";
            lblHeading.Size = new Size(547, 44);
            lblHeading.TabIndex = 2;
            lblHeading.Text = "VISION CONTROL CENTER";
            lblHeading.TextAlign = ContentAlignment.BottomCenter;
            lblHeading.UseCompatibleTextRendering = true;
            // 
            // StatusStripMenu
            // 
            StatusStripMenu.BackColor = Color.Gainsboro;
            StatusStripMenu.Font = new Font("Palatino Linotype", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            StatusStripMenu.ImageScalingSize = new Size(20, 20);
            StatusStripMenu.Items.AddRange(new ToolStripItem[] { toolStripMenu, toolStripDocument, tsslUserManual, toolStripVersion });
            StatusStripMenu.Location = new Point(0, 429);
            StatusStripMenu.Name = "StatusStripMenu";
            StatusStripMenu.Size = new Size(553, 29);
            StatusStripMenu.TabIndex = 4;
            StatusStripMenu.Text = "Selection mode and info";
            // 
            // toolStripMenu
            // 
            toolStripMenu.Font = new Font("Palatino Linotype", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            toolStripMenu.Name = "toolStripMenu";
            toolStripMenu.Size = new Size(217, 23);
            toolStripMenu.Spring = true;
            toolStripMenu.Text = "Condot System Pvt. Ltd.";
            // 
            // toolStripDocument
            // 
            toolStripDocument.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold);
            toolStripDocument.IsLink = true;
            toolStripDocument.Name = "toolStripDocument";
            toolStripDocument.Size = new Size(114, 23);
            toolStripDocument.Text = "Dev Document";
            toolStripDocument.Click += ToolStripDocument_Click;
            // 
            // toolStripVersion
            // 
            toolStripVersion.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            toolStripVersion.Name = "toolStripVersion";
            toolStripVersion.Size = new Size(65, 23);
            toolStripVersion.Text = "Version";
            // 
            // tsslUserManual
            // 
            tsslUserManual.Font = new Font("Palatino Linotype", 9F, FontStyle.Bold);
            tsslUserManual.IsLink = true;
            tsslUserManual.Name = "tsslUserManual";
            tsslUserManual.Size = new Size(103, 23);
            tsslUserManual.Text = "User Manual";
            tsslUserManual.Click += TsslUserManual_Click;
            // 
            // FrmRedirect
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(553, 458);
            Controls.Add(tblMain);
            Controls.Add(StatusStripMenu);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmRedirect";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "VISION CONTROL CENTER - Redirection Window ";
            Load += FrmRedirect_Load;
            tblMain.ResumeLayout(false);
            StatusStripMenu.ResumeLayout(false);
            StatusStripMenu.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tblMain;
        private Button btnImageMode;
        private Button btnCameraMode;
        private StatusStrip StatusStripMenu;
        private ToolStripStatusLabel toolStripVersion;
        private ToolStripStatusLabel toolStripMenu;
        private Label lblSubHeading;
        private Label lblHeading;
        private Label lblInstrucation;
        private Label lblDivider;
        private ToolStripStatusLabel toolStripDocument;
        private ToolStripStatusLabel tsslUserManual;
    }
}