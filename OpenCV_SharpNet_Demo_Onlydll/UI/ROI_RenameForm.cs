using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenCV_SharpNet_Demo.UI
{
    public partial class ROI_RenameForm : Form
    {
        private string strCurrentRoiName = string.Empty;

        public ROI_RenameForm(string strOldName)
        {
            InitializeComponent();

            strCurrentRoiName = strOldName;
        }
        public string GetNewRoiName { get; private set; } = string.Empty;
        private void ROI_RenameForm_Load(object sender, EventArgs e)
        {
            LblRenameLbl.Text = $"Renaming : {strCurrentRoiName}";

            TxtROINewName.Text = strCurrentRoiName;
            TxtROINewName.Focus();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtROINewName.Text))
            {
                MessageBox.Show("Please enter a valid name.", "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            GetNewRoiName = TxtROINewName.Text.Trim();
            Close();
        }
    }
}
