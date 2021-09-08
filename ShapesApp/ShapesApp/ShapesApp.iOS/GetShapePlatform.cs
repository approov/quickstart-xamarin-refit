using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ShapesApp.iOS;
using Refit;

[assembly: Xamarin.Forms.Dependency(
          typeof(GetShapePlatform))]
namespace ShapesApp.iOS
{
    public class GetShapePlatform : IGetShape
    {
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
            /* Uncomment the lines bellow to use Approov SDK */
            //var aFactory = new ApproovHttpClientFactory();
            //httpClient = aFactory.GetApproovHttpClient("<enter-your-config-string-here>")
            httpClient.BaseAddress = new Uri("https://shapes.approov.io");
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
            catch (Refit.ApiException e) {
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
                // Add the response string
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
