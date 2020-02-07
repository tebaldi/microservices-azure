using ArchT.Services.Search.Infrastructure.DataAccess;
using ArchT.Services.Search.Infrastructure.Database;
using ArchT.Services.Search.Infrastructure.EventHub;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.EventHubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchT.Services.Search.Infrastructure.Extensions
{
    static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                scope.ServiceProvider.GetRequiredService<SearchDb>().Database.Migrate();

            return app;
        }
    }
}
