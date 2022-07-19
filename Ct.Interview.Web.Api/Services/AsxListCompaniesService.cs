using Core.Entities;
using Core.Repository;
using Core.Service;
using Infrastructure.Caching;
using Polly;
using Polly.Retry;

namespace Ct.Interview.Web.Api.Services
{
    public class AsxListCompaniesService : IAsxListedCompaniesService
    {
        private readonly ICache _cache;
        private readonly IRepository<AsxListedCompany> _repository;
        private readonly AsyncRetryPolicy _retryPolicy;
        private readonly ILogger<AsxListCompaniesService> _logger;
        public AsxListCompaniesService(ILogger<AsxListCompaniesService> logger, ICache cache, IRepository<AsxListedCompany> repository)
        {
            _cache = cache;
            _repository = repository;
            _logger = logger;
            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, retryCount, context) =>
                {
                    _logger.LogError($"Error: {ex.Message}, retry count {retryCount}");
                });
        }

        public async Task<AsxListedCompany[]> GetByAsxCode(string asxCode)
        {
            var result = await _retryPolicy.ExecuteAsync(async () => await GetByAsxCodeFromCacheOrDb(asxCode));

            return result.ToArray();
        }

        public async Task<AsxListedCompany[]> GetByAsxCodeFromCacheOrDb(string asxCode)
        {
            var code = asxCode.ToLower();
            var result = await _cache.GetOrCreateAsync(code,
                async () => await _repository.FindEntitiesByConditionAsync(e => e.AsxCode.ToLower() == code));

            return result.ToArray();
        }
    }
}
