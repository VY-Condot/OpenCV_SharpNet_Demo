using MvCamCtrl.NET;
using OpenCV_SharpNet.Enums;
using OpenCV_SharpNet.Models;
using OpenCV_SharpNet_Demo.Services;
using OpenCV_SharpNet_Demo.UI;
using OpenCV_SharpNet_Demo.UserControls;
using OpenCV_SharpNet.Models.GS1_QC;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.ComponentModel;
using System.Configuration;
using System.Drawing.Imaging;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using CvRect = OpenCvSharp.Rect;
using Point = System.Drawing.Point;

// Aliases
using SysPoint = System.Drawing.Point;
using SysRect = System.Drawing.Rectangle;
using OpenCV_SharpNet.Interfaces;
using OpenCV_SharpNet.Enums;
using OpenCV_SharpNet.Services;
using OpenCV_SharpNet.Interfaces;

namespace OpenCV_SharpNet_Demo
{
    public partial class CameraDemo : Form
    {
        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        public const Int32 CUSTOMER_PIXEL_FORMAT = unchecked((Int32)0x80000000);

        MyCamera.MV_CC_DEVICE_INFO_LIST m_stDeviceList = new MyCamera.MV_CC_DEVICE_INFO_LIST();
        private MyCamera m_MyCamera = new MyCamera();
        bool m_bGrabbing = false;
        Thread m_hReceiveThread = null;
        MyCamera.MV_FRAME_OUT_INFO_EX m_stFrameInfo = new MyCamera.MV_FRAME_OUT_INFO_EX();

        UInt32 m_nBufSizeForDriver = 0;
        IntPtr m_BufForDriver = IntPtr.Zero;
        private static Object BufForDriverLock = new Object();

        Bitmap m_bitmap = null;
        PixelFormat m_bitmapPixelFormat = PixelFormat.DontCare;
        IntPtr m_ConvertDstBuf = IntPtr.Zero;
        UInt32 m_nConvertDstBufLen = 0;

        IntPtr displayHandle = IntPtr.Zero;
        private delegate void listresult2(Int32 ErrorCode);
        private listresult2 ListResult2;
        public Bitmap CaptureImage = null;

        //// Core Data
        private Mat currentImage;
        private List<RoiObject> rois = new List<RoiObject>();

        // ====================================================================
        // HIGH PERFORMANCE UX VARIABLES
        // ====================================================================
        private Bitmap _displayBitmap = null; // Caches the image so UI doesn't lag
        private double _smoothedExecutionTime = 0; // EMA Smoother for stable UI numbers
        Int64 ImgCount = 0; // Tracks frames for the OpenCV GC Relief Valve

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

        public CameraDemo()
        {
            InitializeComponent();
            FlowPnlRoiData.DoubleBuffered(true);
            ImageCanvas.MouseWheel += ImageCanvas_MouseWheel;
        }

        // ====================================================================
        // NEW SAFE IMAGE SETTER (Prevents Memory Leaks & UI Lag)
        // ====================================================================
        //private void SetCurrentImage(Mat newMat)
        //{
        //    if (currentImage != null && currentImage != newMat) currentImage.Dispose();
        //    currentImage = newMat;

        //    if (_displayBitmap != null) _displayBitmap.Dispose();

        //    if (currentImage != null && !currentImage.Empty())
        //    {
        //        _displayBitmap = BitmapConverter.ToBitmap(currentImage);
        //    }
        //}

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

        private void CameraDemo_Load(object sender, EventArgs e)
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
            SetCtrlWhenClose();

            MyCamera.MV_CC_Initialize_NET();
            DeviceListAcq();
            ListResult2 = ShowList2;
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

            var scrollControl = FlowPnlRoiData.Controls.Cast<IRoiControl>().FirstOrDefault(x => x.BoundedROI == selectedRoi);
            if (scrollControl != null) FlowPnlRoiData.ScrollControlIntoView((Control)scrollControl);
        }

        private void UpdateRoiSelectionVisuals()
        {
            for (int i = 0; i < FlowPnlRoiData.Controls.Count; i++)
            {
                if (FlowPnlRoiData.Controls[i] is IRoiControl ctrl)
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
                    //SetDataInDataGridView(rois);

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
                            //string strTempChar = (IntCharCounter < roi.ExpectedText?.Length) ? roi.ExpectedText[IntCharCounter].ToString() : string.Empty;
                            //if (IntCharCounter < roi.ExpectedText?.Length) IntCharCounter++;

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

            if (currentMode == MouseMode.PanningImage || m_bGrabbing)
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
            else if ((currentMode == MouseMode.ResizingRoi && selectedRoi != null) || m_bGrabbing)
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
                IsRunGS1QcCheck = IsGenerateReport
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

            CvRect safeBox = selectedRoi.Box.Intersect(new CvRect(0, 0, currentImage.Width, currentImage.Height));
            if (safeBox.Width == 0 || safeBox.Height == 0) return;

            using Mat crop = new Mat(currentImage, safeBox);
            Mat rotatedCrop = null;
            Mat deskewedCrop = null;
            Mat trainingSourceImage = null;
            Mat tempMorphedImage = null;

            try
            {
                OcrEngine.RotateImage(crop, out rotatedCrop, selectedRoi.RotationAngle);
                deskewedCrop = OcrEngine.DeskewImage(rotatedCrop, out double skewAngle);

                if (selectedRoi.MorphOp == MorphOperation.None)
                {
                    trainingSourceImage = deskewedCrop;
                }
                else
                {
                    tempMorphedImage = new Mat();
                    using Mat tempGray = new Mat();
                    if (deskewedCrop.Channels() == 3) Cv2.CvtColor(deskewedCrop, tempGray, ColorConversionCodes.BGR2GRAY);
                    else deskewedCrop.CopyTo(tempGray);

                    using Mat tempTh = new Mat();
                    OcrEngine.ProcessImageForMode(tempGray, tempTh, selectedRoi.SegmentationMode);
                    MorphologyProcessor.Apply(tempTh, selectedRoi.MorphOp, selectedRoi.MorphKernelWidth, selectedRoi.MorphKernelHeight, selectedRoi.MorphIterations);

                    Cv2.BitwiseNot(tempTh, tempMorphedImage);
                    trainingSourceImage = tempMorphedImage;
                }

                bool oldFilterState = OcrEngine.IsNeglectGarabageChar;
                OcrEngine.IsNeglectGarabageChar = false;

                var boxes = OcrEngine.GetCharacterBoxes(deskewedCrop, selectedRoi);

                OcrEngine.IsNeglectGarabageChar = oldFilterState;

                if (boxes.Count == 0)
                {
                    MessageBox.Show("No blobs found. Try adjusting Min/Max Width and Height.");
                    return;
                }

                List<Mat> crops = new List<Mat>();
                foreach (var b in boxes) crops.Add(new Mat(trainingSourceImage, b).Clone());

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

                foreach (var c in crops) c.Dispose();
            }
            finally
            {
                if (rotatedCrop != null && !rotatedCrop.IsDisposed) rotatedCrop.Dispose();
                if (deskewedCrop != null && !deskewedCrop.IsDisposed) deskewedCrop.Dispose();
                if (tempMorphedImage != null && !tempMorphedImage.IsDisposed) tempMorphedImage.Dispose();
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

            // 1. LATENCY LOCK
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;

            // 2. MATH
            foreach (var r in rois)
            {
                r.CharResults?.Clear();
                OcrEngine.DecodeRoi(currentImage, r);
                totalTimeTaken += r.TimeTakenForDecoding.TotalMilliseconds;
            }

            // 3. LATENCY UNLOCK
            GCSettings.LatencyMode = GCLatencyMode.Interactive;

            // 4. FAST GC (C# Strings)
            GC.Collect(0, GCCollectionMode.Optimized, false);

            // 5. DEEP OPENCV FLUSH (Every 50 Loads - uses existing ImgCount)
            if (ImgCount % 50 == 0)
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
                        string jsonString = JsonSerializer.Serialize(wrapper, options);
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
            if (ActiveControl is TextBox || ActiveControl is IRoiControl || ActiveControl is NumericUpDown || (ActiveControl is ComboBox c && c.DroppedDown))
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }

            if (keyData == (Keys.Control | Keys.C)) { CopyROI(); return true; }
            if (keyData == (Keys.Control | Keys.V))
            {
                System.Drawing.Point clientPoint = ImageCanvas.PointToClient(Cursor.Position);
                if (ImageCanvas.ClientRectangle.Contains(clientPoint)) { PasteROI(clientPoint); return true; }
            }

            if (keyData == (Keys.Control | Keys.D) || keyData == Keys.Delete) { DeletROI(selectedRoi); return true; }
            if (keyData == (Keys.Control | Keys.A)) { PerformSelectAll(); DisplaySelectInfo(DisplayInfo.AllSelectionMode); return true; }

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
        private void UpdateSidePanelPreview(IRoiControl targetCtrl, RoiObject targetRoi)
        {
            if (currentImage == null || targetCtrl == null || targetRoi == null) return;
            using Mat previewMat = OcrEngine.GenerateRoiControlPreview(currentImage, targetRoi);
            if (previewMat != null) targetCtrl.SetPreviewImage(previewMat.ToBitmap());
        }

        private void BnEnum_Click(object sender, EventArgs e) { DeviceListAcq(); }

        private void DeviceListAcq()
        {
            System.GC.Collect();
            cbDeviceList.Items.Clear();
            m_stDeviceList.nDeviceNum = 0;
            int nRet = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE | MyCamera.MV_GENTL_GIGE_DEVICE
                | MyCamera.MV_GENTL_CAMERALINK_DEVICE | MyCamera.MV_GENTL_CXP_DEVICE | MyCamera.MV_GENTL_XOF_DEVICE, ref m_stDeviceList);
            if (0 != nRet) { ShowErrorMsg("Enumerate devices fail!", 0); return; }

            for (int i = 0; i < m_stDeviceList.nDeviceNum; i++)
            {
                MyCamera.MV_CC_DEVICE_INFO device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_stDeviceList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));
                string strUserDefinedName = "";
                if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                {
                    MyCamera.MV_GIGE_DEVICE_INFO_EX gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO_EX)MyCamera.ByteToStruct(device.SpecialInfo.stGigEInfo, typeof(MyCamera.MV_GIGE_DEVICE_INFO_EX));
                    if ((gigeInfo.chUserDefinedName.Length > 0) && (gigeInfo.chUserDefinedName[0] != '\0'))
                    {
                        strUserDefinedName = MyCamera.IsTextUTF8(gigeInfo.chUserDefinedName) ? Encoding.UTF8.GetString(gigeInfo.chUserDefinedName).TrimEnd('\0') : Encoding.Default.GetString(gigeInfo.chUserDefinedName).TrimEnd('\0');
                        cbDeviceList.Items.Add("GEV: " + DeleteTail(strUserDefinedName) + " (" + gigeInfo.chSerialNumber + ")");
                    }
                    else cbDeviceList.Items.Add("GEV: " + gigeInfo.chManufacturerName + " " + gigeInfo.chModelName + " (" + gigeInfo.chSerialNumber + ")");
                }
                else if (device.nTLayerType == MyCamera.MV_USB_DEVICE)
                {
                    MyCamera.MV_USB3_DEVICE_INFO_EX usbInfo = (MyCamera.MV_USB3_DEVICE_INFO_EX)MyCamera.ByteToStruct(device.SpecialInfo.stUsb3VInfo, typeof(MyCamera.MV_USB3_DEVICE_INFO_EX));
                    if ((usbInfo.chUserDefinedName.Length > 0) && (usbInfo.chUserDefinedName[0] != '\0'))
                    {
                        strUserDefinedName = MyCamera.IsTextUTF8(usbInfo.chUserDefinedName) ? Encoding.UTF8.GetString(usbInfo.chUserDefinedName).TrimEnd('\0') : Encoding.Default.GetString(usbInfo.chUserDefinedName).TrimEnd('\0');
                        cbDeviceList.Items.Add("U3V: " + DeleteTail(strUserDefinedName) + " (" + usbInfo.chSerialNumber + ")");
                    }
                    else cbDeviceList.Items.Add("U3V: " + usbInfo.chManufacturerName + " " + usbInfo.chModelName + " (" + usbInfo.chSerialNumber + ")");
                }
            }

            if (m_stDeviceList.nDeviceNum != 0) cbDeviceList.SelectedIndex = 0;
        }

        private void SetCtrlWhenOpen()
        {
            bnOpen.Enabled = false;
            bnClose.Enabled = true;
            bnStartGrab.Enabled = true;
            bnStopGrab.Enabled = false;
            bnContinuesMode.Enabled = true;
            bnContinuesMode.Checked = true;
            bnTriggerMode.Enabled = true;
            cbSoftTrigger.Enabled = false;
            bnTriggerExec.Enabled = false;
            tbExposure.Enabled = true;
            tbGain.Enabled = true;
            tbFrameRate.Enabled = true;
            bnGetParam.Enabled = true;
            bnSetParam.Enabled = true;
        }

        private void BnOpen_Click(object sender, EventArgs e)
        {
            if (m_stDeviceList.nDeviceNum == 0 || cbDeviceList.SelectedIndex == -1) { ShowErrorMsg("No device, please select", 0); return; }

            MyCamera.MV_CC_DEVICE_INFO device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_stDeviceList.pDeviceInfo[cbDeviceList.SelectedIndex], typeof(MyCamera.MV_CC_DEVICE_INFO));

            if (null == m_MyCamera)
            {
                m_MyCamera = new MyCamera();
                if (null == m_MyCamera) { ShowErrorMsg("Applying resource fail!", MyCamera.MV_E_RESOURCE); return; }
            }

            int nRet = m_MyCamera.MV_CC_CreateDevice_NET(ref device);
            if (MyCamera.MV_OK != nRet) { ShowErrorMsg("Create device fail!", nRet); return; }

            nRet = m_MyCamera.MV_CC_OpenDevice_NET();
            if (MyCamera.MV_OK != nRet) { m_MyCamera.MV_CC_DestroyDevice_NET(); ShowErrorMsg("Device open fail!", nRet); return; }

            if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
            {
                int nPacketSize = m_MyCamera.MV_CC_GetOptimalPacketSize_NET();
                if (nPacketSize > 0)
                {
                    nRet = m_MyCamera.MV_CC_SetIntValueEx_NET("GevSCPSPacketSize", nPacketSize);
                    if (nRet != MyCamera.MV_OK) ShowErrorMsg("Set Packet Size failed!", nRet);
                }
                else ShowErrorMsg("Get Packet Size failed!", nPacketSize);
            }

            m_MyCamera.MV_CC_SetEnumValue_NET("AcquisitionMode", (uint)MyCamera.MV_CAM_ACQUISITION_MODE.MV_ACQ_MODE_CONTINUOUS);
            m_MyCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
            BnGetParam_Click(null, null);
            SetCtrlWhenOpen();
        }

        private void SetCtrlWhenClose()
        {
            bnOpen.Enabled = true;
            bnClose.Enabled = false;
            bnStartGrab.Enabled = false;
            bnStopGrab.Enabled = false;
            bnContinuesMode.Enabled = false;
            bnTriggerMode.Enabled = false;
            cbSoftTrigger.Enabled = false;
            bnTriggerExec.Enabled = false;
            //bnSaveBmp.Enabled = false;
            //bnSaveJpg.Enabled = false;
            //bnSaveTiff.Enabled = false;
            //bnSavePng.Enabled = false;
            tbExposure.Enabled = false;
            tbGain.Enabled = false;
            tbFrameRate.Enabled = false;
            bnGetParam.Enabled = false;
            bnSetParam.Enabled = false;
        }

        private void BnClose_Click(object sender, EventArgs e)
        {
            m_bGrabbing = false;

            if (m_hReceiveThread != null)
            {
                if (!m_hReceiveThread.Join(1000)) { }
                m_hReceiveThread = null;
            }

            if (m_BufForDriver != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(m_BufForDriver);
                m_BufForDriver = IntPtr.Zero;
            }

            if (m_MyCamera != null)
            {
                m_MyCamera.MV_CC_CloseDevice_NET();
                m_MyCamera.MV_CC_DestroyDevice_NET();
            }

            SetCtrlWhenClose();
        }

        private void BnContinuesMode_CheckedChanged(object sender, EventArgs e)
        {
            if (bnContinuesMode.Checked)
            {
                m_MyCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
                cbSoftTrigger.Enabled = false;
                bnTriggerExec.Enabled = false;
            }
        }

        private void BnTriggerMode_CheckedChanged(object sender, EventArgs e)
        {
            if (bnTriggerMode.Checked)
            {
                m_MyCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MyCamera.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON);
                if (cbSoftTrigger.Checked)
                {
                    m_MyCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MyCamera.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE);
                    if (m_bGrabbing) bnTriggerExec.Enabled = true;
                }
                else m_MyCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MyCamera.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE0);
                cbSoftTrigger.Enabled = true;
            }
        }

        private void SetCtrlWhenStartGrab()
        {
            bnStartGrab.Enabled = false;
            bnStopGrab.Enabled = true;
            if (bnTriggerMode.Checked && cbSoftTrigger.Checked) bnTriggerExec.Enabled = true;
            //bnSaveBmp.Enabled = true;
            //bnSaveJpg.Enabled = true;
            //bnSaveTiff.Enabled = true;
            //bnSavePng.Enabled = true;
        }

        private Boolean IsMono(UInt32 enPixelType)
        {
            switch (enPixelType)
            {
                case (UInt32)MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono1p:
                case (UInt32)MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono2p:
                case (UInt32)MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono4p:
                case (UInt32)MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8:
                case (UInt32)MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8_Signed:
                case (UInt32)MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono10:
                case (UInt32)MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono10_Packed:
                case (UInt32)MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono12:
                case (UInt32)MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono12_Packed:
                case (UInt32)MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono14:
                case (UInt32)MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono16:
                    return true;
                default:
                    return false;
            }
        }

        private Int32 NecessaryOperBeforeGrab()
        {
            MyCamera.MVCC_INTVALUE_EX stWidth = new MyCamera.MVCC_INTVALUE_EX();
            int nRet = m_MyCamera.MV_CC_GetIntValueEx_NET("Width", ref stWidth);
            if (MyCamera.MV_OK != nRet) { ShowErrorMsg("Get Width Info Fail!", nRet); return nRet; }

            MyCamera.MVCC_INTVALUE_EX stHeight = new MyCamera.MVCC_INTVALUE_EX();
            nRet = m_MyCamera.MV_CC_GetIntValueEx_NET("Height", ref stHeight);
            if (MyCamera.MV_OK != nRet) { ShowErrorMsg("Get Height Info Fail!", nRet); return nRet; }

            MyCamera.MVCC_ENUMVALUE stPixelFormat = new MyCamera.MVCC_ENUMVALUE();
            nRet = m_MyCamera.MV_CC_GetEnumValue_NET("PixelFormat", ref stPixelFormat);
            if (MyCamera.MV_OK != nRet) { ShowErrorMsg("Get Pixel Format Fail!", nRet); return nRet; }

            if ((Int32)MyCamera.MvGvspPixelType.PixelType_Gvsp_Undefined == (Int32)stPixelFormat.nCurValue)
            {
                ShowErrorMsg("Unknown Pixel Format!", MyCamera.MV_E_UNKNOW);
                return MyCamera.MV_E_UNKNOW;
            }
            else if (IsMono(stPixelFormat.nCurValue))
            {
                m_bitmapPixelFormat = PixelFormat.Format8bppIndexed;
                if (IntPtr.Zero != m_ConvertDstBuf)
                {
                    Marshal.FreeHGlobal(m_ConvertDstBuf);
                    m_ConvertDstBuf = IntPtr.Zero;
                }

                m_nConvertDstBufLen = (UInt32)(stWidth.nCurValue * stHeight.nCurValue);
                m_ConvertDstBuf = Marshal.AllocHGlobal((Int32)m_nConvertDstBufLen);
                if (IntPtr.Zero == m_ConvertDstBuf) { ShowErrorMsg("Malloc Memory Fail!", MyCamera.MV_E_RESOURCE); return MyCamera.MV_E_RESOURCE; }
            }
            else
            {
                m_bitmapPixelFormat = PixelFormat.Format24bppRgb;
                if (IntPtr.Zero != m_ConvertDstBuf)
                {
                    Marshal.FreeHGlobal(m_ConvertDstBuf);
                    m_ConvertDstBuf = IntPtr.Zero;
                }

                m_nConvertDstBufLen = (UInt32)(3 * stWidth.nCurValue * stHeight.nCurValue);
                m_ConvertDstBuf = Marshal.AllocHGlobal((Int32)m_nConvertDstBufLen);
                if (IntPtr.Zero == m_ConvertDstBuf) { ShowErrorMsg("Malloc Memory Fail!", MyCamera.MV_E_RESOURCE); return MyCamera.MV_E_RESOURCE; }
            }

            if (null != m_bitmap)
            {
                m_bitmap.Dispose();
                m_bitmap = null;
            }
            m_bitmap = new Bitmap((Int32)stWidth.nCurValue, (Int32)stHeight.nCurValue, m_bitmapPixelFormat);

            if (PixelFormat.Format8bppIndexed == m_bitmapPixelFormat)
            {
                ColorPalette palette = m_bitmap.Palette;
                for (int i = 0; i < palette.Entries.Length; i++) palette.Entries[i] = Color.FromArgb(i, i, i);
                m_bitmap.Palette = palette;
            }

            return MyCamera.MV_OK;
        }

        private void BnStartGrab_Click(object sender, EventArgs e)
        {
            int nRet = NecessaryOperBeforeGrab();
            if (MyCamera.MV_OK != nRet) return;

            displayHandle = ImageCanvas.Handle;
            m_bGrabbing = true;

            m_stFrameInfo.nFrameLen = 0;
            m_stFrameInfo.enPixelType = MyCamera.MvGvspPixelType.PixelType_Gvsp_Undefined;

            m_hReceiveThread = new Thread(ReceiveThreadProcess);

            // 2. High Priority Thread!
            m_hReceiveThread.Priority = ThreadPriority.Highest;
            m_hReceiveThread.Start();

            nRet = m_MyCamera.MV_CC_StartGrabbing_NET();
            if (MyCamera.MV_OK != nRet)
            {
                m_bGrabbing = false;
                m_hReceiveThread.Join();
                ShowErrorMsg("Start Grabbing Fail!", nRet);
                return;
            }

            SetCtrlWhenStartGrab();
        }

        private void CbSoftTrigger_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSoftTrigger.Checked)
            {
                m_MyCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MyCamera.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE);
                if (m_bGrabbing) bnTriggerExec.Enabled = true;
            }
            else
            {
                m_MyCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MyCamera.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE0);
                bnTriggerExec.Enabled = false;
            }
        }

        private void BnTriggerExec_Click(object sender, EventArgs e)
        {
            int nRet = m_MyCamera.MV_CC_SetCommandValue_NET("TriggerSoftware");
            if (MyCamera.MV_OK != nRet) ShowErrorMsg("Trigger Software Fail!", nRet);
        }

        private void SetCtrlWhenStopGrab()
        {
            bnStartGrab.Enabled = true;
            bnStopGrab.Enabled = false;
            bnTriggerExec.Enabled = false;
            //bnSaveBmp.Enabled = false;
            //bnSaveJpg.Enabled = false;
            //bnSaveTiff.Enabled = false;
            //bnSavePng.Enabled = false;
        }

        private void BnStopGrab_Click(object sender, EventArgs e)
        {
            m_bGrabbing = false;
            int nRet = m_MyCamera.MV_CC_StopGrabbing_NET();
            if (nRet != MyCamera.MV_OK) ShowErrorMsg("Stop Grabbing Fail!", nRet);
            SetCtrlWhenStopGrab();
        }

        private void BnSaveBmp_Click(object sender, EventArgs e)
        {
            if (false == m_bGrabbing) { ShowErrorMsg("Not Start Grabbing", 0); return; }
            MyCamera.MV_SAVE_IMG_TO_FILE_PARAM stSaveFileParam = new MyCamera.MV_SAVE_IMG_TO_FILE_PARAM();
            lock (BufForDriverLock)
            {
                if (m_stFrameInfo.nFrameLen == 0) { ShowErrorMsg("Save Bmp Fail!", 0); return; }
                stSaveFileParam.enImageType = MyCamera.MV_SAVE_IAMGE_TYPE.MV_Image_Bmp;
                stSaveFileParam.enPixelType = m_stFrameInfo.enPixelType;
                stSaveFileParam.pData = m_BufForDriver;
                stSaveFileParam.nDataLen = m_stFrameInfo.nFrameLen;
                stSaveFileParam.nHeight = m_stFrameInfo.nHeight;
                stSaveFileParam.nWidth = m_stFrameInfo.nWidth;
                stSaveFileParam.iMethodValue = 2;
                stSaveFileParam.pImagePath = ConfigurationManager.AppSettings["ImagePath"] + "Image_w" + stSaveFileParam.nWidth.ToString() + "_h" + stSaveFileParam.nHeight.ToString() + "_fn" + m_stFrameInfo.nFrameNum.ToString() + ".bmp";
                int nRet = m_MyCamera.MV_CC_SaveImageToFile_NET(ref stSaveFileParam);
                if (MyCamera.MV_OK != nRet) { ShowErrorMsg("Save Bmp Fail!", nRet); return; }
            }
            ShowErrorMsg("Save Succeed!", 0);
        }

        private void BnSaveJpg_Click(object sender, EventArgs e)
        {
            if (false == m_bGrabbing) { ShowErrorMsg("Not Start Grabbing", 0); return; }
            MyCamera.MV_SAVE_IMG_TO_FILE_PARAM stSaveFileParam = new MyCamera.MV_SAVE_IMG_TO_FILE_PARAM();
            lock (BufForDriverLock)
            {
                if (m_stFrameInfo.nFrameLen == 0) { ShowErrorMsg("Save Jpeg Fail!", 0); return; }
                stSaveFileParam.enImageType = MyCamera.MV_SAVE_IAMGE_TYPE.MV_Image_Jpeg;
                stSaveFileParam.enPixelType = m_stFrameInfo.enPixelType;
                stSaveFileParam.pData = m_BufForDriver;
                stSaveFileParam.nDataLen = m_stFrameInfo.nFrameLen;
                stSaveFileParam.nHeight = m_stFrameInfo.nHeight;
                stSaveFileParam.nWidth = m_stFrameInfo.nWidth;
                stSaveFileParam.nQuality = 80;
                stSaveFileParam.iMethodValue = 2;
                stSaveFileParam.pImagePath = ConfigurationManager.AppSettings["ImagePath"] + "Image_w" + stSaveFileParam.nWidth.ToString() + "_h" + stSaveFileParam.nHeight.ToString() + "_fn" + m_stFrameInfo.nFrameNum.ToString() + ".jpg";
                int nRet = m_MyCamera.MV_CC_SaveImageToFile_NET(ref stSaveFileParam);
                if (MyCamera.MV_OK != nRet) { ShowErrorMsg("Save Jpeg Fail!", nRet); return; }
            }
            ShowErrorMsg("Save Succeed!", 0);
        }
        private void BtnAddBarCodeRoi_Click(object sender, EventArgs e)
        {
            AddRoi(RoiType.Barcode);
        }

        private void BnSavePng_Click(object sender, EventArgs e)
        {
            if (false == m_bGrabbing) { ShowErrorMsg("Not Start Grabbing", 0); return; }
            MyCamera.MV_SAVE_IMG_TO_FILE_PARAM stSaveFileParam = new MyCamera.MV_SAVE_IMG_TO_FILE_PARAM();
            lock (BufForDriverLock)
            {
                if (m_stFrameInfo.nFrameLen == 0) { ShowErrorMsg("Save Png Fail!", 0); return; }
                stSaveFileParam.enImageType = MyCamera.MV_SAVE_IAMGE_TYPE.MV_Image_Png;
                stSaveFileParam.enPixelType = m_stFrameInfo.enPixelType;
                stSaveFileParam.pData = m_BufForDriver;
                stSaveFileParam.nDataLen = m_stFrameInfo.nFrameLen;
                stSaveFileParam.nHeight = m_stFrameInfo.nHeight;
                stSaveFileParam.nWidth = m_stFrameInfo.nWidth;
                stSaveFileParam.nQuality = 8;
                stSaveFileParam.iMethodValue = 2;
                stSaveFileParam.pImagePath = ConfigurationManager.AppSettings["ImagePath"] + "Image_w" + stSaveFileParam.nWidth.ToString() + "_h" + stSaveFileParam.nHeight.ToString() + "_fn" + m_stFrameInfo.nFrameNum.ToString() + ".png";
                int nRet = m_MyCamera.MV_CC_SaveImageToFile_NET(ref stSaveFileParam);
                if (MyCamera.MV_OK != nRet) { ShowErrorMsg("Save Png Fail!", nRet); return; }
            }
            ShowErrorMsg("Save Succeed!", 0);
        }

        private void BnSaveTiff_Click(object sender, EventArgs e)
        {
            if (false == m_bGrabbing) { ShowErrorMsg("Not Start Grabbing", 0); return; }
            MyCamera.MV_SAVE_IMG_TO_FILE_PARAM stSaveFileParam = new MyCamera.MV_SAVE_IMG_TO_FILE_PARAM();
            lock (BufForDriverLock)
            {
                if (m_stFrameInfo.nFrameLen == 0) { ShowErrorMsg("Save Tiff Fail!", 0); return; }
                stSaveFileParam.enImageType = MyCamera.MV_SAVE_IAMGE_TYPE.MV_Image_Tif;
                stSaveFileParam.enPixelType = m_stFrameInfo.enPixelType;
                stSaveFileParam.pData = m_BufForDriver;
                stSaveFileParam.nDataLen = m_stFrameInfo.nFrameLen;
                stSaveFileParam.nHeight = m_stFrameInfo.nHeight;
                stSaveFileParam.nWidth = m_stFrameInfo.nWidth;
                stSaveFileParam.iMethodValue = 2;
                stSaveFileParam.pImagePath = ConfigurationManager.AppSettings["ImagePath"] + "Image_w" + stSaveFileParam.nWidth.ToString() + "_h" + stSaveFileParam.nHeight.ToString() + "_fn" + m_stFrameInfo.nFrameNum.ToString() + ".tif";
                int nRet = m_MyCamera.MV_CC_SaveImageToFile_NET(ref stSaveFileParam);
                if (MyCamera.MV_OK != nRet) { ShowErrorMsg("Save Tiff Fail!", nRet); return; }
            }
            ShowErrorMsg("Save Succeed!", 0);
        }

        private void BnGetParam_Click(object sender, EventArgs e)
        {
            MyCamera.MVCC_FLOATVALUE stParam = new MyCamera.MVCC_FLOATVALUE();
            int nRet = m_MyCamera.MV_CC_GetFloatValue_NET("ExposureTime", ref stParam);
            if (MyCamera.MV_OK == nRet) tbExposure.Text = stParam.fCurValue.ToString("F1");

            nRet = m_MyCamera.MV_CC_GetFloatValue_NET("Gain", ref stParam);
            if (MyCamera.MV_OK == nRet) tbGain.Text = stParam.fCurValue.ToString("F1");

            nRet = m_MyCamera.MV_CC_GetFloatValue_NET("ResultingFrameRate", ref stParam);
            if (MyCamera.MV_OK == nRet) tbFrameRate.Text = stParam.fCurValue.ToString("F1");
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            // 1. Force Windows to treat this app as High Priority (prevents background apps from stealing CPU)
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.High;

            System.Threading.ThreadPool.SetMinThreads(Environment.ProcessorCount * 2, Environment.ProcessorCount * 2);

            // Populate Enums
            CmbSegments.DataSource = Enum.GetValues(typeof(SegmentationMode)); // <--- NEW

            // Events
            CmbSegments.SelectedIndexChanged += CmbSegments_SelectedIndexChanged;

            //1.Ensure templates are loaded(Just in case)
            if (OcrEngine.TemplateVectors.Count == 0) OcrEngine.ReloadTemplates();

            // Load templates from folder
            TemplateManager.LoadTemplates();

            Text = $"{Application.ProductName} : Template based OCR + Barcode - Multi ROI + Training + TM";

            toolStripVersion.Text = $"Version: {Application.ProductVersion.Split('+')[0]}";

            //display msg
            DisplaySelectInfo(DisplayInfo.Default);

            //DISBALE CONTROLS
            SetCtrlWhenClose();

            // ch: 初始化 SDK | en: Initialize SDK
            MyCamera.MV_CC_Initialize_NET();

            // ch: 枚举设备 | en: Enum Device List
            DeviceListAcq();

            ListResult2 = ShowList2;
        }
        private void BnSetParam_Click(object sender, EventArgs e)
        {
            try
            {
                float.Parse(tbExposure.Text);
                float.Parse(tbGain.Text);
                float.Parse(tbFrameRate.Text);
            }
            catch { ShowErrorMsg("Please enter correct type!", 0); return; }

            m_MyCamera.MV_CC_SetEnumValue_NET("ExposureAuto", 0);
            int nRet = m_MyCamera.MV_CC_SetFloatValue_NET("ExposureTime", float.Parse(tbExposure.Text));
            if (nRet != MyCamera.MV_OK) ShowErrorMsg("Set Exposure Time Fail!", nRet);

            m_MyCamera.MV_CC_SetEnumValue_NET("GainAuto", 0);
            nRet = m_MyCamera.MV_CC_SetFloatValue_NET("Gain", float.Parse(tbGain.Text));
            if (nRet != MyCamera.MV_OK) ShowErrorMsg("Set Gain Fail!", nRet);

            nRet = m_MyCamera.MV_CC_SetFloatValue_NET("AcquisitionFrameRate", float.Parse(tbFrameRate.Text));
            if (nRet != MyCamera.MV_OK) ShowErrorMsg("Set Frame Rate Fail!", nRet);
        }

        public void ReceiveThreadProcess()
        {
            MyCamera.MV_FRAME_OUT stFrameInfo = new MyCamera.MV_FRAME_OUT();
            MyCamera.MV_DISPLAY_FRAME_INFO stDisplayInfo = new MyCamera.MV_DISPLAY_FRAME_INFO();
            MyCamera.MV_PIXEL_CONVERT_PARAM stConvertInfo = new MyCamera.MV_PIXEL_CONVERT_PARAM();
            int nRet = MyCamera.MV_OK;

            while (m_bGrabbing)
            {
                nRet = m_MyCamera.MV_CC_GetImageBuffer_NET(ref stFrameInfo, 1000);
                if (nRet == MyCamera.MV_OK)
                {
                    lock (BufForDriverLock)
                    {
                        if (m_BufForDriver == IntPtr.Zero || stFrameInfo.stFrameInfo.nFrameLen > m_nBufSizeForDriver)
                        {
                            if (m_BufForDriver != IntPtr.Zero)
                            {
                                Marshal.FreeHGlobal(m_BufForDriver);
                                m_BufForDriver = IntPtr.Zero;
                            }

                            m_BufForDriver = Marshal.AllocHGlobal((Int32)stFrameInfo.stFrameInfo.nFrameLen);
                            if (m_BufForDriver == IntPtr.Zero) return;
                            m_nBufSizeForDriver = stFrameInfo.stFrameInfo.nFrameLen;
                        }

                        m_stFrameInfo = stFrameInfo.stFrameInfo;
                        CopyMemory(m_BufForDriver, stFrameInfo.pBufAddr, stFrameInfo.stFrameInfo.nFrameLen);

                        stConvertInfo.nWidth = stFrameInfo.stFrameInfo.nWidth;
                        stConvertInfo.nHeight = stFrameInfo.stFrameInfo.nHeight;
                        stConvertInfo.enSrcPixelType = stFrameInfo.stFrameInfo.enPixelType;
                        stConvertInfo.pSrcData = stFrameInfo.pBufAddr;
                        stConvertInfo.nSrcDataLen = stFrameInfo.stFrameInfo.nFrameLen;
                        stConvertInfo.pDstBuffer = m_ConvertDstBuf;
                        stConvertInfo.nDstBufferSize = m_nConvertDstBufLen;

                        if (PixelFormat.Format8bppIndexed == m_bitmap.PixelFormat)
                        {
                            stConvertInfo.enDstPixelType = MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8;
                            m_MyCamera.MV_CC_ConvertPixelType_NET(ref stConvertInfo);
                        }
                        else
                        {
                            stConvertInfo.enDstPixelType = MyCamera.MvGvspPixelType.PixelType_Gvsp_BGR8_Packed;
                            m_MyCamera.MV_CC_ConvertPixelType_NET(ref stConvertInfo);
                        }

                        BitmapData bitmapData = m_bitmap.LockBits(new Rectangle(0, 0, stConvertInfo.nWidth, stConvertInfo.nHeight), ImageLockMode.ReadWrite, m_bitmap.PixelFormat);
                        CopyMemory(bitmapData.Scan0, stConvertInfo.pDstBuffer, (UInt32)(bitmapData.Stride * m_bitmap.Height));
                        m_bitmap.UnlockBits(bitmapData);
                    }

                    Bitmap oldBitmap = CaptureImage;
                    CaptureImage = new Bitmap(stFrameInfo.stFrameInfo.nWidth, stFrameInfo.stFrameInfo.nHeight, stFrameInfo.stFrameInfo.nWidth, System.Drawing.Imaging.PixelFormat.Format8bppIndexed, stFrameInfo.pBufAddr);

                    ColorPalette cp = CaptureImage.Palette;
                    for (int i = 0; i < 256; ++i) cp.Entries[i] = System.Drawing.Color.FromArgb(i, i, i);
                    CaptureImage.Palette = cp;

                    stDisplayInfo.pData = stFrameInfo.pBufAddr;
                    stDisplayInfo.nDataLen = stFrameInfo.stFrameInfo.nFrameLen;
                    stDisplayInfo.nWidth = stFrameInfo.stFrameInfo.nWidth;
                    stDisplayInfo.nHeight = stFrameInfo.stFrameInfo.nHeight;
                    stDisplayInfo.enPixelType = stFrameInfo.stFrameInfo.enPixelType;
                    m_MyCamera.MV_CC_DisplayOneFrame_NET(ref stDisplayInfo);

                    Invoke((MethodInvoker)delegate
                    {
                        if (m_bGrabbing)
                        {
                            // USE NEW SAFE IMAGE SETTER INSTEAD OF .ToMat() ONLY
                            SetCurrentImage(CaptureImage.ToMat());

                            if (rois.Any(r => r.IsAnchor)) AlignRois();
                            ImageCanvas.Invalidate();
                        }
                    });

                    m_MyCamera.MV_CC_FreeImageBuffer_NET(ref stFrameInfo);
                    if (oldBitmap != null) oldBitmap.Dispose();
                    listBoxResult1.Invoke(ListResult2, new object[] { nRet });
                }
                else
                {
                    if (bnTriggerMode.Checked) Thread.Sleep(5);
                }
            }
        }

        private void ShowErrorMsg(string csMessage, int nErrorNum)
        {
            string errorMsg;
            if (nErrorNum == 0) errorMsg = csMessage;
            else errorMsg = csMessage + ": Error =" + String.Format("{0:X}", nErrorNum);

            switch (nErrorNum)
            {
                case MyCamera.MV_E_HANDLE: errorMsg += " Error or invalid handle "; break;
                case MyCamera.MV_E_SUPPORT: errorMsg += " Not supported function "; break;
                case MyCamera.MV_E_BUFOVER: errorMsg += " Cache is full "; break;
                case MyCamera.MV_E_CALLORDER: errorMsg += " Function calling order error "; break;
                case MyCamera.MV_E_PARAMETER: errorMsg += " Incorrect parameter "; break;
                case MyCamera.MV_E_RESOURCE: errorMsg += " Applying resource failed "; break;
                case MyCamera.MV_E_NODATA: errorMsg += " No data "; break;
                case MyCamera.MV_E_PRECONDITION: errorMsg += " Precondition error, or running environment changed "; break;
                case MyCamera.MV_E_VERSION: errorMsg += " Version mismatches "; break;
                case MyCamera.MV_E_NOENOUGH_BUF: errorMsg += " Insufficient memory "; break;
                case MyCamera.MV_E_UNKNOW: errorMsg += " Unknown error "; break;
                case MyCamera.MV_E_GC_GENERIC: errorMsg += " General error "; break;
                case MyCamera.MV_E_GC_ACCESS: errorMsg += " Node accessing condition error "; break;
                case MyCamera.MV_E_ACCESS_DENIED: errorMsg += " No permission "; break;
                case MyCamera.MV_E_BUSY: errorMsg += " Device is busy, or network disconnected "; break;
                case MyCamera.MV_E_NETER: errorMsg += " Network error "; break;
            }
            MessageBox.Show(errorMsg, "PROMPT");
        }

        private string DeleteTail(string strUserDefinedName)
        {
            strUserDefinedName = Regex.Unescape(strUserDefinedName);
            int nIndex = strUserDefinedName.IndexOf("\0");
            if (nIndex >= 0) strUserDefinedName = strUserDefinedName.Remove(nIndex);
            return strUserDefinedName;
        }

        private Boolean IsMonoData(MyCamera.MvGvspPixelType enGvspPixelType)
        {
            switch (enGvspPixelType)
            {
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono12_Packed:
                    return true;
                default:
                    return false;
            }
        }

        private Boolean IsColorData(MyCamera.MvGvspPixelType enGvspPixelType)
        {
            switch (enGvspPixelType)
            {
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_YUV422_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_YUV422_YUYV_Packed:
                    return true;
                default:
                    return false;
            }
        }

        public async void ShowList2(Int32 ErrorCode)
        {
            if (MyCamera.MV_OK == ErrorCode)
            {
                try
                {
                    ImgCount++;
                    BtnDecodeAllROI.PerformClick();
                }
                catch { }
            }
        }

        private void CameraDemo_FormClosing(object sender, FormClosingEventArgs e)
        {
            BnClose_Click(sender, e);
            MyCamera.MV_CC_Finalize_NET();
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

        private void SaveAsBMPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BnSaveBmp_Click(sender, e);
        }

        private void SaveAsJPGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BnSaveJpg_Click(sender, e);
        }

        private void SaveAsTIFFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BnSaveTiff_Click(sender, e);
        }

        private void SaveAsPNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BnSavePng_Click(sender, e);
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
    }
}