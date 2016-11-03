using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace Retailizer.UWP
{
    internal interface IImageFilter
    {
        Task ProcessImageAsync(BitmapDecoder bitmapDecoder, IRandomAccessStream imageStream, string cameraId);
    }
}