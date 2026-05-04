using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCV_SharpNet.Models
{
    public class CharTemplate
    {
        //public Mat Vector { get; set; }      // The 30x30 visual shape

        public float[] Vector { get; set; }
        public double AspectRatio { get; set; } // Width / Height
        public double FillDensity { get; set; } // How much ink? (0.0 to 1.0)
    }
}
