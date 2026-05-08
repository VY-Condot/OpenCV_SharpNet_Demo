using OpenCV_SharpNet_Demo;
using OpenCV_SharpNet_Demo.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenCV_SharpNet_Demo.UI
{
    public partial class FrmRedirect : Form
    {
        public FrmRedirect()
        {
            InitializeComponent();
        }

        // Property to hold the user's choice
        public string SelectedMode { get; private set; } = string.Empty;

        private void BtnImageMode_Click(object sender, EventArgs e)
        {
            //MainForm mainForm = new MainForm();
            //mainForm.Show();


            //Hide();

            SelectMode("IMAGE");
        }
        private void BtnCameraMode_Click(object sender, EventArgs e)
        {
            SelectMode("CAMERA");
        }

        private void SelectMode(string mode)
        {
            SelectedMode = mode;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void FrmRedirect_Load(object sender, EventArgs e)
        {
            toolStripVersion.Text = $"Version: {Application.ProductVersion.Split('+')[0]}";
        }

        private void BtnImageMode_MouseEnter(object sender, EventArgs e)
        {
            btnImageMode.BackColor = Color.AliceBlue;
        }

        private void BtnCameraMode_MouseEnter(object sender, EventArgs e)
        {
            btnCameraMode.BackColor = Color.AliceBlue;
        }

        private void BtnImageMode_MouseLeave(object sender, EventArgs e)
        {
            btnImageMode.BackColor = Color.White;
        }

        private void BtnCameraMode_MouseLeave(object sender, EventArgs e)
        {
            btnCameraMode.BackColor = Color.White;
        }

        private void ToolStripDocument_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    // 1. Build the dynamic path based on where the .exe is running
            //    string docPath = System.IO.Path.Combine(Application.StartupPath, "Documents", "SystemDocumentation.html");

            //    // 2. Check if the file exists to prevent crashes
            //    if (System.IO.File.Exists(docPath))
            //    {
            //        // 3. Open it in the user's default web browser (Chrome, Edge, etc.)
            //        // Note: UseShellExecute = true is required in modern .NET to open URLs/HTML files safely.
            //        var psi = new System.Diagnostics.ProcessStartInfo
            //        {
            //            FileName = docPath,
            //            UseShellExecute = true
            //        };
            //        System.Diagnostics.Process.Start(psi);
            //    }
            //    else
            //    {
            //        MessageBox.Show("Documentation file is missing or corrupted.\nPath checked: " + docPath,
            //                        "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error opening documentation: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}

            //open document
            OpenDocument.Open();
        }

        private void TsslUserManual_Click(object sender, EventArgs e)
        {
            //open document
            string DocPath = ConfigurationManager.AppSettings["UserManualPath"] ?? $"{AppDomain.CurrentDomain.BaseDirectory}Documents\\UserManual.html";
            OpenDocument.Open(DocPath);
        }
    }
}
