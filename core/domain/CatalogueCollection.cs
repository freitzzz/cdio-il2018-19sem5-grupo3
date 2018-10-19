using System;
using System.Collections.Generic;
using core.dto;
using Microsoft.EntityFrameworkCore.Infrastructure;
using support.dto;

namespace core.domain
{
    /// <summary>
    /// Represents a Catalogue Collection (list of customized products and customized product collections)
    /// </summary>
    /// <typeparam name="CatalogueCollectionDTO">DTO Type</typeparam>
    public class CatalogueCollection : DTOAble<CatalogueCollectionDTO>
    {
        ///<summary>
        ///Constant that represents the message that ocurrs if the Customized Products are not valid
        ///</summary>
        private const string INVALID_LIST = "The Customized Product List is not valid!";

        ///<summary>
        ///Constant that represents the message that ocurrs if the Customized Product Collection is not valid
        ///</summary>
        private const string INVALID_COLLECTION = "The Customized Product Collection is not valid!";

        ///<summary>
        ///Database Identifier
        ///</summary>
        public long Id { get; internal set; }

        ///<summary>
        ///List of Customized Products
        ///</summary>
        private List<CustomizedProduct> _customizedProducts; //!private field used for lazy loading, do not use this for storing or fetching data
        public List<CustomizedProduct> customizedProducts { get => LazyLoader.Load(this, ref _customizedProducts); protected set => _customizedProducts = value; }

        ///<summary>
        ///Customized Product Collection
        ///</summary>
        private CustomizedProductCollection _customizedProductCollection; //!private field used for lazy loading, do not use this for storing or fetching data
        public CustomizedProductCollection customizedProductCollection { get => LazyLoader.Load(this, ref _customizedProductCollection); protected set => _customizedProductCollection = value; }

        /// <summary>
        /// LazyLoader injected by the framework.
        /// </summary>
        /// <value>Private gets/sets the lazy loader</value>
        private ILazyLoader LazyLoader { get; set; }

        ///<summary>
        /// Empty constructor for ORM
        /// </summary>
        protected CatalogueCollection() { }

        /// <summary>
        /// Constructor used for injecting the LazyLoader.
        /// </summary>
        /// <param name="lazyLoader">LazyLoader being injected.</param>
        private CatalogueCollection(ILazyLoader lazyLoader)
        {
            this.LazyLoader = lazyLoader;
        }

        /// <summary>
        /// Builds a new CatologueCollection with a list of customized products and a customized product collection
        /// </summary>
        /// <param name="customizedProduct">list with the customized products collection name</param>
        /// <param name="customizedProductCollection">collection</param> 
        public CatalogueCollection(List<CustomizedProduct> customizedProduct, CustomizedProductCollection customizedProductCollection)
        {
            checkAttributes(customizedProduct, customizedProductCollection);
            this.customizedProducts = customizedProduct;
            this.customizedProductCollection = customizedProductCollection;
        }

        /// <summary>
        /// Buils a new CatologueCollection with a customized product collection
        /// </summary>
        /// <param name="customizedProductCollection">customized product collection</param>
        public CatalogueCollection(CustomizedProductCollection customizedProductCollection)
        {
            if (customizedProductCollection == null) throw new ArgumentException(INVALID_COLLECTION);
            this.customizedProducts = new List<CustomizedProduct>();
            this.customizedProductCollection = customizedProductCollection;
        }

        /// <summary>
        /// Checks if parameters are valid
        /// </summary>
        /// <param name="customizedProduct">list with the customized products collection name</param>
        /// <param name="customizedProductCollection">collection</param> 
        private void checkAttributes(List<CustomizedProduct> customizedProduct, CustomizedProductCollection customizedProductCollection)
        {
            if (customizedProduct == null || customizedProduct.Capacity == 0) throw new ArgumentException(INVALID_LIST);
            if (customizedProductCollection == null) throw new ArgumentException(INVALID_COLLECTION);
        }

        /// <summary>
        /// toDTO method
        ///</summary>
        public CatalogueCollectionDTO toDTO()
        {
            CatalogueCollectionDTO CatalogueCollectionDTO = new CatalogueCollectionDTO();
            List<CustomizedProductDTO> list = new List<CustomizedProductDTO>();
            foreach (CustomizedProduct customizedProduct in this.customizedProducts)
            {
                list.Add(customizedProduct.toDTO());
            }
            CatalogueCollectionDTO.customizedProductsDTO = list;
            CatalogueCollectionDTO.Id = this.Id;
            CatalogueCollectionDTO.customizedProductCollectionDTO = this.customizedProductCollection.toDTO();
            return CatalogueCollectionDTO;
        }

        ///<summary>
        ///Returns the generated hash code of the CommercialCatalogue.
        ///</summary>
        public override int GetHashCode()
        {
            return customizedProductCollection.GetHashCode() + customizedProducts.GetHashCode();
        }

        ///<summary>
        ///Checks if a certain Customized is the same as a received object.
        ///</summary>
        ///<param name = "obj">object to compare to the current Customized Catalogue</param>
        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                CatalogueCollection CatalogueCollection = (CatalogueCollection)obj;
                return customizedProductCollection.Equals(CatalogueCollection.customizedProductCollection) && customizedProducts.Equals(CatalogueCollection.customizedProducts);
            }
        }

        ///<summary>
        ///Returns a textual description of the CommercialCatalogue.
        ///</summary>
        public override string ToString()
        {
            return string.Format("List of Customized Products: {0}, Customized Product Collection {1}", customizedProducts.ToString(), customizedProductCollection.ToString());
        }

        /*  public void addCustomizedProductToCollection(CustomizedProduct customizedProduct){
             if()
         }  */
    }
}