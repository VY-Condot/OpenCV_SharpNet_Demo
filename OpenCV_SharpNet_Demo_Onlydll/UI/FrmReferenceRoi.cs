using CsplCam.Library.Models;
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
    public partial class FrmReferenceRoi : Form
    {
        public FrmReferenceRoi(List<RoiObject> lstRoiObject)
        {
            InitializeComponent();
            this.lstRoiObjects = lstRoiObject;
        }
        List<RoiObject> lstRoiObjects = new List<RoiObject>();


        public int SelectRoiID { get; set; } = -1;
        private void FrmReferenceRoi_Load(object sender, EventArgs e)
        {
            foreach (var item in lstRoiObjects)
            {
                lstReference.Items.Add($"ID : {item.Id}    Name : {item.Name}");
            }
        }

        private void GetReferenceID()
        {
            if(-1 < lstReference.SelectedIndex)
                SelectRoiID = lstRoiObjects[lstReference.SelectedIndex].Id;
        }

        private void LstReference_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetReferenceID();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if(-1 >= SelectRoiID)
            {
                MessageBox.Show("Please select a valid ROI.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
