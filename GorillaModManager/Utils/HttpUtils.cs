using System.Net.Http;

namespace GorillaModManager.Utils
{
    public static class HttpUtils
    {
        public static HttpClient MakeGMClient()
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Add("User-Agent", "Gorilla-Mod-Manager");

            return client;
        }
    }
}
