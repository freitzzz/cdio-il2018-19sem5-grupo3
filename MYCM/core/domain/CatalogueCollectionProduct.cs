using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace core.domain
{
    /// <summary>
    /// Class that represents a Join Table, establishing the connectiong between a CatalogueCollection and a CustomizedProduct.
    /// </summary>
    public class CatalogueCollectionProduct
    {

        //*Since this class represents a join table, its primary key is composed of 3 foreign keys: the CommercialCatalogue's ID, 
        //*the CustomizedProductCollection's ID (both are part of CatalogueCollection's primary key) and the CustomizedProduct's ID.


        private const string ERROR_NULL_CATALOGUE_COLLECTION = "The Catalogue Collection can not be null";

        private const string ERROR_NULL_CUSTOMIZED_PRODUCT = "The Customized Product can not be null";

        /// <summary>
        /// CommercialCatalogue's persistence identifier.
        /// </summary>
        /// <value>Gets/Protected sets the CommercialCatalogue's persistence identifier.</value>
        public long commercialCatalogueId { get; protected set; }

        /// <summary>
        /// CustomizedProductCollection's persistence identifier.
        /// </summary>
        /// <value>Gets/Protected sets the CustomizedProductCollection's persistence identifier.</value>
        public long customizedProductCollectionId { get; protected set; }

        /// <summary>
        /// CatalogueCollection in which the CustomizedProduct is included.
        /// </summary>
        /// <value>Gets/sets the CatalogueCollection.</value>
        private CatalogueCollection _catalogueCollection;   //!private field used for lazy loading, do not use this for storing or fetching data
        public CatalogueCollection catalogueCollection
        {
            get => LazyLoader.Load(this, ref _catalogueCollection); protected set => _catalogueCollection = value;
        }

        /// <summary>
        /// CustomizedProduct's database identifier.
        /// </summary>
        /// <value>Gets/sets the CustomizedProduct's database identifier.</value>
        public long customizedProductId { get; protected set; }

        /// <summary>
        /// CustomizedProduct associated to the CatalogueCollectionProduct.
        /// </summary>
        /// <value>Gets/sets the CustomizedProduct.</value>
        private CustomizedProduct _customizedProduct;   //!private field used for lazy loading, do not use this for storing or fetching data
        public CustomizedProduct customizedProduct
        {
            get => LazyLoader.Load(this, ref _customizedProduct); protected set => _customizedProduct = value;
        }

        /// <summary>
        /// LazyLoader injected by the framework.
        /// </summary>
        /// <value>Gets/sets the value of the injected LazyLoader.</value>
        private ILazyLoader LazyLoader { get; set; }

        /// <summary>
        /// Private constructor used for injecting a LazyLoader.
        /// </summary>
        /// <param name="lazyLoader">LazyLoader being injected.</param>
        private CatalogueCollectionProduct(ILazyLoader lazyLoader)
        {
            this.LazyLoader = lazyLoader;
        }

        /// <summary>
        /// Empty constructor used by ORM.
        /// </summary>
        protected CatalogueCollectionProduct() { }

        /// <summary>
        /// Creates a new instance of CatalogueCollectionProduct with a givne CatalogueCollection and a CustomizedProduct.
        /// </summary>
        /// <param name="catalogueCollection">CatalogueCollection to which this CatalogueCollectionProduct belongs.</param>
        /// <param name="customizedProduct">CustomizedProduct associated to this CatalogueCollectionProduct.</param>
        public CatalogueCollectionProduct(CatalogueCollection catalogueCollection, CustomizedProduct customizedProduct)
        {
            checkParameters(catalogueCollection, customizedProduct);
            this.catalogueCollection = catalogueCollection;
            this.customizedProduct = customizedProduct;
        }

        /// <summary>
        /// Checks if the instances of CatalogueCollection and CustomizedProduct are not null; If they are an ArgumentException is thrown.
        /// </summary>
        /// <param name="catalogueCollection">Instance of CatalogueCollection being checked.</param>
        /// <param name="customizedProduct">Instance of CustomizedProduct being checked.</param>
        private void checkParameters(CatalogueCollection catalogueCollection, CustomizedProduct customizedProduct)
        {
            if (catalogueCollection == null)
            {
                throw new ArgumentException(ERROR_NULL_CATALOGUE_COLLECTION);
            }
            if (customizedProduct == null)
            {
                throw new ArgumentException(ERROR_NULL_CUSTOMIZED_PRODUCT);
            }
        }
    }
}