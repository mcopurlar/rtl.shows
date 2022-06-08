using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace Rtl.Shows.Scraper.Services.ServiceClients;

public class TvMazeRetryPolicy
{
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(7, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (result, span, attemptNumber, arg4) =>
                {
                    Console.WriteLine($"******************{result?.Result?.StatusCode}*************** {DateTime.Now.ToString("hh.mm.ss.ffffff")} <{attemptNumber}>");
                });
    }
}