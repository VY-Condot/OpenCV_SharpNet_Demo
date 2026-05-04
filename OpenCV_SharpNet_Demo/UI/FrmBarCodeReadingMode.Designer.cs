namespace OpenCV_SharpNet_Ver_3_0_1.UI
{
    partial class FrmBarCodeReadingMode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBarCodeReadingMode));
            groupBox1 = new GroupBox();
            rdNoneMode = new RadioButton();
            rdSuperResolutionMode = new RadioButton();
            rdBruteForceRecoveryMode = new RadioButton();
            button1 = new Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(rdNoneMode);
            groupBox1.Controls.Add(rdSuperResolutionMode);
            groupBox1.Controls.Add(rdBruteForceRecoveryMode);
            groupBox1.Location = new Point(43, 27);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(288, 228);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Mode";
            // 
            // rdNoneMode
            // 
            rdNoneMode.AutoSize = true;
            rdNoneMode.Location = new Point(33, 147);
            rdNoneMode.Name = "rdNoneMode";
            rdNoneMode.Size = new Size(109, 24);
            rdNoneMode.TabIndex = 2;
            rdNoneMode.Text = "None Mode";
            rdNoneMode.UseVisualStyleBackColor = true;
            // 
            // rdSuperResolutionMode
            // 
            rdSuperResolutionMode.AutoSize = true;
            rdSuperResolutionMode.Enabled = false;
            rdSuperResolutionMode.Location = new Point(33, 102);
            rdSuperResolutionMode.Name = "rdSuperResolutionMode";
            rdSuperResolutionMode.Size = new Size(221, 24);
            rdSuperResolutionMode.TabIndex = 1;
            rdSuperResolutionMode.Text = "Super Resolution Mode(WIP)";
            rdSuperResolutionMode.UseVisualStyleBackColor = true;
            rdSuperResolutionMode.Click += RdSuperResolutionMode_Click;
            // 
            // rdBruteForceRecoveryMode
            // 
            rdBruteForceRecoveryMode.AutoSize = true;
            rdBruteForceRecoveryMode.Location = new Point(33, 50);
            rdBruteForceRecoveryMode.Name = "rdBruteForceRecoveryMode";
            rdBruteForceRecoveryMode.Size = new Size(237, 24);
            rdBruteForceRecoveryMode.TabIndex = 0;
            rdBruteForceRecoveryMode.Text = "BRUTE-FORCE GRID RECOVERY";
            rdBruteForceRecoveryMode.UseVisualStyleBackColor = true;
            rdBruteForceRecoveryMode.CheckedChanged += RdBruteForceRecoveryMode_CheckedChanged;
            rdBruteForceRecoveryMode.Click += RdBruteForceRecoveryMode_Click;
            // 
            // button1
            // 
            button1.Location = new Point(128, 280);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 1;
            button1.Text = "OK";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Button1_Click;
            // 
            // FrmBarCodeReadingMode
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(368, 327);
            Controls.Add(button1);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmBarCodeReadingMode";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "BarCode Reading Mode";
            FormClosing += FrmBarCodeReadingMode_FormClosing;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private RadioButton rdSuperResolutionMode;
        private RadioButton rdBruteForceRecoveryMode;
        private Button button1;
        private RadioButton rdNoneMode;
    }
}