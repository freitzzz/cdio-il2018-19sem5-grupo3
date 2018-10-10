using support.domain.ddd;
using core.domain;
using support.utils;
using support.dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using core.dto;

namespace core.domain
{
    /**
    <summary>
        Class that represents a CommercialCatalogue.
        <br>CommercialCatalogue is an entity;
    </summary>
    <typeparam name = "string">Generic-Type of the CommercialCatalogue entity identifier</typeparam>
    */
    public class CommercialCatalogue : DTOAble<CommercialCatalogueDTO>
    {
        /**
        <summary>
            Constant that represents the message that ocurrs if the CommercialCatalogue's reference is not valid.
        </summary>
        */
        private const string INVALID_COM_CATALOGUE_REFERENCE = "The CommercialCatalogue's reference is not valid!";

        /**
        <summary>
            Constant that represents the message that ocurrs if the CommercialCatalogue's designation is not valid.
        </summary>
        */
        private const string INVALID_COM_CATALOGUE_DESIGNATION = "The CommercialCatalogue's designation is not valid!";
        /**
        <summary>
            Constant that represents the message that ocurrs if the CommercialCatalogue's<Collection are not valid.
        </summary>
        */
        private const string INVALID_COM_CATALOGUE_CUST_PRODUCTS = "The CommercialCatalogue's<Collection are not valid!";
        public long Id { get; set; }

        /**
        <summary>
            String with the CommercialCatalogue's reference.
        </summary>
        */
        public string reference { get; protected set; }

        /** 
        <summary>
            String with the CommercialCatalogue's designation.
        </summary>
        */
        public string designation { get; set; }

        /**
        <summary>
            List with all the CommercialCatalogue's<Collection.
        </summary>
        **/
        public List<Collection> collectionList { get; set; } = new List<Collection>();
        /// <summary>
        /// Empty constructor used by ORM.
        /// </summary>
        protected CommercialCatalogue() { }

        /**
        <summary>
            Long with the CommercialCatalogue's database ID.
        </summary>
         */
        private long persistence_id { get; set; }
        /**
       <summary>
           Commercial Catalogue's valueOf
           <param name = "reference">string with the new CommercialCatalogue's reference</param>
           <param name = "designation">string with the new CommercialCatalogue's designation</param>
            <param name = "collectionList">List with the new CommercialCatalogue's custoProduct</param>
       </summary>
        */
        public static CommercialCatalogue valueOf(string reference, string designation, List<Collection> collectionList)
        {
            return new CommercialCatalogue(reference, designation, collectionList);
        }

        /**
        <summary>
            Builds a new instance of CommercialCatalogue, receiving its reference, designation and list of the<Collection.
        </summary>
        <param name = "reference">string with the new CommercialCatalogue's reference</param>
        <param name = "designation">string with the new CommercialCatalogue's designation</param>
        <param name = "custoProduct">List with the new CommercialCatalogue's custoProduct</param>
         */
        public CommercialCatalogue(string reference, string designation,
        List<Collection> custoProduct)
        {
            checkComCatalogueProperties(reference, designation, custoProduct);
            this.reference = reference;
            this.designation = designation;
            this.collectionList = custoProduct;
        }

        /**
        <summary>
            Checks if the CommercialCatalogue's properties are valid.
        </summary>
        <param name = "reference">String with the CommercialCatalogue's reference</param>
        <param name = "designation">String with the CommercialCatalogue's designation</param>
        <param name = "custoProduct">List with the new CommercialCatalogue's custoProduct</param>
        */
        private void checkComCatalogueProperties(string reference,
        string designation, List<Collection> custoProduct)
        {
            if (Strings.isNullOrEmpty(reference)) throw new ArgumentException(INVALID_COM_CATALOGUE_REFERENCE);
            if (Strings.isNullOrEmpty(designation)) throw new ArgumentException(INVALID_COM_CATALOGUE_DESIGNATION);
            if (Collections.isListNull(custoProduct) || Collections.isListEmpty(custoProduct)) throw new ArgumentException(INVALID_COM_CATALOGUE_CUST_PRODUCTS);
        }

        /**
            Changes the CommercialCatalogue's reference.
         */
        public void changeReference(string reference)
        {
            if (String.IsNullOrEmpty(reference)) throw new ArgumentException(INVALID_COM_CATALOGUE_REFERENCE);
            this.reference = reference;
        }

        /**
            Changes the CommercialCatalogue's designation.
         */
        public void changeDesignation(string designation)
        {
            if (String.IsNullOrEmpty(designation)) throw new ArgumentException(INVALID_COM_CATALOGUE_DESIGNATION);
            this.designation = designation;
        }

        /**
        <summary>
            Returns the CommercialCatalogue's identity.
        </summary>
        <returns>String with the CommercialCatalogue's identity</returns>
         */
        public string id()
        {
            return reference;
        }

        /**
        <summary>
            Adds a new<Collection to the CommercialCatalogue's list of<Collection.
        </summary>
        <param name = "Collection"<Collection to add</param>
        <returns>True if the<Collection is successfully added, false if not</returns>
        */
        public bool addCollection(Collection collection)
        {
            if (collection == null || collectionList.Contains(collection)) return false;
            collectionList.Add(collection);
            return true;
        }
        /**
        <summary>
            Adds a Customized Product to a list of Collections.
        </summary>
        <param name = "Collection" Collection to be added from</param>
        <param name = "CustomizedProduct" customizedProduct to add </param>
        <returns>True if the<Collection is successfully removed, false if not</returns>
        */
        public bool addCustomizedProductToCollection(Collection collection,CustomizedProduct customizedProduct)
        {
            if (collection == null || customizedProduct == null || collection.list.Contains(customizedProduct)) return false;

            return collection.addCustomizedProduct(customizedProduct);
        }

        /**
        <summary>
            Removes a Collection from the CommercialCatalogue's list of<Collection.
        </summary>
        <param name = <Collection"<Collection to remove</param>
        <returns>True if the<Collection is successfully removed, false if not</returns>
        */
        public bool removeCollection(Collection collection)
        {
            if (collection == null) return false;
            return collectionList.Remove(collection);
        }

         /**
        <summary>
            Removes a Customized Product from a list of Collections.
        </summary>
        <param name = <Collection"<Collection to remove</param>
        <returns>True if the<Collection is successfully removed, false if not</returns>
        */
        public bool removeCustomizedProductFromCollection(Collection collection,CustomizedProduct customizedProduct)
        {
             if (collection == null || customizedProduct == null || !collection.list.Contains(customizedProduct))return false;
            return collection.list.Remove(customizedProduct);
        }
        /**
        <summary>
            Checks if the CommercialCatalogue has a certain<Collection on its list of<Collection.
        </summary>
        <param name = <Collection"<Collection to check</param>
        <returns>True if the<Collection exists, false if not</returns>
        */
        public bool hasCollection(Collection collection)
        {
            if (collection == null) return false;
            return collectionList.Contains(collection);
        }
        /**
        <summary>
            Checks if a certain CommercialCatalogue's identity is the same as the current CommercialCatalogue.
        </summary>
        <param name = "comparingEntity">string with the CommercialCatalogue's identity to compare</param>
        <returns>True if both CommercialCatalogue' identities are the same, false if not</returns>
        */
        public bool sameAs(string comparingEntity)
        {
            return id().Equals(comparingEntity);
        }

        /** <summary>
            Returns the current CommercialCatalogue as a DTO.
        </summary>
        <returns>DTO with the current DTO representation of the CommercialCatalogue</returns>
        */
        public CommercialCatalogueDTO toDTO()
        {
            CommercialCatalogueDTO dto = new CommercialCatalogueDTO();

            dto.reference = this.reference;
            dto.designation = this.designation;

            List<CollectionDTO> dtoCustoProducts = new List<CollectionDTO>();
            foreach (Collection c in collectionList)
            {
                dtoCustoProducts.Add(c.toDTO());
            }
            dto.collectionList = dtoCustoProducts;

            return dto;
        }

        /**
        <summary>
            Returns a textual description of the CommercialCatalogue.
        </summary>
         */
        public override string ToString()
        {
            return string.Format("Designation: {0}, Reference {1}", designation, reference);
        }

        /**
        <summary>
            Returns the generated hash code of the CommercialCatalogue.
        </summary>
         */
        public override int GetHashCode()
        {
            return reference.GetHashCode();
        }

        /**
        <summary>
            Checks if a certain CommercialCatalogue is the same as a received object.
        </summary>
        <param name = "obj">object to compare to the current CommercialCatalogue</param>
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
                CommercialCatalogue commercialCatalogue = (CommercialCatalogue)obj;
                return reference.Equals(commercialCatalogue.reference);
            }
        }
        /**
        <summary>
            Inner static class which represents the CommercialCatalogue's properties used to map on data holders (e.g. DTO)
        </summary>
         */
        public static class Properties
        {
            /**
           <summary>
                Constant that represents the context of the Properties.
            </summary>
            */
            public const string CONTEXT = "CommercialCatalogue";

            /**
            <summary>
                Constant that represents the name of the Property which maps the CommercialCatalogue's database ID.
            </summary>
             */
            public const string DATABASE_ID_PROPERTY = "id";

            /**
            <summary>
                Constant that represents the name of the Property which maps the CommercialCatalogue's reference.
            </summary>
             */
            public const string REFERENCE_PROPERTY = "reference";

            /**
            <summary>
                Constant that represents the name of the Property which maps the CommercialCatalogue's designation.
            </summary>
             */
            public const string DESIGNATION_PROPERTY = "designation";

            /**
            <summary>
                Constant that represents the name of the Property which maps the CommercialCatalogue's<Collections.
            </summary>
             */
            public const string CUSTOMIZED_PRODUCTS_PROPERTY = "Collections";

        }

    }
}