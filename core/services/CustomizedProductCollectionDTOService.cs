using core.domain;
using core.dto;
using core.persistence;
using System.Collections.Generic;

namespace core.services{

    /// <summary>
    /// Service class that helps the transformation of CustomizedProductCollectionDTO into CustomizedProductCollection since some information needs to be accessed on the persistence context
    /// </summary>
    public sealed class CustomizedProductCollectionDTOService{
        /// <summary>
        /// Transforms a customized product collection dto into a collection of customized products via service
        /// </summary>
        /// <param name="customizedProductCollectionDTO">CustomizedProductCollectionDTO with the customized product collection dto being transformed</param>
        /// <returns>CustomizedProductCollection with the collection of customized products transformed from the dto</returns>
        public CustomizedProductCollection transform(CustomizedProductCollectionDTO customizedProductCollectionDTO){
            string name=customizedProductCollectionDTO.name;
            if(customizedProductCollectionDTO.customizedProducts==null)
                return new CustomizedProductCollection(name);
            List<CustomizedProduct> customizedProducts=new List<CustomizedProduct>(PersistenceContext.repositories().createCustomizedProductRepository().findCustomizedProductsByTheirPIDS(customizedProductCollectionDTO.customizedProducts));
            
            return new CustomizedProductCollection(name,customizedProducts);
        }
    }
}