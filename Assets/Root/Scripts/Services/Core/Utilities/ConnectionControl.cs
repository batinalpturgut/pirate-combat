using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace Root.Scripts.Services.Core.Utilities
{
    public static class ConnectionControl
    {
        private static readonly List<string> CheckUrls = new List<string>
        {
            "https://www.google.com",
            "https://www.microsoft.com",
            "https://www.apple.com",
            "https://www.amazon.com",
            "https://www.bing.com",
            "https://www.yahoo.com"
        };

        public static void CheckConnection(Action<bool> callback)
        {
            int remainingRequests = CheckUrls.Count;
            bool isConnected = false;

            foreach (string checkUrl in CheckUrls)
            {
                UnityWebRequest request = UnityWebRequest.Get(checkUrl);
                request.timeout = 10;

                request.SendWebRequest().completed += _ =>
                {
                    if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
                    {
                        if (!isConnected)
                        {
                            isConnected = true;
                            callback?.Invoke(true);
                        }
                    }

                    remainingRequests--;

                    if (remainingRequests == 0 && !isConnected)
                    {
                        callback?.Invoke(false);
                    }
                };
            }
        }
    }
}