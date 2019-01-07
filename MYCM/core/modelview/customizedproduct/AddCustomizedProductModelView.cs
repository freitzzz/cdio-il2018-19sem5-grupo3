using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using core.dto;
using core.modelview.customizeddimensions;
using core.modelview.customizedmaterial;

namespace core.modelview.customizedproduct
{
    /// <summary>
    /// Class representing the ModelView used for adding an instance of CustomizedProduct.
    /// </summary>
    [DataContract]
    public class AddCustomizedProductModelView
    {
        /// <summary>
        /// CustomizedProduct's parent's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the persistence identifier.</value>
        [IgnoreDataMember]
        public long? parentCustomizedProductId { get; set; }

        /// <summary>
        /// Persistence identifier of the Slot to which the CustomizedProduct will be added.
        /// </summary>
        /// <value>Gets/Sets the persistence identifier.</value>
        [IgnoreDataMember]
        public long? insertedInSlotId { get; set; }

        /// <summary>
        /// User's authentication token.
        /// </summary>
        /// <value>Gets/Sets the user's authentication token.</value>
        [IgnoreDataMember]
        public string userAuthToken { get; set; }

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
        /// AddCustomizedDimensionsModelView with the CustomizedProduct's dimensions
        /// </summary>
        [DataMember(Name = "customizedDimensions")]
        public AddCustomizedDimensionsModelView customizedDimensions { get; set; }

        /// <summary>
        /// AddCustomizedMaterialModelView with the CustomizedProduct's material
        /// </summary>
        [DataMember(Name = "customizedMaterial")]
        public AddCustomizedMaterialModelView customizedMaterial { get; set; }
    }
}