using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;

namespace Blogless
{
	public static class BlogFunctions
    {
        private const string ApiBlogPath = "api/blog/";

        [FunctionName(nameof(BlogFunctions) + "-" + nameof(GetBlog))]
        public static async Task<IActionResult> GetBlog(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ApiBlogPath)] HttpRequest req)
            => new OkObjectResult("Get blog");

        [FunctionName(nameof(BlogFunctions) + "-" + nameof(UpdateBlog))]
        public static async Task<IActionResult> UpdateBlog(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = ApiBlogPath)] HttpRequest req)
            => new OkObjectResult("Update blog");
    }
}
