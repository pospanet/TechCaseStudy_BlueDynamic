using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media.FaceAnalysis;
using Windows.Storage.Streams;
using ImageProcessorCore;

namespace Retailizer.UWP
{
    internal class LocalFaceDetector : IImageFilter
    {
        private const int FaceImageBoxPaddingPercentage = 25;

        private readonly FaceDetector _faceDetector;
        private readonly IImagePersiter _imagePersiter;

        public LocalFaceDetector(FaceDetector faceDetector, IImagePersiter imagePersiter)
        {
            _faceDetector = faceDetector;
            _imagePersiter = imagePersiter;
        }

        public async Task ProcessImageAsync(BitmapDecoder bitmapDecoder, IRandomAccessStream imageStream,
            string cameraId)
        {
            try
            {
                SoftwareBitmap image =
                    await
                        bitmapDecoder.GetSoftwareBitmapAsync(bitmapDecoder.BitmapPixelFormat,
                            BitmapAlphaMode.Premultiplied);

                const BitmapPixelFormat faceDetectionPixelFormat = BitmapPixelFormat.Gray8;
                if (image.BitmapPixelFormat != faceDetectionPixelFormat)
                {
                    image = SoftwareBitmap.Convert(image, faceDetectionPixelFormat);
                }
                IEnumerable<DetectedFace> detectedFaces = await _faceDetector.DetectFacesAsync(image);

                if (detectedFaces!=null)
                {
                    List<Stream> faceImages = new List<Stream>();
                    foreach (DetectedFace face in detectedFaces)
                    {
                        MemoryStream faceImageStream = new MemoryStream();
                        Image faceImage = new Image(imageStream.AsStreamForRead());
                        int width, height, xStartPosition, yStartPosition;
                        EnlargeFaceBoxSize(face, image, out width, out height, out xStartPosition,
                            out yStartPosition);
                        faceImage.Crop(width, height,
                            new Rectangle(xStartPosition, yStartPosition,
                                width, height)).SaveAsJpeg(faceImageStream, 80);
                        faceImages.Add(faceImageStream);
                    }


                    await _imagePersiter.PersistAsync(faceImages, cameraId);
                }
            }
            catch (Exception ex)
            {
                //ToDo Logging
                Debug.WriteLine(ex.Message);
            }
        }

        private static void EnlargeFaceBoxSize(DetectedFace face, SoftwareBitmap image, out int width, out int height,
            out int xStartPosition,
            out int yStartPosition)
        {
            width = (int) face.FaceBox.Width;
            height = (int) face.FaceBox.Height;
            int paddingWidth = (int) (face.FaceBox.Width*FaceImageBoxPaddingPercentage/100);
            int paddingHeight = (int) (face.FaceBox.Height*FaceImageBoxPaddingPercentage/100);
            xStartPosition = (int) face.FaceBox.X;
            yStartPosition = (int) face.FaceBox.Y;
            if (xStartPosition >= paddingWidth)
            {
                xStartPosition = xStartPosition - paddingWidth;
                width = width + paddingWidth;
            }
            else
            {
                width = width + xStartPosition;
                xStartPosition = 0;
            }
            if (yStartPosition >= paddingHeight)
            {
                yStartPosition = yStartPosition - paddingHeight;
                height = height + paddingHeight;
            }
            else
            {
                height = height + paddingHeight;
                yStartPosition = 0;
            }
            if (image.PixelWidth >= xStartPosition + width + paddingWidth)
            {
                width = width + paddingWidth;
            }
            else
            {
                width = image.PixelWidth - xStartPosition;
            }
            if (image.PixelHeight >= yStartPosition + height + paddingHeight)
            {
                height = height + paddingHeight;
            }
            else
            {
                height = image.PixelHeight - yStartPosition;
            }
        }
    }
}