using CsplCam.Library.Interfaces;
using CsplCam.Library.Models;
using OpenCvSharp;
using System.Text.Json;
using CvRect = OpenCvSharp.Rect;
using Point = System.Drawing.Point;

namespace CsplCam.Library.Services
{
    public class RoiManager : IRoiManager
    {
        public async Task<List<RoiObject>> LoadRoi(Mat currentImage, string path)
        {
            //validation 
            if(!Path.Exists(path))
                throw new FileNotFoundException($"File not found: {path}");

            List<RoiObject> rois = new List<RoiObject>();

           string jsonString = await File.ReadAllTextAsync(path);
            List<RoiDataTransfer> loadedDtos = null;
            int savedWidth = currentImage.Width;
            int savedHeight = currentImage.Height;

            try
            {
                var wrapper = JsonSerializer.Deserialize<RoiConfigWrapper>(jsonString);
                if (wrapper != null && wrapper.Rois != null)
                {
                    loadedDtos = wrapper.Rois;
                    if (wrapper.OriginalImageWidth > 0) savedWidth = wrapper.OriginalImageWidth;
                    if (wrapper.OriginalImageHeight > 0) savedHeight = wrapper.OriginalImageHeight;


                    // Load OCR engine settings
                    OcrEngine.OcvTargetMatchConfidence = wrapper.OcvTargetMatchConfidence;
                    OcrEngine.AspectRatioDifferenceMultiplier = wrapper.AspectRatioDifferenceMultiplier;
                    OcrEngine.DensityDifferenceMultiplier = wrapper.DensityDifferenceMultiplier;
                    OcrEngine.AspectRatioDifferenceThreshold = wrapper.AspectRatioDifferenceThreshold;
                    OcrEngine.AspectRatioPenaltyValue = wrapper.AspectRatioPenaltyValue;
                    OcrEngine.SkewAngle = wrapper.SkewAngle;
                }
            }
            catch
            {
                loadedDtos = JsonSerializer.Deserialize<List<RoiDataTransfer>>(jsonString);
            }

            if (loadedDtos == null || loadedDtos.Count == 0) new List<RoiObject>();

            double scaleX = (double)currentImage.Width / savedWidth;
            double scaleY = (double)currentImage.Height / savedHeight;

            foreach (var dto in loadedDtos)
            {
                var logicRoi = ROIConverter.ToLogic(dto);

                if (scaleX != 1.0 || scaleY != 1.0)
                {
                    int newX = (int)(logicRoi.Box.X * scaleX);
                    int newY = (int)(logicRoi.Box.Y * scaleY);
                    int newW = (int)(logicRoi.Box.Width * scaleX);
                    int newH = (int)(logicRoi.Box.Height * scaleY);
                    logicRoi.Box = new CvRect(newX, newY, newW, newH);

                    logicRoi.AnchorTop = (int)(logicRoi.AnchorTop * scaleY);
                    logicRoi.AnchorBottom = (int)(logicRoi.AnchorBottom * scaleY);
                    logicRoi.AnchorLeft = (int)(logicRoi.AnchorLeft * scaleX);
                    logicRoi.AnchorRight = (int)(logicRoi.AnchorRight * scaleX);

                    if (logicRoi.AnchorTemplate != null && !logicRoi.AnchorTemplate.Empty())
                    {
                        Mat resizedTemplate = new Mat();
                        Cv2.Resize(logicRoi.AnchorTemplate, resizedTemplate,
                            new OpenCvSharp.Size(logicRoi.AnchorTemplate.Width * scaleX, logicRoi.AnchorTemplate.Height * scaleY),
                            0, 0, InterpolationFlags.Cubic);
                        logicRoi.AnchorTemplate.Dispose();
                        logicRoi.AnchorTemplate = resizedTemplate;
                    }
                }
                rois.Add(logicRoi);
            }

            return rois;
        }

        public async Task<bool> SaveRoi(Mat currentImage, List<RoiObject> roiObjects, string path)
        {
            try
            {
                var dataToSave = new List<RoiDataTransfer>();
                foreach (var roi in roiObjects) dataToSave.Add(ROIConverter.FromLogic(roi));

                var wrapper = new RoiConfigWrapper
                {
                    OriginalImageWidth = currentImage?.Width ?? 1,
                    OriginalImageHeight = currentImage?.Height ?? 1,

                    //get saved ocr engine config
                    OcvTargetMatchConfidence = OcrEngine.OcvTargetMatchConfidence,
                    AspectRatioDifferenceMultiplier = OcrEngine.AspectRatioDifferenceMultiplier,
                    DensityDifferenceMultiplier = OcrEngine.DensityDifferenceMultiplier,
                    AspectRatioDifferenceThreshold = OcrEngine.AspectRatioDifferenceThreshold,
                    AspectRatioPenaltyValue = OcrEngine.AspectRatioPenaltyValue,
                    SkewAngle = OcrEngine.SkewAngle,


                    Rois = dataToSave
                };

                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(wrapper, options); // FIX: Serialized 'wrapper', not 'dataToSave'
                
                await File.WriteAllTextAsync(path, jsonString);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
