using Mappy.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mappy.Infrastructure.Persistence.Confiugraions;

public class OutBoxMessageConfiguration: IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id)
            .HasColumnType("bigint")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property("_xmin")
            .HasColumnName("xmin")
            .IsRowVersion();

        builder.Property(m => m.OccurredOn);
        builder.Property(m => m.IsProcessed);
        builder.Property(m => m.Body);
        builder.Property(m => m.Type);
    }
}