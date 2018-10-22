using support.domain.ddd;
using core.domain;
using support.utils;
using support.dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using core.dto;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace core.domain
{

    ///<summary>
    ///Class that represents a CommercialCatalogue.
    ///<br>CommercialCatalogue is an entity;
    ///</summary>
    ///<typeparam name = "string">Generic-Type of the CommercialCatalogue entity identifier</typeparam>

    public class CommercialCatalogue : AggregateRoot<string>, DTOAble<CommercialCatalogueDTO>
    {

        ///<summary>
        ///Constant that represents the message that ocurrs if the CommercialCatalogue's reference is not valid.
        ///</summary>
        private const string INVALID_COM_CATALOGUE_REFERENCE = "The CommercialCatalogue's reference is not valid!";

        ///<summary>
        ///Constant that represents the message that ocurrs if the CommercialCatalogue's designation is not valid.
        ///</summary>
        private const string INVALID_COM_CATALOGUE_DESIGNATION = "The CommercialCatalogue's designation is not valid!";

        ///<summary>
        ///Constant that represents the message that ocurrs if the CommercialCatalogue's<Collection are not valid.
        ///</summary>
        private const string INVALID_COM_CATALOGUE_CUST_PRODUCTS = "The CommercialCatalogue's<Collection are not valid!";

        /// <summary>
        /// Database identifier.
        /// </summary>
        /// <value></value>
        public long Id { get; internal set; }


        ///<summary>
        ///String with the CommercialCatalogue's reference.
        ///</summary>

        public string reference { get; protected set; }

        ///<summary>
        ///String with the CommercialCatalogue's designation.
        ///</summary>
        public string designation { get; protected set; }

        ///<summary>
        ///List with all the CommercialCatalogue's<Collection.
        ///</summary>
        private List<CatalogueCollection> _collectionList; //!private field used for lazy loading, do not use this for storing or fetching data
        public List<CatalogueCollection> collectionList { get => LazyLoader.Load(this, ref _collectionList); protected set => _collectionList = value; }

        /// <summary>
        /// LazyLoader injected by the framework
        /// </summary>
        /// <value>Private Gets/Sets the LazyLoader</value>
        private ILazyLoader LazyLoader { get; set; }

        /// <summary>
        /// Empty constructor used by ORM.
        /// </summary>
        protected CommercialCatalogue() { }

        /// <summary>
        /// Constructor used for injecting the LazyLoader
        /// </summary>
        /// <param name="lazyLoader">LazyLoader being injected</param>
        private CommercialCatalogue(ILazyLoader lazyLoader)
        {
            this.LazyLoader = lazyLoader;
        }

        ///<summary>
        ///Builds a new instance of CommercialCatalogue, receiving its reference, designation and list of the<Collection.
        ///</summary>
        ///<param name = "reference">string with the new CommercialCatalogue's reference</param>
        ///<param name = "designation">string with the new CommercialCatalogue's designation</param>
        ///<param name = "custoProduct">List with the new CommercialCatalogue's custoProduct</param>
        public CommercialCatalogue(string reference, string designation,
        List<CatalogueCollection> custoProduct)
        {
            checkCommercialCatalogueProperties(reference, designation, custoProduct);
            this.reference = reference;
            this.designation = designation;
            this.collectionList = custoProduct;
        }

        ///<summary>
        ///Builds a new instance of CommercialCatalogue, receiving its reference, designation and list of the<Collection.
        ///</summary>
        ///<param name = "reference">string with the new CommercialCatalogue's reference</param>
        ///<param name = "designation">string with the new CommercialCatalogue's designation</param>
        public CommercialCatalogue(string reference, string designation)
        {
            checkCommercialCatalogueProperties(reference, designation);
            this.reference = reference;
            this.designation = designation;
            this.collectionList = new List<CatalogueCollection>();
        }

        /// <summary>
        /// Checks if the CommercialCatalogue's properties are valid.
        /// </summary>
        /// <param name="reference">String with the CommercialCatalogue's reference</param>
        /// <param name="designation">String with the CommercialCatalogue's designation</param>
        private void checkCommercialCatalogueProperties(string reference, string designation)
        {
            if (Strings.isNullOrEmpty(reference)) throw new ArgumentException(INVALID_COM_CATALOGUE_REFERENCE);
            if (Strings.isNullOrEmpty(designation)) throw new ArgumentException(INVALID_COM_CATALOGUE_DESIGNATION);
        }

        ///<summary>
        ///Checks if the CommercialCatalogue's properties are valid.
        ///</summary>
        ///<param name = "reference">String with the CommercialCatalogue's reference</param>
        ///<param name = "designation">String with the CommercialCatalogue's designation</param>
        ///<param name = "custoProduct">List with the new CommercialCatalogue's custoProduct</param>
        private void checkCommercialCatalogueProperties(string reference,
        string designation, List<CatalogueCollection> custoProduct)
        {
            checkCommercialCatalogueProperties(reference, designation);
            if (Collections.isListNull(custoProduct) || Collections.isListEmpty(custoProduct)) throw new ArgumentException(INVALID_COM_CATALOGUE_CUST_PRODUCTS);
        }

        /// <summary>
        ///Changes the CommercialCatalogue's reference.
        /// </summary>
        /// <param name="reference">Catalogues new reference</param>//
        public void changeReference(string reference)
        {
            if (String.IsNullOrEmpty(reference)) throw new ArgumentException(INVALID_COM_CATALOGUE_REFERENCE);
            this.reference = reference;
        }

        /// <summary>
        /// Changes the CommercialCatalogue's designation.
        /// </summary>
        /// <param name="designation">Catalogues new designation</param>
        public void changeDesignation(string designation)
        {
            if (String.IsNullOrEmpty(designation)) throw new ArgumentException(INVALID_COM_CATALOGUE_DESIGNATION);
            this.designation = designation;
        }

        ///<summary>
        ///Returns the CommercialCatalogue's identity.
        ///</summary>
        ///<returns>String with the CommercialCatalogue's identity</returns>
        public string id()
        {
            return reference;
        }

        ///<summary>
        ///Adds a new<Collection> to the CommercialCatalogue's list of<Collection>.
        ///</summary>
        ///<param name = "Collection"<Collection to add</param>
        ///<returns>True if the<Collection is successfully added, false if not</returns>
        public bool addCollection(CatalogueCollection collection)
        {
            if (collection == null || collectionList.Contains(collection)) return false;
            collectionList.Add(collection);
            return true;
        }


        ///<summary>
        ///Removes a Collection from the CommercialCatalogue's list of<Collection>.
        ///</summary>
        ///<param name = <Collection>"<Collection> to remove</param>
        ///<returns>True if the<Collection is successfully removed, false if not</returns>
        public bool removeCollection(CatalogueCollection collection)
        {
            if (collection == null) return false;
            return collectionList.Remove(collection);
        }



        ///<summary>
        ///Checks if the CommercialCatalogue has a certain<Collection on its list of<Collection.
        ///</summary>
        ///<param name = <Collection>"<Collection> to check</param>
        ///<returns>True if the<Collection exists, false if not</returns>

        public bool hasCollection(CustomizedProductCollection collection)
        {
            if (collection == null) return false;
            foreach (CatalogueCollection customizedCatalogue in collectionList)
            {
                if (customizedCatalogue.customizedProductCollection.Equals(collection))
                {
                    return true;
                }
            }
            return false;
        }

        ///<summary>
        ///Checks if a certain CommercialCatalogue's identity is the same as the current CommercialCatalogue.
        ///</summary>
        ///<param name = "comparingEntity">string with the CommercialCatalogue's identity to compare</param>
        ///<returns>True if both CommercialCatalogue' identities are the same, false if not</returns>
        public bool sameAs(string comparingEntity)
        {
            return id().Equals(comparingEntity);
        }

        ///<summary>
        ///Returns the current CommercialCatalogue as a DTO.
        ///</summary>
        ///<returns>DTO with the current DTO representation of the CommercialCatalogue</returns>
        public CommercialCatalogueDTO toDTO()
        {
            CommercialCatalogueDTO dto = new CommercialCatalogueDTO();

            dto.reference = this.reference;
            dto.designation = this.designation;
            dto.id = this.Id;
            List<CatalogueCollectionDTO> dtoCustoProducts = new List<CatalogueCollectionDTO>();
            foreach (CatalogueCollection c in collectionList)
            {
                dtoCustoProducts.Add(c.toDTO());
            }
            dto.collectionList = dtoCustoProducts;

            return dto;
        }

        ///<summary>
        ///Returns a textual description of the CommercialCatalogue.
        ///</summary>
        public override string ToString()
        {
            return string.Format("Designation: {0}, Reference {1}", designation, reference);
        }

        ///<summary>
        ///Returns the generated hash code of the CommercialCatalogue.
        ///</summary>
        public override int GetHashCode()
        {
            return reference.GetHashCode();
        }

        ///<summary>
        ///Checks if a certain CommercialCatalogue is the same as a received object.
        ///</summary>
        ///<param name = "obj">object to compare to the current CommercialCatalogue</param>
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



    }
}