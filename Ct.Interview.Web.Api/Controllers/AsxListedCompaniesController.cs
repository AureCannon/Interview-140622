using AutoMapper;
using Core.Service;
using Ct.Interview.Web.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Retry;

namespace Ct.Interview.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsxListedCompaniesController : ControllerBase
    {
        private readonly IAsxListedCompaniesService _asxListedCompaniesService;
        private readonly IMapper _mapper;
        private IAsyncPolicy _retryPolicy;

        public AsxListedCompaniesController(IAsxListedCompaniesService asxListedCompaniesService, IMapper mapper)
        {
            _asxListedCompaniesService = asxListedCompaniesService;
            _mapper = mapper;
            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt =>
                {
                    var timeToWait = TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
                    ///TODO: logger here
                    return timeToWait;
                });
        }

        [HttpGet]
        public async Task<ActionResult<AsxListedCompanyResponse[]>> Get(string asxCode)
        {
            var asxListedCompanies = _retryPolicy
                .ExecuteAsync(async () => await _asxListedCompaniesService.GetByAsxCode(asxCode));
            var mappedCompanies = _mapper.Map<IEnumerable<AsxListedCompanyResponse>>(asxListedCompanies);

            return Ok(mappedCompanies);
        }
    }
}
