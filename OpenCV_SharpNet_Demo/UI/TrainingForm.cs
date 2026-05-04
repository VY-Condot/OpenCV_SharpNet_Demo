using OpenCV_SharpNet.Services;
using OpenCV_SharpNet_Demo.Services;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace OpenCV_SharpNet_Demo
{
    public partial class TrainingForm : Form
    {
        private List<(Mat image, TextBox input)> trainingItems = new List<(Mat, TextBox)>();
        List<TextBox> lstTxtGen = new();

        public TrainingForm(List<Mat> charImages)
        {
            InitializeComponent();

            Text = "Train Characters";
            //Size = new System.Drawing.Size(800, 600);

            PopulateGrid(charImages);
        }

        private void PopulateGrid(List<Mat> charImages)
        {
            int index = 0;
            foreach (var img in charImages)
            {
                Panel itemPanel = new Panel { Width = 100, Height = 150, Margin = new Padding(3), BorderStyle = BorderStyle.FixedSingle };

                PictureBox pb = new()
                {
                    Image = BitmapConverter.ToBitmap(img),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Width = 100,
                    Height = 100,
                    Margin = new Padding(3, 3, 3, 3)
                };

                TextBox tb = new()
                {
                    Width = 60,
                    Top = 110,
                    Margin = new Padding(3, 3, 3, 3),
                    Left = 20,
                    MaxLength = 1,
                    BorderStyle = BorderStyle.FixedSingle,
                    TextAlign = HorizontalAlignment.Center,
                    Font = new Font("Arial", 14, FontStyle.Bold),
                    Tag = index
                };

                // Auto-advance
                tb.TextChanged += (s, e) =>
                {
                    if (tb.Text.Length == 1) SelectNextControl(tb, true, true, true, true);
                };

                // Auto-advance
                tb.KeyDown += (s, e) =>
                {
                    if (tb.Text.Length <= 0 && e.KeyCode == Keys.Back) SelectNextControl(tb, false, true, true, true);
                };

                itemPanel.Controls.Add(pb);
                itemPanel.Controls.Add(tb);

                //storing textbox control
                lstTxtGen.Add(tb);

                //controls add 
                FLowPnlTrainedChars.Controls.Add(itemPanel);

                trainingItems.Add((img, tb));
                index++;
            }
        }

        private void TrainingForm_Load(object sender, EventArgs e)
        {

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            //check before saving weather all the characters are filled by user not 
            var empetyTxtBox = lstTxtGen.OrderBy(P => (int)P.Tag).Where(P => string.IsNullOrWhiteSpace(P.Text)).FirstOrDefault();

            if(empetyTxtBox != null)
            {
                MessageBox.Show($"Blob {((int)empetyTxtBox.Tag) + 1} has no label.","Alert",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                empetyTxtBox.Focus();
                return;
            }

            foreach (var item in trainingItems)
            {
                string label = item.input.Text.Trim();
                if (!string.IsNullOrEmpty(label))
                {
                    OcrEngine.SaveTemplate(item.image, label);
                }
            }
            OcrEngine.ReloadTemplates();
            MessageBox.Show("Training saved.");
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}