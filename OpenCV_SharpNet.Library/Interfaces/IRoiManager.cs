using CsplCam.Library.Models;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsplCam.Library.Interfaces
{
    public interface IRoiManager
    {
        Task<bool> SaveRoi(Mat currentImage,List<RoiObject> roiObjects,string path);

        Task<List<RoiObject>> LoadRoi(Mat currentImage,string path);
    }
}
