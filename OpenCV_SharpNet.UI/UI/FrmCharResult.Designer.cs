namespace OpenCV_SharpNet.UI
{
    partial class FrmCharResult
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCharResult));
            dgvCharResult = new DataGridView();
            tblMain = new TableLayoutPanel();
            tblBottom = new TableLayoutPanel();
            lblMinScoreValue = new Label();
            lblMinScore = new Label();
            lblMinHeightValue = new Label();
            lblMinHeight = new Label();
            lblMinWidthValue = new Label();
            lblWidth = new Label();
            Char = new DataGridViewTextBoxColumn();
            Width = new DataGridViewTextBoxColumn();
            Height = new DataGridViewTextBoxColumn();
            Score = new DataGridViewTextBoxColumn();
            CharX = new DataGridViewTextBoxColumn();
            CharY = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvCharResult).BeginInit();
            tblMain.SuspendLayout();
            tblBottom.SuspendLayout();
            SuspendLayout();
            // 
            // dgvCharResult
            // 
            dgvCharResult.AllowUserToAddRows = false;
            dgvCharResult.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvCharResult.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvCharResult.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCharResult.Columns.AddRange(new DataGridViewColumn[] { Char, Width, Height, Score, CharX, CharY });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvCharResult.DefaultCellStyle = dataGridViewCellStyle3;
            dgvCharResult.Dock = DockStyle.Fill;
            dgvCharResult.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvCharResult.Location = new Point(3, 3);
            dgvCharResult.Name = "dgvCharResult";
            dgvCharResult.RowHeadersVisible = false;
            dgvCharResult.RowHeadersWidth = 51;
            dgvCharResult.Size = new Size(637, 536);
            dgvCharResult.TabIndex = 0;
            // 
            // tblMain
            // 
            tblMain.ColumnCount = 1;
            tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblMain.Controls.Add(dgvCharResult, 0, 0);
            tblMain.Controls.Add(tblBottom, 0, 1);
            tblMain.Dock = DockStyle.Fill;
            tblMain.Location = new Point(0, 0);
            tblMain.Name = "tblMain";
            tblMain.RowCount = 2;
            tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100.000008F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 51F));
            tblMain.Size = new Size(643, 593);
            tblMain.TabIndex = 1;
            // 
            // tblBottom
            // 
            tblBottom.ColumnCount = 6;
            tblBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 113F));
            tblBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 77F));
            tblBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 117F));
            tblBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 76F));
            tblBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 122F));
            tblBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 67F));
            tblBottom.Controls.Add(lblMinScoreValue, 5, 0);
            tblBottom.Controls.Add(lblMinScore, 4, 0);
            tblBottom.Controls.Add(lblMinHeightValue, 3, 0);
            tblBottom.Controls.Add(lblMinHeight, 2, 0);
            tblBottom.Controls.Add(lblMinWidthValue, 1, 0);
            tblBottom.Controls.Add(lblWidth, 0, 0);
            tblBottom.Dock = DockStyle.Fill;
            tblBottom.Location = new Point(3, 545);
            tblBottom.Name = "tblBottom";
            tblBottom.RowCount = 1;
            tblBottom.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblBottom.Size = new Size(637, 45);
            tblBottom.TabIndex = 1;
            // 
            // lblMinScoreValue
            // 
            lblMinScoreValue.AutoSize = true;
            lblMinScoreValue.Dock = DockStyle.Fill;
            lblMinScoreValue.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblMinScoreValue.Location = new Point(508, 3);
            lblMinScoreValue.Margin = new Padding(3);
            lblMinScoreValue.Name = "lblMinScoreValue";
            lblMinScoreValue.Size = new Size(126, 39);
            lblMinScoreValue.TabIndex = 5;
            lblMinScoreValue.Text = "0";
            lblMinScoreValue.TextAlign = ContentAlignment.MiddleLeft;
            lblMinScoreValue.UseCompatibleTextRendering = true;
            // 
            // lblMinScore
            // 
            lblMinScore.AutoSize = true;
            lblMinScore.Dock = DockStyle.Fill;
            lblMinScore.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMinScore.Location = new Point(386, 3);
            lblMinScore.Margin = new Padding(3);
            lblMinScore.Name = "lblMinScore";
            lblMinScore.Size = new Size(116, 39);
            lblMinScore.TabIndex = 4;
            lblMinScore.Text = "Min Score : ";
            lblMinScore.TextAlign = ContentAlignment.MiddleLeft;
            lblMinScore.UseCompatibleTextRendering = true;
            // 
            // lblMinHeightValue
            // 
            lblMinHeightValue.AutoSize = true;
            lblMinHeightValue.Dock = DockStyle.Fill;
            lblMinHeightValue.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblMinHeightValue.Location = new Point(310, 3);
            lblMinHeightValue.Margin = new Padding(3);
            lblMinHeightValue.Name = "lblMinHeightValue";
            lblMinHeightValue.Size = new Size(70, 39);
            lblMinHeightValue.TabIndex = 3;
            lblMinHeightValue.Text = "0";
            lblMinHeightValue.TextAlign = ContentAlignment.MiddleLeft;
            lblMinHeightValue.UseCompatibleTextRendering = true;
            // 
            // lblMinHeight
            // 
            lblMinHeight.AutoSize = true;
            lblMinHeight.Dock = DockStyle.Fill;
            lblMinHeight.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMinHeight.Location = new Point(193, 3);
            lblMinHeight.Margin = new Padding(3);
            lblMinHeight.Name = "lblMinHeight";
            lblMinHeight.Size = new Size(111, 39);
            lblMinHeight.TabIndex = 2;
            lblMinHeight.Text = "Min Height : ";
            lblMinHeight.TextAlign = ContentAlignment.MiddleLeft;
            lblMinHeight.UseCompatibleTextRendering = true;
            // 
            // lblMinWidthValue
            // 
            lblMinWidthValue.AutoSize = true;
            lblMinWidthValue.Dock = DockStyle.Fill;
            lblMinWidthValue.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblMinWidthValue.Location = new Point(116, 3);
            lblMinWidthValue.Margin = new Padding(3);
            lblMinWidthValue.Name = "lblMinWidthValue";
            lblMinWidthValue.Size = new Size(71, 39);
            lblMinWidthValue.TabIndex = 1;
            lblMinWidthValue.Text = "0";
            lblMinWidthValue.TextAlign = ContentAlignment.MiddleLeft;
            lblMinWidthValue.UseCompatibleTextRendering = true;
            // 
            // lblWidth
            // 
            lblWidth.AutoSize = true;
            lblWidth.Dock = DockStyle.Fill;
            lblWidth.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblWidth.Location = new Point(3, 3);
            lblWidth.Margin = new Padding(3);
            lblWidth.Name = "lblWidth";
            lblWidth.Size = new Size(107, 39);
            lblWidth.TabIndex = 0;
            lblWidth.Text = "Min Width : ";
            lblWidth.TextAlign = ContentAlignment.MiddleLeft;
            lblWidth.UseCompatibleTextRendering = true;
            // 
            // Char
            // 
            dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Char.DefaultCellStyle = dataGridViewCellStyle2;
            Char.HeaderText = "Char";
            Char.MinimumWidth = 6;
            Char.Name = "Char";
            Char.ReadOnly = true;
            Char.Width = 125;
            // 
            // Width
            // 
            Width.HeaderText = "Width";
            Width.MinimumWidth = 6;
            Width.Name = "Width";
            Width.ReadOnly = true;
            Width.Width = 125;
            // 
            // Height
            // 
            Height.HeaderText = "Height";
            Height.MinimumWidth = 6;
            Height.Name = "Height";
            Height.ReadOnly = true;
            Height.Width = 125;
            // 
            // Score
            // 
            Score.HeaderText = "Score";
            Score.MinimumWidth = 6;
            Score.Name = "Score";
            Score.ReadOnly = true;
            Score.Width = 125;
            // 
            // CharX
            // 
            CharX.HeaderText = "CharX";
            CharX.MinimumWidth = 6;
            CharX.Name = "CharX";
            CharX.ReadOnly = true;
            CharX.Width = 125;
            // 
            // CharY
            // 
            CharY.HeaderText = "CharY";
            CharY.MinimumWidth = 6;
            CharY.Name = "CharY";
            CharY.ReadOnly = true;
            CharY.Width = 125;
            // 
            // FrmCharResult
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(643, 593);
            Controls.Add(tblMain);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmCharResult";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Char Result";
            Load += FrmCharResult_Load;
            ((System.ComponentModel.ISupportInitialize)dgvCharResult).EndInit();
            tblMain.ResumeLayout(false);
            tblBottom.ResumeLayout(false);
            tblBottom.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvCharResult;
        private TableLayoutPanel tblMain;
        private TableLayoutPanel tblBottom;
        private Label lblWidth;
        private Label lblMinScoreValue;
        private Label lblMinScore;
        private Label lblMinHeightValue;
        private Label lblMinHeight;
        private Label lblMinWidthValue;
        private DataGridViewTextBoxColumn Char;
        private DataGridViewTextBoxColumn Width;
        private DataGridViewTextBoxColumn Height;
        private DataGridViewTextBoxColumn Score;
        private DataGridViewTextBoxColumn CharX;
        private DataGridViewTextBoxColumn CharY;
    }
}