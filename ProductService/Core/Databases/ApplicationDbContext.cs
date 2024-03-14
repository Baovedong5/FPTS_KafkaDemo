using Microsoft.EntityFrameworkCore;
using ProductService.Core.Databases.EntitiesTypeConfigurations;
using ProductService.Core.Models;

namespace ProductService.Core.Databases
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        public DbSet<TableProduct> TableProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<TableProduct>(new TableProductEntityTypeConfiguration());
        }
    }
}
