using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using core.domain;
using support.dto;

namespace core.dto
{
    /// <summary>
    /// Represents a CommercialCatalogue's Data Transfer Object. 
    /// </summary>
    [DataContract]
    public class CommercialCatalogueDTO : DTO, DTOParseable<CommercialCatalogue, CommercialCatalogueDTO>
    {
        /// <summary>
        /// CommercialCatalogue's database identifier.
        /// </summary>
        /// <value></value>
        [DataMember]
        public long id { get; set; }

        /// <summary>
        /// CommercialCatalogue's reference.
        /// </summary>
        /// <value>Gets/sets the value of the reference field.</value>
        [DataMember]
        public string reference { get; set; }

        /// <summary>
        /// CommercialCatalogue's designation.
        /// </summary>
        /// <value>Gets/sets the value of the designation field.</value>
        [DataMember]
        public string designation { get; set; }

        /// <summary>
        /// CommercialCatalogue's list of customized products.
        /// </summary>
        /// <value>Gets/sets the value of the customized products field.</value>
        [DataMember]
        public List<CustomizedProductDTO> custProducts { get; set; }
        /// <summary>
        /// Returns this DTO's equivalent Entity
        /// </summary>
        /// <returns>DTO's equivalent Entity</returns>
        public CommercialCatalogue toEntity()
        {
            List<CustomizedProduct> custProducts = new List<CustomizedProduct>();

            foreach (CustomizedProductDTO dto in this.custProducts)
            {
                custProducts.Add(dto.toEntity());
            }

            CommercialCatalogue commercialCatalogue = new CommercialCatalogue(this.reference, this.designation, custProducts);
            commercialCatalogue.Id = this.id;

            return commercialCatalogue;
        }
    }
}
