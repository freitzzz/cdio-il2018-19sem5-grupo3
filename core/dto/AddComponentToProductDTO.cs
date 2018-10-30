using System.Runtime.Serialization;

namespace core.dto{
    [DataContract]
    /// <summary>
    /// A Model View DTO representation for the add component to a product context
    /// </summary>
    public sealed class AddComponentToProductDTO{
        /// <summary>
        /// Long with the resource ID of the product which will be complemented
        /// </summary>
        public long productID{get;set;}
        /// <summary>
        /// Long with the resource ID of the product which is going to act as the complemented product
        /// </summary>
        [DataMember(Name="id")]
        public long complementedProductID{get;set;}
        /// <summary>
        /// Boolean with the decision regarding the component mandatory
        /// </summary>
        [DataMember(Name="mandatory")]
        public bool mandatory{get;set;}
    }
}