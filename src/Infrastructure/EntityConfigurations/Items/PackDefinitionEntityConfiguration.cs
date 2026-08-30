using Domain.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations.Items;

public class PackDefinitionEntityConfiguration
    : IEntityTypeConfiguration<PackDefinition>
{
    public void Configure(EntityTypeBuilder<PackDefinition> builder)
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
            .HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.Items)
            .WithOne(x => x.PackDefinition)
            .HasForeignKey(x => x.PackDefinitionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
