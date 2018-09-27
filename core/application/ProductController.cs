using System;
using System.Collections.Generic;
using support.dto;
using core.domain;
using core.persistence;


namespace core.application
{
    /// <summary>
    /// Core ProductController class
    /// </summary>
    public class ProductController
    {
        /// <summary>
        /// Fetches a list of all products present in the product repository
        /// </summary>
        /// <returns>a list of all of the products DTOs</returns>
        public List<DTO> findAllProducts(){
            List<DTO> productDTOList = new List<DTO>();

            PersistenceContext.repositories().createProductRepository().findAll();

            return productDTOList;
        }

        /// <summary>
        /// Fetches a product from the product repository given an ID
        /// </summary>
        /// <param name="productID">the product's ID</param>
        /// <returns></returns>
        public DTO findProductByID(string productID){
           return PersistenceContext.repositories().createProductRepository().find(productID).toDTO();
        }
    }
}