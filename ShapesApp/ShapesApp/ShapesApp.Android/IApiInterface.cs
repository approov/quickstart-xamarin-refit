using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
namespace ShapesApp.Droid
{
    public interface IApiInterface
    {
        [Get("/v1/hello")]
        Task<Dictionary<string, string>> GetHello();
        [Get("/v2/shapes")]
        Task<Dictionary<string, string>> GetShape();
    }
}
