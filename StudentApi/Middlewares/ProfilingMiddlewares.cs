using System.Diagnostics;

namespace StudentApi.Middlewares
{
    public class ProfilingMiddlewares
    {
        private readonly RequestDelegate Next;
        private readonly ILogger<ProfilingMiddlewares> Logger;
        public ProfilingMiddlewares(RequestDelegate next, ILogger<ProfilingMiddlewares> logger)
        {
            Next = next;
            Logger = logger;
        }


        public async Task Invoke(HttpContext context)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            await Next(context);
            stopWatch.Stop();

            Logger.LogInformation($"{context.Request.Path}  {stopWatch.Elapsed}");

        }

    }
}
