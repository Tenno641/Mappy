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

        builder.Property(s => s.Name);
        builder.Property(s => s.ImageUri);
        
        builder.Property(s => s.ItineraryId)
            .HasColumnType("uuid")
            .ValueGeneratedNever();
    }
}