using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Habita.Domain.Entities;

namespace Habita.Infrastructure.Persistence.EntityConfigurations;

public sealed class FavoritePropertyConfiguration : IEntityTypeConfiguration<FavoriteProperty>
{
    public void Configure(EntityTypeBuilder<FavoriteProperty> builder)
    {
        builder.ToTable("FavoriteProperties");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();

                               
        builder.Property(x => x.ClientId).IsRequired().HasMaxLength(128);

                        
        builder.Property(x => x.PropertyId).IsRequired();

        builder
            .HasOne(x => x.Property)
            .WithMany()
            .HasForeignKey(x => x.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired(false);

                                                                
        builder.HasIndex(x => new { x.ClientId, x.PropertyId }).IsUnique();
        builder.HasIndex(x => x.PropertyId);
    }
}
