using Domain.Inventories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations.Inventories;

public class InventoryItemEntityConfiguration
    : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Quantity)
            .HasPrecision(18, 3)
            .IsRequired();

        builder
            .Property(x => x.Notes)
            .HasMaxLength(2000);

        builder
            .Property(x => x.SerialNumber)
            .HasMaxLength(200);

        builder
            .Property(x => x.AssetNumber)
            .HasMaxLength(200);

        builder
            .HasOne(x => x.Inventory)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.InventoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.ItemVariant)
            .WithMany(x => x.InventoryItems)
            .HasForeignKey(x => x.ItemVariantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.PackDefinition)
            .WithMany(x => x.InventoryItems)
            .HasForeignKey(x => x.PackDefinitionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
