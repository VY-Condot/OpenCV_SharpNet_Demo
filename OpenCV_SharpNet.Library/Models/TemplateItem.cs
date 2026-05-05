using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsplCam.Library.Models
{
    [DebuggerStepThrough]
    public class TemplateItem
    {
        public string Name { get; set; } // Filename (e.g. "Ok_Label")
        public Mat Image { get; set; }   // The loaded image
    }
}
