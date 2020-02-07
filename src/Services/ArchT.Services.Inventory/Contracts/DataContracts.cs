using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchT.Services.Inventory.Contracts
{
    public class ProductData
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
    }
}
