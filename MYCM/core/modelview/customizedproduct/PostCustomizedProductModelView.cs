using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using core.dto;

namespace core.modelview.customizedproduct
{
    /// <summary>
    /// ModelView that represents a CustomizedProduct
    /// </summary>
    [DataContract]
    public class PostCustomizedProductModelView
    {
        // <summary>
        /// CustomizedProducts's database identifier
        /// </summary>
        /// <value>Gets/sets the value of the database identifier field.</value>
        [DataMember(Name = "id")]
        public long id { get; set; }

        /// <summary>
        /// Identifier of the Product that the Customized Product is built off of
        /// </summary>
        /// <value>Gets/Sets the identifier</value>
        [DataMember(Name = "productId")]
        public long productId { get; set; }

        /// <summary>
        /// String with the CustomizedProduct's reference
        /// </summary>
        [DataMember(Name = "reference")]
        public string reference { get; set; }

        /// <summary>
        /// String with the CustomizedProduct's designation
        /// </summary>
        [DataMember(Name = "designation")]
        public string designation { get; set; }

        /// <summary>
        /// CustomizedMaterialDTO with the CustomizedProduct's material
        /// </summary>
        [DataMember(Name = "customizedMaterial")]
        public CustomizedMaterialDTO customizedMaterialDTO { get; set; }

        /// <summary>
        /// CustomizedDimensionsDTO with the CustomizedProduct's dimensions
        /// </summary>
        [DataMember(Name = "customizedDimensions")]
        public CustomizedDimensionsDTO customizedDimensionsDTO { get; set; }

        /// <summary>
        /// List of the customized products slots dimensions
        /// </summary>
        [DataMember(Name = "slots")]
        public List<CustomizedDimensionsDTO> slots { get; set; }

    }
}