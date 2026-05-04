using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCV_SharpNet.Models
{
    public class CharResult
    {
        public Rect Box { get; set; }
        public string? Text { get; set; }
        public double Score { get; set; }
        public bool IsGood { get; set; }

        // --- NEW PROPERTY ---
        public bool IsExpectedMatch { get; set; } // Does it match the expected character?



        // ADD THIS! So we can save the exact corners
        public Point2f[] ExactCorners { get; set; }


        //for drwa shape on UI
        public OpenCvSharp.Point[] Polygon { get; set; }
    }
}
