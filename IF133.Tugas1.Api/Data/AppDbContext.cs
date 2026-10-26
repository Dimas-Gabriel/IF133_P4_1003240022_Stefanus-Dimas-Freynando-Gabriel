using Microsoft.EntityFrameworkCore;
using IF133.Tugas1.Api.Entities;

namespace IF133.Tugas1.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
}
