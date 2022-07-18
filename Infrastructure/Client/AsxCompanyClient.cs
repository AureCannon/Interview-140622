using Core;
using Polly;
using Polly.Retry;
using System.Linq;
using System.Net;

namespace Infrastructure.Client
{
    public class AsxCompanyClient
    {
        private readonly HttpClient _httpClient;
        private AsyncRetryPolicy _retryPolicy;

        public AsxCompanyClient(HttpClient httpClient, AsxSettings settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.ListedSecuritiesCsvUrl);
            var maxRetries = 3;
            HttpStatusCode?[] httpStatusCodesWorthRetrying =
            {
                HttpStatusCode.RequestTimeout,
                HttpStatusCode.BadGateway,
                HttpStatusCode.ServiceUnavailable,
                HttpStatusCode.GatewayTimeout,
                HttpStatusCode.InternalServerError
            };
            var l = httpStatusCodesWorthRetrying.Contains(HttpStatusCode.NoContent);
            _retryPolicy = Policy
                .Handle<HttpRequestException>(ex => httpStatusCodesWorthRetrying.Contains(ex.StatusCode))
                .WaitAndRetryAsync(maxRetries, retryAttempt =>
                {
                    var timeToWait = TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
                    ///TODO: logger here
                    return timeToWait;
                });
        }

        public async Task<string> GetAsxCompaniesAsStringAsync()
        {
            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                var response = await _httpClient.GetAsync("");
                return await response.Content.ReadAsStringAsync();
            });

            return result;
        }
    }
}
