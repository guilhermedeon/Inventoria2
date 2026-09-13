using Domain.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations.Items;

public class ItemDefinitionEntityConfiguration
    : IEntityTypeConfiguration<ItemDefinition>
{
    public void Configure(EntityTypeBuilder<ItemDefinition> builder)
    {
        builder.ToTable("ItemDefinitions");
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
            .Property(x => x.Brand)
            .HasMaxLength(100);

        builder
            .Property(x => x.Manufacturer)
            .HasMaxLength(200);

        builder
            .Property(x => x.Model)
            .HasMaxLength(200);

        builder
            .Property(x => x.Sku)
            .HasMaxLength(100);

        builder
            .HasIndex(x => x.Sku)
            .IsUnique();

        builder
            .HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.Variants)
            .WithOne(x => x.ItemDefinition)
            .HasForeignKey(x => x.ItemDefinitionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
