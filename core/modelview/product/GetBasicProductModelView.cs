using System.Runtime.Serialization;

namespace core.modelview.product{
    /// <summary>
    /// Model View representation for the fetch product basic information context
    /// </summary>
    [DataContract]
    public sealed class GetBasicProductModelView{
        /// <summary>
        /// Long with the product ID
        /// </summary>
        [DataMember(Name="id")]
        public long id{get;set;}

        /// <summary>
        /// String with the product reference
        /// </summary>
        [DataMember(Name="reference")]
        public string reference{get;set;}

        /// <summary>
        /// String with the product designation
        /// </summary>
        [DataMember(Name="designation")]
        public string designation{get;set;}
    }   
}