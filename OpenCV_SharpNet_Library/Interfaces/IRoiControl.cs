using OpenCV_SharpNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCV_SharpNet.Interfaces
{
    public interface IRoiControl
    {
        RoiObject BoundedROI { get; }

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
