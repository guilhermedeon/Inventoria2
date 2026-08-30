using System;
using System.Collections.Generic;
using System.Text;
using Domain.Classifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations.Classifications;

public class TagAssignmentEntityConfiguration
    : IEntityTypeConfiguration<TagAssignment>
{
    public void Configure(EntityTypeBuilder<TagAssignment> builder)
    {
        builder
            .HasKey(x => new
            {
                x.TagId,
                x.EntityId,
                x.EntityType
            });

        builder
            .HasOne(x => x.Tag)
            .WithMany(x => x.Assignments)
            .HasForeignKey(x => x.TagId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(x => x.EntityType)
            .IsRequired();
    }
}
