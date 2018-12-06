using System.Runtime.Serialization;
using System;
using core.dto;

namespace core.modelview.customizedproduct
{
    /// <summary>
    /// ModelView representing the update information of a customized product
    /// </summary>
    [DataContract]
    public class UpdateCustomizedProductModelView
    {
        /// <summary>
        /// PID of the customized product
        /// </summary>
        /// <value>Gets/Sets the identifier</value>
        [IgnoreDataMember]
        public long Id {get; set;}

        /// <summary>
        /// Updated reference of the customized product
        /// </summary>
        /// <value>Gets/Sets the reference</value>
        [DataMember(Name = "reference")]
        public string reference {get; set;}

        /// <summary>
        /// Updated designation of the customized product
        /// </summary>
        /// <value>Gets/Sets the designation</value>
        [DataMember(Name = "designation")]
        public string designation {get; set;}

        /// <summary>
        /// Updated customized dimensions of the customized product
        /// </summary>
        /// <value>Gets/Sets the customized dimensions</value>
        [DataMember(Name = "customizedDimensions")]
        public CustomizedDimensionsDTO customizedDimensions {get;set;}

        /// <summary>
        /// Updated customized material of the customized product
        /// </summary>
        /// <value>Gets/Sets the customized material</value>
        [DataMember(Name = "customizedMaterial")]
        public CustomizedMaterialDTO customizedMaterial {get; set;}
    }
}