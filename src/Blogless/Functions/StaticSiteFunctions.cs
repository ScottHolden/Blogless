using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;

namespace Blogless
{
	public static class StaticSiteFunctions
    {
        [FunctionName(nameof(StaticSiteFunctions) + "-" + nameof(IndexFile))]
        public static IActionResult IndexFile(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "/")] HttpRequest req)
            => GetFile("index.html");

        [FunctionName(nameof(StaticSiteFunctions) + "-" + nameof(AssetsFolder))]
        public static IActionResult AssetsFolder(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "assets/{path}")] HttpRequest req,
            string path)
            => GetFile("assets/" + path);


        private static IActionResult GetFile(string path)
        {
            IFileManager fileManager = new EmbededFileManager();
            IContentTypeProvider contentTypeProvider = new FileExtensionContentTypeProvider();

            if (!fileManager.TryGetFileStream(path, out Stream stream))
            {
                return new NotFoundResult();
            }

            if (!contentTypeProvider.TryGetContentType(path, out string contentType))
            {
                contentType = "application/octet-stream";
            }

            return new FileStreamResult(stream, contentType);
        }
    }
}
