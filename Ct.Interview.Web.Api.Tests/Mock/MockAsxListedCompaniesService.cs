using Ct.Interview.Web.Api.Models;
using Ct.Interview.Web.Api.Services;

namespace Ct.Interview.Web.Api.Tests.Mock
{
    public class MockAsxListedCompaniesService : IAsxListedCompaniesService
    {
        public bool IsException { get; set; }
        public List<AsxListedCompany> AsxListedCompanies { get; set; }

        public async Task<AsxListedCompany[]> GetByAsxCode(string asxCode)
        {
            if (IsException)
            {
                throw new Exception();
            };

            return await Task<AsxListedCompany[]>.Factory.StartNew(() =>
            {
                return AsxListedCompanies.Where(x => x.AsxCode == asxCode).ToArray();
            });
        }

        public async Task UpdateListfromCSV()
        {
            if (IsException)
            {
                throw new Exception();
            };

            await Task.Factory.StartNew(() =>
            {

            });

        }
    }
}
