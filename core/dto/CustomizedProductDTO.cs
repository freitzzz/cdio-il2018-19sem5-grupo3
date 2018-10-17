using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using core.domain;
using support.dto;
using System.Linq;

namespace core.dto
{
    /// <summary>
    /// DTO that represents a ConfiguredProduct instance
    /// </summary>
    [DataContract]
    public class CustomizedProductDTO : DTO, DTOParseable<CustomizedProduct, CustomizedProductDTO>
    {
        // <summary>
        /// CustomizedProducts's database identifier
        /// </summary>
        /// <value>Gets/sets the value of the database identifier field.</value>
        [DataMember]
        public long id { get; set; }

        /// <summary>
        /// String with the CustomizedProduct's reference
        /// </summary>
        public string reference { get; set; }

        /// <summary>
        /// String with the CustomizedProduct's designation
        /// </summary>
        public string designation { get; set; }

        /// <summary>
        /// CustomizedMaterialDTO with the CustomizedProduct's material
        /// </summary>
        public CustomizedMaterial customizedMaterialDTO;

        /// <summary>
        /// CustomizedDimensionsDTO with the CustomizedProduct's dimensions
        /// </summary>
        public CustomizedDimensionsDTO customizedDimensionsDTO;

        /// <summary>
        /// ProductDTO with the CustomizedProduct's product
        /// </summary>
        public ProductDTO productDTO;

        /// <summary>
        /// SlotListDTO with the CustomizedProduct's list of SlotDTOs with the Slots
        /// </summary>
        public SlotListDTO slotsDTO = new SlotListDTO();

        /// <summary>
        /// Returns CustomizedProductDTO's equivalent CustomizedProduct
        /// </summary>
        /// <returns>CustomizedProductDTO's equivalent CustomizedProduct</returns>
        public CustomizedProduct toEntity()
        {
            List<Slot> slots = DTOUtils.reverseDTOS(slotsDTO.slotDTOs).ToList();

            CustomizedProduct instanceFromDTO = new CustomizedProduct(reference, designation,
            customizedMaterialDTO, customizedDimensionsDTO.toEntity(), productDTO.toEntity(), slots);
            instanceFromDTO.Id = this.id;
            return instanceFromDTO;
        }
    }
}