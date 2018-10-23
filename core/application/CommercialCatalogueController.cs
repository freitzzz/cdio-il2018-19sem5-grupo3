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
        /// Adds or Removes the collection of a CommercialCatalogue
        /// </summary>
        /// <param name="customizedCatalogueDTO">UpdateCommercialCatalogueDTO with the data regarding the commercialCatalogue update</param>
        /// <returns>boolean true if the update was successful, false if not</returns>
        public bool updateCollection(UpdateCommercialCatalogueDTO customizedCatalogueDTO)
        {

            CommercialCatalogueRepository comCatalogueRepo = PersistenceContext.repositories().createCommercialCatalogueRepository();
            CommercialCatalogue newComCatalogue = comCatalogueRepo.find(customizedCatalogueDTO.id);

            bool updatedWithSuccess = true;
            bool perfomedAtLeastOneUpdate = false;

            if (customizedCatalogueDTO.catalogueCollectionDTOToAdd != null)
            {
                foreach (CatalogueCollectionDTO collection in customizedCatalogueDTO.catalogueCollectionDTOToAdd)
                {
                    updatedWithSuccess &= newComCatalogue.addCollection(collection.toEntity());
                    perfomedAtLeastOneUpdate = true;
                }
            }
            if (customizedCatalogueDTO.catalogueCollectionDTOToRemove != null)
            {
                foreach (CatalogueCollectionDTO collection in customizedCatalogueDTO.catalogueCollectionDTOToRemove)
                {
                    updatedWithSuccess &= newComCatalogue.removeCollection(collection.toEntity());
                    perfomedAtLeastOneUpdate = true;
                }
            }
            if (!perfomedAtLeastOneUpdate || !updatedWithSuccess) return false;

            updatedWithSuccess &= comCatalogueRepo.update(newComCatalogue) != null;
            return updatedWithSuccess;

        }
    }
}