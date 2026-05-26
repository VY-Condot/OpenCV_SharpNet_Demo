using CsplCam.Library.Models;
using CsplCam.Library.Models.GS1_QC;
using OpenCV_SharpNet.UI.UserControls;
using OpenCV_SharpNet.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenCV_SharpNet.UI.UI.GS1_QC
{
    public partial class FrmGradingSetting : Form
    {
        public FrmGradingSetting(RoiObject roiObject)
        {
            InitializeComponent();
            RoiObject = roiObject;
        }

        private RoiObject RoiObject { get; set; }

        //caching of setting windows
        Dictionary<GradingSystems, GradingView> _gradingViews = new();

        private void FrmGradingSetting_Load(object sender, EventArgs e)
        {
            var gradeSystems = Enum.GetValues<GradingSystems>();

            //append data into
            lstGradeSytems.Items.Clear();
            lstGradeSytems.DataSource = gradeSystems;
        }

        private void LstGradeSytems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstGradeSytems.SelectedIndex <= -1) return;

            //check name exists in dictinary or not 
            var name = (GradingSystems)lstGradeSytems.SelectedItem;

            //check in dictionary if not exists then create instance
            var gradingView = GetOrCreateInstance(name);

            //set current grAding system
            gradingView.GradingSystems = name;

            if (!splitContainer1.Panel2.Controls.Contains(gradingView))
            {
                gradingView.Dock = DockStyle.Fill;
                splitContainer1.Panel2.Controls.Add(gradingView);
            }

            gradingView.BringToFront();

            gradingView.BindData();
        }

        private GradingView GetOrCreateInstance(GradingSystems name)
        {
            if(_gradingViews.TryGetValue(name,out var gradingView))
            {
                return gradingView;
            }
            else
            {
                var tempgradingView = new GradingView(RoiObject);
                _gradingViews.Add(name, tempgradingView);
                return tempgradingView;
            }
        }
    }
}
