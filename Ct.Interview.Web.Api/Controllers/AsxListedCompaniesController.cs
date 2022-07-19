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

        /// <summary>
        /// Get ASX Listed company by ASX code
        /// </summary>
        /// <param name="asxCode"></param>
        /// <returns>AsxListedCompanyResponse[]</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AsxListedCompanyResponse[]), 200)]
        [ProducesErrorResponseType(typeof(NotFoundResult))]
        public async Task<ActionResult<AsxListedCompanyResponse[]>> Get(string asxCode)
        {
            if (string.IsNullOrEmpty(asxCode))
                return BadRequest("ASX code should not be null");
            var asxListedCompanies = await _asxListedCompaniesService.GetByAsxCode(asxCode);
            if (asxListedCompanies == null || asxListedCompanies.Length == 0) return NotFound();
            var mappedCompanies = _mapper.Map<IEnumerable<AsxListedCompanyResponse>>(asxListedCompanies);

            return Ok(mappedCompanies);
        }
    }
}
