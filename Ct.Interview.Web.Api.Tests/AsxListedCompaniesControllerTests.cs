using Ct.Interview.Web.Api.Controllers;
using Ct.Interview.Web.Api.Models;
using Ct.Interview.Web.Api.Tests.Mock;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ct.Interview.Web.Api.Tests
{
    [TestClass]
    public class AsxListedCompaniesControllerTests
    {
        MockAsxListedCompaniesService _mockService;
        AsxListedCompaniesController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _mockService = new MockAsxListedCompaniesService();
            _controller = new AsxListedCompaniesController(_mockService);
        }

        //[TestMethod]
        //public void Get_WhenCalled_ReturnsOkObjectResult()
        //{
        //    _mockService.AsxListedCompanies = GetSampleCompanies();

        //    dynamic result = _controller.Get("MOQ").Result;

        //    Assert.IsNotNull(result.Value);
        //}

        [TestMethod]
        public void Get_WhenCalled_ReturnsOkObjectResult()
        {
            var asxCode = "MOQ";
            _mockService.AsxListedCompanies = GetSampleCompanies();

            var result = _controller.Get(asxCode).Result;

            Assert.IsTrue(result is OkObjectResult);
        }

        [TestMethod]
        public void Get_WhenCalled_ReturnsBadRequestResult()
        {
            var asxCode = "";
            _mockService.AsxListedCompanies = GetSampleCompanies();
            _mockService.IsException = true;

            var result = _controller.Get(asxCode).Result;

            Assert.IsTrue(result is BadRequestResult);
        }

        [TestMethod]
        public void UpdateList_ReturnsOkResult()
        {
            var list = GetSampleCompanies();
            list.Add(new AsxListedCompany
            {
                CompanyName = "360 CAPITAL GROUP",
                AsxCode = "TGP",
                GicsIndustryGroup = "Real Estate"
            });

            _mockService.AsxListedCompanies = list;

            var result = _controller.UpdateList().Result;

            Assert.IsTrue(result is OkResult);
        }

        [TestMethod]
        public void Patch_WhenCalled_ReturnsBadRequestResult()
        {
            _mockService.AsxListedCompanies = GetSampleCompanies();
            _mockService.IsException = true;

            var result = _controller.UpdateList().Result;

            Assert.IsTrue(result is BadRequestResult);
        }

        private List<AsxListedCompany> GetSampleCompanies()
        {
            return new List<AsxListedCompany>
                    {
                    new AsxListedCompany{
                        Id = 1,
                        CompanyName = "MOQ LIMITED",
                        AsxCode = "MOQ",
                        GicsIndustryGroup = "Software & Services"
                    },
                    new AsxListedCompany{
                        Id = 2,
                        CompanyName = "1414 DEGREES LIMITED",
                        AsxCode = "14D",
                        GicsIndustryGroup = "Capital Goods"
                    },
                    new AsxListedCompany{
                        Id = 3,
                        CompanyName = "1ST GROUP LIMITED",
                        AsxCode = "1ST",
                        GicsIndustryGroup = "Health Care Equipment & Services"
                    }
                };
        }

    }
}
