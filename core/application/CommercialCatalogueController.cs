using System;
using System.Collections.Generic;
using core.domain;
using core.persistence;
using core.dto;
using core.services;
using support.dto;
using support.utils;
namespace core.application
{
    public class CommercialCatalogueController
    {
        /// <summary>
        /// Adds a new CommercialCatalogue
        /// </summary>
        /// <param name="comCatalogueAsDTO">DTO with the commercialCatalogue information</param>
        /// <returns>DTO with the created commercialCatalogue DTO, null if the commercialCatalogue was not created</returns>
        public CommercialCatalogueDTO addCommercialCatalogue(CommercialCatalogueDTO comCatalogueAsDTO)
        {


            string reference = comCatalogueAsDTO.reference;
            string designation = comCatalogueAsDTO.designation;
            List<CatalogueCollection> collections = new List<CatalogueCollection>();

            if (comCatalogueAsDTO.collectionList != null)
            {
                foreach (CatalogueCollectionDTO collection in comCatalogueAsDTO.collectionList)
                {
                    collections.Add(collection.toEntity());
                }
            }
            CommercialCatalogue newComCatalogue;
            if (comCatalogueAsDTO.collectionList.Count == 0)
            {
                newComCatalogue = new CommercialCatalogue(reference, designation);
            }
            else
            {
                newComCatalogue = new CommercialCatalogue(reference, designation, collections);
            }
            CommercialCatalogue createdComCatalogue = PersistenceContext.repositories().createCommercialCatalogueRepository().save(newComCatalogue);
            if (createdComCatalogue == null) return null;
            return createdComCatalogue.toDTO();
        }
        /// <summary>
        /// Fetches a list of all commercialCatalogues present in the commercialCatalogue repository
        /// </summary>
        /// <returns>a list of all of the commercialCatalogues DTOs</returns>
        public List<CommercialCatalogueDTO> findAll()
        {
            List<CommercialCatalogueDTO> comCatalogueDTOList = new List<CommercialCatalogueDTO>();

            IEnumerable<CommercialCatalogue> comCatalogueList = PersistenceContext.repositories().createCommercialCatalogueRepository().findAll();

            if (comCatalogueList == null || !comCatalogueList.GetEnumerator().MoveNext())
            {
                return null;
            }

            foreach (CommercialCatalogue comCatalogue in comCatalogueList)
            {
                comCatalogueDTOList.Add(comCatalogue.toDTO());
            }

            return comCatalogueDTOList;
        }

        /// <summary>
        /// Returns a commercialCatalogue which has a certain persistence id
        /// </summary>
        /// <param name="comCatalogueDTO">CommercialCatalogueDTO with the commercialCatalogue information</param>
        /// <returns>CommercialCatalogueDTO with the commercialCatalogue which has a certain persistence id</returns>
        public CommercialCatalogueDTO findComCatalogueByID(CommercialCatalogueDTO comCatalogueDTO)
        {
            return PersistenceContext.repositories().createCommercialCatalogueRepository().find(comCatalogueDTO.id).toDTO();
        }


        /// <summary>
        /// Adds a new Collection to a CommercialCatalogue
        /// </summary>
        /// <param name="ID">id of the commercial catalogue</param>
        /// <param name="customizedCatalogueDTO">DTO with the customized product collection</param>
        /// <returns>DTO with the created product DTO, null if the product was not created</returns>
        public CommercialCatalogueDTO updateCollection( UpdateCommercialCatalogueDTO customizedCatalogueDTO)
        {

            //
            //Transform CustomizedProductCollection Dto to entity
            if(customizedCatalogueDTO.catalogueCollectionDTOToAdd != null){ //Necessity to add a catallogue collection
                CommercialCatalogue newComCatalogue = PersistenceContext.repositories().createCommercialCatalogueRepository().find(customizedCatalogueDTO.id);

                foreach(CatalogueCollectionDTO catalogueCollectionDTO in customizedCatalogueDTO.catalogueCollectionDTOToAdd){
                    
                }
            }

           /*  List<CustomizedProduct> list = new List<CustomizedProduct>();
            foreach (CustomizedProductDTO customizedProductDTO in customizedCatalogueDTO.customizedProductsDTO)
            {
                CustomizedProduct customizedProduct = PersistenceContext.repositories().createCustomizedProductRepository().find(customizedProductDTO.id);
                list.Add(customizedProduct);
            }

            CatalogueCollection customizedCatalogue = new CatalogueCollection(list, collection);
            bool test = newComCatalogue.addCollection(customizedCatalogue); */
           /*  if (!test)
            {
                return null;
            } */
            CommercialCatalogue createdComCatalogue = PersistenceContext.repositories().createCommercialCatalogueRepository().update(null);//newComCatalogue);
            if (createdComCatalogue == null) return null;
            return createdComCatalogue.toDTO();


        }


        /// <summary>
        /// Removes a new Collection to a CommercialCatalogue
        /// </summary>
        /// <param name="id">DTO with the product information</param>
        /// <param name="idC">DTO with the product information</param>
        /// <returns>DTO with the created product DTO, null if the product was not created</returns>
        public CommercialCatalogueDTO removeCollection(long id, long idC)
        {

            CommercialCatalogue newComCatalogue = PersistenceContext.repositories().createCommercialCatalogueRepository().find(id);
            //Transform CustomizedProductCollection Dto to entity
            bool flag = false;
            foreach (CatalogueCollection catalogueCollection in newComCatalogue.collectionList)
            {
                if (catalogueCollection.Id == idC)
                {
                    newComCatalogue.removeCollection(catalogueCollection);
                    flag = true;
                }
            }
            if (flag)
            { //if it was possible to remove the CatalogueCollection
                CommercialCatalogue createdComCatalogue = PersistenceContext.repositories().createCommercialCatalogueRepository().update(newComCatalogue);
                return createdComCatalogue.toDTO();

            }

            return null;
        }






    }
}