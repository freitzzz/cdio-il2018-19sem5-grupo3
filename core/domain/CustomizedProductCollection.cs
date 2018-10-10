using System;
using System.Collections.Generic;
using core.dto;
using support.domain.ddd;
using support.dto;
using support.utils;

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
        private const string INVALID_CUSTOMIZED_PRODUCT_COLLECTION_NAME="The collection name is invalid!";

        /// <summary>
        /// Constant that represents the message that occures if the customized products of the collection are invalid
        /// </summary>
        private const string INVALID_COLLECTION_CUSTOMIZED_PRODUCTS="The collection customized products are invalid!";

         /**
        <summary>
            Constant that represents the message that ocurrs if the Customized Product
        </summary>
         */
        private const string INVALID_CUSTOMIZED_PRODUCT = "The Customized Product is invalid!";


        /**
        <summary>
            Constant that represents the message that ocurrs if a string is invalid.
        </summary>
         */
        private const string INVALID_STRING = "The String is invalid!";

        /// <summary>
        /// Persistence identifier of the current CustomizedProductCollection
        /// </summary>
        public long Id { get; set; }


        /// <summary>
        /// String with the collection name
        /// </summary>
        public string name { get; protected set; }

        /// <summary>
        /// List with the collection customized products
        /// </summary>
        public List<CustomizedProduct> customizedProducts{get;protected set;}

        /// <summary>
        /// Builds a new CustomizedProductCollection with the collection name
        /// </summary>
        /// <param name="name">string with the customized products collection name</param>
        public CustomizedProductCollection(string name){
            checkCustomizedProductCollectionProperties(name);
            this.name=name;
            this.customizedProducts=new List<CustomizedProduct>();
        }

        /// <summary>
        /// Builds a new CustomizedProductCollection with the collection name and customized products
        /// </summary>
        /// <param name="name">string with the customized products collection name</param>
        /// <param name="customizedProducts">IEnumerable with the collection customized products</param>
        public CustomizedProductCollection(string name,IEnumerable<CustomizedProduct> customizedProducts){
            checkCustomizedProductCollectionProperties(name);
            checkCollectionCustomizedProducts(customizedProducts);
            this.name=name;
            this.customizedProducts=new List<CustomizedProduct>(customizedProducts);
        }

        /// <summary>
        /// Returns the current collection identity
        /// </summary>
        /// <returns>String with the collection identity</returns>
        public string id(){return name;}


        /// <summary>
        /// Checks if a certain customized product collection identity is the same as the current one
        /// </summary>
        /// <param name="comparingEntity">string with the comparing customized product collection identity</param>
        /// <returns>boolean true if both identities are the same, false if not</returns>
        public bool sameAs(string comparingEntity){return id().Equals(comparingEntity);}

        /// <summary>
        /// Returns the current collection of customized products as a DTO
        /// </summary>
        /// <returns>CustomizedProductCollectionDTO with the current DTO representation of the customized products collection</returns>
        public CustomizedProductCollectionDTO toDTO(){
            CustomizedProductCollectionDTO dto = new CustomizedProductCollectionDTO();
            dto.name=this.name;
            dto.id=this.Id;
            dto.customizedProducts=new List<CustomizedProductDTO>(DTOUtils.parseToDTOS(customizedProducts));
            return dto;
        }

        /**
        <summary>
            Checks if a certain Collection is the same as a received object.
        </summary>
        <param name = "comparingCustomizedProductCollection">object to compare to the current Collection</param>
         */
        public override bool Equals(object comparingCustomizedProductCollection){
            if(this==comparingCustomizedProductCollection)return true;
            return comparingCustomizedProductCollection is CustomizedProductCollection 
                && ((CustomizedProductCollection)comparingCustomizedProductCollection).id().Equals(this.id());
        }

        /**
        <summary>
            Returns the generated hash code of the Customized Material.
        </summary>
         */
        public override int GetHashCode()
        {
            return id().GetHashCode();
        }

        /**
       <summary>
           Returns a textual description of the Collection.
       </summary>
        */
        public override string ToString()
        {
            return string.Format("Name {0}",name);
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
        private void checkCollectionCustomizedProducts(IEnumerable<CustomizedProduct> enumerableCustomizedProducts){
            if(Collections.isEnumerableNullOrEmpty(enumerableCustomizedProducts))
                throw new ArgumentException(INVALID_COLLECTION_CUSTOMIZED_PRODUCTS);
            checkCustomizedProductsDuplicates(enumerableCustomizedProducts);
        }

        /// <summary>
        /// Checks if an enumerable of customized products have duplicates
        /// </summary>
        /// <param name="customizedProducts">IEnumerable with the customized products</param>
        private void checkCustomizedProductsDuplicates(IEnumerable<CustomizedProduct> customizedProducts){
            HashSet<int> customizedProductsHashes=new HashSet<int>();
            foreach(CustomizedProduct customizedProduct in customizedProducts)
                if(!customizedProductsHashes.Add(customizedProduct.GetHashCode()))
                    throw new ArgumentException(INVALID_COLLECTION_CUSTOMIZED_PRODUCTS);
        }
    }
}