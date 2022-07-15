using Ct.Interview.Web.Api.Models;

namespace Ct.Interview.Web.Api.Services
{
    public interface IAsxListedCompaniesService
    {
        Task<AsxListedCompany[]> GetByAsxCode(string asxCode);
        Task UpdateListfromCSV();
    }
}
