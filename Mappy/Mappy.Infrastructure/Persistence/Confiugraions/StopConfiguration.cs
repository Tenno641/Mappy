using Mappy.Domain.Stops;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mappy.Infrastructure.Persistence.Confiugraions;

public class StopConfiguration: IEntityTypeConfiguration<Stop>
{
    public void Configure(EntityTypeBuilder<Stop> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .HasColumnType("uuid")
            .ValueGeneratedOnAdd();
        
        builder.Property("_name")
            .HasColumnName("Name")
            .HasMaxLength(128)
            .IsRequired();

        builder.Property<Uri>("_imageUri")
            .HasColumnName("ImageUri")
            .HasConversion(v => v.ToString(), v => new Uri(v));

        builder.Property(s => s.ItineraryId)
            .HasColumnType("uuid")
            .ValueGeneratedNever();
    }
}