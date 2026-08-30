using Domain.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations.Items;

public class ItemVariantEntityConfiguration
    : IEntityTypeConfiguration<ItemVariant>
{
    public void Configure(EntityTypeBuilder<ItemVariant> builder)
    {
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
            .Property(x => x.Sku)
            .HasMaxLength(100);

        builder
            .Property(x => x.Barcode)
            .HasMaxLength(100);

        builder
            .HasIndex(x => x.Sku)
            .IsUnique();

        builder
            .HasIndex(x => x.Barcode);

        builder
            .HasOne(x => x.ItemDefinition)
            .WithMany(x => x.Variants)
            .HasForeignKey(x => x.ItemDefinitionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
