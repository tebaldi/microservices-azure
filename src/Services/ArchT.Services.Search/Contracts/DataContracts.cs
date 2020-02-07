using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchT.Services.Search.Contracts
{
    public class RequestFilter
    {
        public string Command { get; set; }
        public string[] Params { get; set; }
    }
    public class OrderReportRequest
    {
        public string Select { get; set; }
        public RequestFilter Filter { get; set; }
        public string Order { get; set; }
    }

    public class OrderReportData
    {   
        public string OrderId { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderLines { get; set; }
        public decimal OrderTotal { get; set; }
        public string ProductId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
    }
}
