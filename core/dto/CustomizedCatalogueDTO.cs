using System.Collections.Generic;
using core.domain;
using support.dto;

namespace core.dto
{
    public class CustomizedCatalogueDTO : DTO, DTOParseable<CustomizedCatalogue, CustomizedCatalogueDTO>
    {
         /** 
        <summary>
         Id of object.
        </summary>
        */
        public long Id { get; set; }
        /** 
       <summary>
        List of Customized Products DTO
       </summary>
       */
        public List<CustomizedProductDTO> customizedProductsDTO { get;  set; }

        /** 
       <summary>
        Customized Product Collection DTO
       </summary>
       */
        public CustomizedProductCollectionDTO customizedProductCollectionDTO { get;  set; }

        public CustomizedCatalogue toEntity()
        {
            throw new System.NotImplementedException();
        }
    }
}