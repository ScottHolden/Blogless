using Blogless.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Blogless
{
    public static class PageFunctions
    {
        private const string ApiPagePath = "api/page/";
        private const string ApiPageIdPath = ApiPagePath + "{id:guid}";
        private const string ApiPageIdContentPath = ApiPageIdPath + "/content";

        private static readonly PageService _pageService = new PageService();

        [FunctionName(nameof(PageFunctions) + "-" + nameof(PageBase))]
        public static Task<IActionResult> PageBase(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = ApiPagePath)] HttpRequest req,
            ILogger log)
            => req.Method switch
            {
                "get" => _pageService.ListPagesAsync().AsActionResultAsync(),
                "post" => _pageService.CreatePageAsync(req.DeserializeBodyJson<Page>()).AsActionResultAsync(),
                _ => NotFound()
            };

        [FunctionName(nameof(PageFunctions) + "-" + nameof(PageItem))]
        public static Task<IActionResult> PageItem(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "put", "delete", Route = ApiPageIdPath)] HttpRequest req,
            Guid id)
            => req.Method switch
            {
                "get" => _pageService.GetPageAsync(id).AsActionResultAsync(),
                "put" => _pageService.UpdatePageAsync(id, req.DeserializeBodyJson<Page>()).AsActionResultAsync(),
                "delete" => _pageService.RemovePageAsync(id).AsActionResultAsync(),
                _ => NotFound()
            };

        [FunctionName(nameof(PageFunctions) + "-" + nameof(PageContent))]
        public static Task<IActionResult> PageContent(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "put", Route = ApiPageIdContentPath)] HttpRequest req,
            Guid id)
            => req.Method switch
            {
                "get" => _pageService.GetPageContentAsync(id).AsActionResultAsync(),
                "put" => _pageService.UpdatePageContentAsync(id, req.Body).AsActionResultAsync(),
                _ => NotFound()
            };

        private static Task<IActionResult> NotFound()
            => Task.FromResult((IActionResult)new NotFoundResult());
    }
}
