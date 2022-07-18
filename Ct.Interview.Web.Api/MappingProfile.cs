using AutoMapper;
using Core.Entities;

namespace Ct.Interview.Web.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AsxListedCompany, AsxListedCompany>().ReverseMap();
        }
    }
}
