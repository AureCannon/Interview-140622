using CsvHelper;
using CsvHelper.Configuration;
using Ct.Interview.Web.Api.Data;
using Ct.Interview.Web.Api.Mappers;
using Ct.Interview.Web.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;
using System.Text;

namespace Ct.Interview.Web.Api.Services
{
    public class AsxListedCompaniesService : IAsxListedCompaniesService
    {
        private readonly AsxSettings _asxSettings;
        private readonly ApplicationDbContext _db;
        private HttpClient _client;
        private readonly IMemoryCache _memoryCache;

        public AsxListedCompaniesService(AsxSettings asxSettings, ApplicationDbContext db, IMemoryCache memoryCache, IHttpClientFactory clientFactory)
        {
            _asxSettings = asxSettings;
            _db = db;
            _client = clientFactory.CreateClient("HttpClient");
            _memoryCache = memoryCache;
        }

        public async Task<AsxListedCompany[]> GetByAsxCode(string asxCode)
        {
            try
            {
                AsxListedCompany[] companies;

                companies = _memoryCache.Get<AsxListedCompany[]>("companies");

                if (companies is null)
                {
                    companies = await _db.AsxListedCompanies.Where(s => s.AsxCode == asxCode).ToArrayAsync();
                    _memoryCache.Set("companies", companies, TimeSpan.FromMinutes(1));
                }

                return companies;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task UpdateListfromCSV()
        {
            try
            {
                using (var response = await _client.GetAsync(_asxSettings.ListedSecuritiesCsvUrl))
                {
                    var responseContent = await response.Content.ReadAsStreamAsync();

                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                    };

                    using (var reader = new StreamReader(responseContent))
                    using (var csv = new CsvReader(reader, config))
                    {
                        for (var i = 0; i < 1; i++)
                        {
                            csv.Read();
                        }

                        csv.Context.RegisterClassMap<AsxListedCompanyMap>();
                        var records = csv.GetRecords<AsxListedCompany>().ToList();

                        List<AsxListedCompany> dbRecords;
                        dbRecords = _memoryCache.Get<List<AsxListedCompany>>("companies");
                        if (dbRecords is null)
                        {
                            dbRecords = _db.AsxListedCompanies.ToList();
                            _memoryCache.Set("companies", dbRecords, TimeSpan.FromMinutes(1));
                        }

                        foreach (var item in records)
                        {
                            if (!dbRecords.Select(s => s.CompanyName).Contains(item.CompanyName))
                            {
                                _db.AsxListedCompanies.Add(item);
                            }
                        }
                        await _db.SaveChangesAsync();
                    }
                }
                
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
