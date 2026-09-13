using Domain.Inventories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations.Inventories;

public class InventoryEntityConfiguration
    : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.ToTable("Inventories");
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder
            .Property(x => x.Description)
            .HasMaxLength(2000);

        builder
            .HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.ParentInventory)
            .WithMany(x => x.ChildInventories)
            .HasForeignKey(x => x.ParentInventoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
