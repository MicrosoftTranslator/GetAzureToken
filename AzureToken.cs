using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Translator.Api
{
    /// <summary>
    /// Client to call Cognitive Services Azure Auth Token service in order to get an access token.
    /// </summary>
    public class AzureAuthTokenSource
    {
        private const string serviceUrl = "https://api.cognitive.microsoft.com/sts/v1.0/issueToken";
        private const string OcpApimSubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";
        private TimeSpan tokenLifeTime = new TimeSpan(0, 5, 0); //set token lifetime to 5 minutes
        private static string storedTokenValue = string.Empty;
        private static DateTime storedTokenTime = DateTime.MinValue;
        public static string subscriptionKey { get; set; } = string.Empty;

        public Uri ServiceUrl { get; private set; }

        /// <summary>
        /// Creates a client to obtain an access token.
        /// </summary>
        /// <param name="serviceUrl">URL of the service to target.</param>
        public AzureAuthTokenSource(string Key)
        {
            if (string.IsNullOrWhiteSpace(serviceUrl)) throw new ArgumentNullException(nameof(serviceUrl));
            subscriptionKey = Key;
            Uri actualUri;
            if (!Uri.TryCreate(serviceUrl, UriKind.Absolute, out actualUri))
            {                
                throw new ArgumentException(nameof(serviceUrl), $"Invalid service URL: {serviceUrl}");
            }
            this.ServiceUrl = new Uri(serviceUrl);
        }

        /// <summary>
        /// Gets a token for the specified subscription. Returned token is valid for 5 minutes.
        /// </summary>
        /// <param name="subscriptionKey">Subscription secret key.</param>
        /// <returns>The encoded JWT token. If less that 5 minutes have elapsed since last request, the previous token is returned.</returns>
        public async Task<string> GetAccessTokenAsync()
        {
            if ((DateTime.Now - storedTokenTime) < tokenLifeTime ) return storedTokenValue;
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = this.ServiceUrl;
                request.Content = new StringContent(string.Empty);
                request.Headers.TryAddWithoutValidation(OcpApimSubscriptionKeyHeader, subscriptionKey);
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var token = await response.Content.ReadAsStringAsync();
                storedTokenTime = DateTime.Now;
                storedTokenValue = "Bearer " + token;
                return "Bearer " + token;
            }
        }
    }
}
