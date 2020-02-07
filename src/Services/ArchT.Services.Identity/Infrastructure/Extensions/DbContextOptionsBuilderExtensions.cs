using ArchT.Services.Identity.Infrastructure.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
    static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder Configure(this DbContextOptionsBuilder optionsBuilder, IConfiguration configuration)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("IdentityDbConnection"), oa =>
                oa.MigrationsHistoryTable("__MigrationHistory", IdentityDb.Schema));

            return optionsBuilder;
        }
    }
}
