using core.domain;
using core.dto;
using core.persistence;
using core.services;
using support.dto;
using support.utils;
using System;
using System.Collections.Generic;

namespace core.application{
    /// <summary>
    /// Application controller for customized products
    /// </summary>
    public sealed class CustomizedProductController{

        /// Fetches all available customized products
        /// </summary>        
        /// <summary>
        /// <returns>List with all available customized products</returns>
        public List<CustomizedProductDTO> findAllCustomizedProducts(){
            return Collections.enumerableAsList(
                DTOUtils.parseToDTOS(
                    PersistenceContext.repositories().createCustomizedProductRepository().findAllCustomizedProducts()
                )
            );
        }

        /// <summary>
        /// Fetches a customized product by its id
        /// </summary>
        /// <param name="customizedProductDTO">CustomizedProductDTO with the customized product id information</param>
        /// <returns>CustomizedProductDTO with the fetched customized product information</returns>
        public CustomizedProductDTO findCustomizedProductByID(CustomizedProductDTO customizedProductDTO){
            return PersistenceContext.repositories().createCustomizedProductRepository().find(customizedProductDTO.id).toDTO();
        }

        /// <summary>
        /// Adds a new customized product
        /// </summary>
        /// <param name="customizedProductDTO">CustomizedProductDTO with the customized product information</param>
        /// <returns>CustomizedProductDTO with the created customized product information</returns>
        public CustomizedProductDTO addCustomizedProduct(CustomizedProductDTO customizedProductDTO){
            CustomizedProduct customizedProduct=new CustomizedProductDTOService().transform(customizedProductDTO);
            return PersistenceContext.repositories().createCustomizedProductRepository().save(customizedProduct).toDTO();
        }
    }
}