using System.Net.Http;
using System.Collections.Generic;
using ShapesApp.Droid;
using System;
using System.Threading.Tasks;
using Refit;

[assembly: Xamarin.Forms.Dependency(
          typeof(GetShapePlatform))]
namespace ShapesApp.Droid
{
    public class GetShapePlatform : IGetShape
    {
        
        /* The secret key: REPLACE with shapes_api_key_placeholder if using SECRET-PROTECTION */
        string shapes_api_key = "yXClypapWNHIifHUWmBIyPFAm";
        // Refit API interface
        private IApiInterface apiClient;
        /* Comment out the line to use Approov SDK */
        private static HttpClient httpClient;
        /* Uncomment the line to use Approov SDK */
        //private static ApproovHttpClient httpClient;
        public GetShapePlatform()
        {
            /* Comment out the line to use Approov SDK */
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Api-Key", shapes_api_key);
            /* Uncomment the lines bellow to use Approov SDK */
            //var factory = new ApproovHttpClientFactory();
            //httpClient = factory.GetApproovHttpClient("<enter-your-config-string-here>");
            // Add substitution header: Uncomment if using SECRET-PROTECTION
            //AndroidApproovHttpClient.AddSubstitutionHeader("Api-Key", null);
            try
            {
                apiClient = RestService.For<IApiInterface>(httpClient);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception during RestService: " + ex.Message);
            }
        }

        public Dictionary<string, string> GetHello()
        {
            Task<Dictionary<string, string>> response = GetHelloAsync();
            return response.Result;
        }

        private async Task<Dictionary<string, string>> GetHelloAsync()
        {
            try
            {
                Dictionary<string, string> response = await apiClient.GetHello().ConfigureAwait(false);
                response["response"] = "OK";
                return response;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine("Exception in getHello(): " + e.Message);
                Dictionary<string, string> response = new Dictionary<string, string>();
                response["response"] = e.ReasonPhrase;
                return response;
            }
        }

        public Dictionary<string, string> GetShape()
        {
            Task<Dictionary<string, string>> response = GetShapeAsync();
            return response.Result;
        }

        private async Task<Dictionary<string, string>> GetShapeAsync()
        {
            try
            {
                Dictionary<string, string> response = await apiClient.GetShape().ConfigureAwait(false);
                response["response"] = "OK";
                return response;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine("Exception in GetShape(): " + e.Message);
                Dictionary<string, string> response = new Dictionary<string, string>();
                response["response"] = e.ReasonPhrase;
                return response;
            }
        }
    }
}
