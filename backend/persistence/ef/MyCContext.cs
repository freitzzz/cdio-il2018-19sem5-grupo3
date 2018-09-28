using Microsoft.EntityFrameworkCore;
using core.domain;

namespace backend.persistence.ef
{
    public class MyCContext : DbContext
    {
        public DbSet<Material> Material { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public MyCContext(DbContextOptions<MyCContext> options) : base(options) { }
    
    }
}