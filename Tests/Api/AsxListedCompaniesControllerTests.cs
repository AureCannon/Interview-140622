using AutoMapper;
using Core.Entities;
using Core.Service;
using Ct.Interview.Web.Api;
using Ct.Interview.Web.Api.Controllers;
using Ct.Interview.Web.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Api
{
    public class AsxListedCompaniesControllerTests
    {
        [Fact]
        public void Get_ReturnsArrayOfAsxListedCompanyResponse_WhenAsxCodeMatch()
        {
            var asxCode = "MOQ";
            
            var mockService = new Mock<IAsxListedCompaniesService>();
            mockService.Setup(x => x.GetByAsxCode(asxCode))
                .ReturnsAsync(GetTestCompanies());
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = mockMapper.CreateMapper();
            var sut = new AsxListedCompaniesController(mockService.Object, mapper);
            
            var result = sut.Get(asxCode).Result;

            var okResult = result.Result as OkObjectResult;
            var value = okResult.Value as IEnumerable<AsxListedCompanyResponse>;
            Assert.NotNull(result);
            Assert.True(value.Count() > 0);
        }

        [Fact]
        public void Get_ReturnsNotFoundResult_WhenAsxCodeDoesNotMatch()
        {
            var asxCode = "MMM";

            var mockService = new Mock<IAsxListedCompaniesService>();
            mockService.Setup(x => x.GetByAsxCode(asxCode))
                .ReturnsAsync(GetTestCompaniesNoMatch());
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = mockMapper.CreateMapper();
            var sut = new AsxListedCompaniesController(mockService.Object, mapper);

            var result = sut.Get(asxCode).Result;

            var notFoundResult = result.Result as NotFoundResult;
            Assert.NotNull(result);
            Assert.True(notFoundResult.GetType() == typeof(NotFoundResult));
        }

        private AsxListedCompany[] GetTestCompanies()
        {
            return new AsxListedCompany[]
            {
                new AsxListedCompany
                {
                    CompanyName = "MOQ LIMITED",
                    AsxCode = "MOQ",
                    GicsIndustryGroup = "Software & Services"
                }
            };
        }

        private AsxListedCompany[] GetTestCompaniesNoMatch()
        {
            return new AsxListedCompany[] { };
        }
    }
}
