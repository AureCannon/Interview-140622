using Ct.Interview.Web.Api.DTO;
using Ct.Interview.Web.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ct.Interview.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsxListedCompaniesController : ControllerBase
    {
        private readonly IAsxListedCompaniesService _asxListedCompaniesService;

        public AsxListedCompaniesController(IAsxListedCompaniesService asxListedCompaniesService)
        {
            _asxListedCompaniesService = asxListedCompaniesService;
        }

        /// <summary>
        /// GET: api/AsxListedCompanies?asxCode=<value>
        /// </summary>
        /// <param name="asxCode"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get(string asxCode)
        {
            try
            {
                var asxListedCompanies = await _asxListedCompaniesService.GetByAsxCode(asxCode);

                if (asxListedCompanies.Count() > 0)
                    return Ok(asxListedCompanies);
                else
                    return NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// PATCH: api/AsxListedCompanies?asxCode=<value>
        /// </summary>
        /// <returns></returns>
        [HttpPatch]
        public async Task<IActionResult> UpdateList()
        {
            try
            {
                await _asxListedCompaniesService.UpdateListfromCSV();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
