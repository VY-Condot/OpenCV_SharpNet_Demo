using CsplCam.Library.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsplCam.Library.Interfaces
{
    public interface IRoiControl
    {
        RoiObject BoundedROI { get; }

        Size? ControlSize { get; protected set; }

        //create eventhandler for roi changed
        event EventHandler SelectionClick;
        event EventHandler SettingsChanged;
        event EventHandler DecodeRequested;

        //new event for open the anchor setting window
        event EventHandler OpenAnchorSettingsWindow;

        //new event for open the roi anchor reference window
        event EventHandler OpenRoiReferenceWindow;

        //funtions
        void BindData(RoiObject roi, bool isSelected);
        void SetPreviewImage(System.Drawing.Bitmap newImage);
        void SetSelectionState(bool isSelected);
    }
}
