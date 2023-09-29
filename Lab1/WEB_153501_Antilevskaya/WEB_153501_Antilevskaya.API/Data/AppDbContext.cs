using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using WEB_153501_Antilevskaya.Domain.Entities;

namespace WEB_153501_Antilevskaya.API.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exhibit>()
            .HasOne(e => e.Category)
            .WithMany(a => a.Exhibits)
            .HasForeignKey(e => e.CategoryId)
            .HasPrincipalKey(e => e.Id);
    }
    public DbSet<Category> Category { get; set; }
    public DbSet<Exhibit> Exhibit { get; set; }
}
