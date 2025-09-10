using Microsoft.EntityFrameworkCore;
using WebATB.Data.Entities;

namespace WebATB.Data;

public class AppATBDbContext : DbContext
{
    public AppATBDbContext(DbContextOptions<AppATBDbContext> options)
        : base(options)
    {
    }

    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<ProductImageEntity> ProductImages { get; set; }
}
