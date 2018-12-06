using System.Collections.Generic;
using System.Runtime.Serialization;
using core.dto;
using core.modelview.customizeddimensions;
using core.modelview.customizedmaterial;

namespace core.modelview.customizedproduct
{
    /// <summary>
    /// Model View representing the information to send when a GET By Id request is performed
    /// </summary>
    [DataContract]
    public class GetCustomizedProductModelView : BasicCustomizedProductModelView
    {
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
        [DataMember(Name = "customizedMaterial")]
        public GetCustomizedMaterialModelView customizedMaterial { get; set; }
    
        /// <summary>
        /// CustomizedProducts slot list
        /// </summary>
        /// <value></value>
        [DataMember(Name = "slots", EmitDefaultValue = false)]
        public List<GetCustomizedProductSlotModelView> slots {get; set;}
    }
}