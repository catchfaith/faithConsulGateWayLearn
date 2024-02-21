using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsulRegistHelper.HttpHelper
{
    public interface IHttpExtensionsService
    {
        ValueTask<string> GetAsync(string url);
        ValueTask<string> PostAddHeaderAsync(string url, object body, Dictionary<string, string> dictionary);
        ValueTask<string> PostAsync(string url, object body);
        ValueTask<string> PostNoBodyAsync(string url);
    }
}