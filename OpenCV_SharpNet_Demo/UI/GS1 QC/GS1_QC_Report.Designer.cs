namespace OpenCV_SharpNet_Demo.UI.GS1_QC
{
    partial class GS1_QC_Report
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
            listBox1 = new ListBox();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.Dock = DockStyle.Fill;
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(10, 10);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(433, 359);
            listBox1.TabIndex = 0;
            // 
            // GS1_QC_Report
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(453, 372);
            Controls.Add(listBox1);
            DoubleBuffered = true;
            Name = "GS1_QC_Report";
            Padding = new Padding(10, 10, 10, 3);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "GS1 QC Report";
            Load += GS1_QC_Report_Load;
            ResumeLayout(false);
        }

        #endregion

        private ListBox listBox1;
    }
}