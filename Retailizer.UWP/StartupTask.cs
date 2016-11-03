using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Devices.Enumeration;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.FaceAnalysis;
using Windows.Media.MediaProperties;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage.Streams;
using Microsoft.Practices.Unity;

namespace Retailizer.UWP
{
    public sealed class StartupTask : IBackgroundTask
    {
        public BackgroundTaskDeferral Deferral { get; private set; }


        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            Deferral = taskInstance.GetDeferral();

            UnityContainer container = await InitializeDiContainer();

            try
            {
                await
                    InitializeCameraAsync()
                        .ContinueWith(devices => Capture(devices.Result, container.Resolve<IImageFilter>()));
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.WriteLine(string.Concat("Access to camera denied. Message: ", ex.Message));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Concat("Not specified exception. Message: ", ex.Message));
            }
        }

        private static async Task<UnityContainer> InitializeDiContainer()
        {
            UnityContainer container = new UnityContainer();

            container.RegisterType<IConfiguration, StaticConfiguration>();
            container.RegisterType<IDeviceConfiguration, DeviceConfiguration>();
            container.RegisterInstance(typeof(EasClientDeviceInformation), new EasClientDeviceInformation());
            container.RegisterType<IImagePersiter, AzureImagePersister>();
            container.RegisterType<IImageFilter, LocalFaceDetector>();
            container.RegisterInstance(typeof(FaceDetector),
                FaceDetector.IsSupported ? await FaceDetector.CreateAsync() : null);

            return container;
        }

        private static async Task SetMaxResolution(MediaCapture mediaCapture)
        {
            IReadOnlyList<IMediaEncodingProperties> res =
                mediaCapture.VideoDeviceController.GetAvailableMediaStreamProperties(MediaStreamType.VideoPreview);
            uint maxResolution = 0;
            int indexMaxResolution = 0;

            if (res.Count >= 1)
            {
                for (int i = 0; i < res.Count; i++)
                {
                    VideoEncodingProperties vp = (VideoEncodingProperties) res[i];

                    if (vp.Width <= maxResolution) continue;
                    indexMaxResolution = i;
                    maxResolution = vp.Width;
                }
                await
                    mediaCapture.VideoDeviceController.SetMediaStreamPropertiesAsync(MediaStreamType.VideoPreview,
                        res[indexMaxResolution]);
            }
        }

        private static async Task<MediaCapture[]> InitializeCameraAsync()
        {
            List<MediaCapture> mediaCaptureDevices = new List<MediaCapture>();
            DeviceInformationCollection videoDevices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);

            if (!videoDevices.Any())
            {
                Debug.WriteLine("No cameras found.");
                return mediaCaptureDevices.ToArray();
            }
            foreach (DeviceInformation device in videoDevices)
            {
                MediaCapture mediaCapture = new MediaCapture();

                MediaCaptureInitializationSettings mediaInitSettings = new MediaCaptureInitializationSettings
                {
                    VideoDeviceId = device.Id
                };

                await mediaCapture.InitializeAsync(mediaInitSettings);
                await SetMaxResolution(mediaCapture);


                mediaCaptureDevices.Add(mediaCapture);
            }

            return mediaCaptureDevices.ToArray();
        }

        private static async Task<Tuple<BitmapDecoder, IRandomAccessStream>> GetPhotoStreamAsync(
            MediaCapture mediaCapture)
        {
            InMemoryRandomAccessStream photoStream = new InMemoryRandomAccessStream();
            await mediaCapture.CapturePhotoToStreamAsync(ImageEncodingProperties.CreateJpeg(), photoStream);
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(photoStream);
            return new Tuple<BitmapDecoder, IRandomAccessStream>(decoder, photoStream.CloneStream());
        }

        private async Task Capture(IEnumerable<MediaCapture> devices, IImageFilter filter)
        {
            IEnumerable<MediaCapture> cameraDevices = devices as MediaCapture[] ?? devices.ToArray();
            while (true)
            {
                foreach (MediaCapture cameraDevice in cameraDevices)
                {
                    string cameraId = cameraDevice.MediaCaptureSettings.VideoDeviceId;

                    Debug.WriteLine($"Processing camera: {cameraId}");
                    BitmapDecoder bitmapDecoder;
                    IRandomAccessStream imageStream;
                    try
                    {
                        Tuple<BitmapDecoder, IRandomAccessStream> photoData = await GetPhotoStreamAsync(cameraDevice);
                        bitmapDecoder = photoData.Item1;
                        imageStream = photoData.Item2;

                        Debug.WriteLine($"Got stream from camera: {cameraId}");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Camera {cameraId} failed with message: {ex.Message}");
                        continue;
                    }
#pragma warning disable 4014
                    filter?.ProcessImageAsync(bitmapDecoder, imageStream, cameraId);
#pragma warning restore 4014
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}