using CsvHelper;
using Ct.Interview.Web.Api.Mapper;
using System.Globalization;
using System.Net.Http.Headers;

namespace Ct.Interview.Web.Api
{
    public class AsxListedCompaniesService : IAsxListedCompaniesService
    {
        private readonly HttpClient client = new();
        private readonly IConfiguration _configuration;

        public AsxListedCompaniesService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<AsxListedCompany[]> GetByAsxCode(string asxCode)
        {
            AsxListedCompany[] asxListedCompanies = Array.Empty<AsxListedCompany>();

            var stream = RunAsync().GetAwaiter().GetResult();

            if (stream != null)
            {
                using (var reader = new StreamReader(stream))
                using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        csvReader.Read();
                    }

                    csvReader.ReadHeader();

                    csvReader.Context.RegisterClassMap<AsxListedCompanyMap>();

                    asxListedCompanies = csvReader.GetRecords<AsxListedCompany>().Where(x => String.Equals(x.AsxCode, asxCode, StringComparison.CurrentCultureIgnoreCase)).ToArray();
                }
            }
            return Task.FromResult(asxListedCompanies);
        }

        private async Task<Stream> RunAsync()
        {
            var uri = new Uri(_configuration["AsxSettings:ListedSecuritiesCsvUrl"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Stream stream = new MemoryStream();

            HttpResponseMessage response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            }
            else
            {
                Console.WriteLine(response.ToString());
            }

            return stream;
        }
    }
}
