using core.domain;
using core.dto;
using support.domain.ddd;
using System.Collections.Generic;

namespace core.persistence{
    /// <summary>
    /// Interface that represents the repository functionalities for collections of customized products
    /// </summary>
    /// <typeparam name="CustomizedProductCollection">Generic-Type of the repository aggregate root</typeparam>
    /// <typeparam name="long">Generic-Type of the aggregate entity persistence identifier</typeparam>
    /// <typeparam name="string">Generic-Type of the aggreagate entity identity</typeparam>
    public interface CustomizedProductCollectionRepository:Repository<CustomizedProductCollection,long,string>{

    }
}