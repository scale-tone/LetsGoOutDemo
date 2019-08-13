using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace LetsGoOutDemo.ServiceFabric.WebApi
{
    static class Utils
    {
        /// <summary>
        /// Asp.Net Core can't map plain text to method parameters, so need to handcraft request body parsing
        /// </summary>
        public static async Task<string> ReadAsStringAsync(this HttpRequest request)
        {
            using(var reader = new StreamReader(request.Body))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
