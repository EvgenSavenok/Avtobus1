using Avtobus1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Avtobus1.Infrastructure.Configurations;

public class UrlConfiguration
{
    public void Configure(EntityTypeBuilder<UrlRecord> builder)
    {
        builder.HasKey(url => url.Id);

        builder.Property(url => url.OriginalUrl)
            .IsRequired()
            .HasMaxLength(2048);

        builder.Property(url => url.ShortCode)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasIndex(url => url.ShortCode)
            .IsUnique();

        builder.Property(url => url.CreatedAt)
            .IsRequired();

        builder.Property(url => url.ClickCount)
            .IsRequired()
            .HasDefaultValue(0);
    }
}