using Core.Entities;
using Core.Repository;
using Core.Service;
using Ct.Interview.Web.Api.Services;
using Infrastructure.Caching;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ct.Interview.Web.Api.Extensions
{
    public static class ConfigurationExtension
    {
        public static void ConfigureDependencies(this IServiceCollection services)
        {
            services.AddSingleton<ICache, Cache>();
            services.AddScoped<IRepository<AsxListedCompany>, Repository<AsxListedCompany>>();
            services.AddScoped<IAsxListedCompaniesService, AsxListCompaniesService>();
        }
    }
}
