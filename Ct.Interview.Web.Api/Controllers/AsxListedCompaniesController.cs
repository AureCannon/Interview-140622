using AutoMapper;
using Core.Service;
using Ct.Interview.Web.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Polly;

namespace Ct.Interview.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsxListedCompaniesController : ControllerBase
    {
        private readonly IAsxListedCompaniesService _asxListedCompaniesService;
        private readonly IMapper _mapper;

        public AsxListedCompaniesController(IAsxListedCompaniesService asxListedCompaniesService, IMapper mapper)
        {
            _asxListedCompaniesService = asxListedCompaniesService;
            _mapper = mapper;
            
        }

        [HttpGet]
        public async Task<ActionResult<AsxListedCompanyResponse[]>> Get(string asxCode)
        {
            var asxListedCompanies = await _asxListedCompaniesService.GetByAsxCode(asxCode);
            if (asxListedCompanies == null) return NotFound();
            var mappedCompanies = _mapper.Map<IEnumerable<AsxListedCompanyResponse>>(asxListedCompanies);

            return Ok(mappedCompanies);
        }
    }
}
