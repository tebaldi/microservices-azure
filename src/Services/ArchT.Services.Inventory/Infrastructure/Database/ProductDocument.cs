using ArchT.Services.Inventory.Application.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchT.Services.Inventory.Infrastructure.Database
{
    class ProductDocument : DbDocument
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public override string CollectionName => "Products";
    }
}
