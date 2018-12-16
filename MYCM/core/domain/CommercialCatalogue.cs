using support.domain.ddd;
using support.utils;
using support.dto;
using System;
using System.Collections.Generic;
using core.dto;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Linq;
using support.domain;

namespace core.domain
{

    ///<summary>
    ///Class that represents a CommercialCatalogue.
    ///<br>CommercialCatalogue is an entity;
    ///</summary>
    ///<typeparam name = "string">Generic-Type of the CommercialCatalogue entity identifier</typeparam>

    public class CommercialCatalogue : Activatable, AggregateRoot<string>, DTOAble<CommercialCatalogueDTO>
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
        ///Constant that represents the message that ocurrs if the CommercialCatalogue's Collection are not valid.
        ///</summary>
        private const string INVALID_COM_CATALOGUE_CUST_PRODUCTS = "The CommercialCatalogue's Collection are not valid!";

        /// <summary>
        /// Constant that represents the message presented when a null instance of CatalogueCollection is attempted to be added.
        /// </summary>
        private const string NULL_COLLECTION = "Unable to add invalid collection.";

        /// <summary>
        /// Constant that represents the message presented when a duplicate instance of CatalogueCollection is attempted to be added.
        /// </summary>
        private const string DUPLICATE_COLLECTION = "Unable to add duplicate collection.";

        /// <summary>
        /// Constant that represents the message presented when a given CatalogueCollection can't be removed.
        /// </summary>
        private const string UNABLE_TO_REMOVE_COLLECTION = "Unable to remove collection.";

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
        private List<CatalogueCollection> _catalogueCollectionList; //!private field used for lazy loading, do not use this for storing or fetching data
        public List<CatalogueCollection> catalogueCollectionList
        {
            get => LazyLoader.Load(this, ref _catalogueCollectionList); protected set => _catalogueCollectionList = value;
        }

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
        ///Creates a new instance of CommercialCatalogue, receiving its reference and designation.<Collection.
        ///</summary>
        ///<param name = "reference">string with the new CommercialCatalogue's reference</param>
        ///<param name = "designation">string with the new CommercialCatalogue's designation</param>
        public CommercialCatalogue(string reference, string designation)
        {
            checkCommercialCatalogueProperties(reference, designation);
            this.reference = reference;
            this.designation = designation;
            this.catalogueCollectionList = new List<CatalogueCollection>();
        }

        ///<summary>
        ///Creates a new instance of CommercialCatalogue, receiving its reference, designation and IEnumerable of CatalogueCollection.<Collection.
        ///</summary>
        ///<param name = "reference">string with the new CommercialCatalogue's reference</param>
        ///<param name = "designation">string with the new CommercialCatalogue's designation</param>
        ///<param name = "catalogueCollections">IEnumerable of CatalogueCollection.</param>
        public CommercialCatalogue(string reference, string designation,
            IEnumerable<CatalogueCollection> catalogueCollections) : this(reference, designation)
        {
            checkCatalogueCollections(catalogueCollections);
            foreach (CatalogueCollection catalogueCollection in catalogueCollections)
            {
                this.addCollection(catalogueCollection);
            }
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
        ///Checks if the CommercialCatalogue's IEnumerable 
        ///</summary>
        ///<param name = "catalogueCollections">List with the new CommercialCatalogue's custoProduct</param>
        private void checkCatalogueCollections(IEnumerable<CatalogueCollection> catalogueCollections)
        {
            if (Collections.isEnumerableNullOrEmpty(catalogueCollections)) throw new ArgumentException(INVALID_COM_CATALOGUE_CUST_PRODUCTS);
        }

        /// <summary>
        ///Changes the CommercialCatalogue's reference.
        /// </summary>
        /// <param name="reference">Catalogues new reference</param>
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

        /// <summary>
        /// Adds an instance of CatalogueCollection from the CommercialCatalogue.
        /// </summary>
        /// <param name="collection">Instance of CatalogueCollection being added.</param>
        /// <exception cref="System.ArgumentException">Thrown when the provided CatalogueCollection is null or has already been added.</exception>
        public void addCollection(CatalogueCollection collection)
        {
            if (collection == null)
            {
                throw new ArgumentException(NULL_COLLECTION);
            }

            if (hasCollection(collection))
            {
                throw new ArgumentException(DUPLICATE_COLLECTION);
            }

            catalogueCollectionList.Add(collection);
        }


        /// <summary>
        /// Removes an instance of CatalogueCollection from the CommercialCatalogue.
        /// </summary>
        /// <param name="collection">Instance of CatalogueCollection being removed.</param>
        /// <exception cref="System.ArgumentException">Thrown when the given instance of CatalogueCollection could not be removed.</exception>
        public void removeCollection(CatalogueCollection collection)
        {
            if (!catalogueCollectionList.Remove(collection))
            {
                throw new ArgumentException(UNABLE_TO_REMOVE_COLLECTION);
            }
        }


        /// <summary>
        /// Checks if the CommercialCatalogue has a given instance of CatalogueCollection.
        /// </summary>
        /// <param name="catalogueCollection">Instance of CatalogueCollection being checked.</param>
        /// <returns>true if the CommercialCatalogue has the CatalogueCollection; false, otherwise</returns>
        public bool hasCollection(CatalogueCollection catalogueCollection)
        {
            return this.catalogueCollectionList.Where(cc => cc.Equals(catalogueCollection)).Any();
        }


        ///<summary>
        ///Returns the CommercialCatalogue's identity.
        ///</summary>
        ///<returns>String with the CommercialCatalogue's identity</returns>
        public string id()
        {
            return this.reference;
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
        [Obsolete]
        public CommercialCatalogueDTO toDTO()
        {
            CommercialCatalogueDTO dto = new CommercialCatalogueDTO();

            dto.reference = this.reference;
            dto.designation = this.designation;
            dto.id = this.Id;
            List<CatalogueCollectionDTO> catalogueCollectionDTOs = new List<CatalogueCollectionDTO>();
            foreach (CatalogueCollection catalogueCollection in catalogueCollectionList)
            {
                catalogueCollectionDTOs.Add(catalogueCollection.toDTO());
            }
            dto.catalogueCollectionDTOs = catalogueCollectionDTOs;

            return dto;
        }

        ///<summary>
        ///Returns the generated hash code of the CommercialCatalogue.
        ///</summary>
        public override int GetHashCode()
        {
            int hash = 71;

            hash = hash * 83 + this.reference.GetHashCode();

            return hash;
        }

        ///<summary>
        ///Checks if a certain CommercialCatalogue is the same as a received object.
        ///</summary>
        ///<param name = "obj">object to compare to the current CommercialCatalogue</param>
        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }

            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }

            CommercialCatalogue other = (CommercialCatalogue)obj;

            return this.reference.Equals(other.reference);
        }

        ///<summary>
        ///Returns a textual description of the CommercialCatalogue.
        ///</summary>
        public override string ToString()
        {
            return string.Format("Reference {0}", reference);
        }

    }
}