using System;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace core.domain
{
    /// <summary>
    /// Represents a join class between a CommercialCatalogue and CatalogueCollection.
    /// </summary>
    public class CommercialCatalogueCatalogueCollection
    {
        /// <summary>
        /// Constant representing an error message being presented when an instance is attempted to be built with a null CommercialCatalogue.
        /// </summary>
        private const string ERROR_NULL_CATALOGUE = "The Commercial Catalogue can not be null.";

        /// <summary>
        /// Constant representing an error message being presented when an instance is attempted to be built with a null CatalogueCollection.
        /// </summary>
        private const string ERROR_NULL_CATALOGUE_COLLECTION = "The Catalogue Collection can not be null.";

        /// <summary>
        /// The CommercialCatalogue's database identifier (foreign key, part of the compound primary key).
        /// </summary>
        /// <value>Gets/sets the CommercialCatalogue's database identifier.</value>
        public long commercialCatalogueId { get; protected set; }
        /// <summary>
        /// CommercialCatalogue to which this is associated with.
        /// </summary>
        /// <value>Gets/sets the value of the CommercialCatalogue.</value>
        private CommercialCatalogue _commercialCatalogue; //!private field used for lazy loading, do not use this for storing or fetching data
        public CommercialCatalogue commercialCatalogue
        {
            get => LazyLoader.Load(this, ref _commercialCatalogue); protected set => _commercialCatalogue = value;
        }

        /// <summary>
        /// The CatalogueCollection's database identifier (foreign key, part of the compound primary key).
        /// </summary>
        /// <value></value>
        public long catalogueCollectionId { get; protected set; }
        /// <summary>
        /// CatalogueCollection to which this is associated with.
        /// </summary>
        /// <value>Gets/sets the value of the CatalogueCollection.</value>
        private CatalogueCollection _catalogueCollection; //!private field used for lazy loading, do not use this for storing or fetching data
        public CatalogueCollection catalogueCollection
        {
            get => LazyLoader.Load(this, ref _catalogueCollection); protected set => _catalogueCollection = value;
        }

        /// <summary>
        /// LazyLoader injected by the framework.
        /// </summary>
        /// <value>Gets/sets the value of the LazyLoader.</value>
        private ILazyLoader LazyLoader { get; set; }

        /// <summary>
        /// Private constructor used by the framework for injecting the LazyLoader.
        /// </summary>
        /// <param name="lazyLoader">LazyLoader being injected by the framework.</param>
        private CommercialCatalogueCatalogueCollection(ILazyLoader lazyLoader)
        {
            this.LazyLoader = lazyLoader;
        }

        /// <summary>
        /// Constructor used by ORM.
        /// </summary>
        protected CommercialCatalogueCatalogueCollection() { }

        /// <summary>
        /// Builds a new instance of CommercialCatalogueCatalogueCollection for a given CommercialCatalogue and CatalogueCollection.
        /// </summary>
        /// <param name="catalogue">CommercialCatalogue to which this instance will be associated.</param>
        /// <param name="catalogueCollection">CatalogueCollection to which this instance will be associated.</param>
        public CommercialCatalogueCatalogueCollection(CommercialCatalogue catalogue, CatalogueCollection catalogueCollection)
        {
            checkParameters(commercialCatalogue, catalogueCollection);
            this.commercialCatalogue = catalogue;
            this.catalogueCollection = catalogueCollection;
        }


        /// <summary>
        /// Checks if the constructor parameters are not null; If one of them is, an ArgumentException is thrown.
        /// </summary>
        /// <param name="commercialCatalogue">CommercialCatalogue being checked.</param>
        /// <param name="catalogueCollection">CatalogueCollection being checked.</param>
        private void checkParameters(CommercialCatalogue commercialCatalogue, CatalogueCollection catalogueCollection)
        {
            if (commercialCatalogue == null)
            {
                throw new ArgumentException(ERROR_NULL_CATALOGUE);
            }
            if (catalogueCollection == null)
            {
                throw new ArgumentException(ERROR_NULL_CATALOGUE_COLLECTION);
            }
        }
    }
}