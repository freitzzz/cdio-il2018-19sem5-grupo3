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
        /// Database set containing all of the saved instances of CommercialCatalogue.
        /// </summary>
        /// <value>Gets/sets the database set containing all the saved instances of CommercialCatalogue.</value>
        public DbSet<CommercialCatalogue> CommercialCatalogue { get; set; }

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
        /// Database set containing all of the saved instances of CustomizedProduct.
        /// </summary>
        /// <value>Gets/sets the database set containing all the saved instances of CustomizedProduct.</value>
        public DbSet<CustomizedProductCollection> CustomizedProduct { get; set; }

        /// <summary>
        /// Database set containing all of the saved instances of CustomizedProductCollection.
        /// </summary>
        /// <value>Gets/sets the database set containing all the saved instances of CustomizedProductCollection.</value>
        public DbSet<CustomizedProductCollection> CustomizedProductCollection { get; set; }
        

        /// <summary>
        /// Constructs a new database for the MakeYourCloset application.
        /// </summary>
        /// <param name="options">The options for the context.</param>
        /// <returns>New instance of MyCContext.</returns>
        public MyCContext(DbContextOptions<MyCContext> options) : base(options) { BackendConfiguration.entityFrameworkContext = this; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //!Define How Entities Are Mapped Here
            //Dimension inheritance mapping
            builder.Entity<ContinuousDimensionInterval>().HasBaseType<Dimension>();
            builder.Entity<DiscreteDimensionInterval>().HasBaseType<Dimension>();
            builder.Entity<SingleValueDimension>().HasBaseType<Dimension>();


            builder.Entity<DiscreteDimensionInterval>().HasMany(i => i.values).WithOne();   //one-to-many relationship

            builder.Entity<Material>().HasMany(m => m.Colors).WithOne();        //one-to-many relationship
            builder.Entity<Material>().HasMany(m => m.Finishes).WithOne();      //one-to-many relationship

            builder.Entity<Product>().HasOne(p => p.productCategory);           //one-to-one relationship
            builder.Entity<Product>().HasMany(p => p.depthValues).WithOne();    //one-to-many relationship
            builder.Entity<Product>().HasMany(p => p.widthValues).WithOne();    //one-to-many relationship
            builder.Entity<Product>().HasMany(p => p.heightValues).WithOne();   //one-to-many relationship
            builder.Entity<Product>().HasMany(p => p.productMaterials).WithOne(pm =>pm.product);

            builder.Entity<ProductMaterial>().HasOne(m =>m.material).WithMany();

            builder.Entity<CommercialCatalogue>().HasMany(c => c.collectionList).WithOne();
            builder.Entity<ProductCategory>().HasOne(c => c.parent).WithOne().HasForeignKey<ProductCategory>(c => c.parentId);
        }
    }
}