using System;
using System.Collections.Generic;
using core.dto;
using Microsoft.EntityFrameworkCore.Infrastructure;
using support.domain.ddd;
using support.dto;
using support.utils;
using System.Linq;

namespace core.domain
{
    /// <summary>
    /// Class that represents a collection of customized products
    /// <br>CustomizedProductCollection is an aggregate
    /// </summary>
    /// <typeparam name="CustomizedProductCollectionDTO">Generic-Type of the customized product collection DTO</typeparam>
    public class CustomizedProductCollection : AggregateRoot<string>, DTOAble<CustomizedProductCollectionDTO>
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
        ///Constant that represents the message that ocurrs if the Customized Product
        ///</summary>
        private const string INVALID_CUSTOMIZED_PRODUCT = "The Customized Product is invalid!";

        ///<summary>
        ///Constant that represents the message that ocurrs if a string is invalid.
        ///</summary>
        private const string INVALID_STRING = "The String is invalid!";

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
        /// Boolean which tells if the current collection of customized products is available
        /// </summary>
        public bool available { get; protected set; }

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
            checkCustomizedProductCollectionProperties(name);
            this.name = name;
            this.available = true;
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
        /// Add's a customized product to the current customized products collection
        /// </summary>
        /// <param name="customizedProduct">CustomizedProduct with the customized product being added</param>
        /// <returns>boolean true if the customized product was added with success, false if not</returns>
        public bool addCustomizedProduct(CustomizedProduct customizedProduct)
        {
            if (!isCustomizedProductValidForAddition(customizedProduct))
                return false;
            collectionProducts.Add(new CollectionProduct(this, customizedProduct));
            return true;
        }

        /// <summary>
        /// Changes the name of the current customized products collection
        /// </summary>
        /// <param name="name">string with the new collection name</param>
        /// <returns>boolean true if the collection name was changed with success, false if not</returns>
        public bool changeName(string name)
        {
            if (String.IsNullOrEmpty(name) || this.name.Equals(name)) return false;
            this.name = name;
            return true;
        }

        /// <summary>
        /// Removes a customized product from the current customized products collection
        /// </summary>
        /// <param name="customizedProduct">CustomizedProduct with the customized product being removed</param>
        /// <returns>boolean true if the customized product was removed with success, false if not</returns>
        public bool removeCustomizedProduct(CustomizedProduct customizedProduct) {
            //remove the instace of CollectionProduct with a matching CustomizedProduct     
            return collectionProducts.Remove(collectionProducts.Where(cc => cc.customizedProduct.Equals(customizedProduct)).FirstOrDefault()); 
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
        /// Disables the current customized products collection
        /// </summary>
        /// <returns>boolean true if the current collection was disabled with success, false if not</returns>
        public bool disable()
        {
            if (!available) return false;
            this.available = false;
            return true;
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
            CustomizedProductCollectionDTO dto = new CustomizedProductCollectionDTO();
            dto.name = this.name;
            dto.id = this.Id;
            IEnumerable<CustomizedProduct> customizedProducts = collectionProducts.Select(cc => cc.customizedProduct);
            dto.customizedProducts = new List<CustomizedProductDTO>(DTOUtils.parseToDTOS(customizedProducts));
            return dto;
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
        ///Returns the generated hash code of the Customized Material.
        ///</summary>
        public override int GetHashCode()
        {
            return id().GetHashCode();
        }

        ///<summary>
        ///Returns a textual description of the Collection.
        ///</summary>
        public override string ToString()
        {
            return string.Format("Name {0}", name);
        }

        /// <summary>
        /// Checks if a customized product is valid for addition on the collection
        /// </summary>
        /// <param name="customizedProduct">CustomizedProduct with the customized product being validated</param>
        /// <returns>boolean true if the customized product is valid for addition, false if not</returns>
        private bool isCustomizedProductValidForAddition(CustomizedProduct customizedProduct)
        {
            return customizedProduct != null && !collectionProducts.Select(cc => cc.customizedProduct).Contains(customizedProduct);
        }

        /// <summary>
        /// Checks if the customized product collection properties are valid
        /// </summary>
        /// <param name="name">String with the customized product collection name</param>
        private void checkCustomizedProductCollectionProperties(string name)
        {
            if (String.IsNullOrEmpty(name)) throw new ArgumentException(INVALID_CUSTOMIZED_PRODUCT_COLLECTION_NAME);
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
    }
}