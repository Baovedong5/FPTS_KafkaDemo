using Microsoft.EntityFrameworkCore;
using ProductPersistenceService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPersistenceService.Core.Databases
{
    internal class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<TableProduct> TableProducts { get; set; }
    }
}
