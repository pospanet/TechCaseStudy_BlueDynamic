using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Retailizer.UWP
{
    internal interface IImagePersiter
    {
        Task PersistAsync(IEnumerable<Stream> streams, string cameraId);
    }
}