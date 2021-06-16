using Blogless.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Blogless
{
	public static class FunctionExtentions
    {
        public static T DeserializeBodyJson<T>(this HttpRequest req)
        {
			JsonSerializer serializer = new();
			using var sr = new StreamReader(req.Body);
			using var jsonTextReader = new JsonTextReader(sr);
			return serializer.Deserialize<T>(jsonTextReader);
		}
        public static Task<IActionResult> AsActionResultAsync<T>(this Task<(Result, T)> task)
            => task.ContinueWith<IActionResult>((Task<(Result Result, T Value)> completedTask) 
                => completedTask.Result.Result switch {
                    Result.Success when completedTask.Result.Value is Stream s => new FileStreamResult(s, "text/plain"),
                    Result.Success => new OkObjectResult(completedTask.Result.Value),
                    Result.NotFound => new NotFoundResult(),
                    _ => throw new NotImplementedException(),
                }, TaskContinuationOptions.OnlyOnRanToCompletion);

        public static Task<IActionResult> AsActionResultAsync(this Task<Result> task)
            => task.ContinueWith<IActionResult>((Task<Result> completedTask)
                => completedTask.Result switch {
                    Result.Success => new AcceptedResult(),
                    Result.NotFound => new NotFoundResult(),
                    _ => throw new NotImplementedException(),
                }, TaskContinuationOptions.OnlyOnRanToCompletion);
    }
}
