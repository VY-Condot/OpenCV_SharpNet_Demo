using CsplCam.Library.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsplCam.Library.Models
{
    [DebuggerStepThrough]
    // Add this class at the bottom of MainForm.cs
    public class RoiConfigWrapper
    {
        public int OriginalImageWidth { get; set; }
        public int OriginalImageHeight { get; set; }



        //engine setting parameters
        public double OcvTargetMatchConfidence { get; set; } = 0.80;
        public double AspectRatioDifferenceMultiplier { get; set; } = 0.8;
        public double DensityDifferenceMultiplier { get; set; } = 0.5;
        public double AspectRatioDifferenceThreshold { get; set; } = 0.20;
        public double AspectRatioPenaltyValue { get; set; } = 2.5;
        public double SkewAngle { get; set; } = 10;

        public List<RoiDataTransfer> Rois { get; set; }
    }
}
