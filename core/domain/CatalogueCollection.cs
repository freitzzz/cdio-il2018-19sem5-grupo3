using System;
using System.Collections.Generic;
using core.dto;
using support.dto;

namespace core.domain
{
    public class CatalogueCollection : DTOAble<CatalogueCollectionDTO>
    {
        /**
        <summary>
            Constant that represents the message that ocurrs if the Customized Products are not valid
        </summary>
        */
        private const string INVALID_LIST = "The Customized Product List is not valid!";

        /**
        <summary>
            Constant that represents the message that ocurrs if the Customized Product Collection is not valid
        </summary>
        */
        private const string INVALID_COLLECTION = "The Customized Product Collection is not valid!";
        /** 
        <summary>
         Id of object.
        </summary>
        */
        public long Id { get; internal set; }
        /** 
       <summary>
        List of Customized Products
       </summary>
       */
        public virtual List<CustomizedProduct> customizedProduct { get; protected set; }

        /** 
       <summary>
        Customized Product Collection
       </summary>
       */
        public virtual CustomizedProductCollection customizedProductCollection { get; protected set; }

        /// <summary>
        /// Builds a new CustomizedProductCollection with the collection name
        /// </summary>
        /// <param name="customizedProduct">list with the customized products collection name</param>
        /// <param name="customizedProductCollection">collection</param> 
        /// 
        public CatalogueCollection(List<CustomizedProduct> customizedProduct, CustomizedProductCollection customizedProductCollection)
        {
            checkAttributes(customizedProduct, customizedProductCollection);
            this.customizedProduct = customizedProduct;
            this.customizedProductCollection = customizedProductCollection;
        }

        public CatalogueCollection(CustomizedProductCollection customizedProductCollection)
        {
            if (customizedProductCollection == null) throw new ArgumentException(INVALID_COLLECTION);
            this.customizedProduct = new List<CustomizedProduct>();
            this.customizedProductCollection = customizedProductCollection;
        }

        /// <summary>
        /// Empty constructor
        /// 
        protected CatalogueCollection()
        {
        }

        /// <summary>
        /// Checks if parameters are valid
        /// </summary>
        /// <param name="customizedProduct">list with the customized products collection name</param>
        /// <param name="customizedProductCollection">collection</param> 
        /// 
        private void checkAttributes(List<CustomizedProduct> customizedProduct, CustomizedProductCollection customizedProductCollection)
        {
            if (customizedProduct == null || customizedProduct.Capacity == 0) throw new ArgumentException(INVALID_LIST);
            if (customizedProductCollection == null) throw new ArgumentException(INVALID_COLLECTION);
        }

        /// <summary>
        /// toDTO method
        ///</summary>
        /// 
        public CatalogueCollectionDTO toDTO()
        {
            CatalogueCollectionDTO CatalogueCollectionDTO = new CatalogueCollectionDTO();
            List<CustomizedProductDTO> list = new List<CustomizedProductDTO>();
            foreach (CustomizedProduct customizedProduct in this.customizedProduct)
            {
                list.Add(customizedProduct.toDTO());
            }
            CatalogueCollectionDTO.customizedProductsDTO = list;
            CatalogueCollectionDTO.Id = this.Id;
            CatalogueCollectionDTO.customizedProductCollectionDTO = this.customizedProductCollection.toDTO();
            return CatalogueCollectionDTO;
        }

        /**
        <summary>
            Returns the generated hash code of the CommercialCatalogue.
        </summary>
         */
        public override int GetHashCode()
        {
            return customizedProductCollection.GetHashCode() + customizedProduct.GetHashCode();
        }

        /**
        <summary>
            Checks if a certain Customized is the same as a received object.
        </summary>
        <param name = "obj">object to compare to the current Customized Catalogue</param>
         */
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
                return customizedProductCollection.Equals(CatalogueCollection.customizedProductCollection) && customizedProduct.Equals(CatalogueCollection.customizedProduct);
            }
        }

        /**
        <summary>
            Returns a textual description of the CommercialCatalogue.
        </summary>
         */
        public override string ToString()
        {
            return string.Format("List of Customized Products: {0}, Customized Product Collection {1}", customizedProduct.ToString(), customizedProductCollection.ToString());
        }
/* 
        public void addCustomizedProductToCollection(CustomizedProduct customizedProduct,){

        } */
    }
}