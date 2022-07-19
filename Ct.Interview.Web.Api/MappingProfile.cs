using AutoMapper;
using Core.Entities;
using Ct.Interview.Web.Api.Models;

namespace Ct.Interview.Web.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AsxListedCompany, AsxListedCompanyResponse>().ReverseMap();
        }
    }
}
