using OpenCV_SharpNet.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCV_SharpNet.Models
{
    [DebuggerStepThrough]
    // Add this class at the bottom of MainForm.cs
    public class RoiConfigWrapper
    {
        public int OriginalImageWidth { get; set; }
        public int OriginalImageHeight { get; set; }
        public List<RoiDataTransfer> Rois { get; set; }
    }
}
