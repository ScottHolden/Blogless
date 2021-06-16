using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogless.Services
{
    public record Page(string Title);
	public record Blog(string Title);
	public class PageService
    {
        public async Task<(Result, Page)> GetPageAsync(Guid pageId)
        {
            return (Result.Success, new Page("Hello"));
		}
        public async Task<(Result, string)> GetPageContentAsync(Guid pageId)
        {
            return (Result.Success, "Page Content");
        }
		public async Task<(Result, Page[])> ListPagesAsync()
		{
			return (Result.Success, new[] { new Page("Hello"), new Page("Second") });
		}
		public async Task<(Result, Guid)> CreatePageAsync(Page body)
		{
			return (Result.Success, Guid.NewGuid());
		}
		public async Task<Result> UpdatePageContentAsync(Guid id, Stream body)
		{
			return Result.Success;
		}
		public async Task<Result> UpdatePageAsync(Guid id, Page body)
		{
			return Result.Success;
		}
		public async Task<Result> RemovePageAsync(Guid id)
		{
			return Result.Success;
		}
	}
    public class BlogService
    {
		public async Task<(Result, Blog)> GetBlogAsync(Guid pageId)
		{
			return (Result.Success, new Blog("Hello"));
		}
		public async Task<Result> UpdateBlogAsync(Guid id, Blog body)
		{
			return Result.Success;
		}
	}

    public enum Result
    {
        Success,
        NotFound
	}
}
