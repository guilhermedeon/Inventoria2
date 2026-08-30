using Domain.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations.Items;

public class PackDefinitionItemEntityConfiguration
    : IEntityTypeConfiguration<PackDefinitionItem>
{
    public void Configure(EntityTypeBuilder<PackDefinitionItem> builder)
    {
        builder
            .HasKey(x => new
            {
                x.PackDefinitionId,
                x.ItemVariantId
            });

        builder
            .Property(x => x.Quantity)
            .HasPrecision(18, 3)
            .IsRequired();

        builder
            .HasOne(x => x.PackDefinition)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.PackDefinitionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.ItemVariant)
            .WithMany(x => x.Packs)
            .HasForeignKey(x => x.ItemVariantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
