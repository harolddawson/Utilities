using System;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;

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
    }
}