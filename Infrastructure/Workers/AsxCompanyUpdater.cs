using Core.Entities;
using Core.Repository;
using Infrastructure.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Workers
{
    public class AsxCompanyUpdater : BackgroundService
    {
        //schedule a run every 4 hours since I don't know
        //the exact time the file will be updated
        private readonly PeriodicTimer _timer = new(TimeSpan.FromHours(4));
        private readonly AsxCompanyClient _asxCompanyClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AsxCompanyUpdater> _logger;

        public AsxCompanyUpdater(ILogger<AsxCompanyUpdater> logger, IServiceProvider serviceProvider, AsxCompanyClient asxCompanyClient)
        {
            _asxCompanyClient = asxCompanyClient;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                using (IServiceScope scope = _serviceProvider.CreateScope())
                {
                    var repository = scope.ServiceProvider.GetRequiredService<IRepository<AsxListedCompany>>();
                    var response = await _asxCompanyClient.GetAsxCompaniesAsStringAsync();
                    var list = GetCompanyList(response);
                    var companiesInDb = await repository.GetAllAsync();

                    //filter for companies not yet in DB and insert them
                    var companiesToBeAdded = list.Where(l => !companiesInDb.Any(c => l.AsxCode == c.AsxCode));
                    if (companiesToBeAdded.Any())
                    {
                        repository.AddRange(companiesToBeAdded);
                        await repository.SaveChangesAsync();
                        _logger.LogInformation($"Companies table updated. {companiesToBeAdded.Count()} Companies added");
                    }
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
                    CompanyName = companyInfo[0].Replace("\"", ""),
                    AsxCode = companyInfo[1].Replace("\"", ""),
                    GicsIndustryGroup = companyInfo[2].Replace("\"", ""),
                };
                list.Add(company);
            }

            return list;
        }
    }
}
