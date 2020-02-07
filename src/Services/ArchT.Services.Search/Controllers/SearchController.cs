using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ArchT.Services.Search.Contracts;
using ArchT.Services.Search.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArchT.Services.Search.Controllers
{
    [ApiController, Route("api/v1/search")]
    public class SearchController : ControllerBase
    {
        private readonly SearchDb dbContext;

        public SearchController(SearchDb dbContext) => this.dbContext = dbContext;

        [HttpGet, Route("reports/orders")]
        public async Task<IEnumerable<OrderReportData>> GetOrdersReport([FromQuery]OrderReportRequest request)
        {
            var q = (
                from o in dbContext.OrdersReport
                join p in dbContext.Products.DefaultIfEmpty() on o.ProductId equals p.ProductId
                select new OrderReportData
                {
                    OrderId = o.OrderId, OrderDate = o.OrderDate, ProductId = o.ProductId, OrderLines = o.OrderLines,
                    OrderTotal = o.OrderTotal, OrderStatus = o.OrderStatus, Amount = o.Amount, Price = o.Price,
                    Name = p.Name
                });

            if (!string.IsNullOrEmpty(request.Select))
                q = q.Select<OrderReportData>($"new({request.Select})");

            if (!string.IsNullOrEmpty(request.Filter?.Command))
                q = q.Where(request.Filter.Command, request.Filter.Params);

            if (!string.IsNullOrEmpty(request.Order))
                q = q.OrderBy(request.Order);

            return await q.ToArrayAsync();
        }
    }
}
