using AppCore.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            var navigation = builder.Metadata.FindNavigation(nameof(Order.Items));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(b => b.BuyerId)
                .IsRequired()
                .HasMaxLength(256);

            builder.OwnsOne(o => o.ShipToAddress, p =>
              {
                  p.WithOwner();

                  p.Property(a => a.ZipCode)
                      .IsRequired()
                      .HasMaxLength(18);

                  p.Property(a => a.Street)
                      .HasMaxLength(180)
                      .IsRequired();

                  p.Property(a => a.State)
                      .HasMaxLength(60);

                  p.Property(a => a.Country)
                      .HasMaxLength(90)
                      .IsRequired();

                  p.Property(a => a.City)
                      .HasMaxLength(100)
                      .IsRequired();
              });
        }
    }
}
