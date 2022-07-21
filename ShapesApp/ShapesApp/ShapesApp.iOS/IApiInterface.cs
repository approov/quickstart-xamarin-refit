using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
namespace ShapesApp.iOS
{
    public interface IApiInterface
    {
        [Get("https://shapes.approov.io/v1/hello")]
        Task<Dictionary<string, string>> GetHello();
        [Get("https://shapes.approov.io/v1/shapes/")]
        Task<Dictionary<string, string>> GetShape();
    }
}