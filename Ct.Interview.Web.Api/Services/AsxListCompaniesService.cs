using Core.Entities;
using Core.Repository;
using Core.Service;
using Infrastructure.Caching;

namespace Ct.Interview.Web.Api.Services
{
    public class AsxListCompaniesService : IAsxListedCompaniesService
    {
        private readonly ICache _cache;
        private readonly IRepository<AsxListedCompany> _repository;

        public AsxListCompaniesService(ICache cache, IRepository<AsxListedCompany> repository)
        {
            _cache = cache;
            _repository = repository;
        }

        public async Task<AsxListedCompany[]> GetByAsxCode(string asxCode)
        {
            var companies = await _cache.GetOrCreateAsync(asxCode,
                async () => await _repository.FindEntitiesByConditionAsync(e => e.AsxCode == asxCode));
            return companies.ToArray();
        }
    }
}
