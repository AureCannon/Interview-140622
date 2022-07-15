using System.ComponentModel.DataAnnotations.Schema;

namespace Ct.Interview.Web.Api.Models
{
    public class AsxListedCompany
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = String.Empty;
        public string AsxCode { get; set; } = String.Empty;
        public string GicsIndustryGroup { get; set; } = String.Empty;
    }
}