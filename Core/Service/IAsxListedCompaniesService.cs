using Core.Entities;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IAsxListedCompaniesService
    {
        Task<AsxListedCompany[]> GetByAsxCode(string asxCode);
    }
}
