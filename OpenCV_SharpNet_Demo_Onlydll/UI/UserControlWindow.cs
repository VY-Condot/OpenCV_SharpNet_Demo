using CsplCam.Library.Interfaces;
using CsplCam.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenCV_SharpNet_Demo_Onlydll.UI
{
    public partial class UserControlWindow : Form, IRoiControl
    {

        private IRoiControl _roiControl;

        public RoiObject BoundedROI => _roiControl.BoundedROI;

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

        public UserControlWindow(IRoiControl roiControl)
        {
            InitializeComponent();

            _roiControl = roiControl;
            var ctrl = roiControl as Control;

            if (ctrl != null)
            {
                ctrl.Dock = DockStyle.Fill;
                this.Controls.Add(ctrl);

                // Form settings to ensure it perfectly fits the user control
                this.ClientSize = new Size(ctrl.Width, ctrl.Height);
            }

            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
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

            base.OnFormClosing(e);
        }

        public void BindData(RoiObject roi, bool isSelected)
        {
            _roiControl.BindData(roi, isSelected);
            this.Text = $"Settings - {roi.Name} ({roi.Type})";
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
            Text = $"Settings - {_roiControl.BoundedROI.Name} ({_roiControl.BoundedROI.Type})";
        }
    }
}
