using core.domain;
using core.dto;
using support.domain.ddd;
using System.Collections.Generic;

namespace core.persistence{
    /// <summary>
    /// Interface that represents the repository functionalities for customized products
    /// </summary>
    /// <typeparam name="CustomizedProduct">Generic-Type of the repository aggregate root</typeparam>
    /// <typeparam name="long">Generic-Type of the aggregate entity persistence identifier</typeparam>
    /// <typeparam name="string">Generic-Type of the aggreagate entity identity</typeparam>
    public interface CustomizedProductRepository:Repository<CustomizedProduct,long,string>{
        /// <summary>
        /// Fetches all customized products by their persistence IDS
        /// </summary>
        /// <returns>IEnumerable with all customized products by their PIDS</returns>
        IEnumerable<CustomizedProduct> findCustomizedProductsByTheirPIDS(IEnumerable<CustomizedProductDTO> customizedProductDTOS);
        /// <summary>
        /// Fetches all available customized products
        /// </summary>
        /// <returns>IEnumerable with all available customized products</returns>
        IEnumerable<CustomizedProduct> findAllCustomizedProducts();
    }
}