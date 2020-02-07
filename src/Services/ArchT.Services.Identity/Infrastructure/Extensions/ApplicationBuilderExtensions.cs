using ArchT.Services.Identity.Infrastructure.Database;
using Microsoft.AspNetCore.Builder;

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

namespace ArchT.Services.Identity.Infrastructure.Extensions
{
    static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                scope.ServiceProvider.GetRequiredService<IdentityDb>().Database.Migrate();

            return app;
        }
    }
}
