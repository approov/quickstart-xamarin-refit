using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
namespace ShapesApp.iOS
{
    public interface IApiInterface
    {
        [Get(GetShapePlatform.helloURL)]
        Task<Dictionary<string, string>> GetHello();
        [Get(GetShapePlatform.shapesURL)]
        Task<Dictionary<string, string>> GetShape();
    }
}