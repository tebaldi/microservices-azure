using ArchT.Services.Search.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ArchT.Services.Search.Infrastructure.Database
{
    public class DesignTimeDatabaseContextFactory : IDesignTimeDbContextFactory<SearchDb>
    {
        SearchDb IDesignTimeDbContextFactory<SearchDb>.CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .AddJsonFile($"appsettings.Development.json", true)
               .AddEnvironmentVariables();

            var configuration = builder.Build();
            return new SearchDb(new DbContextOptionsBuilder().Configure(configuration).Options);
        }
    }
}
