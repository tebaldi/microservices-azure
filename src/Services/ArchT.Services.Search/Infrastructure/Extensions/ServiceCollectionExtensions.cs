using ArchT.Services.Search.Infrastructure.Database;
using ArchT.Services.Search.Infrastructure.EventHub;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ArchT.Services.Search.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        { 
            services.AddLogging();
            services.AddDbContext<SearchDb>(options => options.Configure(configuration));
        }
    }
}
