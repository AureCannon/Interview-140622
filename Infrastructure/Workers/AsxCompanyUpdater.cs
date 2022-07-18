using Core.Entities;
using Core.Repository;
using Infrastructure.Client;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Workers
{
    public class AsxCompanyUpdater : BackgroundService
    {
        private readonly IRepository<AsxListedCompany> _repository;
        private readonly PeriodicTimer _timer = new(TimeSpan.FromDays(1));
        private readonly AsxCompanyClient _asxCompanyClient;

        public AsxCompanyUpdater(IRepository<AsxListedCompany> repository, AsxCompanyClient asxCompanyClient)
        {
            _repository = repository;
            _asxCompanyClient = asxCompanyClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                var response = await _asxCompanyClient.GetAsxCompaniesAsStringAsync();
                var list = GetCompanyList(response);
                var companiesInDb = await _repository.GetAllAsync();

                //filter for companies not yet in DB and insert them
                var companiesToBeAdded = list.Where(l => !companiesInDb.Any(c => l.AsxCode == c.AsxCode));
                if (companiesToBeAdded.Any())
                {
                    _repository.AddRange(companiesToBeAdded);
                    await _repository.SaveChangesAsync();
                }
            }
            while (await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested);
        }

        private static List<AsxListedCompany> GetCompanyList(string str)
        {
            var split = str.Split("\n");
            var rows = split.Where((src, idx) => idx > 2 && !string.IsNullOrEmpty(src));
            var list = new List<AsxListedCompany>();
            foreach (var row in rows)
            {
                var companyInfo = row.Split(",");
                if (companyInfo.Length < 3) continue;
                var company = new AsxListedCompany
                {
                    CompanyName = companyInfo[0],
                    AsxCode = companyInfo[1],
                    GicsIndustryGroup = companyInfo[2],
                };
                list.Add(company);
            }

            return list;
        }
    }
}
