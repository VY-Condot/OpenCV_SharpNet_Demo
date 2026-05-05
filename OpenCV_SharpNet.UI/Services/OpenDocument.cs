using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCV_SharpNet.UI.Services
{
    public static class OpenDocument
    {
        public static string DocPath { get; set; } = ConfigurationManager.AppSettings["DocPath"] ?? $"{AppDomain.CurrentDomain.BaseDirectory}Documents\\SystemDocumentation.html";

        public static void Open(string docPath)
        {
            try
            {
                // 1. Build the dynamic path based on where the .exe is running
                //string docPath = System.IO.Path.Combine(Application.StartupPath, "Documents", "SystemDocumentation.html");

                // 2. Check if the file exists to prevent crashes
                if (System.IO.File.Exists(docPath))
                {
                    // 3. Open it in the user's default web browser (Chrome, Edge, etc.)
                    // Note: UseShellExecute = true is required in modern .NET to open URLs/HTML files safely.
                    var psi = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = docPath,
                        UseShellExecute = true
                    };
                    System.Diagnostics.Process.Start(psi);
                }
                else
                {
                    MessageBox.Show("Documentation file is missing or corrupted.\nPath checked: " + docPath,
                                    "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening documentation: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void Open()
        {
            try
            {
                // 1. Build the dynamic path based on where the .exe is running
                //string docPath = System.IO.Path.Combine(Application.StartupPath, "Documents", "SystemDocumentation.html");

                // 2. Check if the file exists to prevent crashes
                if (System.IO.File.Exists(DocPath))
                {
                    // 3. Open it in the user's default web browser (Chrome, Edge, etc.)
                    // Note: UseShellExecute = true is required in modern .NET to open URLs/HTML files safely.
                    var psi = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = DocPath,
                        UseShellExecute = true
                    };
                    System.Diagnostics.Process.Start(psi);
                }
                else
                {
                    MessageBox.Show("Documentation file is missing or corrupted.\nPath checked: " + DocPath,
                                    "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening documentation: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
