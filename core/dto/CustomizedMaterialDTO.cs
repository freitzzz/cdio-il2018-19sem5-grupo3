using System.Collections.Generic;
using System.Runtime.Serialization;
using core.domain;
using support.dto;
namespace core.dto
{
    /// <summary>
    /// Represents a CustomizedMaterial's Data Transfer Object. 
    /// </summary>
    [DataContract]
    public class CustomizedMaterialDTO : DTO, DTOParseable<CustomizedMaterial, CustomizedMaterialDTO>
    {
        /// <summary>
        /// CustomizedMaterial's database identifier.
        /// </summary>
        /// <value></value>
        [DataMember]
        public long id { get; set; }

        /// <summary>
        /// CustomizedMaterial's of material.
        /// </summary>
        /// <value>Gets/sets the value of the material field.</value>
        [DataMember]
        public MaterialDTO material { get; set; }
        /// <summary>
        /// CustomizedMaterial's of color.
        /// </summary>
        /// <value>Gets/sets the value of the color field.</value>
        [DataMember]
        public ColorDTO color { get; set; }

        /// <summary>
        /// Material's of available finish.
        /// </summary>
        /// <value>Gets/sets the value of the finish field.</value>
        [DataMember]
        public FinishDTO finish { get; set; }
        /// <summary>
        /// Returns this DTO's equivalent Entity
        /// </summary>
        /// <returns>DTO's equivalent Entity</returns>
        public CustomizedMaterial toEntity()
        {

            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material.toEntity(), color.toEntity(), finish.toEntity());
            customizedMaterial.Id = this.id;

            return customizedMaterial;
        }

    }
}