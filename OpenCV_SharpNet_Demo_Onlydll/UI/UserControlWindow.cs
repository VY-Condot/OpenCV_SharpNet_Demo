using CsplCam.Library.Interfaces;
using CsplCam.Library.Models;
using OpenCV_SharpNet_Demo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCV_SharpNet_Demo_Onlydll;

namespace OpenCV_SharpNet_Demo_Onlydll.UI
{
    public partial class UserControlWindow : Form, IRoiControl
    {
        MainForm frm;

        private IRoiControl _roiControl;
        public static UserControlWindow Instance { get; private set; }

        public RoiObject BoundedROI => _roiControl.BoundedROI;
        Size? IRoiControl.ControlSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        // Proxy events directly to the inner control so we don't duplicate event handlers!
        public event EventHandler SelectionClick
        {
            add => _roiControl.SelectionClick += value;
            remove => _roiControl.SelectionClick -= value;
        }
        public event EventHandler SettingsChanged
        {
            add => _roiControl.SettingsChanged += value;
            remove => _roiControl.SettingsChanged -= value;
        }
        public event EventHandler DecodeRequested
        {
            add => _roiControl.DecodeRequested += value;
            remove => _roiControl.DecodeRequested -= value;
        }
        public event EventHandler OpenAnchorSettingsWindow
        {
            add => _roiControl.OpenAnchorSettingsWindow += value;
            remove => _roiControl.OpenAnchorSettingsWindow -= value;
        }
        public event EventHandler OpenRoiReferenceWindow
        {
            add => _roiControl.OpenRoiReferenceWindow += value;
            remove => _roiControl.OpenRoiReferenceWindow -= value;
        }

        event EventHandler IRoiControl.SelectionClick
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event EventHandler IRoiControl.SettingsChanged
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event EventHandler IRoiControl.DecodeRequested
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event EventHandler IRoiControl.OpenAnchorSettingsWindow
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event EventHandler IRoiControl.OpenRoiReferenceWindow
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        //public static UserControlWindow GetInstance(IRoiControl roiControl) => Instance ??= new UserControlWindow(new UserControlWindow(roiControl));

        // ====================================================================
        // SINGLETON PATTERN: Track instances per IRoiControl
        // ====================================================================
        //private static Dictionary<IRoiControl, UserControlWindow> _instances = new();

        /// <summary>
        /// Gets or creates a singleton window for the given ROI control.
        /// If a window already exists and is open, it brings it to focus.
        /// </summary>
        //public static UserControlWindow GetInstance(IRoiControl roiControl)
        //{
        //    if (_instances.ContainsKey(roiControl))
        //    {
        //        var existingWindow = _instances[roiControl];

        //        // If window still exists and not disposed
        //        if (existingWindow != null && !existingWindow.IsDisposed)
        //        {
        //            // 🔴 BRING TO FOCUS - Don't create a new one!
        //            existingWindow.BringToFront();
        //            existingWindow.Focus();
        //            existingWindow.WindowState = FormWindowState.Normal;
        //            return existingWindow;
        //        }

        //        // Window was closed, remove the dead reference
        //        _instances.Remove(roiControl);
        //    }

        //    // Create a new window ONLY if none exists
        //    //UserControlWindow newWindow = new UserControlWindow(roiControl);
        //    //_instances[roiControl] = newWindow;

        //    return newWindow;
        //}

        CameraDemo _cameraDemo;
        public UserControlWindow(IRoiControl roiControl, MainForm frm1,CameraDemo cameraDemo)
        {
            InitializeComponent();
            frm = frm1;
            _cameraDemo = cameraDemo;
            //_roiControl = roiControl;
            //var ctrl = roiControl as Control;

            //if (ctrl != null)
            //{
            //    //// Form settings to ensure it perfectly fits the user control
            //    Size = new Size(ctrl.Width, ctrl.Height);
            //    AutoScaleMode = AutoScaleMode.Dpi;
            //    ctrl.Dock = DockStyle.Fill;
            //    Controls.Add(ctrl);
            //}

            ////this.FormBorderStyle = FormBorderStyle.FixedDialog;
            ////this.MaximizeBox = false;
            ////this.MinimizeBox = false;
            //this.StartPosition = FormStartPosition.CenterParent;


            //_roiControl = roiControl;
            //var ctrl = roiControl as Control;

            //if (ctrl != null)
            //{
            //    // Add the control FIRST, then set the form size
            //    ctrl.Dock = DockStyle.Fill;
            //    Controls.Add(ctrl);

            //    // CRITICAL FIX: Calculate form size from control's preferred size
            //    // Add padding for form borders, title bar, and margins
            //    int formWidth = ctrl.Width + SystemInformation.FrameBorderSize.Width * 2;
            //    int formHeight = ctrl.Height + SystemInformation.CaptionHeight + SystemInformation.FrameBorderSize.Height * 2;

            //    // Set the form client size to match the control size
            //    this.ClientSize = new Size(ctrl.Width, ctrl.Height);

            //    // Alternative: Use the form size if you prefer
            //    // this.Size = new Size(formWidth, formHeight);
            //}

            //this.StartPosition = FormStartPosition.CenterParent;
            //this.FormBorderStyle = FormBorderStyle.FixedDialog;
            //this.MaximizeBox = false;
            //this.MinimizeBox = false;
            //this.AutoScaleMode = AutoScaleMode.Dpi;




            _roiControl = roiControl;
            var ctrl = roiControl as Control;

            if (ctrl != null)
            {
                // CRITICAL FIX: Get the actual preferred size of the control
                // This works for all control types (ROIControl, RoiControlTemplate, RoiControlBarCode)
                //Size preferredSize = _ControlInitialSize = ctrl.ClientSize;


                Size preferredSize = _roiControl.ControlSize.Value;

                // If PreferredSize is not set properly, fall back to the control's current size
                if (preferredSize.Width <= 0 || preferredSize.Height <= 0)
                {
                    preferredSize = ctrl.Size;
                }

                // Set the form's client size to match the control's actual size
                this.ClientSize = Size = preferredSize;

                // Add the control FIRST, then set the form size
                ctrl.Dock = DockStyle.Fill;
                Controls.Add(ctrl);
            }

            this.StartPosition = FormStartPosition.CenterParent;
            this.AutoScaleMode = AutoScaleMode.Dpi;
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        // CRITICAL FIX: Unparent the user control BEFORE the form disposes. 
        //        // This prevents the control from being destroyed when the dialog closes, 
        //        // allowing it to be reused the next time the user clicks the button.
        //        var ctrl = _roiControl as Control;
        //        if (ctrl != null && this.Controls.Contains(ctrl))
        //        {
        //            this.Controls.Remove(ctrl);
        //        }

        //        if (components != null)
        //        {
        //            components.Dispose();
        //        }
        //    }
        //    base.Dispose(disposing);
        //}

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // 🚨 CRITICAL FIX TO AVOID THE DISPOSE PROBLEM 🚨
            // Unparent the user control BEFORE the form closes and destroys its children
            var ctrl = _roiControl as Control;
            if (ctrl != null && this.Controls.Contains(ctrl))
            {
                this.Controls.Remove(ctrl);
            }

            //e.Cancel = true;

            if(frm != null)
                frm.grpRoiControls.Enabled = true;
            if(_cameraDemo != null)
                _cameraDemo.GrpRoiControls.Enabled = true;

            base.OnFormClosing(e);

           /// this.Close();
        }

        public void BindData(RoiObject roi, bool isSelected)
        {
            _roiControl.BindData(roi, isSelected);
            //this.Text = $"Settings - {roi.Name} ({roi.Type})";
        }

        public void SetPreviewImage(Bitmap newImage)
        {
            _roiControl.SetPreviewImage(newImage);
        }

        public void SetSelectionState(bool isSelected)
        {
            _roiControl.SetSelectionState(isSelected);
        }

        private void UserControlWindow_Load(object sender, EventArgs e)
        {
            Text = $"Settings - {_roiControl.BoundedROI.Type}";
        }

        void IRoiControl.BindData(RoiObject roi, bool isSelected)
        {
            throw new NotImplementedException();
        }

        void IRoiControl.SetPreviewImage(Bitmap newImage)
        {
            throw new NotImplementedException();
        }

        void IRoiControl.SetSelectionState(bool isSelected)
        {
            throw new NotImplementedException();
        }
    }
}
