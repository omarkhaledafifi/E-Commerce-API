namespace Persistence.Data.Configurations
{
    internal class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(item => item.Price)
                 .HasColumnType("decimal(18,3)");


            builder.OwnsOne(item => item.Product, product => product.WithOwner());
        }
    }
}
