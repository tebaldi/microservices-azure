using ArchT.Services.Search.Contracts;
using ArchT.Services.Search.Infrastructure.Configuration;
using ArchT.Services.Search.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ArchT.Services.Search.Infrastructure.Database
{
    public class SearchDb : DbContext
    {
        public const string Schema = "SearchDb";

        public SearchDb(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.Entity<OrderReportData>().ToTable("OrdersReport").HasKey(r => new { r.OrderId, r.ProductId });
            modelBuilder.Entity<ProductUpdated>().ToTable("Products").HasKey(r => new { r.ProductId });
        }

        public DbSet<OrderReportData> OrdersReport { get; set; }
        public DbSet<ProductUpdated> Products { get; set; }
    }
}
