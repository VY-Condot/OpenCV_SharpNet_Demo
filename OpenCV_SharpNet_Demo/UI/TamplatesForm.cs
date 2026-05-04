using OpenCV_SharpNet.Services;
using OpenCV_SharpNet_Demo.Services;
using System.Data;

namespace OpenCV_SharpNet_Demo.UI
{
    public partial class TamplatesForm : Form
    {
        Dictionary<string, string?[]> DictTamplates = new Dictionary<string, string?[]>();
        PictureBox picCurrentSelection;

        public TamplatesForm()
        {
            InitializeComponent();

            Text = "Taught Character Templates";
            DoubleBuffered = true;
        }

        //Create UI for showing taught characters tamplates
        private void CreateUI(string TamPlateName)
        {
            FlowPnlTamplates.Controls.Clear();

            picCurrentSelection = null;

            //set on label
            LblImageName.Text = $"ImageName";

            //get tamplates images
            DictTamplates.TryGetValue(TamPlateName, out string?[] TampImages);

            //set total tamplates count of charcacter
            LblTotalTamplates.Text = $"Total Tamplates : {TampImages.Length}";

            foreach (var tamplate in TampImages)
            {
                PictureBox pictureBox = new PictureBox
                {
                    BackColor = Color.LightGray,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Width = 150,
                    Height = 150,
                    Tag = tamplate,
                    BorderStyle = BorderStyle.FixedSingle
                };

                pictureBox.Paint += PictureBox_Paint;
                pictureBox.Click += PictureBox_Click;

                //load image in pictire box
                LoadImageInPicturesBox(pictureBox, tamplate);

                FlowPnlTamplates.Controls.Add(pictureBox);
            }
        }

        private void PictureBox_Click(object? sender, EventArgs e)
        {
            var pb = sender as PictureBox;
            SelectedImage(pb);
        }

        //selection of image
        private void SelectedImage(PictureBox pb)
        {
            if (picCurrentSelection != null)
            {
                var oldImage = picCurrentSelection;
                picCurrentSelection = null;
                oldImage.Invalidate();
            }

            //set new image
            picCurrentSelection = pb;

            //set on label
            LblImageName.Text = $"{Path.GetFileName(pb?.Tag?.ToString())}";

            pb.Invalidate();
        }

        private void PictureBox_Paint(object? sender, PaintEventArgs e)
        {
            var pb = sender as PictureBox;

            if (pb != picCurrentSelection) return;

            //add ractangle around selected picture box
            using Pen pen = new Pen(Color.Red, 2);
            Rectangle rectangle = pb.ClientRectangle;
            rectangle.Inflate(-1, -1);
            e.Graphics.DrawRectangle(pen, rectangle);
        }

        //load image in pictire box
        private void LoadImageInPicturesBox(PictureBox Pb, string imagePath)
        {
            using Stream imgStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);

            Pb.Image = Image.FromStream(imgStream);
        }

        private Dictionary<string, string?[]> GetTamplates()
        {
            DictTamplates.Clear();

            var FolderNames = Directory.GetDirectories(OcrEngine.DatasetRoot, "*", SearchOption.TopDirectoryOnly);

            foreach (var item in FolderNames)
            {
                var GetTamplatesImages = Directory.GetFiles(item);

                string strkeyName = Path.GetFileName(item);

                if (!DictTamplates.ContainsKey(strkeyName))
                {
                    DictTamplates[strkeyName] = GetTamplatesImages;
                }
            }

            return DictTamplates;
        }

        private void TamplatesForm_Load(object sender, EventArgs e)
        {
            SplitConCharTamplates.SplitterDistance = (int)TblPnlLabel.ColumnStyles[0].Width;
            LoadTamplateName();

            LstCharacters.SelectedIndex = 0;
        }

        //load Names of Tamplates{
        public void LoadTamplateName()
        {
            var TamPlates = GetTamplates();

            var Names = TamPlates.Select(P => P.Key).ToArray();

            LstCharacters.Items.AddRange(Names);
        }

        private void LstCharacters_SelectedValueChanged(object sender, EventArgs e)
        {
            if (LstCharacters.SelectedIndex < 0) return;

            string? strTampName = LstCharacters.SelectedItem?.ToString();

            CreateUI(strTampName);
        }

        private void TamplatesForm_SizeChanged(object sender, EventArgs e)
        {
            SplitConCharTamplates.SplitterDistance = (int)TblPnlLabel.ColumnStyles[0].Width;
        }

        private void BtnDeleteTamplates_Click(object sender, EventArgs e)
        {
            if (picCurrentSelection is null)
            {
                MessageBox.Show("Please select a tamplate to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string? filePath = picCurrentSelection.Tag?.ToString();

            //delete file 
            if (filePath != null)
            {
                File.Delete(filePath);

                //relodad UI
                GetTamplates();
                LstCharacters_SelectedValueChanged(null,null);

                MessageBox.Show("Tamplate Deleted", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
