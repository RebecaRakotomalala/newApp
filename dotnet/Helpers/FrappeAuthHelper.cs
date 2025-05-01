using System.Net.Http;

namespace newApp.Helpers
{
    public static class FrappeAuthHelper
    {
        private const string apiKey = "73350ed9ae7972a";
        private const string apiSecret = "16105bb9f498033";

        public static void AjouterAuthorization(HttpClient client)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"token {apiKey}:{apiSecret}");
        }
    }
}
