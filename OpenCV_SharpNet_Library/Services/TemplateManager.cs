using OpenCV_SharpNet.Enums;
using OpenCV_SharpNet.Models;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CvRect = OpenCvSharp.Rect;
using Point = System.Drawing.Point;

namespace OpenCV_SharpNet.Services
{
    public static class TemplateManager
    {
        // This list holds the images in RAM. If this is empty, matching FAILS.
        public static List<TemplateItem> GlobalTemplates = new List<TemplateItem>();
        //public static string TemplateSavePath { get; set; } = Path.Combine(Application.StartupPath, "Templates");
        public static string TemplateSavePath { get; set; } =  @"D:\py\Templates"; //ConfigurationManager.AppSettings["TemplatePath"] ??

        public static void LoadTemplates()
        {
            // 1. Clear old memory to avoid duplicates
            foreach (var t in GlobalTemplates)
            {
                if (t.Image != null && !t.Image.IsDisposed) t.Image.Dispose();
            }
            GlobalTemplates.Clear();

            // 2. Construct Path
            string folder = TemplateSavePath;

            // 3. Create folder if it doesn't exist
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
                return; // Nothing to load yet
            }

            // 4. Load all PNGs
            var files = Directory.GetFiles(folder, "*.png");

            foreach (var file in files)
            {
                // Load Grayscale (Important for ORB matching)
                Mat mat = Cv2.ImRead(file, ImreadModes.Grayscale);

                if (!mat.Empty())
                {
                    GlobalTemplates.Add(new TemplateItem
                    {
                        Name = Path.GetFileNameWithoutExtension(file),
                        Image = mat
                    });
                }
            }

            System.Diagnostics.Debug.WriteLine($"Templates Loaded: {GlobalTemplates.Count}");
        }

        public static void SaveTemplate(RoiObject roi,Mat currentImage)
        {
            if (roi.Type != RoiType.TemplateMatch) return;

            // 1. Get the Crop
            CvRect safeBox = roi.Box.Intersect(new CvRect(0, 0, currentImage.Width, currentImage.Height));
            if (safeBox.Width == 0 || safeBox.Height == 0) return;

            //using Mat crop = new Mat(currentImage, safeBox);
            //using Mat grayCrop = new Mat();
            //Cv2.CvtColor(crop, grayCrop, ColorConversionCodes.BGR2GRAY);

            // NEW SAFE CODE:
            using Mat crop = new Mat(currentImage, safeBox);
            using Mat grayCrop = new Mat();

            // Check if the image actually has color channels before converting!
            if (crop.Channels() >= 3)
            {
                Cv2.CvtColor(crop, grayCrop, ColorConversionCodes.BGR2GRAY);
            }
            else
            {
                // It is already a 1-channel Monochrome image from the camera, just copy it!
                crop.CopyTo(grayCrop);
            }

            // 2. Prepare Folder
            //string templateFolder = Path.Combine(Application.StartupPath, "Templates");
            string templateFolder = TemplateSavePath;
            if (!Directory.Exists(templateFolder)) Directory.CreateDirectory(templateFolder);

            // 3. AUTO-GENERATE FILENAME
            // Start with the ROI name (e.g., "ROI_1" or "Logo")
            string baseName = string.IsNullOrWhiteSpace(roi.Name) ? "Template" : MakeValidFileName(roi.Name);

            string fileName = baseName + ".png";
            string fullPath = Path.Combine(templateFolder, fileName);

            // Check if file exists. If so, append numbers (Logo_1.png, Logo_2.png...)
            int counter = 1;
            while (File.Exists(fullPath))
            {
                fileName = $"{baseName}_{counter}.png";
                fullPath = Path.Combine(templateFolder, fileName);
                counter++;
            }

            // 4. Save silently
            Cv2.ImWrite(fullPath, grayCrop);

            // 5. Reload Library
            LoadTemplates();

            // 6. Visual Feedback (Optional: show in status bar instead of MessageBox)
            // MessageBox.Show($"Auto-saved: {fileName}"); 
            //System.Diagnostics.Debug.WriteLine($"Template saved: {fileName}");
            //MessageBox.Show("Engine is trained on new templates", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static string MakeValidFileName(string name)
        {
            string invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            foreach (char c in invalidChars)
            {
                name = name.Replace(c.ToString(), "_");
            }
            return name.Replace(" ", "_"); // Replace spaces with underscores
        }
    }
}
