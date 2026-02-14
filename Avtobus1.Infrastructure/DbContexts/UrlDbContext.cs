using Avtobus1.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Avtobus1.Infrastructure.DbContexts;

public class UrlDbContext(DbContextOptions<UrlDbContext> options) : DbContext(options)
{
    public DbSet<UrlRecord> UrlRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UrlDbContext).Assembly);
    }
}