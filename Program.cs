using Microsoft.Translator.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_TranslateSample
{
    class Program
    {

        const string subscriptionKey = "21c8b1995c444ec0ad246f0eb440aa9d";

        static void Main(string[] args)
        {
            TranslatorService.LanguageServiceClient translatorService = new TranslatorService.LanguageServiceClient();

            //An example of how to call the methods to get a token
            AzureAuthTokenSource authTokenSource = new AzureAuthTokenSource();
            string token = string.Empty;

            try
            {
                token = authTokenSource.GetAccessTokenAsync(subscriptionKey).GetAwaiter().GetResult();
            }
            catch
            {
                throw new Exception("Invalid Azure subscription key");
            }


            Console.WriteLine("Translated to French: {0}", translatorService.Translate(token, "Hello World", "en", "fr", "text/plain", "general", string.Empty));
            Console.ReadKey();
        }
    }
}
