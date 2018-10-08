using core.domain;
using core.dto;
using System.Collections.Generic;
using support.domain.ddd;
using support.persistence.repositories;

namespace core.persistence{
    /// <summary>
    /// Interface that represents the repository functionalities for Component entities
    /// </summary>
    /// <typeparam name="Component">Generic-Type of the repository entity</typeparam>
    /// <typeparam name="long">Generic-Type of the repository persistence ID</typeparam>
    /// <typeparam name="Product">Generic-Type of the repository entity identifier</typeparam>
    public interface ComponentRepository:Repository<Component,long,Product>{
        /// <summary>
        /// Fetches an enumerable of components by their ids
        /// </summary>
        /// <param name="componentsDTO">IEnumerable with the components information</param>
        /// <returns>IEnumerable with the fetched components</returns>
        IEnumerable<Component> fetchComponentsByIDS(IEnumerable<ComponentDTO> componentsDTO);
    }
}