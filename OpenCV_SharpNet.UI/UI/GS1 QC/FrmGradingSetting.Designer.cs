namespace OpenCV_SharpNet.UI.UI.GS1_QC
{
    partial class FrmGradingSetting
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
            splitContainer1 = new SplitContainer();
            grpBoxGradeSystems = new GroupBox();
            lstGradeSytems = new ListBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.SuspendLayout();
            grpBoxGradeSystems.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(grpBoxGradeSystems);
            splitContainer1.Size = new Size(974, 528);
            splitContainer1.SplitterDistance = 301;
            splitContainer1.TabIndex = 0;
            // 
            // grpBoxGradeSystems
            // 
            grpBoxGradeSystems.Controls.Add(lstGradeSytems);
            grpBoxGradeSystems.Dock = DockStyle.Fill;
            grpBoxGradeSystems.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            grpBoxGradeSystems.Location = new Point(0, 0);
            grpBoxGradeSystems.Name = "grpBoxGradeSystems";
            grpBoxGradeSystems.Size = new Size(301, 528);
            grpBoxGradeSystems.TabIndex = 0;
            grpBoxGradeSystems.TabStop = false;
            grpBoxGradeSystems.Text = "Grade Systems";
            grpBoxGradeSystems.UseCompatibleTextRendering = true;
            // 
            // lstGradeSytems
            // 
            lstGradeSytems.Dock = DockStyle.Fill;
            lstGradeSytems.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lstGradeSytems.FormattingEnabled = true;
            lstGradeSytems.Location = new Point(3, 23);
            lstGradeSytems.Name = "lstGradeSytems";
            lstGradeSytems.Size = new Size(295, 502);
            lstGradeSytems.TabIndex = 0;
            lstGradeSytems.SelectedIndexChanged += LstGradeSytems_SelectedIndexChanged;
            // 
            // FrmGradingSetting
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(974, 528);
            Controls.Add(splitContainer1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "FrmGradingSetting";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FrmGradingSetting";
            Load += FrmGradingSetting_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            grpBoxGradeSystems.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private GroupBox grpBoxGradeSystems;
        private ListBox lstGradeSytems;
    }
}