using Microsoft.EntityFrameworkCore;
using core.domain;
using backend.config;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using System;

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
        public DbSet<CustomizedProduct> CustomizedProduct { get; set; }

        /// <summary>
        /// Database set containing all of the saved instances of CustomizedProductCollection.
        /// </summary>
        /// <value>Gets/sets the database set containing all the saved instances of CustomizedProductCollection.</value>
        public DbSet<CustomizedProductCollection> CustomizedProductCollection { get; set; }

        /// <summary>
        /// Database set containing all of the saved instances of MaterialPriceTableEntry
        /// </summary>
        /// <value>Gets/Sets the database set containing all the saved instances of MaterialPriceTableEntry</value>
        public DbSet<MaterialPriceTableEntry> MaterialPriceTable { get; set; }

        /// <summary>
        /// Database set containing all of the saved instances of FinishPriceTableEntry
        /// </summary>
        /// <value>Gets/Sets the database set containing all the saved instances of FinishPriceTableEntry</value>
        public DbSet<FinishPriceTableEntry> FinishPriceTable { get; set; }

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

            //PriceTableEntry inheritance mapping
            /* builder.Entity<MaterialPriceTableEntry>().HasBaseType<PriceTableEntry>();
            builder.Entity<FinishPriceTableEntry>().HasBaseType<PriceTableEntry>(); */
            builder.Entity<DiscreteDimensionInterval>().HasMany(i => i.values); //one-to-many relationship

            builder.Entity<Measurement>().HasOne(m => m.height);        //one-to-one relationship
            builder.Entity<Measurement>().HasOne(m => m.depth);         //one-to-one relationship
            builder.Entity<Measurement>().HasOne(m => m.width);         //one-to-one relationship
            builder.Entity<Measurement>().HasMany(m => m.restrictions); //one-to-many relationship

            //Configure many-to-one relationship between parent and child ProductCategory
            builder.Entity<ProductCategory>().HasOne(c => c.parent).WithMany().HasForeignKey(c => c.parentId);

            builder.Entity<Material>().HasMany(m => m.Colors);                  //one-to-many relationship
            builder.Entity<Material>().HasMany(m => m.Finishes);                //one-to-many relationship

            //Configure many-to-many relationship between Product and Material
            builder.Entity<ProductMaterial>().HasKey(pm => new { pm.productId, pm.materialId });
            builder.Entity<ProductMaterial>().HasOne(pm => pm.product).WithMany(p => p.productMaterials).HasForeignKey(pm => pm.productId);
            builder.Entity<ProductMaterial>().HasOne(pm => pm.material).WithMany().HasForeignKey(pm => pm.materialId);
            builder.Entity<ProductMaterial>().HasMany(pm => pm.restrictions);

            //TODO: remove join class, if possible
            //NOTE: This "join class" is only here as a workaround for now
            builder.Entity<ProductMeasurement>().HasKey(pm => new { pm.productId, pm.measurementId });
            builder.Entity<ProductMeasurement>().HasOne(pm => pm.product).WithMany(p => p.productMeasurements).HasForeignKey(pm => pm.productId);
            builder.Entity<ProductMeasurement>().HasOne(pm => pm.measurement);

            builder.Entity<Product>().HasOne(p => p.productCategory);           //many-to-one relationship
            builder.Entity<Product>().OwnsOne(p => p.slotWidths);               //embedded ProductSlotWidths

            builder.Entity<Component>().HasKey(c => new { c.fatherProductId, c.complementaryProductId });
            builder.Entity<Component>().HasOne(c => c.fatherProduct).WithMany(p => p.components).HasForeignKey(cp => cp.fatherProductId);
            builder.Entity<Component>().HasOne(c => c.complementaryProduct).WithMany().HasForeignKey(cp => cp.complementaryProductId);
            builder.Entity<Component>().HasMany(c => c.restrictions);

            builder.Entity<CustomizedProduct>().HasOne(cp => cp.product);       //one-to-one relationship
            builder.Entity<CustomizedProduct>().OwnsOne(cp => cp.customizedDimensions); //embedded Dimensions
            builder.Entity<CustomizedProduct>().HasOne(cp => cp.customizedMaterial);
            builder.Entity<CustomizedProduct>().HasMany(cp => cp.slots).WithOne().OnDelete(DeleteBehavior.Cascade);        //one-to-many relationship

            builder.Entity<CustomizedMaterial>().HasOne(cm => cm.material).WithMany();
            builder.Entity<CustomizedMaterial>().HasOne(cm => cm.finish);
            builder.Entity<CustomizedMaterial>().HasOne(cm => cm.color);


            builder.Entity<Slot>().OwnsOne(s => s.slotDimensions);              //embedded Dimensions
            builder.Entity<Slot>().HasMany(s => s.customizedProducts).WithOne(cp => cp.insertedInSlot).HasForeignKey(cp => cp.insertedInSlotId).OnDelete(DeleteBehavior.Cascade);          //one-to-many relationship

            //Compound key for CollectionProduct
            builder.Entity<CollectionProduct>().HasKey(cp => new { cp.customizedProductId, cp.customizedProductCollectionId });
            //Many-to-many relationship between CustomizedProductCollection and CustomizedProduct
            builder.Entity<CollectionProduct>().HasOne(cp => cp.customizedProductCollection)
                .WithMany(c => c.collectionProducts).HasForeignKey(cp => cp.customizedProductCollectionId);
            builder.Entity<CollectionProduct>().HasOne(cp => cp.customizedProduct).WithMany().HasForeignKey(cp => cp.customizedProductId);


            //Compound key for CatalogueCollectionProduct
            //Many-to-Many relationship between CatalogueCollection and CustomizedProduct
            builder.Entity<CatalogueCollectionProduct>().HasKey(ccp => new { ccp.catalogueCollectionId, ccp.customizedProductId });
            builder.Entity<CatalogueCollectionProduct>().HasOne(ccp => ccp.customizedProduct)
                .WithMany().HasForeignKey(ccp => ccp.customizedProductId);
            builder.Entity<CatalogueCollectionProduct>().HasOne(ccp => ccp.catalogueCollection)
                .WithMany(cc => cc.catalogueCollectionProducts).HasForeignKey(ccp => ccp.catalogueCollectionId);

            builder.Entity<CommercialCatalogueCatalogueCollection>().HasKey(cccc => new { cccc.commercialCatalogueId, cccc.catalogueCollectionId });
            builder.Entity<CommercialCatalogueCatalogueCollection>()
                .HasOne(cccc => cccc.commercialCatalogue).WithMany(cc => cc.catalogueCollectionList).HasForeignKey(cccc => cccc.commercialCatalogueId);
            builder.Entity<CommercialCatalogueCatalogueCollection>().HasOne(cccc => cccc.catalogueCollection).WithOne();


            //TimePeriod conversion mapping
            var localDateTimeConverter = new ValueConverter<LocalDateTime, DateTime>(v => v.ToDateTimeUnspecified(), v => LocalDateTime.FromDateTime(v));
            builder.Entity<TimePeriod>().Property(tp => tp.startingDate).HasConversion(localDateTimeConverter);
            builder.Entity<TimePeriod>().Property(tp => tp.endingDate).HasConversion(localDateTimeConverter);


            // Material Price Table Entries Mapping
            builder.Entity<MaterialPriceTableEntry>().HasKey(mpte => new { mpte.Id });
            builder.Entity<MaterialPriceTableEntry>().HasOne(mpte => mpte.entity).WithMany();
            builder.Entity<MaterialPriceTableEntry>().OwnsOne(mpte => mpte.price);
            builder.Entity<MaterialPriceTableEntry>().HasOne(mpte => mpte.timePeriod);

            // Finish Price Table Entries Mapping
            builder.Entity<FinishPriceTableEntry>().HasKey(fpte => new { fpte.Id });
            builder.Entity<FinishPriceTableEntry>().HasOne(fpte => fpte.entity);
            builder.Entity<FinishPriceTableEntry>().OwnsOne(fpte => fpte.price);
            builder.Entity<FinishPriceTableEntry>().HasOne(fpte => fpte.timePeriod);

            //TODO: DISABLE CASCADE DELETION FROM JOIN TABLES
        }
    }
}