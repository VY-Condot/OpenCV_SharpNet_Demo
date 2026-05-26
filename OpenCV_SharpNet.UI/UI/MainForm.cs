using CsplCam.Library.Enums;
using CsplCam.Library.Models;
using OpenCV_SharpNet.UI.Services;
using CsplCam.Library.Models.GS1_QC;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.ComponentModel;
using System.Configuration;
using System.Runtime;
using System.Text.Json;
using CvRect = OpenCvSharp.Rect;
using Point = System.Drawing.Point;

// Aliases
using SysPoint = System.Drawing.Point;
using SysRect = System.Drawing.Rectangle;
using CsplCam.Library.Interfaces;
using CsplCam.Library.Services;
using CsplCam.Library.Services.AI;
using OpenCV_SharpNet.UserControls;
using OpenCV_SharpNet.UI.UI.GS1_QC;

namespace OpenCV_SharpNet.UI
{
    public partial class MainForm : Form
    {
        //// Core Data
        private Mat currentImage;
        private List<RoiObject> rois = new List<RoiObject>();

        // ====================================================================
        // HIGH PERFORMANCE UX VARIABLES
        // ====================================================================
        private Bitmap _displayBitmap = null; // Caches the image so UI doesn't lag
        private double _smoothedExecutionTime = 0; // EMA Smoother for stable UI numbers
        private long _imgCount = 0; // Tracks frames for the OpenCV GC Relief Valve

        // State
        private float zoom = 1.0f;
        private SysPoint panOffset = new SysPoint(0, 0);
        private MouseMode currentMode = MouseMode.None;
        private RoiObject selectedRoi = null;
        private ResizeHandle activeHandle = ResizeHandle.None;
        private SysPoint lastMousePos;
        private const int HANDLE_SIZE = 8;
        private int intCurrentSelectFileIndex = -1;
        private double totalTimeTaken = 0;

        private RoiObject? _clipboardRoi = null;
        bool IsGenerateReport = bool.TryParse(ConfigurationManager.AppSettings["IsGenerateReport"], out var res) ? res : false;
        List<GS1_QC_CheckResult> lstGs1Res = new();

        public MainForm()
        {
            InitializeComponent();
            FlowPnlRoiData.DoubleBuffered(true);
            ImageCanvas.MouseWheel += ImageCanvas_MouseWheel;
        }

        // Add these class-level variables to track the image size
        private int _lastImageWidth = 0;
        private int _lastImageHeight = 0;

        private void SetCurrentImage(Mat newMat)
        {
            if (newMat == null || newMat.Empty()) return;

            // =========================================================================
            // THE FIX: PREVENT ROIs FROM GOING OUT OF BOUNDS WHEN IMAGE CHANGES
            // =========================================================================
            if (_lastImageWidth > 0 && _lastImageHeight > 0 &&
               (_lastImageWidth != newMat.Width || _lastImageHeight != newMat.Height))
            {
                // Calculate the exact ratio between the old image and the new image
                double scaleX = (double)newMat.Width / _lastImageWidth;
                double scaleY = (double)newMat.Height / _lastImageHeight;

                foreach (var roi in rois)
                {
                    // 1. Scale the ROI Box proportionally
                    int newX = (int)Math.Round(roi.Box.X * scaleX);
                    int newY = (int)Math.Round(roi.Box.Y * scaleY);
                    int newW = (int)Math.Round(roi.Box.Width * scaleX);
                    int newH = (int)Math.Round(roi.Box.Height * scaleY);

                    // Safety Clamp: Ensure scaling didn't accidentally push it 1 pixel over the edge
                    if (newX + newW > newMat.Width) newW = newMat.Width - newX;
                    if (newY + newH > newMat.Height) newH = newMat.Height - newY;
                    if (newX < 0) newX = 0;
                    if (newY < 0) newY = 0;

                    roi.Box = new CvRect(newX, newY, newW, newH);

                    // 2. Scale the Anchor Search Area proportionally
                    if (roi.IsAnchor)
                    {
                        roi.AnchorTop = (int)Math.Round(roi.AnchorTop * scaleY);
                        roi.AnchorBottom = (int)Math.Round(roi.AnchorBottom * scaleY);
                        roi.AnchorLeft = (int)Math.Round(roi.AnchorLeft * scaleX);
                        roi.AnchorRight = (int)Math.Round(roi.AnchorRight * scaleX);

                        // 3. Scale the Anchor Template Image so template matching doesn't crash
                        if (roi.AnchorTemplate != null && !roi.AnchorTemplate.Empty())
                        {
                            int tW = (int)Math.Round(roi.AnchorTemplate.Width * scaleX);
                            int tH = (int)Math.Round(roi.AnchorTemplate.Height * scaleY);

                            // Prevent resizing to 0
                            tW = Math.Max(1, tW);
                            tH = Math.Max(1, tH);

                            Mat resizedTemplate = new Mat();
                            Cv2.Resize(roi.AnchorTemplate, resizedTemplate, new OpenCvSharp.Size(tW, tH));

                            roi.AnchorTemplate.Dispose();
                            roi.AnchorTemplate = resizedTemplate;
                        }
                    }
                }
            }

            // Update the trackers for the next time the image changes
            _lastImageWidth = newMat.Width;
            _lastImageHeight = newMat.Height;

            // Proceed with normal image update
            if (currentImage != null && currentImage != newMat) currentImage.Dispose();
            currentImage = newMat;

            if (_displayBitmap != null) _displayBitmap.Dispose();
            _displayBitmap = BitmapConverter.ToBitmap(currentImage);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 1. Force Windows to treat this app as High Priority
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.High;
            System.Threading.ThreadPool.SetMinThreads(Environment.ProcessorCount * 2, Environment.ProcessorCount * 2);

            CmbSegments.DataSource = Enum.GetValues(typeof(SegmentationMode));
            CmbSegments.SelectedIndexChanged += CmbSegments_SelectedIndexChanged;

            if (OcrEngine.TemplateVectors.Count == 0) OcrEngine.ReloadTemplates();
            TemplateManager.LoadTemplates();

            Text = $"{Application.ProductName} : Template based OCR + Barcode - Multi ROI + Training + TM";
            toolStripVersion.Text = $"Version: {Application.ProductVersion.Split('+')[0]}";
            DisplaySelectInfo(DisplayInfo.Default);



            // Inside your Form_Load:

            string aiFolderPath = System.IO.Path.Combine(Application.StartupPath, "AI.DataSet");

            // 1. Init WeChat QR
            OpenCV_WeChatQREngine_AI.Initialize(aiFolderPath);

            // 2. Init Super Resolution (Make sure the filename matches what you downloaded)
            string srModelPath = System.IO.Path.Combine(aiFolderPath, "ESPCN_x2.pb");
            SuperResolutionEngine_AI.Initialize(srModelPath);

            // 3. Init YOLOv8 Detector
            string yoloModelPath = System.IO.Path.Combine(aiFolderPath, "yolov8n_barcode.onnx");
            YoloDetector_AI.Initialize(yoloModelPath);
        }

        private void BtnLoadImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = true;
                ofd.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.tiff|All Files|*.*";

                if (ofd.ShowDialog() != DialogResult.OK) return;

                var allfiles = ofd.FileNames;
                if (allfiles.Length <= 0)
                {
                    MessageBox.Show("No files selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                foreach (var r in rois) if (r.AnchorTemplate != null) r.AnchorTemplate.Dispose();
                rois.Clear();
                selectedRoi = null;
                ClearRoiControl();

                LstImageList.Items.Clear();
                LstImageList.Items.AddRange(allfiles);

                DisplaySelectInfo(DisplayInfo.ImageCounting);
                intCurrentSelectFileIndex = 0;
                LstImageList.SelectedIndex = 0;

                if (currentImage != null)
                {
                    float sx = (float)ImageCanvas.Width / currentImage.Width;
                    float sy = (float)ImageCanvas.Height / currentImage.Height;
                    zoom = Math.Min(sx, sy) * 0.9f;
                    int dispW = (int)(currentImage.Width * zoom);
                    int dispH = (int)(currentImage.Height * zoom);
                    panOffset = new SysPoint((ImageCanvas.Width - dispW) / 2, (ImageCanvas.Height - dispH) / 2);
                }
                ImageCanvas.Invalidate();
            }
        }

        private void LstImageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LstImageList.SelectedIndex == -1) return;

            intCurrentSelectFileIndex = LstImageList.SelectedIndex;
            string filePath = LstImageList.SelectedItem.ToString();

            try
            {
                Mat newImg = Cv2.ImRead(filePath);
                if (newImg.Empty()) return;

                // USE NEW SAFE IMAGE SETTER
                SetCurrentImage(newImg);

                foreach (var r in rois) r.CharResults?.Clear();

                if (rois.Any(r => r.IsAnchor)) AlignRois();

                if (rois.Count > 0)
                {
                    BtnDecodeAllROI.PerformClick();
                    return;
                }

                RefreshRightPanel();
                ImageCanvas.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading image: " + ex.Message);
            }
        }

        private void ClearRoiControl()
        {
            TblBlobSettings.Visible = false;
            FlowPnlRoiData.Controls.Clear();
        }

        private void RefreshRightPanel()
        {
            if (selectedRoi is null || (selectedRoi.Type != RoiType.Text && selectedRoi.Type != RoiType.Barcode && selectedRoi.Type != RoiType.TemplateMatch))
            {
                FlowPnlRoiData.Controls.Clear();
                return;
            }

            TblBlobSettings.Visible = true;

            if (FlowPnlRoiData.Controls.Count != rois.Count) RebuildRoiList();
            else UpdateRoiSelectionVisuals();

            //var scrollControl = FlowPnlRoiData.Controls.Cast<ROIControl>().FirstOrDefault(x => x.BoundedROI == selectedRoi);

            var scrollControl = FlowPnlRoiData.Controls.Cast<IRoiControl>().FirstOrDefault(x => x.BoundedROI == selectedRoi);
            if (scrollControl != null) FlowPnlRoiData.ScrollControlIntoView((Control)scrollControl);
        }

        private void UpdateRoiSelectionVisuals()
        {
            for (int i = 0; i < FlowPnlRoiData.Controls.Count; i++)
            {
                if (FlowPnlRoiData.Controls[i] is IRoiControl ctrl)  //FlowPnlRoiData.Controls[i] is ROIControl ctrl
                {
                    bool isSelected = (ctrl.BoundedROI == selectedRoi);
                    ctrl.SetSelectionState(isSelected);
                    ctrl.BindData(ctrl.BoundedROI, isSelected);
                }
            }
        }

        private void RebuildRoiList()
        {
            FlowPnlRoiData.SuspendLayout();
            FlowPnlRoiData.Controls.Clear();

            foreach (var roi in rois)
            {
                //IRoiControl roiCtrl = roi.Type == RoiType.Barcode ? new ROIControlBarCode() : new ROIControl();

                IRoiControl roiCtrl = roi.Type switch
                {
                    RoiType.Text => new ROIControl(),
                    RoiType.Barcode => new ROIControlBarCode(),
                    RoiType.TemplateMatch => new RoiControlTemplate(),
                    _ => new ROIControl()
                };


                roiCtrl.BindData(roi, (selectedRoi == roi));
                UpdateSidePanelPreview(roiCtrl, roi);

                roiCtrl.SelectionClick += (s, e) =>
                {
                    selectedRoi = roi;
                    RefreshRightPanel();
                    ImageCanvas.Invalidate();
                };

                roiCtrl.SettingsChanged += (s, e) =>
                {
                    ValidateAndFixBoxOrientation(roi);
                    UpdateSidePanelPreview(roiCtrl, roi);
                    ImageCanvas.Invalidate();
                };

                roiCtrl.OpenAnchorSettingsWindow += (s, e) => OpenAnchorSettingWindow(roi, rOIControl: roiCtrl);
                roiCtrl.OpenRoiReferenceWindow += (s, e) => OpenRoiReferenceWindow(roi);

                roiCtrl.DecodeRequested += (s, e) =>
                {
                    totalTimeTaken = 0;
                    selectedRoi = roi;

                    GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
                    GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;

                    roi.CharResults?.Clear();
                    OcrEngine.DecodeRoi(currentImage, roi);
                    totalTimeTaken += roi.TimeTakenForDecoding.TotalMilliseconds;

                    GCSettings.LatencyMode = GCLatencyMode.Interactive;
                    GC.Collect(0, GCCollectionMode.Optimized, false); // FAST SYNC GC

                    lstGs1Res.Add(roi.Gs1QcResult);

                    SetDataInDataGridView(roi);

                    DisplaySelectInfo(DisplayInfo.TimeTaken);

                    RefreshRightPanel();
                    ImageCanvas.Invalidate();
                };

                FlowPnlRoiData.Controls.Add((Control)roiCtrl);
            }
            FlowPnlRoiData.ResumeLayout();
        }

        private void OpenRoiReferenceWindow(RoiObject roiObject)
        {
            if (roiObject.IsUseRoiReference)
            {
                var flRoi = rois.Where(x => x.Id != roiObject.Id).ToList();

                if (flRoi is null || flRoi.Count == 0)
                {
                    MessageBox.Show("No other rois available for reference.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    roiObject.IsUseRoiReference = false;
                    RefreshRightPanel();
                    return;
                }

                using (var form = new FrmReferenceRoi(flRoi))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        roiObject.ReferenceRoiID = form.SelectRoiID;
                        MessageBox.Show($"Reference Set for {roiObject.Name} ");
                    }
                    else
                    {
                        roiObject.IsUseRoiReference = false;
                        roiObject.ReferenceRoiID = -1;
                        RefreshRightPanel();
                    }
                }
            }
        }

        private void ImageCanvas_Paint(object sender, PaintEventArgs e)
        {
            // BLAZING FAST RENDERING WITH _displayBitmap CACHE
            if (currentImage == null || currentImage.IsDisposed || _displayBitmap == null) return;

            Graphics g = e.Graphics;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

            int w = (int)(currentImage.Width * zoom);
            int h = (int)(currentImage.Height * zoom);
            g.DrawImage(_displayBitmap, panOffset.X, panOffset.Y, w, h);

            using var penNormal = new Pen(Color.Magenta, 2);
            using var penSel = new Pen(Color.Cyan, 2);
            using var fontLabel = new Font("Arial", 10, FontStyle.Bold);
            using var brushLabel = new SolidBrush(Color.Yellow);
            using var brushLabelForFont = new SolidBrush(Color.DarkRed);
            using var penTrainGood = new Pen(Color.Yellow, 2);
            using var brushTrainGood = new SolidBrush(Color.Yellow);
            using var penTrainBad = new Pen(Color.Red, 2);
            using var brushTrainBad = new SolidBrush(Color.Red);
            using var penMatch = new Pen(Color.Blue, 2);
            using var brushMatch = new SolidBrush(Color.Blue);
            using var penOverlay = new Pen(Color.Blue, 2);
            using var brushOverlay = new SolidBrush(Color.Blue);
            using var penPass = new Pen(Color.Lime, 3);
            using var penFail = new Pen(Color.Red, 3);

            float dynamicFontSize = Math.Max(5, 12 * zoom);
            using var fontChar = new Font("Consolas", dynamicFontSize, FontStyle.Bold);
            using var fmtCenter = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            using var fmtTop = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far };
            using var fmtSide = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };

            using var penArrow = new Pen(Color.LimeGreen, 3);
            using var arrowCap = new System.Drawing.Drawing2D.AdjustableArrowCap(4, 4);
            penArrow.CustomEndCap = arrowCap;

            foreach (var roi in rois)
            {

                // =====================================================================
                // THE VISUAL FIX: Intersection for UI only
                // =====================================================================
                // This 'visibleBox' is just for drawing. It does NOT change your anchor logic.
                CvRect visibleBox = roi.Box.Intersect(new CvRect(0, 0, currentImage.Width, currentImage.Height));

                // If the entire box is off-screen, don't draw anything
                if (visibleBox.Width <= 0 || visibleBox.Height <= 0) continue;


                //SysRect r = ImageToScreen(roi.Box);
                SysRect r = ImageToScreen(visibleBox);
                bool isSel = (roi == selectedRoi) || roi.IsSelected;
                bool isPrimary = (roi == selectedRoi);

                g.DrawRectangle(isSel ? penSel : penNormal, r);

                if (!roi.IsNameChanged) g.DrawString($"ROI {roi.Id}", fontLabel, brushLabelForFont, r.Left, r.Top - 25);
                else g.DrawString($"{roi.Name}({roi.Type})", fontLabel, brushLabelForFont, r.Left, r.Top - 25);

                if (isPrimary)
                {
                    var handles = GetHandleRects(r);
                    foreach (var hRect in handles.Values) g.FillRectangle(brushLabel, hRect);
                }

                DrawExternalArrow(g, ImageToScreen(roi.Box), roi.RotationAngle);

                // =========================================================
                // VISUAL UX: DRAW ANCHOR SEARCH BOUNDARY
                // =========================================================
                if (roi.IsAnchor && isSel)
                {
                    int startX = Math.Max(0, roi.Box.X - roi.AnchorLeft);
                    int startY = Math.Max(0, roi.Box.Y - roi.AnchorTop);
                    int endX = Math.Min(currentImage.Width, roi.Box.X + roi.Box.Width + roi.AnchorRight);
                    int endY = Math.Min(currentImage.Height, roi.Box.Y + roi.Box.Height + roi.AnchorBottom);

                    int searchW = endX - startX;
                    int searchH = endY - startY;

                    if (searchW > 0 && searchH > 0)
                    {
                        CvRect searchBoxImg = new CvRect(startX, startY, searchW, searchH);
                        SysRect searchBoxScreen = ImageToScreen(searchBoxImg);

                        using var searchPen = new Pen(Color.Orange, 2);
                        searchPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        g.DrawRectangle(searchPen, searchBoxScreen);

                        using var searchFont = new Font("Arial", 8, FontStyle.Italic);
                        using var searchBrush = new SolidBrush(Color.Orange);
                        g.DrawString("Search Area", searchFont, searchBrush, searchBoxScreen.X, searchBoxScreen.Y - 15);
                    }
                }

                if (roi.Type == RoiType.TemplateMatch)
                {
                    string scoreTxt = $"{(roi.TmScore * 100):0}%";
                    g.DrawString(scoreTxt, fontLabel, brushLabelForFont, r.Right + 5, r.Top);
                    if (roi.TmScore > 0) g.DrawRectangle(roi.TmPass ? penPass : penFail, r);

                    foreach (var cr in roi.CharResults)
                    {
                        CvRect diffAbs = new CvRect(roi.Box.X + cr.Box.X, roi.Box.Y + cr.Box.Y, cr.Box.Width, cr.Box.Height);
                        SysRect sr = ImageToScreen(diffAbs);
                        g.DrawRectangle(penTrainBad, sr);
                        g.DrawString(cr.Text, fontChar, brushTrainBad, sr.Left + (sr.Width / 2.0f), sr.Top - 2, fmtTop);
                    }
                }

                if (roi.Type == RoiType.Barcode && roi.CharResults.Count > 0)
                {
                    foreach (var cr in roi.CharResults)
                    {

                        // Check if we successfully captured the 4 tilted corners
                        if (cr.Polygon != null && cr.Polygon.Length == 4)
                        {
                            // Convert the 4 OpenCV image points into Screen coordinates (accounting for Pan & Zoom)
                            System.Drawing.Point[] drawPts = new System.Drawing.Point[4];
                            for (int i = 0; i < 4; i++)
                            {
                                int absX = roi.Box.X + cr.Polygon[i].X;
                                int absY = roi.Box.Y + cr.Polygon[i].Y;

                                drawPts[i] = new System.Drawing.Point(
                                    (int)(absX * zoom) + panOffset.X,
                                    (int)(absY * zoom) + panOffset.Y
                                );
                            }

                            // Draw a beautiful Tilted Green Polygon!
                            g.DrawPolygon(penPass, drawPts);

                            // Draw the text above the Top-Left corner of the barcode
                            g.DrawString(cr.Text, fontChar, brushMatch, drawPts[0].X, drawPts[0].Y - 15);
                        }
                        else
                        {
                            CvRect bcAbs = new CvRect(roi.Box.X + cr.Box.X, roi.Box.Y + cr.Box.Y, cr.Box.Width, cr.Box.Height);
                            SysRect sr = ImageToScreen(bcAbs);
                            g.DrawRectangle(penPass, sr);
                            g.DrawString(cr.Text, fontChar, brushMatch, sr.Left + (sr.Width / 2.0f), sr.Top - 2, fmtTop);
                        }
                    }
                }

                if (roi.Type == RoiType.Text && roi.CharResults.Count > 0)
                {
                    bool isOverlayMode = roi.ShowOverlay;
                    bool hasExpected = !string.IsNullOrEmpty(roi.ExpectedText);
                    int IntCharCounter = 0;

                    foreach (var cr in roi.CharResults)
                    {
                        CvRect charAbs = new CvRect(roi.Box.X + cr.Box.X, roi.Box.Y + cr.Box.Y, cr.Box.Width, cr.Box.Height);
                        SysRect sr = ImageToScreen(charAbs);

                        if (isOverlayMode)
                        {
                            string strTempChar = (IntCharCounter < roi.ExpectedText?.Length) ? roi.ExpectedText[IntCharCounter].ToString() : string.Empty;
                            if (IntCharCounter < roi.ExpectedText?.Length) IntCharCounter++;

                            string txt = cr.Text;
                            Brush DesiredBrush = brushOverlay;
                            Pen DesirePen = penOverlay;
                            g.DrawRectangle(DesirePen, sr);

                            if (roi.RotationAngle == RotationAngles.Ninety || roi.RotationAngle == RotationAngles.TwoSeventy)
                                g.DrawString(txt, fontChar, DesiredBrush, sr.Right + 2, sr.Top + (sr.Height / 2.0f), fmtSide);
                            else
                                g.DrawString(txt, fontChar, DesiredBrush, sr.Left + (sr.Width / 2.0f), sr.Top - 2, fmtTop);
                        }
                        else
                        {
                            if (hasExpected)
                            {
                                if (cr.IsExpectedMatch)
                                {
                                    g.DrawRectangle(penMatch, sr);
                                    g.DrawString(cr.Text, fontChar, brushMatch, sr, fmtCenter);
                                }
                                else
                                {
                                    g.DrawRectangle(penTrainBad, sr);
                                    g.DrawString("?", fontChar, brushTrainBad, sr, fmtCenter);
                                }
                            }
                            else
                            {
                                if (cr.IsGood && roi.JustTrained)
                                {
                                    g.DrawRectangle(penTrainGood, sr);
                                    g.DrawString(cr.Text, fontChar, brushTrainGood, sr, fmtCenter);
                                }
                                else
                                {
                                    g.DrawRectangle(penTrainBad, sr);
                                    g.DrawString("?", fontChar, brushTrainBad, sr, fmtCenter);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void PerformSelectAll()
        {
            if (rois.Count == 0) return;
            rois.ForEach(r => r.IsSelected = true);
            ImageCanvas.Invalidate();
        }

        private void DrawExternalArrow(Graphics g, SysRect r, RotationAngles angle)
        {
            int gap = 15;
            int arrowCapSize = 6;
            Color arrowColor = Color.Lime;

            using var pen = new Pen(arrowColor, 3);
            using var arrowCap = new System.Drawing.Drawing2D.AdjustableArrowCap(arrowCapSize, arrowCapSize);
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.CustomEndCap = arrowCap;

            int quarterW = r.Width / 4;
            int quarterH = r.Height / 4;
            Point start = Point.Empty;
            Point end = Point.Empty;

            switch (angle)
            {
                case RotationAngles.Zero:
                    start = new Point(r.Left + quarterW, r.Top - gap);
                    end = new Point(r.Right - quarterW, r.Top - gap);
                    break;
                case RotationAngles.Ninety:
                    start = new Point(r.Right + gap, r.Top + quarterH);
                    end = new Point(r.Right + gap, r.Bottom - quarterH);
                    break;
                case RotationAngles.OneEighty:
                    start = new Point(r.Right - quarterW, r.Bottom + gap);
                    end = new Point(r.Left + quarterW, r.Bottom + gap);
                    break;
                case RotationAngles.TwoSeventy:
                    start = new Point(r.Left - gap, r.Bottom - quarterH);
                    end = new Point(r.Left - gap, r.Top + quarterH);
                    break;
            }
            g.DrawLine(pen, start, end);
        }

        private SysRect ImageToScreen(CvRect r) => new((int)(r.X * zoom) + panOffset.X, (int)(r.Y * zoom) + panOffset.Y, (int)(r.Width * zoom), (int)(r.Height * zoom));
        private OpenCvSharp.Point ScreenToImage(SysPoint p) => new OpenCvSharp.Point((int)((p.X - panOffset.X) / zoom), (int)((p.Y - panOffset.Y) / zoom));

        private Dictionary<ResizeHandle, SysRect> GetHandleRects(SysRect r)
        {
            int h = HANDLE_SIZE;
            int w2 = r.Width / 2; int h2 = r.Height / 2;
            return new Dictionary<ResizeHandle, SysRect>
            {
                { ResizeHandle.TopLeft, new SysRect(r.Left-h, r.Top-h, h*2, h*2) },
                { ResizeHandle.TopRight, new SysRect(r.Right-h, r.Top-h, h*2, h*2) },
                { ResizeHandle.BottomLeft, new SysRect(r.Left-h, r.Bottom-h, h*2, h*2) },
                { ResizeHandle.BottomRight, new SysRect(r.Right-h, r.Bottom-h, h*2, h*2) },
                { ResizeHandle.Top, new SysRect(r.Left+w2-h, r.Top-h, h*2, h*2) },
                { ResizeHandle.Bottom, new SysRect(r.Left+w2-h, r.Bottom-h, h*2, h*2) },
                { ResizeHandle.Left, new SysRect(r.Left-h, r.Top+h2-h, h*2, h*2) },
                { ResizeHandle.Right, new SysRect(r.Right-h, r.Top+h2-h, h*2, h*2) }
            };
        }

        private void ImageCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (currentImage == null) return;
            lastMousePos = e.Location;
            var hit = HitTest(e.Location);

            if (hit.roi != null)
            {
                selectedRoi = hit.roi;
                currentMode = (hit.handle != ResizeHandle.None) ? MouseMode.ResizingRoi : MouseMode.MovingRoi;
                activeHandle = hit.handle;
                selectedRoi.IsSelected = false;
                DisplaySelectInfo(DisplayInfo.SingleSelectionMode);
            }
            else
            {
                rois.ForEach(p => p.IsSelected = false);
                currentMode = MouseMode.PanningImage;
                DisplaySelectInfo(DisplayInfo.ImagePanning);
            }
            ImageCanvas.Invalidate();
        }

        private (RoiObject roi, ResizeHandle handle) HitTest(SysPoint p)
        {
            if (selectedRoi != null)
            {
                var handles = GetHandleRects(ImageToScreen(selectedRoi.Box));
                foreach (var kvp in handles) if (kvp.Value.Contains(p)) return (selectedRoi, kvp.Key);
            }
            for (int i = rois.Count - 1; i >= 0; i--) if (ImageToScreen(rois[i].Box).Contains(p)) return (rois[i], ResizeHandle.None);
            return (null, ResizeHandle.None);
        }

        // ====================================================================
        // EMA SMOOTHED UI UPDATE
        // ====================================================================
        private void SetDataInDataGridView(List<RoiObject> roiDatas)
        {
            if (dgvDecodeTextRec.InvokeRequired)
            {
                dgvDecodeTextRec.Invoke(new Action(() => SetDataInDataGridView(roiDatas)));
                return;
            }

            dgvDecodeTextRec.SuspendLayout();

            foreach (var item in roiDatas)
            {
                double rawTime = item.TimeTakenForDecoding.TotalMilliseconds;

                string displayTime = rawTime + " (ms)";

                string[] strData = new string[] {
                        item.Type.ToString(),
                        item.DecodedText,
                        (item.Type == RoiType.TemplateMatch ? item.DecodedText.ToString() : item.OverAllResult.ToString()),
                        (item.Type == RoiType.TemplateMatch ? (Math.Floor(item.TmScore * 100) / 100.00).ToString("0.00") :(Math.Floor(item.RoiScore * 100) / 100.00).ToString("0.00")),
                        item.SkewAngleOfRoi.ToString("0.00"),
                        displayTime,
                        DateTime.Now.ToString("HH:mm:ss")
                    };

                dgvDecodeTextRec.Rows.Add(strData);
            }

            dgvDecodeTextRec.Sort(DecodeTime, ListSortDirection.Descending);
            if (dgvDecodeTextRec.Rows.Count > 20)
            {
                dgvDecodeTextRec.Rows.RemoveAt(dgvDecodeTextRec.Rows.Count - 1);
            }

            dgvDecodeTextRec.ResumeLayout();
        }


        private void SetDataInDataGridView(RoiObject roiDatas)
        {
            if (dgvDecodeTextRec.InvokeRequired)
            {
                dgvDecodeTextRec.Invoke(new Action(() => SetDataInDataGridView(roiDatas)));
                return;
            }

            dgvDecodeTextRec.SuspendLayout();

            double rawTime = roiDatas.TimeTakenForDecoding.TotalMilliseconds;

            string displayTime = rawTime + " (ms)";

            string[] strData = new string[] {
                        roiDatas.Type.ToString(),
                        roiDatas.DecodedText,
                        (roiDatas.Type == RoiType.TemplateMatch ? roiDatas.DecodedText.ToString() : roiDatas.OverAllResult.ToString()),
                        (roiDatas.Type == RoiType.TemplateMatch ? (Math.Floor(roiDatas.TmScore * 100) / 100.00).ToString("0.00") :(Math.Floor(roiDatas.RoiScore * 100) / 100.00).ToString("0.00")),
                        roiDatas.SkewAngleOfRoi.ToString("0.00"),
                        displayTime,
                        DateTime.Now.ToString("HH:mm:ss")
                    };

            dgvDecodeTextRec.Rows.Add(strData);

            dgvDecodeTextRec.Sort(DecodeTime, ListSortDirection.Descending);
            if (dgvDecodeTextRec.Rows.Count > 20)
            {
                dgvDecodeTextRec.Rows.RemoveAt(dgvDecodeTextRec.Rows.Count - 1);
            }

            dgvDecodeTextRec.ResumeLayout();
        }

        private void ImageCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (currentImage == null) return;

            if (currentMode == MouseMode.None)
            {
                var hit = HitTest(e.Location);
                switch (hit.handle)
                {
                    case ResizeHandle.TopLeft:
                    case ResizeHandle.BottomRight: ImageCanvas.Cursor = Cursors.SizeNWSE; break;
                    case ResizeHandle.TopRight:
                    case ResizeHandle.BottomLeft: ImageCanvas.Cursor = Cursors.SizeNESW; break;
                    case ResizeHandle.Top:
                    case ResizeHandle.Bottom: ImageCanvas.Cursor = Cursors.SizeNS; break;
                    case ResizeHandle.Left:
                    case ResizeHandle.Right: ImageCanvas.Cursor = Cursors.SizeWE; break;
                    case ResizeHandle.None: ImageCanvas.Cursor = (hit.roi != null) ? Cursors.SizeAll : Cursors.Hand; break;
                }
                return;
            }

            if (selectedRoi != null) selectedRoi.CharResults.Clear();

            int dx = e.X - lastMousePos.X;
            int dy = e.Y - lastMousePos.Y;
            int idx = (int)(dx / zoom);
            int idy = (int)(dy / zoom);

            if (idx == 0 && dx != 0) idx = dx > 0 ? 1 : -1;
            if (idy == 0 && dy != 0) idy = dy > 0 ? 1 : -1;

            if (currentMode == MouseMode.PanningImage)
            {
                panOffset.X += dx;
                panOffset.Y += dy;
            }
            else if (currentMode == MouseMode.MovingRoi && selectedRoi != null)
            {
                CvRect r = selectedRoi.Box;
                r.X += idx;
                r.Y += idy;

                if (r.X < 0) r.X = 0;
                if (r.Y < 0) r.Y = 0;
                if (r.X + r.Width > currentImage.Width) r.X = currentImage.Width - r.Width;
                if (r.Y + r.Height > currentImage.Height) r.Y = currentImage.Height - r.Height;

                selectedRoi.Box = r;
            }
            else if (currentMode == MouseMode.ResizingRoi && selectedRoi != null)
            {
                CvRect r = selectedRoi.Box;

                switch (activeHandle)
                {
                    case ResizeHandle.Right: r.Width += idx; break;
                    case ResizeHandle.Bottom: r.Height += idy; break;
                    case ResizeHandle.Left: r.X += idx; r.Width -= idx; break;
                    case ResizeHandle.Top: r.Y += idy; r.Height -= idy; break;
                    case ResizeHandle.BottomRight: r.Width += idx; r.Height += idy; break;
                    case ResizeHandle.BottomLeft: r.X += idx; r.Width -= idx; r.Height += idy; break;
                    case ResizeHandle.TopRight: r.Y += idy; r.Width += idx; r.Height -= idy; break;
                    case ResizeHandle.TopLeft: r.X += idx; r.Y += idy; r.Width -= idx; r.Height -= idy; break;
                }

                int minSize = 10;
                if (r.Width < minSize)
                {
                    if (activeHandle == ResizeHandle.Left || activeHandle == ResizeHandle.TopLeft || activeHandle == ResizeHandle.BottomLeft)
                        r.X -= (minSize - r.Width);
                    r.Width = minSize;
                }
                if (r.Height < minSize)
                {
                    if (activeHandle == ResizeHandle.Top || activeHandle == ResizeHandle.TopLeft || activeHandle == ResizeHandle.TopRight)
                        r.Y -= (minSize - r.Height);
                    r.Height = minSize;
                }

                if (r.X < 0) { r.Width += r.X; r.X = 0; }
                if (r.Y < 0) { r.Height += r.Y; r.Y = 0; }
                if (r.X + r.Width > currentImage.Width) r.Width = currentImage.Width - r.X;
                if (r.Y + r.Height > currentImage.Height) r.Height = currentImage.Height - r.Y;

                selectedRoi.Box = r;
            }

            lastMousePos = e.Location;
            ImageCanvas.Invalidate();
        }

        private void ImageCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            bool wasModifyingRoi = (currentMode == MouseMode.MovingRoi || currentMode == MouseMode.ResizingRoi);

            //if (wasModifyingRoi && selectedRoi != null && currentImage != null)
            //{
            //    var activeCtrl = FlowPnlRoiData.Controls.OfType<ROIControl>().FirstOrDefault(c => c.BoundedROI == selectedRoi);
            //    if (activeCtrl != null) UpdateSidePanelPreview(activeCtrl, selectedRoi);
            //}

            if (wasModifyingRoi && selectedRoi != null && currentImage != null)
            {
                var activeCtrl = FlowPnlRoiData.Controls.OfType<IRoiControl>().FirstOrDefault(c => c.BoundedROI == selectedRoi);
                if (activeCtrl != null) UpdateSidePanelPreview(activeCtrl, selectedRoi);
            }

            currentMode = MouseMode.None;
            activeHandle = ResizeHandle.None;

            if (e.Button == MouseButtons.Right)
            {
                var hit = HitTest(e.Location);
                var hitedROI = hit.roi;

                ContextMenuStrip contextMenu = new ContextMenuStrip();

                if (_clipboardRoi != null)
                {
                    ToolStripMenuItem itemPaste = new ToolStripMenuItem("Paste ROI");
                    itemPaste.Image = CreateMenuIcon("PASTE");
                    itemPaste.ShortcutKeys = Keys.Control | Keys.V;
                    itemPaste.Click += (s, args) => PasteROI(e.Location);
                    contextMenu.Items.Add(itemPaste);
                    contextMenu.Show(ImageCanvas, e.Location);
                }

                if (hitedROI is null) return;

                selectedRoi = hitedROI;
                RefreshRightPanel();
                ImageCanvas.Invalidate();

                ToolStripMenuItem tsmiCopy = new ToolStripMenuItem("Copy ROI");
                tsmiCopy.Image = CreateMenuIcon("COPY");
                tsmiCopy.ShortcutKeys = Keys.Control | Keys.C;
                tsmiCopy.Click += (s, args) => CopyROI();
                contextMenu.Items.Add(tsmiCopy);

                ToolStripMenuItem RenameItem = new ToolStripMenuItem("Rename ROI");
                RenameItem.Image = CreateMenuIcon("RENAME");
                RenameItem.Click += (s, args) =>
                {
                    using (var form = new ROI_RenameForm(hitedROI.Name))
                    {
                        if (form.ShowDialog() == DialogResult.OK) hitedROI.Name = form.GetNewRoiName;
                        hitedROI.IsNameChanged = true;
                    }
                    RefreshRightPanel();
                    ImageCanvas.Invalidate();
                };

                if (hitedROI.Type == RoiType.Text)
                {
                    ToolStripMenuItem rotateItem = new ToolStripMenuItem("Rotate");
                    rotateItem.Image = CreateMenuIcon("ROTATE_MAIN");
                    Action<RotationAngles> changeRotation = (newAngle) =>
                    {
                        hitedROI.RotationAngle = newAngle;
                        ValidateAndFixBoxOrientation(hitedROI);
                        if (currentImage != null) OcrEngine.DecodeRoi(currentImage, hitedROI);
                        RefreshRightPanel();
                        ImageCanvas.Invalidate();
                    };
                    rotateItem.DropDownItems.Add("0° (Left->Right)", CreateMenuIcon("ANGLE_0"), (s, args) => changeRotation(RotationAngles.Zero));
                    rotateItem.DropDownItems.Add("90° (Top->Bottom)", CreateMenuIcon("ANGLE_90"), (s, args) => changeRotation(RotationAngles.Ninety));
                    rotateItem.DropDownItems.Add("180° (Right->Left)", CreateMenuIcon("ANGLE_180"), (s, args) => changeRotation(RotationAngles.OneEighty));
                    rotateItem.DropDownItems.Add("270° (Bottom->Top)", CreateMenuIcon("ANGLE_270"), (s, args) => changeRotation(RotationAngles.TwoSeventy));
                    contextMenu.Items.Add(rotateItem);
                }

                ToolStripMenuItem deleteItem = new ToolStripMenuItem("Delete ROI");
                deleteItem.Image = SystemIcons.Error.ToBitmap();
                deleteItem.Click += (s, args) => DeletROI(hitedROI);

                contextMenu.Items.Add(RenameItem);
                contextMenu.Items.Add(new ToolStripSeparator());
                contextMenu.Items.Add(deleteItem);
                contextMenu.Show(ImageCanvas, e.Location);
            }
            RefreshRightPanel();
        }

        private void DeletROI(RoiObject hitedROI)
        {
            if (hitedROI is null && rois.All(P => !P.IsSelected))
            {
                MessageBox.Show("No ROI selected for deletion.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult userRes = DialogResult.Cancel;
            var TempselectedROI = rois.Where(P => P.Id != hitedROI?.Id);

            if (TempselectedROI.Any())
            {
                if (TempselectedROI.All(P => P.IsSelected))
                {
                    userRes = MessageBox.Show("Do you want to delete all selected ROIs?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (userRes == DialogResult.No) return;
                }
            }

            var GetListData = rois.ToList();
            GetListData.ForEach(P =>
            {
                SelectPreviousRoi(hitedROI);
                if (hitedROI != null) rois.Remove(hitedROI);
                if (P.IsSelected) rois.Remove(P);
                if (rois.Count <= 0) selectedRoi = null;
            });

            RefreshRightPanel();
            ImageCanvas.Invalidate();
        }

        private void CopyROI()
        {
            if (selectedRoi is null)
            {
                MessageBox.Show("No ROI selected for copy.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            _clipboardRoi = selectedRoi.Clone;
        }

        private void PasteROI(System.Drawing.Point clientClickPoint)
        {
            if (_clipboardRoi is null || currentImage is null)
            {
                MessageBox.Show("No ROI is copy to clipboard", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            RoiObject tempROI = _clipboardRoi.Clone;
            int intId = rois.Any() ? rois.Max(P => P.Id) + 1 : 1;
            tempROI.Id = intId;
            tempROI.Name = "ROI " + intId;
            tempROI.IsAnchor = false;

            OpenCvSharp.Point imgPt = ScreenToImage(clientClickPoint);

            int w = tempROI.Box.Width;
            int h = tempROI.Box.Height;
            int newX = imgPt.X - (w / 2);
            int newY = imgPt.Y - (h / 2);

            if (newX < 0) newX = 0;
            if (newY < 0) newY = 0;
            if (newX + w > currentImage.Width) newX = currentImage.Width - w;
            if (newY + h > currentImage.Height) newY = currentImage.Height - h;

            tempROI.Box = new CvRect(newX, newY, w, h);
            rois.Add(tempROI);
            selectedRoi = tempROI;

            RefreshRightPanel();
            ImageCanvas.Invalidate();
        }

        private void ValidateAndFixBoxOrientation(RoiObject roi)
        {
            bool targetIsVertical = (roi.RotationAngle == RotationAngles.Ninety || roi.RotationAngle == RotationAngles.TwoSeventy);
            CvRect r = roi.Box;
            bool currentIsVertical = r.Height > r.Width;

            if (targetIsVertical != currentIsVertical)
            {
                int cx = r.X + (r.Width / 2);
                int cy = r.Y + (r.Height / 2);
                int newW = r.Height;
                int newH = r.Width;
                int newX = cx - (newW / 2);
                int newY = cy - (newH / 2);
                roi.Box = new CvRect(newX, newY, newW, newH);
            }
        }

        private Bitmap CreateMenuIcon(string iconType)
        {
            int size = 16;
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                Color darkBlue = Color.FromArgb(0, 60, 160);
                Color darkGray = Color.FromArgb(80, 80, 80);
                int c = size / 2;

                switch (iconType)
                {
                    case "COPY":
                        using (Pen pBack = new Pen(darkGray, 1))
                        using (Brush bBack = new SolidBrush(Color.WhiteSmoke))
                        {
                            g.FillRectangle(bBack, 2, 2, 8, 10);
                            g.DrawRectangle(pBack, 2, 2, 8, 10);
                        }
                        using (Pen pFront = new Pen(darkBlue, 1))
                        using (Brush bFront = new SolidBrush(Color.White))
                        {
                            g.FillRectangle(bFront, 6, 5, 8, 10);
                            g.DrawRectangle(pFront, 6, 5, 8, 10);
                        }
                        break;
                    case "PASTE":
                        using (Pen pBoard = new Pen(darkBlue, 1))
                        using (Brush bBoard = new SolidBrush(Color.Tan))
                        {
                            g.FillRectangle(bBoard, 3, 2, 10, 13);
                            g.DrawRectangle(pBoard, 3, 2, 10, 13);
                        }
                        using (Brush bPaper = new SolidBrush(Color.White)) g.FillRectangle(bPaper, 5, 5, 6, 8);
                        using (Brush bClip = new SolidBrush(Color.Gray)) g.FillRectangle(bClip, 5, 1, 6, 3);
                        break;
                    case "RENAME":
                        using (Pen p = new Pen(darkBlue, 2))
                        {
                            g.DrawLine(p, 5, 3, 11, 3);
                            g.DrawLine(p, 5, 13, 11, 13);
                            g.DrawLine(p, 8, 3, 8, 13);
                        }
                        using (Brush b = new SolidBrush(Color.OrangeRed))
                        {
                            Point[] pencil = { new Point(10, 10), new Point(14, 10), new Point(12, 14) };
                            g.FillPolygon(b, pencil);
                        }
                        break;
                    case "ROTATE_MAIN":
                        using (Pen p = new Pen(darkBlue, 2))
                        {
                            p.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(3, 3);
                            g.DrawArc(p, 3, 3, 10, 10, 45, 270);
                        }
                        break;
                    case "ANGLE_0":
                        using (Pen p = new Pen(darkGray, 2))
                        {
                            p.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(3, 3);
                            g.DrawLine(p, 2, c, 14, c);
                        }
                        break;
                    case "ANGLE_90":
                        using (Pen p = new Pen(darkGray, 2))
                        {
                            p.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(3, 3);
                            g.DrawLine(p, c, 2, c, 14);
                        }
                        break;
                    case "ANGLE_180":
                        using (Pen p = new Pen(darkGray, 2))
                        {
                            p.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(3, 3);
                            g.DrawLine(p, 14, c, 2, c);
                        }
                        break;
                    case "ANGLE_270":
                        using (Pen p = new Pen(darkGray, 2))
                        {
                            p.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(3, 3);
                            g.DrawLine(p, c, 14, c, 2);
                        }
                        break;
                }
            }
            return bmp;
        }

        private void SelectPreviousRoi(RoiObject objSelectRoi)
        {
            if (rois.Count < 0) return;
            var RoiIndex = rois.IndexOf(objSelectRoi);
            if (RoiIndex <= 0) return;
            selectedRoi = rois[RoiIndex - 1];
        }

        private void ImageCanvas_MouseWheel(object sender, MouseEventArgs e)
        {
            float f = e.Delta > 0 ? 1.1f : 0.9f;
            zoom *= f;

            if (zoom <= 0.05) { MessageBox.Show("Minimum zoom reached.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information); zoom = zoom / f; return; }
            if (zoom >= 1000.00) { MessageBox.Show("Maximum zoom reached.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information); zoom = zoom / f; return; }

            panOffset.X = (int)(e.X - (e.X - panOffset.X) * f);
            panOffset.Y = (int)(e.Y - (e.Y - panOffset.Y) * f);
            ImageCanvas.Invalidate();
        }

        private void AddRoi(RoiType type, bool isFullImageRoi = false)
        {
            if (currentImage == null)
            {
                MessageBox.Show("Please load an image first.", "information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int intDivisor_W = 0;
            int intDivisor_H = 0;

            switch (type)
            {
                case RoiType.Text:
                    DisplaySelectInfo(DisplayInfo.TextROISelection);
                    intDivisor_W = 6;
                    intDivisor_H = 10;
                    break;
                case RoiType.Barcode:
                    DisplaySelectInfo(DisplayInfo.BarcodeROISelection);
                    intDivisor_W = 4;
                    intDivisor_H = 2;
                    break;
                case RoiType.TemplateMatch:
                    intDivisor_W = 6;
                    intDivisor_H = 10;
                    DisplaySelectInfo(DisplayInfo.TemplateMatching);
                    break;
                default:
                    break;
            }

            int w = isFullImageRoi ? currentImage.Width : Math.Max(50, currentImage.Width / intDivisor_W);
            int h = isFullImageRoi ? currentImage.Height : Math.Max(30, currentImage.Height / intDivisor_H);

            SysPoint screenCenter = new SysPoint(ImageCanvas.Width / 2, ImageCanvas.Height / 2);
            OpenCvSharp.Point imgCenter = ScreenToImage(screenCenter);

            int IntId = rois.Any() ? rois.Max(P => P.Id) + 1 : 1;

            var roi = new RoiObject
            {
                Id = IntId,
                Name = "ROI " + IntId,
                Box = new CvRect(imgCenter.X - (w / 2), imgCenter.Y - (h / 2), w, h),
                Type = type,
                IsRunGS1QcCheck = IsGenerateReport,
            };

            CvRect safeBox = roi.Box;
            if (safeBox.X < 0) safeBox.X = 0;
            if (safeBox.Y < 0) safeBox.Y = 0;
            if (safeBox.Right > currentImage.Width) safeBox.Width = currentImage.Width - safeBox.X;
            if (safeBox.Bottom > currentImage.Height) safeBox.Height = currentImage.Height - safeBox.Y;
            roi.Box = safeBox;

            rois.Add(roi);
            selectedRoi = roi;
            ImageCanvas.Invalidate();
        }

        private void BtnAddRoi_Click(object sender, EventArgs e) { AddRoi(RoiType.Text); }

        private void BtnTrainSelROI_Click(object sender, EventArgs e)
        {
            if (currentImage == null || selectedRoi == null) return;

            if (selectedRoi.Type == RoiType.TemplateMatch)
            {
                TrainTmRoi(selectedRoi);
                return;
            }

            if (selectedRoi.Type != RoiType.Text) return;

            //CvRect safeBox = selectedRoi.Box.Intersect(new CvRect(0, 0, currentImage.Width, currentImage.Height));
            //if (safeBox.Width == 0 || safeBox.Height == 0) return;

            //using Mat crop = new Mat(currentImage, safeBox);
            //Mat rotatedCrop = null;
            //Mat deskewedCrop = null;
            //Mat trainingSourceImage = null;
            //Mat tempMorphedImage = null;

            //try
            //{
            //    OcrEngine.RotateImage(crop, out rotatedCrop, selectedRoi.RotationAngle);
            //    deskewedCrop = OcrEngine.DeskewImage(rotatedCrop, out double skewAngle);

            //    if (selectedRoi.MorphOp == MorphOperation.None)
            //    {
            //        trainingSourceImage = deskewedCrop;
            //    }
            //    else
            //    {
            //        tempMorphedImage = new Mat();
            //        using Mat tempGray = new Mat();
            //        if (deskewedCrop.Channels() == 3) Cv2.CvtColor(deskewedCrop, tempGray, ColorConversionCodes.BGR2GRAY);
            //        else deskewedCrop.CopyTo(tempGray);

            //        using Mat tempTh = new Mat();
            //        OcrEngine.ProcessImageForMode(tempGray, tempTh, selectedRoi.SegmentationMode);
            //        MorphologyProcessor.Apply(tempTh, selectedRoi.MorphOp, selectedRoi.MorphKernelWidth, selectedRoi.MorphKernelHeight, selectedRoi.MorphIterations);

            //        Cv2.BitwiseNot(tempTh, tempMorphedImage);
            //        trainingSourceImage = tempMorphedImage;
            //    }

            //    bool oldFilterState = OcrEngine.IsNeglectGarabageChar;
            //    OcrEngine.IsNeglectGarabageChar = false;

            //    var boxes = OcrEngine.GetCharacterBoxes(deskewedCrop, selectedRoi);

            //    OcrEngine.IsNeglectGarabageChar = oldFilterState;

            //    if (boxes.Count == 0)
            //    {
            //        MessageBox.Show("No blobs found. Try adjusting Min/Max Width and Height.");
            //        return;
            //    }

            //    List<Mat> crops = new List<Mat>();
            //    foreach (var b in boxes) crops.Add(new Mat(trainingSourceImage, b).Clone());

            //    using (var dlg = new TrainingForm(crops))
            //    {
            //        if (dlg.ShowDialog(this) == DialogResult.OK)
            //        {
            //            selectedRoi.JustTrained = true;
            //            OcrEngine.DecodeRoi(currentImage, selectedRoi);
            //            selectedRoi.ShowOverlay = false;
            //            RefreshRightPanel();
            //            ImageCanvas.Invalidate();
            //        }
            //    }

            //    foreach (var c in crops) c.Dispose();
            //}
            //finally
            //{
            //    if (rotatedCrop != null && !rotatedCrop.IsDisposed) rotatedCrop.Dispose();
            //    if (deskewedCrop != null && !deskewedCrop.IsDisposed) deskewedCrop.Dispose();
            //    if (tempMorphedImage != null && !tempMorphedImage.IsDisposed) tempMorphedImage.Dispose();
            //}

            List<Mat> crops = OcrEngine.TrainRoi(currentImage, selectedRoi);

            using (var dlg = new TrainingForm(crops))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    selectedRoi.JustTrained = true;
                    OcrEngine.DecodeRoi(currentImage, selectedRoi);
                    selectedRoi.ShowOverlay = false;
                    RefreshRightPanel();
                    ImageCanvas.Invalidate();
                }
            }
        }

        private void TrainTmRoi(RoiObject roi)
        {
            if (currentImage == null || roi == null) return;
            if (roi.Type != RoiType.TemplateMatch) return;
            TemplateManager.SaveTemplate(roi, currentImage);
            OcrEngine.DecodeRoi(currentImage, roi);
            RefreshRightPanel();
            ImageCanvas.Invalidate();
        }

        // ====================================================================
        // THE HIGH PERFORMANCE ENGINE LOOP
        // ====================================================================
        private void BtnDecodeAllROI_Click(object sender, EventArgs e)
        {
            if (currentImage == null) return;
            if (OcrEngine.TemplateVectors.Count == 0) OcrEngine.ReloadTemplates();

            totalTimeTaken = 0;
            _imgCount++;

            // 1. LATENCY LOCK
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;

            // 2. MATH
            foreach (var r in rois)
            {
                r.CharResults?.Clear();
                OcrEngine.DecodeRoi(currentImage, r);
                totalTimeTaken += r.TimeTakenForDecoding.TotalMilliseconds;
                //lstGs1Res.Add(r.Gs1QcResult);
            }

            // 3. LATENCY UNLOCK
            GCSettings.LatencyMode = GCLatencyMode.Interactive;

            // 4. FAST GC (C# Strings)
            GC.Collect(0, GCCollectionMode.Optimized, false);

            // 5. DEEP OPENCV FLUSH (Every 50 Loads)
            if (_imgCount % 50 == 0)
            {
                Task.Run(() =>
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                });
            }

            // 6. UI UPDATE
            Thread thread = new Thread(() => SetDataInDataGridView(rois));
            thread.Start();

            DisplaySelectInfo(DisplayInfo.TimeTaken);
            RefreshRightPanel();
            ImageCanvas.Invalidate();
        }

        private void BtnShowTamplate_Click(object sender, EventArgs e)
        {
            TamplatesForm ObjTamplatesForm = new();
            ObjTamplatesForm.Show();
        }

        private void BtnNextImage_Click(object sender, EventArgs e)
        {
            intCurrentSelectFileIndex += 1;
            if (intCurrentSelectFileIndex >= LstImageList.Items.Count)
            {
                MessageBox.Show("Reach the last image.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                intCurrentSelectFileIndex = LstImageList.Items.Count - 1;
                return;
            }
            LstImageList.SelectedIndex = intCurrentSelectFileIndex;
        }

        private void BtnPreviousImage_Click(object sender, EventArgs e)
        {
            intCurrentSelectFileIndex -= 1;
            if (intCurrentSelectFileIndex < 0)
            {
                MessageBox.Show("Reach the first image.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                intCurrentSelectFileIndex = 0;
                return;
            }
            LstImageList.SelectedIndex = intCurrentSelectFileIndex;
        }

        private void BtnAddBarCodeRoi_Click(object sender, EventArgs e) { AddRoi(RoiType.Barcode); }

        private void SplitConImageViewAndButton_SplitterMoved(object sender, SplitterEventArgs e)
        {
            //TblPnlImageList.ColumnStyles[0].Width = SplitConImageViewAndButton.SplitterDistance;
        }

        //public void OpenAnchorSettingWindow(RoiObject roiObject, ROIControl rOIControl)
        //{
        //    if (roiObject.IsAnchor)
        //    {
        //        OpenCvSharp.Mat previewPattern = null;
        //        if (currentImage != null)
        //        {
        //            using (OpenCvSharp.Mat temp = new OpenCvSharp.Mat(currentImage, roiObject.Box))
        //            {
        //                previewPattern = temp.Clone();
        //            }
        //        }

        //        using (var form = new AnchorForm(previewPattern,
        //            roiObject.AnchorTop == 0 ? 200 : roiObject.AnchorTop,
        //            roiObject.AnchorBottom == 0 ? 200 : roiObject.AnchorBottom,
        //            roiObject.AnchorLeft == 0 ? 200 : roiObject.AnchorLeft,
        //            roiObject.AnchorRight == 0 ? 200 : roiObject.AnchorRight,
        //            roiObject.Name))
        //        {
        //            if (form.ShowDialog() == DialogResult.OK)
        //            {
        //                var getAncSetting = form.GetAnchorSetting;
        //                roiObject.AnchorTop = getAncSetting.Top;
        //                roiObject.AnchorBottom = getAncSetting.Bottom;
        //                roiObject.AnchorLeft = getAncSetting.Left;
        //                roiObject.AnchorRight = getAncSetting.Right;
        //                roiObject.IsUseEdgeMatching = getAncSetting.IsEdgeBasedMatching;
        //                roiObject.IsHighPrecisionMode = getAncSetting.IsHighPrecisionMode;

        //                if (roiObject.AnchorTemplate != null) roiObject.AnchorTemplate.Dispose();
        //                roiObject.AnchorTemplate = previewPattern;

        //                MessageBox.Show($"{roiObject.Name} Anchor configured successfully!");
        //                ImageCanvas.Invalidate();
        //            }
        //            else
        //            {
        //                roiObject.IsAnchor = false;
        //                rOIControl.BindData(roiObject, true);
        //                if (previewPattern != null) previewPattern.Dispose();
        //            }
        //        }
        //    }
        //}

        public void OpenAnchorSettingWindow(RoiObject roiObject, IRoiControl rOIControl)
        {
            if (roiObject.IsAnchor)
            {
                OpenCvSharp.Mat previewPattern = null;
                if (currentImage != null)
                {
                    using (OpenCvSharp.Mat temp = new OpenCvSharp.Mat(currentImage, roiObject.Box))
                    {
                        previewPattern = temp.Clone();
                    }
                }

                using (var form = new AnchorForm(previewPattern,
                    roiObject.AnchorTop == 0 ? 200 : roiObject.AnchorTop,
                    roiObject.AnchorBottom == 0 ? 200 : roiObject.AnchorBottom,
                    roiObject.AnchorLeft == 0 ? 200 : roiObject.AnchorLeft,
                    roiObject.AnchorRight == 0 ? 200 : roiObject.AnchorRight,
                    roiObject.Name))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        var getAncSetting = form.GetAnchorSetting;
                        roiObject.AnchorTop = getAncSetting.Top;
                        roiObject.AnchorBottom = getAncSetting.Bottom;
                        roiObject.AnchorLeft = getAncSetting.Left;
                        roiObject.AnchorRight = getAncSetting.Right;
                        roiObject.IsUseEdgeMatching = getAncSetting.IsEdgeBasedMatching;
                        roiObject.IsHighPrecisionMode = getAncSetting.IsHighPrecisionMode;

                        if (roiObject.AnchorTemplate != null) roiObject.AnchorTemplate.Dispose();
                        roiObject.AnchorTemplate = previewPattern;

                        MessageBox.Show($"{roiObject.Name} Anchor configured successfully!");
                        ImageCanvas.Invalidate();
                    }
                    else
                    {
                        roiObject.IsAnchor = false;
                        rOIControl.BindData(roiObject, true);
                        if (previewPattern != null) previewPattern.Dispose();
                    }
                }
            }
        }

        private void UpdateSidePanelPreview(IRoiControl targetCtrl, RoiObject targetRoi)
        {
            if (currentImage == null || targetCtrl == null || targetRoi == null) return;
            using Mat previewMat = OcrEngine.GenerateRoiControlPreview(currentImage, targetRoi);
            if (previewMat != null) targetCtrl.SetPreviewImage(previewMat.ToBitmap());
        }

        private void AlignRois()
        {
            if (currentImage == null) return;

            Dictionary<int, OpenCvSharp.Point> referenceShifts = new Dictionary<int, OpenCvSharp.Point>();

            foreach (var roi in rois)
            {
                if (!roi.IsAnchor) continue;
                if (roi.IsUseRoiReference) continue;

                OpenCvSharp.Point roiTempPoint = new OpenCvSharp.Point(0, 0);
                bool matchFound = false;

                try
                {
                    int startX = Math.Max(0, roi.Box.X - roi.AnchorLeft);
                    int startY = Math.Max(0, roi.Box.Y - roi.AnchorTop);
                    int endX = Math.Min(currentImage.Width, roi.Box.X + roi.Box.Width + roi.AnchorRight);
                    int endY = Math.Min(currentImage.Height, roi.Box.Y + roi.Box.Height + roi.AnchorBottom);
                    int searchW = endX - startX;
                    int searchH = endY - startY;

                    if (searchW <= 0 || searchH <= 0) continue;
                    CvRect searchRect = new CvRect(startX, startY, searchW, searchH);

                    if (roi.Type == RoiType.Barcode)
                    {
                        OpenCvSharp.Point foundCenter = OcrEngine.FindAnyBarcodeCenter(currentImage, searchRect);
                        if (foundCenter.X != -1)
                        {
                            int absFoundX = startX + foundCenter.X;
                            int absFoundY = startY + foundCenter.Y;
                            int originalCenterX = roi.Box.X + (roi.Box.Width / 2);
                            int originalCenterY = roi.Box.Y + (roi.Box.Height / 2);

                            roiTempPoint = new OpenCvSharp.Point(absFoundX - originalCenterX, absFoundY - originalCenterY);
                            matchFound = true;
                        }
                    }
                    else
                    {
                        if (roi.AnchorTemplate == null || roi.AnchorTemplate.Empty()) continue;
                        if (searchW < roi.AnchorTemplate.Width || searchH < roi.AnchorTemplate.Height) continue;

                        OpenCvSharp.Point matchLoc = OcrEngine.FindMatchLocation(currentImage, roi.AnchorTemplate, searchRect, roi.IsUseEdgeMatching);

                        if (matchLoc.X != -1 && matchLoc.Y != -1)
                        {
                            int foundAbsX = startX + matchLoc.X;
                            int foundAbsY = startY + matchLoc.Y;
                            roiTempPoint = new OpenCvSharp.Point(foundAbsX - roi.Box.X, foundAbsY - roi.Box.Y);
                            matchFound = true;
                        }
                    }
                }
                catch { }

                if (matchFound) referenceShifts.Add(roi.Id, roiTempPoint);
            }

            foreach (var roi in rois)
            {
                int applyDx = 0;
                int applyDy = 0;
                bool shouldMove = false;

                if (roi.IsAnchor && referenceShifts.ContainsKey(roi.Id) && !roi.IsUseRoiReference)
                {
                    applyDx = referenceShifts[roi.Id].X;
                    applyDy = referenceShifts[roi.Id].Y;
                    shouldMove = true;
                }
                else if (referenceShifts.ContainsKey(roi.ReferenceRoiID) && roi.IsUseRoiReference)
                {
                    applyDx = referenceShifts[roi.ReferenceRoiID].X;
                    applyDy = referenceShifts[roi.ReferenceRoiID].Y;
                    shouldMove = true;
                }

                if (shouldMove)
                {
                    CvRect r = roi.Box;
                    r.X += applyDx;
                    r.Y += applyDy;
                    // UNCLAMPED: Boxes can go negative to preserve group geometry
                    roi.Box = r;
                }
            }
            ImageCanvas.Invalidate();
        }

        private void BtnSaveRoi_Click(object sender, EventArgs e)
        {
            if (rois.Count == 0)
            {
                MessageBox.Show("No ROIs to save.");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "OCR Config|*.json";
                sfd.FileName = "RoiConfig.json";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var dataToSave = new List<RoiDataTransfer>();
                        foreach (var roi in rois) dataToSave.Add(ROIConverter.FromLogic(roi));

                        var wrapper = new RoiConfigWrapper
                        {
                            OriginalImageWidth = currentImage?.Width ?? 1,
                            OriginalImageHeight = currentImage?.Height ?? 1,
                            Rois = dataToSave
                        };

                        var options = new JsonSerializerOptions { WriteIndented = true };
                        string jsonString = JsonSerializer.Serialize(wrapper, options); // FIX: Serialized 'wrapper', not 'dataToSave'
                        System.IO.File.WriteAllText(sfd.FileName, jsonString);

                        MessageBox.Show($"Successfully saved {rois.Count} ROIs.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving file: " + ex.Message);
                    }
                }
            }
            DisplaySelectInfo(DisplayInfo.ROISave);
        }

        private void BtnLoadRoi_Click(object sender, EventArgs e)
        {
            if (currentImage == null)
            {
                MessageBox.Show("Please load an image first before loading ROIs.");
                return;
            }

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "OCR Config|*.json";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string jsonString = System.IO.File.ReadAllText(ofd.FileName);
                        List<RoiDataTransfer> loadedDtos = null;
                        int savedWidth = currentImage.Width;
                        int savedHeight = currentImage.Height;

                        try
                        {
                            var wrapper = JsonSerializer.Deserialize<RoiConfigWrapper>(jsonString);
                            if (wrapper != null && wrapper.Rois != null)
                            {
                                loadedDtos = wrapper.Rois;
                                if (wrapper.OriginalImageWidth > 0) savedWidth = wrapper.OriginalImageWidth;
                                if (wrapper.OriginalImageHeight > 0) savedHeight = wrapper.OriginalImageHeight;
                            }
                        }
                        catch
                        {
                            loadedDtos = JsonSerializer.Deserialize<List<RoiDataTransfer>>(jsonString);
                        }

                        if (loadedDtos == null || loadedDtos.Count == 0) return;

                        double scaleX = (double)currentImage.Width / savedWidth;
                        double scaleY = (double)currentImage.Height / savedHeight;

                        foreach (var r in rois) if (r.AnchorTemplate != null) r.AnchorTemplate.Dispose();
                        rois.Clear();
                        selectedRoi = null;

                        foreach (var dto in loadedDtos)
                        {
                            var logicRoi = ROIConverter.ToLogic(dto);

                            if (scaleX != 1.0 || scaleY != 1.0)
                            {
                                int newX = (int)(logicRoi.Box.X * scaleX);
                                int newY = (int)(logicRoi.Box.Y * scaleY);
                                int newW = (int)(logicRoi.Box.Width * scaleX);
                                int newH = (int)(logicRoi.Box.Height * scaleY);
                                logicRoi.Box = new CvRect(newX, newY, newW, newH);

                                logicRoi.AnchorTop = (int)(logicRoi.AnchorTop * scaleY);
                                logicRoi.AnchorBottom = (int)(logicRoi.AnchorBottom * scaleY);
                                logicRoi.AnchorLeft = (int)(logicRoi.AnchorLeft * scaleX);
                                logicRoi.AnchorRight = (int)(logicRoi.AnchorRight * scaleX);

                                if (logicRoi.AnchorTemplate != null && !logicRoi.AnchorTemplate.Empty())
                                {
                                    Mat resizedTemplate = new Mat();
                                    Cv2.Resize(logicRoi.AnchorTemplate, resizedTemplate,
                                        new OpenCvSharp.Size(logicRoi.AnchorTemplate.Width * scaleX, logicRoi.AnchorTemplate.Height * scaleY),
                                        0, 0, InterpolationFlags.Cubic);
                                    logicRoi.AnchorTemplate.Dispose();
                                    logicRoi.AnchorTemplate = resizedTemplate;
                                }
                            }
                            rois.Add(logicRoi);
                        }

                        if (rois.Any(r => r.IsAnchor)) AlignRois();

                        selectedRoi = rois.LastOrDefault();
                        RefreshRightPanel();
                        ImageCanvas.Invalidate();

                        MessageBox.Show($"Loaded {rois.Count} ROIs successfully.");
                        DisplaySelectInfo(DisplayInfo.LoadROI);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading file: " + ex.Message);
                    }
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //if (ActiveControl is TextBox || ActiveControl is ROIControl || ActiveControl is NumericUpDown || (ActiveControl is ComboBox c && c.DroppedDown))
            //{
            //    return base.ProcessCmdKey(ref msg, keyData);
            //}

            if (ActiveControl is TextBox || ActiveControl is IRoiControl || ActiveControl is NumericUpDown || (ActiveControl is ComboBox c && c.DroppedDown))
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }

            if (keyData == (Keys.Control | Keys.C))
            {
                CopyROI();
                return true;
            }

            if (keyData == (Keys.Control | Keys.V))
            {
                System.Drawing.Point clientPoint = ImageCanvas.PointToClient(Cursor.Position);
                if (ImageCanvas.ClientRectangle.Contains(clientPoint))
                {
                    PasteROI(clientPoint);
                    return true;
                }
            }

            if (keyData == (Keys.Control | Keys.D) || keyData == Keys.Delete)
            {
                DeletROI(selectedRoi);
                return true;
            }

            if (keyData == (Keys.Control | Keys.A))
            {
                PerformSelectAll();
                DisplaySelectInfo(DisplayInfo.AllSelectionMode);
                return true;
            }

            if (keyData == Keys.F1)
            {
                OpenDocument.Open();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void DisplaySelectInfo(DisplayInfo displayInfo)
        {
            toolStripMenu.Text = displayInfo switch
            {
                DisplayInfo.TextROISelection => "Text ROI mode: click and drag on canvas to change size of ROI.",
                DisplayInfo.BarcodeROISelection => "Barcode ROI mode : click and drag canvas to draw and change barcode ROI.",
                DisplayInfo.TimeTaken => $"Decode ROI in {totalTimeTaken} ms",
                DisplayInfo.AllSelectionMode => $"ROI selection mode : All ROIs are selected.",
                DisplayInfo.ImageCounting => $"Total {LstImageList.Items.Count} images are selected.",
                DisplayInfo.CurrentImageIndex => $"Current image index: {intCurrentSelectFileIndex + 1} of {LstImageList.Items.Count}",
                DisplayInfo.ImagePanning => $"Image Panning mode: move and zoom image on available canvas area.",
                DisplayInfo.ROISave => $"ROI configuration is saved successfully.Total No. of saved ROI : {rois.Count}",
                DisplayInfo.LoadROI => $"ROI configuration is loaded successfully.Total No. of loaded ROI : {rois.Count}",
                DisplayInfo.SingleSelectionMode => $"ROI selection mode : {selectedRoi?.Name} is selected.",
                _ => "Ready"
            };
        }

        private void BtnAddTempateROI_Click(object sender, EventArgs e) { AddRoi(RoiType.TemplateMatch); }
        private void CmbSegments_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedRoi == null) return;
            selectedRoi.SegmentationMode = (SegmentationMode)CmbSegments.SelectedItem;
        }

        //private void UpdateSidePanelPreview(ROIControl targetCtrl, RoiObject targetRoi)
        //{
        //    if (currentImage == null || targetCtrl == null || targetRoi == null) return;
        //    using Mat previewMat = OcrEngine.GenerateRoiControlPreview(currentImage, targetRoi);
        //    if (previewMat != null) targetCtrl.SetPreviewImage(previewMat.ToBitmap());
        //}

        //private void UpdateSidePanelPreview(ROIControlBarCode targetCtrl, RoiObject targetRoi)
        //{
        //    if (currentImage == null || targetCtrl == null || targetRoi == null) return;
        //    using Mat previewMat = OcrEngine.GenerateRoiControlPreview(currentImage, targetRoi);
        //    if (previewMat != null) targetCtrl.SetPreviewImage(previewMat.ToBitmap());
        //}

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_displayBitmap != null) _displayBitmap.Dispose();
            Environment.Exit(0);
        }

        private void ToolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            OpenDocument.Open();
        }

        private void TsslUserManual_Click(object sender, EventArgs e)
        {
            //open document
            string DocPath = ConfigurationManager.AppSettings["UserManualPath"] ?? $"{AppDomain.CurrentDomain.BaseDirectory}Documents\\UserManual.html";
            OpenDocument.Open(DocPath);
        }

        private void PictureStorageToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void HideMenuoolStripMenuItem_Click(object sender, EventArgs e)
        {
            SplitConImageViewAndButton.Panel1Collapsed = !SplitConImageViewAndButton.Panel1Collapsed;
            hideMenuoolStripMenuItem.Text = SplitConImageViewAndButton.Panel1Collapsed ? "Show Menu" : "Hide Menu"; //change t
        }

        private void HideResultWinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitConBodyMain.Panel2Collapsed = !splitConBodyMain.Panel2Collapsed;
            hideResultWinToolStripMenuItem.Text = splitConBodyMain.Panel2Collapsed ? "Show Result Window" : "Hide Result Window"; //change text
        }

        private void HideGridRecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SplitContainerImageView.Panel2Collapsed = !SplitContainerImageView.Panel2Collapsed;
            hideGridRecToolStripMenuItem.Text = SplitContainerImageView.Panel2Collapsed ? "Show Grid Records" : "Hide Grid Records"; //change text
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            new FrmGradingSetting(selectedRoi).Show();
        }
    }
}