using System.Runtime.Serialization;

namespace core.modelview.component{
    /// <summary>
    /// Model View representation for the fetch component basic information context
    /// </summary>
    [DataContract]
    public sealed class GetBasicComponentModelView{
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
    }
}