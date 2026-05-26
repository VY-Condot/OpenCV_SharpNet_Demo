using System.Drawing;
using System.Drawing.Drawing2D;

namespace OpenCV_SharpNet.UserControls
{
    public partial class PanZoomViewer : UserControl
    {
        // ====================================================================
        // --- CORE DATA ---
        // ====================================================================
        private Bitmap _image;
        private float _zoom = 1.0f;
        private Point _panOffset = new Point(0, 0);

        // State
        private bool _isPanning = false;
        private Point _lastMousePos;
        private bool _needsAutoFit = true;

        // ====================================================================
        // --- PROPERTIES ---
        // ====================================================================
        public Bitmap Image
        {
            get => _image;
            set
            {
                var oldImage = _image;
                _image = value;

                // Only AutoFit on the very first image load, or if requested
                if (_image != null && _needsAutoFit)
                {
                    AutoFit();
                    _needsAutoFit = false;
                }

                // Clean up old memory automatically
                oldImage?.Dispose();

                Invalidate();
            }
        }

        public InterpolationMode Interpolation { get; set; } = InterpolationMode.NearestNeighbor;

        // ====================================================================
        // --- CUSTOM EVENTS (For drawing overlays like ROIs) ---
        // ====================================================================
        public event EventHandler<Graphics> OnCustomPaint;

        public PanZoomViewer()
        {
            InitializeComponent();

            // CRITICAL FOR SMOOTH GRAPHICS (Prevents flickering)
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            // WinForms focus hack for MouseWheel support
            this.MouseEnter += (s, e) => this.Focus();
        }

        // ====================================================================
        // --- RENDERING ---
        // ====================================================================
        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    base.OnPaint(e);
        //    Graphics g = e.Graphics;

        //    g.Clear(Color.Black);

        //    if (_image != null)
        //    {
        //        g.InterpolationMode = Interpolation;
        //        g.PixelOffsetMode = PixelOffsetMode.Half;

        //        int w = (int)(_image.Width * _zoom);
        //        int h = (int)(_image.Height * _zoom);

        //        g.DrawImage(_image, _panOffset.X, _panOffset.Y, w, h);
        //    }

        //    // Let the Parent Form draw things on top (like Purple ROI rectangles!)
        //    OnCustomPaint?.Invoke(this, g);
        //}

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            // CHANGE THIS LINE:
            // g.Clear(Color.Black); 
            g.Clear(this.BackColor); // <--- Now it respects your UI theme!

            if (_image != null)
            {
                g.InterpolationMode = Interpolation;
                g.PixelOffsetMode = PixelOffsetMode.Half;

                int w = (int)(_image.Width * _zoom);
                int h = (int)(_image.Height * _zoom);

                g.DrawImage(_image, _panOffset.X, _panOffset.Y, w, h);
            }

            OnCustomPaint?.Invoke(this, g);
        }

        // ====================================================================
        // --- MOUSE INTERACTIONS (Pan & Zoom) ---
        // ====================================================================
        //protected override void OnMouseWheel(MouseEventArgs e)
        //{
        //    base.OnMouseWheel(e);
        //    if (_image == null) return;

        //    float f = e.Delta > 0 ? 1.1f : 0.9f;
        //    float newZoom = _zoom * f;

        //    if (newZoom <= 0.05f) newZoom = 0.05f;
        //    if (newZoom >= 100.0f) newZoom = 100.0f;

        //    _panOffset.X = (int)(e.X - (e.X - _panOffset.X) * (newZoom / _zoom));
        //    _panOffset.Y = (int)(e.Y - (e.Y - _panOffset.Y) * (newZoom / _zoom));

        //    _zoom = newZoom;
        //    Invalidate();
        //}
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            // ==========================================================
            // FIX 2: PREVENT PARENT PANEL FROM SCROLLING
            // Tell WinForms that the scroll was used for zooming, so stop bubbling it!
            // ==========================================================
            if (e is HandledMouseEventArgs hme)
            {
                hme.Handled = true;
            }

            base.OnMouseWheel(e);
            if (_image == null) return;

            float f = e.Delta > 0 ? 1.1f : 0.9f;
            float newZoom = _zoom * f;

            if (newZoom <= 0.05f) newZoom = 0.05f;
            if (newZoom >= 100.0f) newZoom = 100.0f;

            _panOffset.X = (int)(e.X - (e.X - _panOffset.X) * (newZoom / _zoom));
            _panOffset.Y = (int)(e.Y - (e.Y - _panOffset.Y) * (newZoom / _zoom));

            _zoom = newZoom;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            // Allow panning with Left OR Middle mouse button
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Middle)
            {
                _isPanning = true;
                _lastMousePos = e.Location;
                this.Cursor = Cursors.SizeAll;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_isPanning)
            {
                int dx = e.X - _lastMousePos.X;
                int dy = e.Y - _lastMousePos.Y;

                _panOffset.X += dx;
                _panOffset.Y += dy;

                _lastMousePos = e.Location;
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _isPanning = false;
            this.Cursor = Cursors.Default;
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            AutoFit();
        }

        // ====================================================================
        // --- UTILITIES ---
        // ====================================================================
        //public void AutoFit()
        //{
        //    if (_image == null || this.Width == 0 || this.Height == 0) return;

        //    float sx = (float)this.Width / _image.Width;
        //    float sy = (float)this.Height / _image.Height;

        //    _zoom = Math.Min(sx, sy) * 0.9f;
        //    if (_zoom <= 0) _zoom = 1.0f;

        //    int dispW = (int)(_image.Width * _zoom);
        //    int dispH = (int)(_image.Height * _zoom);

        //    _panOffset = new Point((this.Width - dispW) / 2, (this.Height - dispH) / 2);
        //    Invalidate();
        //}
        // ====================================================================
        // --- UTILITIES ---
        // ====================================================================
        public void AutoFit()
        {
            if (_image == null || this.Width <= 0 || this.Height <= 0) return;

            float sx = (float)this.Width / _image.Width;
            float sy = (float)this.Height / _image.Height;

            // Exact "PictureBoxSizeMode.Zoom" math (Stretches to perfectly fit bounds)
            _zoom = Math.Min(sx, sy);
            if (_zoom <= 0) _zoom = 1.0f;

            int dispW = (int)(_image.Width * _zoom);
            int dispH = (int)(_image.Height * _zoom);

            _panOffset = new Point((this.Width - dispW) / 2, (this.Height - dispH) / 2);

            _needsAutoFit = false; // Successfully fitted
            Invalidate();
        }

        // WinForms Hack: Controls often have 0 Width when first created.
        // This ensures the image zooms perfectly as soon as the panel finishes loading!
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (_image != null && _needsAutoFit)
            {
                AutoFit();
            }
        }

        // Extremely useful for the MainForm to convert Mouse clicks to Image Pixels
        public Point ScreenToImage(Point screenPoint)
        {
            return new Point(
                (int)((screenPoint.X - _panOffset.X) / _zoom),
                (int)((screenPoint.Y - _panOffset.Y) / _zoom)
            );
        }

        // Extremely useful for the MainForm to draw OpenCV rectangles on the screen
        public Rectangle ImageToScreen(OpenCvSharp.Rect r)
        {
            return new Rectangle(
                (int)(r.X * _zoom) + _panOffset.X,
                (int)(r.Y * _zoom) + _panOffset.Y,
                (int)(r.Width * _zoom),
                (int)(r.Height * _zoom)
            );
        }
    }
}