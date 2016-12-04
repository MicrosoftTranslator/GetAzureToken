using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CSharp_TranslateSample
{
    class Program
    {
        private const string SubscriptionKey = "your subscription key";   //Enter here the Key from your Microsoft Translator Text subscription on http://portal.azure.com

        static void Main(string[] args)
        {
            Console.WriteLine("Sync");
            Translate();
            Console.WriteLine("Async");
            TranslateAsync().Wait();
            Console.ReadKey();
        }

        /// Demonstrates getting an access token and using the token to translate.
        private static async Task TranslateAsync()
        {
            var translatorService = new TranslatorService.LanguageServiceClient();
            var authTokenSource = new AzureAuthToken(SubscriptionKey);
            var token = string.Empty;

            try
            {
                token = await authTokenSource.GetAccessTokenAsync();
            }
            catch (HttpRequestException)
            {
                switch (authTokenSource.RequestStatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        Console.WriteLine("Request to token service is not authorized (401). Check that the Azure subscription key is valid.");
                        break;
                    case HttpStatusCode.Forbidden:
                        Console.WriteLine("Request to token service is not authorized (403). For accounts in the free-tier, check that the account quota is not exceeded.");
                        break;
                }
                throw;
            }

            Console.WriteLine("Translated to French: {0}", translatorService.Translate(token, "Hello World", "en", "fr", "text/plain", "general", string.Empty));
        }

        /// Demonstrates getting an access token and using the token to translate synchronously.
        private static void Translate()
        {
            var translatorService = new TranslatorService.LanguageServiceClient();
            var authTokenSource = new AzureAuthToken(SubscriptionKey);
            var token = string.Empty;

            try
            {
                token = authTokenSource.GetAccessToken();
            }
            catch (HttpRequestException)
            {
                switch (authTokenSource.RequestStatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        Console.WriteLine("Request to token service is not authorized (401). Check that the Azure subscription key is valid.");
                        break;
                    case HttpStatusCode.Forbidden:
                        Console.WriteLine("Request to token service is not authorized (403). For accounts in the free-tier, check that the account quota is not exceeded.");
                        break;
                }
                throw;
            }

            Console.WriteLine("Translated to Italian: {0}", translatorService.Translate(token, "Hello World", "en", "it", "text/plain", "general", string.Empty));
        }

    }
}
