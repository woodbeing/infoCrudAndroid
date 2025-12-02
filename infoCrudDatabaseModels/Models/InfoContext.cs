using Microsoft.EntityFrameworkCore;

namespace infoCrudDatabaseModels.Models;

public partial class InfoContext: DbContext
{
    public InfoContext() { }

    public InfoContext(DbContextOptions<InfoContext> options)
        : base(options) { }
    public DbSet<Info> Infos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Info>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("name");

            entity
                .Property(e => e.Address)
                .HasMaxLength(500)
                .HasColumnName("address");
        });
    }
}
