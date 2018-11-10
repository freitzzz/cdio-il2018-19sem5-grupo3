using System;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace core.domain
{
    /// <summary>
    /// Class acting as a join table used for establishing the many-to-many relationship between CustomizedProductCollection and CustomizedProduct.
    /// </summary>
    /// 

    //!Please note that this class should have no behaviour other than simply establish the relationship between the two domain entities
    //!DO NOT USE THIS IN THE DTO'S
    public class CollectionProduct
    {
        /// <summary>
        /// Constant representing the error message that should be presented when attempting to create an instance of CollectionProduct with a null CustomizedProductCollection.
        /// </summary>
        private const string ERROR_NULL_COLLECTION = "The given customized product collection may not be null.";

        /// <summary>
        /// Constant representing the error message that should be presented when attempting to create an instance of CollectionProduct with a null CustomizedProduct.
        /// </summary>
        private const string ERROR_NULL_CUSTOMIZED_PRODUCT = "The given customized product may not be null.";

        /// <summary>
        /// CustomizedProduct's database identifier, this attribute is a foreign key part of the CollectionProduct's primary key.
        /// </summary>
        /// <value>Gets/sets the value of the CustomizedProduct's database identifier.</value>
        public long customizedProductId { get; protected set; }
        private CustomizedProduct _customizedProduct;                       //!private field used for lazy loading, do not use this for storing or fetching data
        /// <summary>
        ///  
        /// </summary>
        /// <param name="_customizedProduct"></param>
        /// <returns></returns>
        public CustomizedProduct customizedProduct { get => LazyLoader.Load(this, ref _customizedProduct); protected set => _customizedProduct = value; }


        /// <summary>
        /// CustomizedProductCollection's database identifier, this attribute is a foreign key part of the CollectionProduct's primary key.
        /// </summary>
        /// <value>Gets/sets the value of the CustomizedProductCollection's database identifier.</value>
        public long customizedProductCollectionId { get; protected set; }
        private CustomizedProductCollection _customizedProductCollection;   //!private field used for lazy loading, do not use this for storing or fetching data
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_customizedProductCollection"></param>
        /// <returns></returns>
        public CustomizedProductCollection customizedProductCollection { get => LazyLoader.Load(this, ref _customizedProductCollection); protected set => _customizedProductCollection = value; }

        /// <summary>
        /// LazyLoader instance injected by the framework.
        /// </summary>
        /// <value>Private get/set the value of the injected LazyLoader.</value>
        private ILazyLoader LazyLoader { get; set; }

        /// <summary>
        /// Private constructor used for injecting the instance of the LazyLoader.
        /// </summary>
        /// <param name="lazyLoader">LazyLoader instance being injected by the framework.</param>
        private CollectionProduct(ILazyLoader lazyLoader)
        {
            this.LazyLoader = lazyLoader;
        }

        /// <summary>
        /// Empty constructor for ORM.
        /// </summary>
        protected CollectionProduct() { }

        /// <summary>
        /// Creates a new instance of CollectionProduct with a given CustomizedProductCollection and a CustomizedProduct.
        /// </summary>
        /// <param name="collection">Instance of CustomizedProductCollection.</param>
        /// <param name="customizedProduct">Instance of CustomizedProduct.</param>
        public CollectionProduct(CustomizedProductCollection collection, CustomizedProduct customizedProduct)
        {
            checkCollection(collection);
            checkCustomizedProduct(customizedProduct);
            this.customizedProductCollection = collection;
            this.customizedProduct = customizedProduct;
        }

        /// <summary>
        /// Checks if the given instance of CustomizedProductCollection is null or not. If it's null then an ArgumentException is thrown.
        /// </summary>
        /// <param name="collection">Instance of CustomizedProductCollection being checked.</param>
        private void checkCollection(CustomizedProductCollection collection)
        {
            if (collection == null)
            {
                throw new ArgumentException(ERROR_NULL_COLLECTION);
            }
        }

        /// <summary>
        /// Checks if the given of CustomizedProduct is null or not. If it's null then an ArgumentException is thrown.
        /// </summary>
        /// <param name="customizedProduct">Instance of CustomizedProduct being checked.</param>
        private void checkCustomizedProduct(CustomizedProduct customizedProduct)
        {
            if (customizedProduct == null)
            {
                throw new ArgumentException(ERROR_NULL_CUSTOMIZED_PRODUCT);
            }
        }


    }
}