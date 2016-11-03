// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
// 
// Microsoft Cognitive Services: http://www.microsoft.com/cognitive
// 
// Microsoft Cognitive Services Github:
// https://github.com/Microsoft/Cognitive
// 
// Copyright (c) Microsoft Corporation
// All rights reserved.
// 
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailizer.Common.Services
{
    public class FaceApiService
    {
        private const int retryCountOnQuotaLimitError = 10;
        private const int retryDelayOnQuotaLimitError = 1000;

        public static Action Throttled; // Set externally what would be called when throttled.

        private FaceServiceClient faceClient;
        
        public FaceApiService(string apiKey)
        {
            faceClient = new FaceServiceClient(apiKey);
        }

        private async Task<TResponse> RunTaskWithAutoRetryOnQuotaLimitExceededError<TResponse>(Func<Task<TResponse>> action)
        {
            int retriesLeft = FaceApiService.retryCountOnQuotaLimitError;
            int delay = FaceApiService.retryDelayOnQuotaLimitError;

            TResponse response = default(TResponse);

            while (true)
            {
                try
                {
                    response = await action();
                    break;
                }
                catch (FaceAPIException exception) when (exception.HttpStatus == (System.Net.HttpStatusCode)429 && retriesLeft > 0)
                {
                    //ErrorTrackingHelper.TrackException(exception, "Face API throttling error");
                    Debug.WriteLine($"Face API throttled. Retries left: {retriesLeft}, Delay: {delay} ms.");
                    if (retriesLeft == 1 && Throttled != null)
                    {
                        Throttled();
                    }

                    await Task.Delay(delay);
                    retriesLeft--;
                    delay *= 2;
                    continue;
                }
                catch (FaceAPIException ex)
                {
                    Debug.WriteLine($"{ex.ErrorCode}: {ex.ErrorMessage}");
                }
            }

            return response;
        }

        private async Task RunTaskWithAutoRetryOnQuotaLimitExceededError(Func<Task> action)
        {
            await RunTaskWithAutoRetryOnQuotaLimitExceededError<object>(async () => { await action(); return null; });
        }

        public async Task<Face[]> DetectAsync(Stream imageStream, bool returnFaceId = true, bool returnFaceLandmarks = false, IEnumerable<FaceAttributeType> returnFaceAttributes = null)
        {
            return await RunTaskWithAutoRetryOnQuotaLimitExceededError(
                () =>
                {
                    Stream copiedStream = new MemoryStream();
                    imageStream.Seek(0, SeekOrigin.Begin);
                    imageStream.CopyTo(copiedStream);
                    copiedStream.Seek(0, SeekOrigin.Begin);
                    return faceClient.DetectAsync(copiedStream, returnFaceId, returnFaceLandmarks, returnFaceAttributes);
                });
        }

        public async Task<IdentifyResult[]> IdentifyAsync(string personGroupId, Guid[] faceIds, int maxNumberOfCandidatesReturned = 1)
        {
            return await RunTaskWithAutoRetryOnQuotaLimitExceededError(() => faceClient.IdentifyAsync(personGroupId, faceIds, maxNumberOfCandidatesReturned));
        }

        public async Task<CreatePersonResult> CreatePersonAsync(string personGroupId, string name, string userData = null)
        {
            return await RunTaskWithAutoRetryOnQuotaLimitExceededError(() => faceClient.CreatePersonAsync(personGroupId, name, userData));
        }
        public async Task<Person> GetPersonAsync(string personGroupId, Guid personId)
        {
            return await RunTaskWithAutoRetryOnQuotaLimitExceededError<Person>(() => faceClient.GetPersonAsync(personGroupId, personId));
        }

        public async Task AddPersonFaceAsync(string personGroupId, Guid personId, Stream imageStream, string userData = null, FaceRectangle targetFace = null)
        {
            await RunTaskWithAutoRetryOnQuotaLimitExceededError(
                () => {
                    Stream copiedStream = new MemoryStream();
                    imageStream.Seek(0, SeekOrigin.Begin);
                    imageStream.CopyTo(copiedStream);
                    copiedStream.Seek(0, SeekOrigin.Begin);
                    return faceClient.AddPersonFaceAsync(personGroupId, personId, copiedStream, userData, targetFace);
                });
        }

        public async Task DeletePersonFaceAsync(string personGroupId, Guid personId, Guid faceId)
        {
            await RunTaskWithAutoRetryOnQuotaLimitExceededError(() => faceClient.DeletePersonFaceAsync(personGroupId, personId, faceId));
        }

        public async Task TrainPersonGroupAsync(string personGroupId)
        {
            await RunTaskWithAutoRetryOnQuotaLimitExceededError(() => faceClient.TrainPersonGroupAsync(personGroupId));
        }
    }
}
