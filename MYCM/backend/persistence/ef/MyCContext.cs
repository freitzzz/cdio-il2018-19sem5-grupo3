using Microsoft.EntityFrameworkCore;
using core.domain;
using backend.config;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using System;
using System.Threading;

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
        /// Database set containing the single saved instance of CustomizedProductSerialNumber.
        /// </summary>
        /// <value>Gets/sets the database set.</value>
        public DbSet<CustomizedProductSerialNumber> CustomizedProductSerialNumber { get; set; }

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
        public MyCContext(DbContextOptions<MyCContext> options) : base(options)
        {
            if (BackendConfiguration.entityFrameworkContexts.containsKey(Thread.CurrentThread.ManagedThreadId))
            {
                BackendConfiguration.entityFrameworkContexts.removeKey(Thread.CurrentThread.ManagedThreadId);
            }
            BackendConfiguration.entityFrameworkContexts.put(Thread.CurrentThread.ManagedThreadId, this);
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //!Define How Entities Are Mapped Here
            //Dimension inheritance mapping
            builder.Entity<ContinuousDimensionInterval>().HasBaseType<Dimension>();
            builder.Entity<DiscreteDimensionInterval>().HasBaseType<Dimension>();
            builder.Entity<SingleValueDimension>().HasBaseType<Dimension>();

            builder.Entity<Algorithm>().HasMany(a => a.inputValues).WithOne();
            builder.Entity<InputValue>().Property<long>("inputId");
            builder.Entity<InputValue>().HasOne(iv => iv.input).WithOne().HasForeignKey<InputValue>("inputId");

            //Algorithm inheritance mapping
            builder.Entity<WidthPercentageAlgorithm>().HasBaseType<Algorithm>();
            builder.Entity<SameMaterialAndFinishAlgorithm>().HasBaseType<Algorithm>();

            //PriceTableEntry inheritance mapping
            /* builder.Entity<MaterialPriceTableEntry>().HasBaseType<PriceTableEntry>();
            builder.Entity<FinishPriceTableEntry>().HasBaseType<PriceTableEntry>(); */
            builder.Entity<DiscreteDimensionInterval>().HasMany(i => i.values).WithOne(); //one-to-many relationship

            builder.Entity<Measurement>().Property<long>("heightDimensionId");
            builder.Entity<Measurement>().Property<long>("widthDimensionId");
            builder.Entity<Measurement>().Property<long>("depthDimensionId");

            builder.Entity<Measurement>().HasOne(m => m.height).WithOne().HasForeignKey<Measurement>("heightDimensionId").OnDelete(DeleteBehavior.Cascade);        //one-to-one relationship
            builder.Entity<Measurement>().HasOne(m => m.width).WithOne().HasForeignKey<Dimension>("depthDimensionId").OnDelete(DeleteBehavior.Cascade);         //one-to-one relationship
            builder.Entity<Measurement>().HasOne(m => m.depth).WithOne().HasForeignKey<Dimension>("widthDimensionId").OnDelete(DeleteBehavior.Cascade);         //one-to-one relationship
            builder.Entity<Measurement>().HasMany(m => m.restrictions).WithOne().OnDelete(DeleteBehavior.Cascade); //one-to-many relationship

            //Configure many-to-one relationship between parent and child ProductCategory
            builder.Entity<ProductCategory>().HasOne(c => c.parent).WithMany().HasForeignKey(c => c.parentId);

            builder.Entity<Material>().HasMany(m => m.Colors).WithOne().OnDelete(DeleteBehavior.Cascade);                  //one-to-many relationship
            builder.Entity<Material>().HasMany(m => m.Finishes).WithOne().OnDelete(DeleteBehavior.Cascade);                //one-to-many relationship

            //Configure many-to-many relationship between Product and Material
            builder.Entity<ProductMaterial>().HasKey(pm => new { pm.productId, pm.materialId });
            builder.Entity<ProductMaterial>().HasOne(pm => pm.product).WithMany(p => p.productMaterials).HasForeignKey(pm => pm.productId);
            builder.Entity<ProductMaterial>().HasOne(pm => pm.material).WithMany().HasForeignKey(pm => pm.materialId);
            builder.Entity<ProductMaterial>().HasMany(pm => pm.restrictions).WithOne().OnDelete(DeleteBehavior.Cascade);

            //TODO: remove join class, if possible
            //NOTE: This "join class" is only here as a workaround for now
            builder.Entity<ProductMeasurement>().HasKey(pm => new { pm.productId, pm.measurementId });
            builder.Entity<ProductMeasurement>().HasOne(pm => pm.product).WithMany(p => p.productMeasurements).HasForeignKey(pm => pm.productId);
            builder.Entity<ProductMeasurement>().HasOne(pm => pm.measurement);

            builder.Entity<Product>().HasOne(p => p.productCategory).WithMany();//many-to-one relationship
            builder.Entity<Product>().OwnsOne(p => p.slotWidths);               //embedded ProductSlotWidths

            builder.Entity<Component>().HasKey(c => new { c.fatherProductId, c.complementaryProductId });
            builder.Entity<Component>().HasOne(c => c.fatherProduct).WithMany(p => p.components).HasForeignKey(cp => cp.fatherProductId);
            builder.Entity<Component>().HasOne(c => c.complementaryProduct).WithMany().HasForeignKey(cp => cp.complementaryProductId);
            builder.Entity<Component>().HasMany(c => c.restrictions).WithOne().OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CustomizedProduct>().Property<long>("customizedDimensionsId");
            builder.Entity<CustomizedProduct>().Property<long?>("customizedMaterialId");
            builder.Entity<CustomizedProduct>().HasOne(cp => cp.product);       //one-to-one relationship
            builder.Entity<CustomizedProduct>().HasOne(cp => cp.customizedDimensions).WithOne().HasForeignKey<CustomizedProduct>("customizedDimensionsId").OnDelete(DeleteBehavior.Cascade); //one-to-one relationship
            builder.Entity<CustomizedProduct>().HasOne(cp => cp.customizedMaterial).WithOne().HasForeignKey<CustomizedProduct>("customizedMaterialId").OnDelete(DeleteBehavior.Cascade);
            builder.Entity<CustomizedProduct>().HasMany(cp => cp.slots).WithOne().OnDelete(DeleteBehavior.Cascade);        //one-to-many relationship

            builder.Entity<CustomizedMaterial>().Property<long?>("finishId");
            builder.Entity<CustomizedMaterial>().Property<long?>("colorId");
            builder.Entity<CustomizedMaterial>().HasOne(cm => cm.material).WithMany();
            builder.Entity<CustomizedMaterial>().HasOne(cm => cm.finish).WithOne().HasForeignKey<CustomizedMaterial>("finishId").OnDelete(DeleteBehavior.Cascade);
            builder.Entity<CustomizedMaterial>().HasOne(cm => cm.color).WithOne().HasForeignKey<CustomizedMaterial>("colorId").OnDelete(DeleteBehavior.Cascade);


            builder.Entity<Slot>().Property<long>("customizedDimensionsId");
            builder.Entity<Slot>().HasOne(s => s.slotDimensions).WithOne().HasForeignKey<Slot>("customizedDimensionsId").OnDelete(DeleteBehavior.Cascade);              //one-to-one relationship
            builder.Entity<Slot>().HasMany(s => s.customizedProducts).WithOne(cp => cp.insertedInSlot).HasForeignKey(cp => cp.insertedInSlotId).OnDelete(DeleteBehavior.Cascade);          //one-to-many relationship

            //Compound key for CollectionProduct
            builder.Entity<CollectionProduct>().HasKey(cp => new { cp.customizedProductId, cp.customizedProductCollectionId });
            //Many-to-many relationship between CustomizedProductCollection and CustomizedProduct
            builder.Entity<CollectionProduct>().HasOne(cp => cp.customizedProductCollection)
                .WithMany(c => c.collectionProducts).HasForeignKey(cp => cp.customizedProductCollectionId);
            builder.Entity<CollectionProduct>().HasOne(cp => cp.customizedProduct).WithMany().HasForeignKey(cp => cp.customizedProductId);


            //Compound key for CatalogueCollectionProduct
            //Many-to-Many relationship between CatalogueCollection and CustomizedProduct
            builder.Entity<CatalogueCollectionProduct>().HasKey(ccp => new { ccp.commercialCatalogueId, ccp.customizedProductCollectionId, ccp.customizedProductId });
            builder.Entity<CatalogueCollectionProduct>().HasOne(ccp => ccp.customizedProduct)
                .WithMany().HasForeignKey(ccp => ccp.customizedProductId);
            builder.Entity<CatalogueCollectionProduct>().HasOne(ccp => ccp.catalogueCollection)
                .WithMany(cc => cc.catalogueCollectionProducts).HasForeignKey(ccp => new { ccp.commercialCatalogueId, ccp.customizedProductCollectionId });

            builder.Entity<CommercialCatalogue>().HasMany(catalogue => catalogue.catalogueCollectionList)
                .WithOne().HasForeignKey(catalogueCollection => catalogueCollection.commercialCatalogueId);

            builder.Entity<CatalogueCollection>().HasKey(cc => new { cc.commercialCatalogueId, cc.customizedProductCollectionId });
            builder.Entity<CatalogueCollection>().HasOne(catalogueCollection => catalogueCollection.customizedProductCollection)
                .WithMany().HasForeignKey(cc => cc.customizedProductCollectionId);
            builder.Entity<CatalogueCollection>().HasMany(catalogueCollection => catalogueCollection.catalogueCollectionProducts);

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
        }
    }
}