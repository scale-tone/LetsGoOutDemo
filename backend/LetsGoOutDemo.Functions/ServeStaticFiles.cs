using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;

namespace LetsGoOutDemo.Functions
{
    public static class ServeStaticFiles
    {
        // Serves static files for client UI
        [FunctionName("ui")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ui/{path1?}/{path2?}/{path3?}")] 
                HttpRequest req,
            string path1, string path2, string path3
        )
        {
            switch(path1)
            {
                case "static":
                    return ServeStatic(path1, path2, path3);
                case "manifest.json":
                    return ServeManifestJson();
                case "favicon.ico":
                    return ServeFavicon();
                default:
                    return ServeIndexHtml();
            }
        }

        private const string wwwroot = "./wwwroot";
        private static IActionResult ServeStatic(string path1, string path2, string path3)
        {
            string contentType;
            switch (path2)
            {
                case "css":
                    contentType = "text/css; charset=utf-8";
                    break;
                case "media":
                    contentType = "image/svg+xml; charset=UTF-8";
                    break;
                default:
                    contentType = "application/javascript; charset=UTF-8";
                    break;
            }
            return new FileStreamResult(File.OpenRead($"{wwwroot}/{path1}/{path2}/{path3}"), contentType);
        }
        private static IActionResult ServeManifestJson()
        {
            return new FileStreamResult(File.OpenRead($"{wwwroot}/manifest.json"), "application/json; charset=UTF-8");
        }
        private static IActionResult ServeFavicon()
        {
            return new FileStreamResult(File.OpenRead($"{wwwroot}/favicon.ico"), "image/x-icon");
        }
        private static IActionResult ServeIndexHtml()
        {
            return new FileStreamResult(File.OpenRead($"{wwwroot}/index.html"), "text/html; charset=UTF-8");
        }
    }
}
