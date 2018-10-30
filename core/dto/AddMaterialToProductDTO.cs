using System.Runtime.Serialization;

namespace core.dto{
    [DataContract]
    /// <summary>
    /// A Model View DTO representation for the add material to a product context
    /// </summary>
    public sealed class AddMaterialToProductDTO{
        /// <summary>
        /// Long with the resource ID of the product which will be complemented
        /// </summary>
        public long productID{get;set;}

        /// <summary>
        /// Long with the resource ID of the material being added
        /// </summary>
        [DataMember(Name="id")]
        public long materialID{get;set;}
    }
}