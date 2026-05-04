using OpenCV_SharpNet.Enums;
using OpenCV_SharpNet.Models;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCV_SharpNet.Services
{
    public class ROIConverter
    {
        // 1. Convert Logic Object -> DTO (For Saving)
        public static RoiDataTransfer FromLogic(RoiObject logicObj)
        {
            byte[]? imgData = null;

            // Convert OpenCV Mat to PNG Byte Array
            if (logicObj.AnchorTemplate != null && !logicObj.AnchorTemplate.Empty())
            {
                Cv2.ImEncode(".png", logicObj.AnchorTemplate, out imgData);
            }

            return new RoiDataTransfer
            {
                Id = logicObj.Id,
                Name = logicObj.Name,
                X = logicObj.Box.X,
                Y = logicObj.Box.Y,
                W = logicObj.Box.Width,
                H = logicObj.Box.Height,
                Type = logicObj.Type.ToString(),
                ShowOverlay = logicObj.ShowOverlay,
                ExpectedText = logicObj.ExpectedText,
                Threshold = logicObj.Threshold,
                MinBlobW = logicObj.MinBlobW,
                MinBlobH = logicObj.MinBlobH,
                MaxBlobW = logicObj.MaxBlobW,
                MaxBlobH = logicObj.MaxBlobH,
                IsAnchor = logicObj.IsAnchor,
                IsUseEdgeMatching = logicObj.IsUseEdgeMatching,
                AnchorTop = logicObj.AnchorTop,
                AnchorBottom = logicObj.AnchorBottom,
                AnchorLeft = logicObj.AnchorLeft,
                AnchorRight = logicObj.AnchorRight,
                AnchorPatternData = imgData,
                RotationAngle = logicObj.RotationAngle,
                IsNameChanged = logicObj.IsNameChanged,


                BarCodeFormat = logicObj.BarCodeFormat,
                MorphIterations  = logicObj.MorphIterations,
                MorphKernelHeight = logicObj.MorphKernelHeight,
                MorphKernelWidth = logicObj.MorphKernelWidth,
                MorphOp = logicObj.MorphOp,
                SegmentationMode = logicObj.SegmentationMode,
                ReferenceRoiID = logicObj.ReferenceRoiID,
                IsUseRoiReference = logicObj.IsUseRoiReference
                //OverallThreshold = logicObj.OverallThreshold
            };
        }

        // 2. Convert DTO -> Logic Object (For Loading)
        public static RoiObject ToLogic(RoiDataTransfer objROITransfer)
        {
            // Convert Bytes back to OpenCV Mat
            Mat? pattern = null;
            if (objROITransfer.AnchorPatternData != null && objROITransfer.AnchorPatternData.Length > 0)
                pattern = Cv2.ImDecode(objROITransfer.AnchorPatternData, ImreadModes.Grayscale);

            // Parse Enum
            Enum.TryParse(objROITransfer.Type, out RoiType rType);

            return new RoiObject
            {
                Id = objROITransfer.Id,
                Name = objROITransfer.Name,
                Box = new Rect(objROITransfer.X, objROITransfer.Y, objROITransfer.W, objROITransfer.H),
                Type = rType,
                ShowOverlay = objROITransfer.ShowOverlay,
                ExpectedText = objROITransfer.ExpectedText,
                Threshold = objROITransfer.Threshold,
                MinBlobW = objROITransfer.MinBlobW,
                MinBlobH = objROITransfer.MinBlobH,
                MaxBlobW = objROITransfer.MaxBlobW,
                MaxBlobH = objROITransfer.MaxBlobH,
                IsAnchor = objROITransfer.IsAnchor,
                IsUseEdgeMatching = objROITransfer.IsUseEdgeMatching,
                AnchorTop = objROITransfer.AnchorTop,
                AnchorBottom = objROITransfer.AnchorBottom,
                AnchorLeft = objROITransfer.AnchorLeft,
                AnchorRight = objROITransfer.AnchorRight,
                AnchorTemplate = pattern,
                RotationAngle = objROITransfer.RotationAngle,
                IsNameChanged = objROITransfer.IsNameChanged,

                BarCodeFormat = objROITransfer.BarCodeFormat,
                MorphIterations = objROITransfer.MorphIterations,
                MorphKernelHeight = objROITransfer.MorphKernelHeight,
                MorphKernelWidth = objROITransfer.MorphKernelWidth,
                MorphOp = objROITransfer.MorphOp,
                SegmentationMode = objROITransfer.SegmentationMode,
                ReferenceRoiID = objROITransfer.ReferenceRoiID,
                IsUseRoiReference = objROITransfer.IsUseRoiReference
                //OverallThreshold = objROITransfer.OverallThreshold
            };
        }
    }
}
