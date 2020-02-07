using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchT.Services.Inventory.Infrastructure.Configuration
{
    public class DbConfig
    {
        public string AccountEndpoint { get; set; }
        public string AccountKeys { get; set; }
        public string DatabaseId { get; set; }
    }
}
