using System;
using System.Collections.Generic;
using core.dto;
using Microsoft.EntityFrameworkCore.Infrastructure;
using support.domain.ddd;
using support.dto;
using support.utils;
using System.Linq;
using support.domain;
using static core.domain.CustomizedProduct;

namespace core.domain
{
    /// <summary>
    /// Class that represents a collection of customized products
    /// <br>CustomizedProductCollection is an aggregate
    /// </summary>
    /// <typeparam name="CustomizedProductCollectionDTO">Generic-Type of the customized product collection DTO</typeparam>
    public class CustomizedProductCollection : Activatable, AggregateRoot<string>, DTOAble<CustomizedProductCollectionDTO>
    {
        /// <summary>
        /// Constant that represents the message that occures if the name of the collection is invalid
        /// </summary>
        private const string INVALID_CUSTOMIZED_PRODUCT_COLLECTION_NAME = "The collection name is invalid!";

        /// <summary>
        /// Constant that represents the message that occures if the customized products of the collection are invalid
        /// </summary>
        private const string INVALID_COLLECTION_CUSTOMIZED_PRODUCTS = "The collection customized products are invalid!";

        ///<summary>
        ///Constant that represents the message that ocurrs if the Customized Product isn't valid
        ///</summary>
        private const string INVALID_CUSTOMIZED_PRODUCT = "The Customized Product is invalid!";

        /// <summary>
        /// Constant that represents the message that occurs if the customized product isn't from the collection
        /// </summary>
        private const string CUSTOMIZED_PRODUCT_NOT_FROM_COLLECTION = "The customized product trying to be removed doesn't exist in this collection";

        /// <summary>
        /// Constant that represents the message that occurs if the customized product trying to be added is in PENDING State
        /// </summary>
        private const string PENDING_CUSTOMIZED_PRODUCT = "The customized product trying to be added to the collection isn't finished yet!";

        /// <summary>
        /// Constant that represents the message that occurs if the customized produc trying to be added is already in the collection
        /// </summary>
        private const string CUSTOMIZED_PRODUCT_EXISTS_IN_COLLECTION = "This customized product is already in the collection!";

        /// <summary>
        /// Persistence identifier of the current CustomizedProductCollection
        /// </summary>
        public long Id { get; internal set; }

        /// <summary>
        /// String with the collection name
        /// </summary>
        public string name { get; protected set; }

        /// <summary>
        /// List with the collection customized products
        /// </summary>
        private List<CollectionProduct> _collectionProducts; //!private field used for lazy loading, do not use this for storing or fetching data
        public List<CollectionProduct> collectionProducts { get => LazyLoader.Load(this, ref _collectionProducts); protected set => _collectionProducts = value; }

        /// <summary>
        /// LazyLoader injected by the framework.
        /// </summary>
        /// <value>Private Gets/Sets the LazyLoader.</value>
        private ILazyLoader LazyLoader;

        /// <summary>
        /// Empty constructor for ORM.
        /// </summary>
        protected CustomizedProductCollection() { }

        /// <summary>
        /// Constructor used for injecting the LazyLoader.
        /// </summary>
        /// <param name="lazyLoader">LazyLoader being injected.</param>
        private CustomizedProductCollection(ILazyLoader lazyLoader)
        {
            this.LazyLoader = lazyLoader;
        }

        /// <summary>
        /// Builds a new CustomizedProductCollection with the collection name
        /// </summary>
        /// <param name="name">string with the customized products collection name</param>
        public CustomizedProductCollection(string name)
        {
            checkCustomizedProductCollectionName(name);
            this.name = name.Trim();
            this.collectionProducts = new List<CollectionProduct>();
        }

        /// <summary>
        /// Builds a new CustomizedProductCollection with the collection name and customized products
        /// </summary>
        /// <param name="name">string with the customized products collection name</param>
        /// <param name="customizedProducts">IEnumerable with the collection customized products</param>
        public CustomizedProductCollection(string name, IEnumerable<CustomizedProduct> customizedProducts) : this(name)
        {
            checkCollectionCustomizedProducts(customizedProducts);
            foreach (CustomizedProduct customizedProduct in customizedProducts)
            {
                this.collectionProducts.Add(new CollectionProduct(this, customizedProduct));
            }
        }

        /// <summary>
        /// Checks if the customized product collection properties are valid
        /// </summary>
        /// <param name="name">String with the customized product collection name</param>
        private void checkCustomizedProductCollectionName(string name)
        {
            if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException(INVALID_CUSTOMIZED_PRODUCT_COLLECTION_NAME);
        }

        /// <summary>
        /// Checks if the collection customized products are valid
        /// </summary>
        /// <param name="enumerableCustomizedProducts">IEnumerable with the collection customized products</param>
        private void checkCollectionCustomizedProducts(IEnumerable<CustomizedProduct> enumerableCustomizedProducts)
        {
            if (Collections.isEnumerableNullOrEmpty(enumerableCustomizedProducts))
                throw new ArgumentException(INVALID_COLLECTION_CUSTOMIZED_PRODUCTS);
            checkCustomizedProductsDuplicates(enumerableCustomizedProducts);
            checkCustomizedProductsState(enumerableCustomizedProducts);
        }

        /// <summary>
        /// Checks if an enumerable of customized products have duplicates
        /// </summary>
        /// <param name="customizedProducts">IEnumerable with the customized products</param>
        private void checkCustomizedProductsDuplicates(IEnumerable<CustomizedProduct> customizedProducts)
        {
            HashSet<int> customizedProductsHashes = new HashSet<int>();
            foreach (CustomizedProduct customizedProduct in customizedProducts)
                if (!customizedProductsHashes.Add(customizedProduct.GetHashCode()))
                    throw new ArgumentException(INVALID_COLLECTION_CUSTOMIZED_PRODUCTS);
        }

        /// <summary>
        /// Checks if any customized product from an enumerable have a PENDING State
        /// </summary>
        /// <param name="customizedProducts">IEnumerable with the customized products</param>
        private void checkCustomizedProductsState(IEnumerable<CustomizedProduct> customizedProducts)
        {
            foreach (CustomizedProduct customizedProduct in customizedProducts)
            {
                if (customizedProduct.status == CustomizationStatus.PENDING)
                {
                    throw new ArgumentException(PENDING_CUSTOMIZED_PRODUCT);
                }
            }
        }

        /// <summary>
        /// Add's a customized product to the current customized products collection
        /// </summary>
        /// <param name="customizedProduct">CustomizedProduct with the customized product being added</param>
        /// <returns>boolean true if the customized product was added with success, false if not</returns>
        public void addCustomizedProduct(CustomizedProduct customizedProduct)
        {
            checkIfCustomizedProductIsValidForAddition(customizedProduct);
            collectionProducts.Add(new CollectionProduct(this, customizedProduct));
        }

        /// <summary>
        /// Changes the name of the current customized products collection
        /// </summary>
        /// <param name="name">string with the new collection name</param>
        /// <returns>boolean true if the collection name was changed with success, false if not</returns>
        public void changeName(string name)
        {
            if (String.IsNullOrWhiteSpace(name) || this.name.Equals(name)) throw new ArgumentException(INVALID_CUSTOMIZED_PRODUCT_COLLECTION_NAME);
            this.name = name;
        }

        /// <summary>
        /// Removes a customized product from the current customized products collection
        /// </summary>
        /// <param name="customizedProduct">CustomizedProduct being removed</param>
        /// <returns>boolean true if the customized product was removed with success, false if not</returns>
        public void removeCustomizedProduct(CustomizedProduct customizedProduct)
        {
            bool removed = collectionProducts.Remove(collectionProducts.Where(cc => cc.customizedProduct.Equals(customizedProduct)).FirstOrDefault());
            if (!removed) throw new ArgumentException(CUSTOMIZED_PRODUCT_NOT_FROM_COLLECTION);
        }

        /// <summary>
        /// Checks if a customized product is valid for addition on the collection
        /// </summary>
        /// <param name="customizedProduct">CustomizedProduct with the customized product being validated</param>
        /// <returns>boolean true if the customized product is valid for addition, false if not</returns>
        private void checkIfCustomizedProductIsValidForAddition(CustomizedProduct customizedProduct)
        {
            if (customizedProduct == null) throw new ArgumentException(INVALID_CUSTOMIZED_PRODUCT);
            if (customizedProduct.status == CustomizationStatus.PENDING) throw new ArgumentException(PENDING_CUSTOMIZED_PRODUCT);
            if (collectionProducts.Select(cc => cc.customizedProduct).Contains(customizedProduct)) throw new ArgumentException(CUSTOMIZED_PRODUCT_EXISTS_IN_COLLECTION);
        }

        /// <summary>
        /// Checks if the CustomizedProductCollection contains the given CustomizedProduct.
        /// </summary>
        /// <param name="customizedProduct">CustomizedProduct being checked.</param>
        /// <returns>true if the CustomizedProductCollection contains the CustomizedProduct; otherwise, false.</returns>
        public bool hasCustomizedProduct(CustomizedProduct customizedProduct)
        {
            return customizedProduct != null && collectionProducts.Select(cp => cp.customizedProduct).Contains(customizedProduct);
        }

        /// <summary>
        /// Returns the current collection identity
        /// </summary>
        /// <returns>String with the collection identity</returns>
        public string id() { return name; }

        /// <summary>
        /// Checks if a certain customized product collection identity is the same as the current one
        /// </summary>
        /// <param name="comparingEntity">string with the comparing customized product collection identity</param>
        /// <returns>boolean true if both identities are the same, false if not</returns>
        public bool sameAs(string comparingEntity) { return id().Equals(comparingEntity); }

        /// <summary>
        /// Returns the current collection of customized products as a DTO
        /// </summary>
        /// <returns>CustomizedProductCollectionDTO with the current DTO representation of the customized products collection</returns>
        public CustomizedProductCollectionDTO toDTO()
        {
            throw new NotImplementedException();
        }

        ///<summary>
        ///Returns the generated hash code of the Customized Material.
        ///</summary>
        public override int GetHashCode()
        {
            return id().GetHashCode();
        }

        ///<summary>
        ///Checks if a certain Collection is the same as a received object.
        ///</summary>
        ///<param name = "comparingCustomizedProductCollection">object to compare to the current Collection</param>
        public override bool Equals(object comparingCustomizedProductCollection)
        {
            if (this == comparingCustomizedProductCollection) return true;
            return comparingCustomizedProductCollection is CustomizedProductCollection
                && ((CustomizedProductCollection)comparingCustomizedProductCollection).id().Equals(this.id());
        }

        ///<summary>
        ///Returns a textual description of the Collection.
        ///</summary>
        public override string ToString()
        {
            return string.Format("Name {0}", name);
        }
    }
}