using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using core.domain;
using support.dto;
namespace core.dto
{
    public class CatalogueCollectionDTO : DTO, DTOParseable<CatalogueCollection, CatalogueCollectionDTO>
    {
        /// <summary>
        /// CatalogueCollection's database identifier.
        /// </summary>
        /// <value></value>
        [DataMember]
        public long Id { get; set; }
        /// <summary>
        /// CatalogueCollection's list of customized products.
        /// </summary>
        /// <value>Gets/sets the value of the customized products field.</value>
        [DataMember]

        public List<CustomizedProductDTO> customizedProductsDTO { get; set; }

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
        public CatalogueCollection toEntity()
        {
            List<CustomizedProduct> custProducts = new List<CustomizedProduct>();

            foreach (CustomizedProductDTO dto in this.customizedProductsDTO)
            {
                custProducts.Add(dto.toEntity());
            }

            CatalogueCollection catalogueCollection = new CatalogueCollection(custProducts, this.customizedProductCollectionDTO.toEntity());
            catalogueCollection.Id = this.Id;

            return catalogueCollection;
        }
    }
}