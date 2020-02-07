using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchT.Services.Inventory.Application.Models
{
    public class StockNotAvailableException : ArgumentOutOfRangeException
    {
        public readonly int Requested;
        public readonly int Available;

        public StockNotAvailableException(int requested, int available)
            : base($"Requested: {requested} Available: {available}")
        {
            Requested = requested;
            Available = available;
        }
    }
}
