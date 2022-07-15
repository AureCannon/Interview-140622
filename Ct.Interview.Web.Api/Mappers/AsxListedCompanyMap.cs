using CsvHelper.Configuration;
using Ct.Interview.Web.Api.Models;

namespace Ct.Interview.Web.Api.Mappers
{
    public sealed class AsxListedCompanyMap : ClassMap<AsxListedCompany>
    {
        public AsxListedCompanyMap()
        {
            Map(s => s.CompanyName).Name("Company name");
            Map(s => s.AsxCode).Name("ASX code");
            Map(s => s.GicsIndustryGroup).Name("GICS industry group");
        }
    }
}
