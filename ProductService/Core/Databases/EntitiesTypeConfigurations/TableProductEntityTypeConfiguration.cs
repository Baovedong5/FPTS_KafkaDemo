using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Core.Models;

namespace ProductService.Core.Databases.EntitiesTypeConfigurations
{
    public class TableProductEntityTypeConfiguration : IEntityTypeConfiguration<TableProduct>
    {
        public void Configure(EntityTypeBuilder<TableProduct> builder)
        {
            builder.ToTable("TableProducts");
            builder.HasKey("Id");
            builder.Property(tp => tp.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(tp => tp.Name).HasColumnName("Name");
            builder.Property(tp => tp.Price).HasColumnName("Price");
            builder.Property(tp => tp.Quantity).HasColumnName("Quantity");
        }
    }
}
