using System;
using System.Collections.Generic;
using core.domain;
using core.persistence;
using core.dto;
using core.services;
using support.dto;
using support.utils;
namespace core.application {
    public class CommercialCatalogueController
    {
        /// <summary>
        /// Adds a new CommercialCatalogue
        /// </summary>
        /// <param name="comCatalogueAsDTO">DTO with the product information</param>
        /// <returns>DTO with the created product DTO, null if the product was not created</returns>
        public CommercialCatalogueDTO addCommercialCatalogue(CommercialCatalogueDTO comCatalogueAsDTO)
        {

            string reference = comCatalogueAsDTO.reference;
            string designation = comCatalogueAsDTO.designation;
            List<CustomizedProductCollection> collections = new List<CustomizedProductCollection>();

            if (comCatalogueAsDTO.collectionList != null)
            {
                foreach (CustomizedProductCollectionDTO collection in comCatalogueAsDTO.collectionList)
                {
                    collections.Add(collection.toEntity());
                }
            }
            CommercialCatalogue newComCatalogue = CommercialCatalogue.valueOf(reference, designation, collections);
            CommercialCatalogue createdComCatalogue = PersistenceContext.repositories().createCommercialCatalogueRepository().save(newComCatalogue);
            if (createdComCatalogue == null) return null;
            return createdComCatalogue.toDTO();
        }
        /// <summary>
        /// Fetches a list of all commercialCatalogues present in the commercialCatalogue repository
        /// </summary>
        /// <returns>a list of all of the commercialCatalogues DTOs</returns>
        public List<CommercialCatalogueDTO> findAll() {
            List<CommercialCatalogueDTO> comCatalogueDTOList = new List<CommercialCatalogueDTO>();

            IEnumerable<CommercialCatalogue> comCatalogueList = PersistenceContext.repositories().createCommercialCatalogueRepository().findAll();

            if (comCatalogueList == null||!comCatalogueList.GetEnumerator().MoveNext()) {
                return null;
            }

            foreach (CommercialCatalogue comCatalogue in comCatalogueList) {
                comCatalogueDTOList.Add(comCatalogue.toDTO());
            }

            return comCatalogueDTOList;
        }

        /// <summary>
        /// Returns a commercialCatalogue which has a certain persistence id
        /// </summary>
        /// <param name="comCatalogueDTO">CommercialCatalogueDTO with the commercialCatalogue information</param>
        /// <returns>CommercialCatalogueDTO with the commercialCatalogue which has a certain persistence id</returns>
        public CommercialCatalogueDTO findComCatalogueByID(CommercialCatalogueDTO comCatalogueDTO) {
            return PersistenceContext.repositories().createCommercialCatalogueRepository().find(comCatalogueDTO.id).toDTO();
        }

    }
}