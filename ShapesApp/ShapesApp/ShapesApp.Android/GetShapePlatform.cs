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

        /* The endpoint version being used: v1 unprotected and v3 for Approov API protection */
        public const string endpointVersion = "v3";
        /* The Shapes URL */
        public const string shapesURL = "https://shapes.approov.io/" + endpointVersion + "/shapes/";
        /* The Hello URL */
        public const string helloURL = "https://shapes.approov.io/" + endpointVersion + "/hello/";
        /* The secret key: REPLACE with shapes_api_key_placeholder if using SECRETS-PROTECTION */
        string shapes_api_key = "yXClypapWNHIifHUWmBIyPFAm";
        // Refit API interface
        private IApiInterface apiClient;
        /* COMMENT this line if using Approov */
        private static HttpClient httpClient;
        /* UNCOMMENT this line if using Approov */
        //private static ApproovHttpClient httpClient;
        public GetShapePlatform()
        {
            /* COMMENT out the line to use Approov SDK */
            httpClient = new HttpClient();
            /* UNCOMMENT the lines bellow to use Approov SDK */
            //ApproovService.Initialize("<enter-your-config-string-here>");
            //httpClient = ApproovService.CreateHttpClient();
            // Add substitution header: Uncomment if using SECRETS-PROTECTION
            //ApproovService.AddSubstitutionHeader("Api-Key", null);
            httpClient.DefaultRequestHeaders.Add("Api-Key", shapes_api_key);
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
