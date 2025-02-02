using System.Runtime.Serialization;
using System;
using core.dto;
using core.modelview.customizeddimensions;
using static core.domain.CustomizedProduct;
using core.modelview.customizedmaterial;

namespace core.modelview.customizedproduct
{

    /// <summary>
    /// ModelView representing the update information of a customized product
    /// </summary>
    [DataContract]
    public class UpdateCustomizedProductModelView
    {
        /// <summary>
        /// CustomizedProduct's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the persistence identifier.</value>
        [IgnoreDataMember]
        public long customizedProductId { get; set; }

        /// <summary>
        /// User's authentication token.
        /// </summary>
        /// <value>Gets/Sets the authentication token.</value>
        [IgnoreDataMember]
        public string userAuthToken { get; set; }

        /// <summary>
        /// Updated reference of the customized product
        /// </summary>
        /// <value>Gets/Sets the reference</value>
        [DataMember]
        public string reference { get; set; }

        /// <summary>
        /// Updated designation of the customized product
        /// </summary>
        /// <value>Gets/Sets the designation</value>
        [DataMember]
        public string designation { get; set; }

        [DataMember(Name = "status")]
        public CustomizationStatus customizationStatus { get; set; }

        /// <summary>
        /// Updated customized dimensions of the customized product
        /// </summary>
        /// <value>Gets/Sets the customized dimensions</value>
        [DataMember(Name = "customizedDimensions")]
        public AddCustomizedDimensionsModelView customizedDimensions { get; set; }

        /// <summary>
        /// Updated customized material of the customized product
        /// </summary>
        /// <value>Gets/Sets the customized material</value>
        [DataMember(Name = "customizedMaterial")]
        public AddCustomizedMaterialModelView customizedMaterial { get; set; }
    }
}