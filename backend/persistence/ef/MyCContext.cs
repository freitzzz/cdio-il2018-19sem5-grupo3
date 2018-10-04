using Microsoft.EntityFrameworkCore;
using core.domain;
using backend.config;
using System.Linq;

namespace backend.persistence.ef
{
    /// <summary>
    /// Class representing the MakeYourCloset application database.
    /// </summary>
    public class MyCContext : DbContext
    {
        /// <summary>
        /// Database set containing all of the saved instances of Material.
        /// </summary>
        /// <value>Gets/sets the database set containing all the saved instances of Material.</value>
        public DbSet<Material> Material { get; set; }

        /// <summary>
        /// Database set containing all of the saved instances of Product.
        /// </summary>
        /// <value>Gets/sets the database set containing all the saved instances of Product.</value>
        public DbSet<Product> Product { get; set; }

        /// <summary>
        /// Database set containing all of the saved instances of ProductCategory.
        /// </summary>
        /// <value>Gets/sets the database set containing all the saved instances of ProductCategory.</value>
        public DbSet<ProductCategory> ProductCategory { get; set; }

        /// <summary>
        /// Constructs a new database for the MakeYourCloset application.
        /// </summary>
        /// <param name="options">The options for the context.</param>
        /// <returns>New instance of MyCContext.</returns>
        public MyCContext(DbContextOptions<MyCContext> options) : base(options) {BackendConfiguration.entityFrameworkContext=this;}


        protected override void OnModelCreating(ModelBuilder builder){
            base.OnModelCreating(builder);

            //!Define How Entities Are Mapped Here
            builder.Entity<Material>().HasMany(m => m.Colors).WithOne();        //one-to-many relationship
            builder.Entity<Material>().HasMany(m => m.Finishes).WithOne();      //one-to-many relationship
        }
    }
}