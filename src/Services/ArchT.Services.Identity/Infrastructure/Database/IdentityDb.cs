using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchT.Services.Identity.Infrastructure.Database
{
    public class IdentityDb : IdentityDbContext
    {
        public const string Schema = "IdentityDb";

        public IdentityDb(DbContextOptions options) : base(options)
        {
        }

        protected IdentityDb()
        {
        }
    }
}
