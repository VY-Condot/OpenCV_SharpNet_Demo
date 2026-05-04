using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCV_SharpNet.Models
{
    public class TemplateItem
    {
        public string Name { get; set; } // Filename (e.g. "Ok_Label")
        public Mat Image { get; set; }   // The loaded image
    }
}
