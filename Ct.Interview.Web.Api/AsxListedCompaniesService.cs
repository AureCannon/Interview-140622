using CsvHelper;
using Ct.Interview.Web.Api.Mapper;
using System.Globalization;
using System.Net.Http.Headers;

namespace Ct.Interview.Web.Api
{
    public class AsxListedCompaniesService : IAsxListedCompaniesService
    {
        private readonly HttpClient client = new();

        public Task<AsxListedCompany[]> GetByAsxCode(string asxCode)
        {
            AsxListedCompany[] asxListedCompanies = Array.Empty<AsxListedCompany>();

            var json = RunAsync(asxCode).GetAwaiter().GetResult();

            if (json != null)
            {
                using (var reader = new StreamReader(json))
                using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        csvReader.Read();
                    }

                    csvReader.ReadHeader();

                    csvReader.Context.RegisterClassMap<AsxListedCompanyMap>();

                    asxListedCompanies = csvReader.GetRecords<AsxListedCompany>().ToArray();
                }
            }
            return Task.FromResult(asxListedCompanies);
        }

        private async Task<Stream> RunAsync(string csvFile)
        {
            client.BaseAddress = new Uri("https://www.asx.com.au/asx/research/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Stream stream = new MemoryStream();

            try
            {
                stream = await GetCsvAsync(csvFile).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return stream;
        }

        private async Task<Stream> GetCsvAsync(string csvFile)
        {
            Stream stream = new MemoryStream();

            HttpResponseMessage response = await client.GetAsync(csvFile);
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
