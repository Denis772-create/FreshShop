using AppCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class CatalogTypeConfiguration : IEntityTypeConfiguration<CatalogType>
    {
        public void Configure(EntityTypeBuilder<CatalogType> builder)
        {
            builder.Property(ct => ct.Id)
                .UseHiLo("ct_hilo")
                .IsRequired(true);

            builder.Property(ct => ct.Type)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
