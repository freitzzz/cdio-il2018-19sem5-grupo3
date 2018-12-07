using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using core.domain;
using support.dto;
using System.Linq;

namespace core.dto
{
    public class CatalogueCollectionDTO : DTO{
        /// <summary>
        /// CatalogueCollection's list of customized products.
        /// </summary>
        /// <value>Gets/sets the value of the customized products field.</value>
        [DataMember]

        public List<CustomizedProductDTO> customizedProductDTOs { get; set; }

        /// <summary>
        /// CatalogueCollection's customizedProductCollectionDTO.
        /// </summary>
        /// <value></value>
        [DataMember]
        public CustomizedProductCollectionDTO customizedProductCollectionDTO { get; set; }

        /// <summary>
        /// Returns this DTO's equivalent Entity
        /// </summary>
        /// <returns>DTO's equivalent Entity</returns>
        /* public CatalogueCollection toEntity()
        {
            List<CustomizedProduct> custProducts = DTOUtils.reverseDTOS(customizedProductDTOs).ToList();

            CatalogueCollection catalogueCollection = new CatalogueCollection(this.customizedProductCollectionDTO.toEntity(), custProducts);

            return catalogueCollection;
        } */
    }
}