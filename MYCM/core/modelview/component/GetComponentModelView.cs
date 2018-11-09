using core.modelview.restriction;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.component{
    /// <summary>
    /// Model View representation for the fetch component information context
    /// </summary>
    [DataContract]
    public sealed class GetComponentModelView{
        /// <summary>
        /// Long with the component ID
        /// </summary>
        [DataMember(Name="id")]
        public long id{get;set;}

        /// <summary>
        /// Long with the product ID which was complemented by the component
        /// </summary>
        [DataMember(Name="productID")]
        public long fatherProductID{get;set;}

        /// <summary>
        /// Boolean with the component mandatory
        /// </summary>
        [DataMember(Name="mandatory")]
        public bool mandatory{get;set;}

        /// <summary>
        /// GetAllRestrictionsModelView with all component restrictions
        /// </summary>
        [DataMember(Name="restrictions")]
        public GetAllRestrictionsModelView restrictions;
    }
}