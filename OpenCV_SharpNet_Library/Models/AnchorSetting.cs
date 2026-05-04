using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCV_SharpNet.Models
{
    [DebuggerStepThrough]
    public class AnchorSetting
    {
        public int Top { get; set; }
        public int Bottom { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public bool IsEdgeBasedMatching { get; set; } = false;
        public bool IsHighPrecisionMode { get; set; } = true;
    }
}
