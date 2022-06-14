using CsvHelper.Configuration;

namespace Ct.Interview.Web.Api.Mapper
{
    public class AsxListedCompanyMap : ClassMap<AsxListedCompany>
    {
        public AsxListedCompanyMap()
        {
            Map(m => m.CompanyName).Name("Company name");
            Map(m => m.AsxCode).Name("ASX code");
            Map(m => m.GicsIndustryGroup).Name("GICS industry group");
        }
    }
}
