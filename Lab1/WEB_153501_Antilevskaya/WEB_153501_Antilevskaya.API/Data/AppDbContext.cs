using Microsoft.EntityFrameworkCore;
using WEB_153501_Antilevskaya.Domain.Entities;

namespace WEB_153501_Antilevskaya.API.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Category { get; set; }
    public DbSet<Exhibit> Exhibit { get; set; }
}
