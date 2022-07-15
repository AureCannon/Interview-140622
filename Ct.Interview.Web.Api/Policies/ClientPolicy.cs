using Polly;
using Polly.Extensions.Http;

namespace Ct.Interview.Web.Api.Policies
{
    public class ClientPolicy
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
              .HandleTransientHttpError()
              .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
              .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}
