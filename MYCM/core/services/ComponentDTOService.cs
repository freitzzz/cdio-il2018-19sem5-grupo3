using core.domain;
using core.dto;
using core.persistence;
using System.Collections.Generic;

namespace core.services{
    /// <summary>
    /// Service class for ComponentDTO
    /// </summary>
    public sealed class ComponentDTOService{

        /// <summary>
        /// Transforms a ComponentDTO into a component
        /// </summary>
        /// <param name="componentDTO">ComponentDTO with the component information</param>
        /// <returns>Component with the transformed component dto</returns>
        public Component transform(ComponentDTO componentDTO){
            ProductDTO productDTO=componentDTO.product;
            Product product=PersistenceContext.repositories().createProductRepository().find(productDTO.id);
            //TODO:RESTRICTIONS ARE STILL IN DEVELOPMENT
            // new Component(product);
            return null;
        }

        /// <summary>
        /// Transforms an enumerable of components dto into components
        /// </summary>
        /// <param name="componentsDTO">IEnumerable with the components dto</param>
        /// <returns>IEnumerable with the transformed components dto</returns>
        public IEnumerable<Product> transform(IEnumerable<ComponentDTO> componentsDTO){
            IEnumerable<ProductDTO> productsDTO=extractProductsDTOFromComponentsDTO(componentsDTO);
            List<Product> products=new List<Product>(PersistenceContext.repositories().createProductRepository().fetchProductsByID(productsDTO));
            return products;
        }

        /// <summary>
        /// Extracts an enumerable of products dto from an enumerable of components dto
        /// </summary>
        /// <param name="componentDTO">IEnumerable with the components dto</param>
        /// <returns>IEnumerable with the extracted products dto</returns>
        private IEnumerable<ProductDTO> extractProductsDTOFromComponentsDTO(IEnumerable<ComponentDTO> componentsDTO){
            List<ProductDTO> productsDTO=new List<ProductDTO>();
            foreach(ComponentDTO componentDTO in componentsDTO)productsDTO.Add(componentDTO.product);
            return productsDTO;
        }
    }
}