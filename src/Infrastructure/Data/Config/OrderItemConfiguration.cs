using AppCore.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(i => i.ItemOrdered, p =>
            {
                p.WithOwner();

                p.Property(pr => pr.ProductName)
                .IsRequired()
                .HasMaxLength(50);
            });

            builder.Property(oi => oi.UnitPrice)
                 .IsRequired(true)
                 .HasColumnType("decimal(18,2)");
        }
    }
}
