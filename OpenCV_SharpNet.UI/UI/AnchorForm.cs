using CsplCam.Library.Models;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace OpenCV_SharpNet.UI
{
    public partial class AnchorForm : Form
    {

        private int intTop, intBottom, intLeft, intRight;
        private string strRoiName = string.Empty;

        //public AnchorSetting objAnchorSetting { get; private set; }
        public AnchorSetting GetAnchorSetting { get; private set; }

        public AnchorForm(Mat anchorTamplate, int top, int bottom, int left, int right, string roiName)
        {
            InitializeComponent();

            intTop = top;
            intBottom = bottom;
            intLeft = left;
            intRight = right;

            strRoiName = roiName;

            Pb_RoiPreview.Image = BitmapConverter.ToBitmap(anchorTamplate);
        }

        private void AnchorForm_Load(object sender, EventArgs e)
        {
            Text = $"ROI Anchor Setting - ({strRoiName})";
            GrpRoiPreview.Text = $"Anchor ROI Preview({strRoiName})";

            //set numpads
            NumPadTop.Value = intTop;
            NumPadBottom.Value = intBottom;
            NumPadLeft.Value = intLeft;
            NumPadRight.Value = intRight;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            GetAnchorSetting = new()
            {
                Top = (int)NumPadTop.Value,
                Bottom = (int)NumPadBottom.Value,
                Left = (int)NumPadLeft.Value,
                Right = (int)NumPadRight.Value
            };

            Close();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
