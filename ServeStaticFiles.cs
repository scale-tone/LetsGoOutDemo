using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace LetsGoOutDemo.Functions
{
    public static class ServeStaticFiles
    {
        // Serves static files for client UI
        [FunctionName("ui")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ui/{p1?}/{p2?}/{p3?}")] HttpRequest req
        )
        {
            var path = req.Path.Value.Replace("/api/ui", wwwroot);
            var contentType = FileMap.FirstOrDefault((kv => path.StartsWith(kv[0])));
            if(contentType != null)
            {
                return File.Exists(path) ? 
                    (IActionResult)new FileStreamResult(File.OpenRead(path), contentType[1]) : 
                    new NotFoundResult();
            }
            // Returning index.html by default, to support client routing
            return new FileStreamResult(File.OpenRead($"{wwwroot}/index.html"), "text/html; charset=UTF-8");
        }

        private const string wwwroot = "wwwroot";
        private static readonly string[][] FileMap = new string[][]
        {
            new [] {$"{wwwroot}/static/css/", "text/css; charset=utf-8"},
            new [] {$"{wwwroot}/static/media/", "image/svg+xml; charset=UTF-8"},
            new [] {$"{wwwroot}/static/js/", "application/javascript; charset=UTF-8"},
            new [] {$"{wwwroot}/manifest.json", "application/json; charset=UTF-8"},
            new [] {$"{wwwroot}/favicon.ico", "image/x-icon"}
        };
    }
}
