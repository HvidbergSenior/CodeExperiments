using System.Net.Http.Headers;

namespace Insight.Tests.End2End
{
    public static class HttpClientExtensions
    {
        public static HttpClient SetToken(this HttpClient client, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return client;
        }

        public static HttpClient RemoveToken(this HttpClient client)
        {
            client.DefaultRequestHeaders.Remove("Authorization");

            return client;
        }
    }
}
