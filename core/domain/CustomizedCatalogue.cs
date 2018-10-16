using System.Collections.Generic;
using support.dto;

namespace core.domain
{
    public class CustomizedCatalogue: DTOAble<CustomizedCatalogueDTO>
    {
        public List<CustomizedProduct> customizedProduct;
        public CustomizedProductCollection customizedProductCollection;


        
    }
}