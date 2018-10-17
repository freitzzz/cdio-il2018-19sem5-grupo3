using System.Collections.Generic;
using core.domain;
using support.dto;

namespace core.dto
{
    public class CatalogueCollectionDTO : DTO, DTOParseable<CatalogueCollection, CatalogueCollectionDTO>
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

        public CatalogueCollection toEntity()
        {
            throw new System.NotImplementedException();
        }
    }
}