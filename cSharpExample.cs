
//An example of how to call the methods to get a token
authTokenSource = new AzureAuthToken("https://api.cognitive.microsoft.com/sts/v1.0/issueToken");
var token = await authTokenSource.GetAccessTokenAsync(this.AzureSubscriptionKeyForText);




using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.MT.Api.Client
{
    /// <summary>
    /// Client to call Cognitive Services Azure Auth Token service in order to get an access token.
    /// </summary>
    public class AzureAuthToken
    {
        private const string OcpApimSubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";

        public Uri ServiceUrl { get; private set; }

        /// <summary>
        /// Creates a client to obtain an access token.
        /// </summary>
        /// <param name="serviceUrl">URL of the service to target.</param>
        public AzureAuthToken(string serviceUrl)
        {
            if (string.IsNullOrWhiteSpace(serviceUrl)) throw new ArgumentNullException(nameof(serviceUrl));

            Uri actualUri;
            if (!Uri.TryCreate(serviceUrl, UriKind.Absolute, out actualUri))
            {                
                throw new ArgumentException(nameof(serviceUrl), $"Invalid service URL: {serviceUrl}");
            }
            this.ServiceUrl = new Uri(serviceUrl);
        }

        /// <summary>
        /// Gets a token for the specified subscription.
        /// </summary>
        /// <param name="subscriptionSecret">Subscription secret key.</param>
        /// <returns>The encoded JWT token.</returns>
        public async Task<string> GetAccessTokenAsync(string subscriptionSecret)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = this.ServiceUrl;
                request.Content = new StringContent(string.Empty);
                request.Headers.TryAddWithoutValidation(OcpApimSubscriptionKeyHeader, subscriptionSecret);
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var token = await response.Content.ReadAsStringAsync();
                return token;
            }
        }

        /// <summary>
        /// Gets a token for the specified subscription. The token is prefixed with "Bearer ".
        /// </summary>
        /// <param name="subscriptionSecret">Subscription secret key.</param>
        /// <returns>The encoded JWT token, prefixed with "Bearer ".</returns>
        public async Task<string> GetBearerAccessTokenAsync(string subscriptionSecret)
        {
            return "Bearer " + await this.GetAccessTokenAsync(subscriptionSecret);
        }
    }
}
