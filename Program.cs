using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Translator.API;

namespace CSharp_TranslateSample
{
    class Program
    {
        private const string SubscriptionKey = "your subscription key";   //Enter here the Key from your Microsoft Translator Text subscription on http://portal.azure.com

        static void Main(string[] args)
        {
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
    }
}
