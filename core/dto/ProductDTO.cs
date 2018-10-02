using System.Collections.Generic;
using System.Runtime.Serialization;
using core.domain;
using support.dto;

namespace core.dto
{
    /// <summary>
    /// Represents a Product's Data Transfer Object.
    /// </summary>
    /// <typeparam name="Product">Type of entity</typeparam>
    /// <typeparam name="ProductDTO">Type of DTO</typeparam>
    [DataContract]
    public class ProductDTO : DTO, DTOParseable<Product, ProductDTO>
    {
        /// <summary>
        /// Product's database identifier.
        /// </summary>
        /// <value>Gets/sets the value of the database identifier field.</value>
        [DataMember]
        public long id {get; set;}

        /// <summary>
        /// Product's reference.
        /// </summary>
        /// <value>Gets/sets the value of the reference field.</value>
        [DataMember]
        public string reference {get; set;}

        /// <summary>
        /// Product's designation.
        /// </summary>
        /// <value>Gets/sets the value of the designation field.</value>
        [DataMember]
        public string designation {get; set;}

        /// <summary>
        /// Product's list of materials used in its production.
        /// </summary>
        /// <value>Gets/sets the value of the materials field.</value>
        [DataMember(EmitDefaultValue = false)]  //no data field data is presented if it's null
        public List<MaterialDTO> materials {get; set;}

        /// <summary>
        /// Product's list of complementary products.
        /// </summary>
        /// <value>Gets/sets the value of the complements field.</value>
        [DataMember(EmitDefaultValue = false)]  //no data field data is presented if it's null
        public List<ComponentDTO> complements {get; set;}

        /// <summary>
        /// Products list of height dimensions
        /// </summary>
        public List<DimensionDTO> heightDimensions{get;set;}

        /// <summary>
        /// Products list of depth dimensions
        /// </summary>
        public List<DimensionDTO> depthDimensions{get;set;}

        /// <summary>
        /// Products list of width dimensions
        /// </summary>
        public List<DimensionDTO> widthDimensions{get;set;}


        public Product toEntity()
        {
            throw new System.NotImplementedException();
        }


        //TODO: Add RestrictionDTO's
    }
}