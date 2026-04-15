using Mappy.Domain.Itineraries;
using Mappy.Infrastructure.Persistence.Convertors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mappy.Infrastructure.Persistence.Confiugraions;

public class ItineraryConfiguration: IEntityTypeConfiguration<Itinerary>
{
    public void Configure(EntityTypeBuilder<Itinerary> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id)
            .HasColumnType("uuid")
            .ValueGeneratedNever();

        builder.Property(i => i.UserId)
            .HasColumnType("uuid")
            .ValueGeneratedNever();
        
        builder.Property("_stopsIds")
            .HasColumnName("StopsIds")
            .HasConversion<ListOfIdsConvertor>();
        
        builder.Property(i => i.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(i => i.Description)
            .HasMaxLength(500);
    }
}