using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ArchT.IntegrationTests.Services.Inventory
{
    public class BaseIntegrationTest: IDisposable
    {
        protected IConfiguration Configuration;
        protected ILoggerFactory LoggerFactory;

        public BaseIntegrationTest()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", optional: false)
                .Build();

            LoggerFactory = new LoggerFactory().AddConsole();
        }

        public virtual void Dispose()
        {
        }
    }
}
