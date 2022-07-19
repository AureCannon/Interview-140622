using Core;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AsxCompanyClient> _logger;
        public AsxCompanyClient(ILogger<AsxCompanyClient> logger, HttpClient httpClient, AsxSettings settings)
        {
            _httpClient = httpClient;
            _logger = logger;
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
                .WaitAndRetryAsync(maxRetries, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, timespan, retryCount) =>
                {
                    _logger.LogError($"Error: {ex.Message}, retry count {retryCount}");
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
