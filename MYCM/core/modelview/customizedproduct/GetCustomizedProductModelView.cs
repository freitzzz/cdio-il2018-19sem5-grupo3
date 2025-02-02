using System.Collections.Generic;
using System.Runtime.Serialization;
using core.dto;
using core.modelview.customizeddimensions;
using core.modelview.customizedmaterial;
using core.modelview.product;
using core.modelview.slot;
using static core.domain.CustomizedProduct;

namespace core.modelview.customizedproduct
{
    /// <summary>
    /// Model View representing the information to send when a GET By Id request is performed
    /// </summary>
    [DataContract]
    public class GetCustomizedProductModelView
    {
        /// <summary>
        /// CustomizedProducts Identifier
        /// </summary>
        /// <value>Gets/Sets the identifier</value>
        [DataMember(Name = "id")]
        public long customizedProductId { get; set; }

        /// <summary>
        /// GetBasicProductModelView with Product data.
        /// </summary>
        /// <value>Gets/Sets the instance of GetBasicProductModelView.</value>
        [DataMember]
        public GetBasicProductModelView product { get; set; }

        /// <summary>
        /// CustomizedProducts reference
        /// </summary>
        /// <value>Gets/Sets the reference</value>
        [DataMember]
        public string reference { get; set; }

        /// <summary>
        /// CustomizedProducts designation
        /// </summary>
        /// <value>Gets/Sets the designation</value>
        [DataMember(EmitDefaultValue = false)] //since this value is optional, do not display null values
        public string designation { get; set; }

        /// <summary>
        /// CustomizedProduct's current customization status.
        /// </summary>
        /// <value>Gets/Sets the status.</value>
        [DataMember]
        public CustomizationStatus status { get; set; }

        /// <summary>
        /// CustomizedProducts customized dimensions
        /// </summary>
        /// <value></value>
        [DataMember(Name = "customizedDimensions")]
        public GetCustomizedDimensionsModelView customizedDimensions { get; set; }

        /// <summary>
        /// CustomizedProducts customized material
        /// </summary>
        /// <value></value>
        [DataMember(Name = "customizedMaterial", EmitDefaultValue = false)]  //a customized product can be created without a material
        public GetCustomizedMaterialModelView customizedMaterial { get; set; }

        /// <summary>
        /// CustomizedProducts slot list
        /// </summary>
        /// <value></value>
        [DataMember(EmitDefaultValue = false)]
        public GetAllSlotsModelView slots { get; set; }
    }
}