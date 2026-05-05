using CsplCam.Library.Models;

namespace OpenCV_SharpNet.UI
{
    public partial class FrmCharResult : Form
    {

        List<CharResult> results;
        public FrmCharResult(List<CharResult> res)
        {
            InitializeComponent();
            results = res;
        }

        public void LoadData()
        {
            dgvCharResult.Rows.Clear();
            dgvCharResult.Columns.Clear();
            dgvCharResult.Columns.Add("Char", "Char");
            dgvCharResult.Columns.Add("Width", "Width");
            dgvCharResult.Columns.Add("Height", "Height");
            dgvCharResult.Columns.Add("Score", "Score");
            dgvCharResult.Columns.Add("CharX", "CharX");
            dgvCharResult.Columns.Add("CharY", "CharY");

            foreach (var r in results)
            {
                dgvCharResult.Rows.Add(r.Text, r.Box.Width, r.Box.Height, r.Score.ToString("F3"), r.Box.X, r.Box.Y);
            }
        }

        private void FrmCharResult_Load(object sender, EventArgs e)
        {
            LoadData();

            if (results.Count > 0)
            {
                //set text value
                lblMinHeightValue.Text = results.Min(r => r.Box.Height).ToString();
                lblMinWidthValue.Text = results.Min(r => r.Box.Width).ToString();
                lblMinScoreValue.Text = results.Min(r => r.Score).ToString("F3");
            }
        }
    }
}
