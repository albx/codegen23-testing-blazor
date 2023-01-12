using Microsoft.EntityFrameworkCore;

namespace CodeGen23.Core;

public class CodeGen23Context : DbContext
{
    public CodeGen23Context(DbContextOptions<CodeGen23Context> options)
        : base(options)
    {
    }

    public DbSet<Card> Cards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var entityBuilder = modelBuilder.Entity<Card>();
        entityBuilder.HasKey(c => c.Id);
        entityBuilder.Property(c => c.Title).HasMaxLength(50).IsRequired();
        entityBuilder.Property(c => c.Description).HasMaxLength(255);
        entityBuilder.Property(c => c.IssuerName).HasMaxLength(255).IsRequired();
    }
}
