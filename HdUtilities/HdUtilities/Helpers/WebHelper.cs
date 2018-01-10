using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using HdUtilities.Extensions;

namespace HdUtilities.Helpers
{
    public class WebHelper
    {
        /// <summary>
        /// This method can be used to download text, HTML, JSON, or other text from a URL. It is an ansynchronous method that can report progress.
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetStringFromUrl(
            string url,
            ICredentials credential = null, 
            DownloadProgressChangedEventHandler downloadProgressHandler = null,
            AsyncCompletedEventHandler downloadCompleteHandler = null)
        {
            using (var webClient = new WebClient())
            {
                if (credential != null)
                {
                    webClient.Credentials = credential;
                }

                if (downloadProgressHandler != null)
                {
                    webClient.DownloadProgressChanged += downloadProgressHandler;
                }
                if (downloadCompleteHandler != null)
                {
                    webClient.DownloadFileCompleted += downloadCompleteHandler;
                }

                return await webClient.DownloadStringTaskAsync(url);
            }
        }

        /// <summary>
        /// This method can be used to download binary data from a URL. It is an ansynchronous method that can report progress.
        /// </summary>
        /// <returns></returns>
        public async Task<byte[]> GetBytesFromUrl(
            string url,
            ICredentials credential = null,
            DownloadProgressChangedEventHandler downloadProgressHandler = null,
            AsyncCompletedEventHandler downloadCompleteHandler = null)
        {
            using (var webClient = new WebClient())
            {
                if (credential != null)
                {
                    webClient.Credentials = credential;
                }

                if (downloadProgressHandler != null)
                {
                    webClient.DownloadProgressChanged += downloadProgressHandler;
                }
                if (downloadCompleteHandler != null)
                {
                    webClient.DownloadFileCompleted += downloadCompleteHandler;
                }

                return await webClient.DownloadDataTaskAsync(url);
            }
        }

        /// <summary>
        /// This method can be used to download text, HTML, JSON, or other data from a URL and saves it to a file. It is an ansynchronous method that can report progress.
        /// </summary>
        /// <returns></returns>
        public async Task DownloadFileFromUrl(
            string url,
            string destinationFilePath,
            ICredentials credential = null,
            DownloadProgressChangedEventHandler downloadProgressHandler = null,
            AsyncCompletedEventHandler downloadCompleteHandler = null)
        {
            if (url.IsNullOrWhitespace())
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (destinationFilePath.IsNullOrWhitespace())
            {
                throw new ArgumentNullException(nameof(destinationFilePath));
            }
            
            using (var webClient = new WebClient())
            {
                if (credential != null)
                {
                    webClient.Credentials = credential;
                }

                if (downloadProgressHandler != null)
                {
                    webClient.DownloadProgressChanged += downloadProgressHandler;
                }
                if (downloadCompleteHandler != null)
                {
                    webClient.DownloadFileCompleted += downloadCompleteHandler;
                }

                await webClient.DownloadFileTaskAsync(url,destinationFilePath);
            }
        }
    }
}