using System.Runtime.Serialization;

namespace core.modelview.product{
    /// <summary>
    /// Model View representation for the update product properties context
    /// </summary>
    [DataContract]
    public sealed class UpdateProductPropertiesModelView{
        /// <summary>
        /// Long with the ID of the product which properties will be updated
        /// </summary>
        [IgnoreDataMember]
        public long id{get;set;}

        /// <summary>
        /// String with the reference to be updated on the product
        /// </summary>
        [DataMember(Name="reference")]
        public string reference{get;set;}

        /// <summary>
        /// String with the designation to be updated on the product
        /// </summary>
        [DataMember(Name="designation")]
        public string designation{get;set;}

        /// <summary>
        /// Product's model's filename.
        /// </summary>
        /// <value>Gets/sets the product's model's filename.</value>
        [DataMember(Name="model")]
        public string modelFilename {get; set;}

        /// <summary>
        /// Long with the ID of the category which will be updated on the product
        /// </summary>
        [DataMember(Name="categoryId")]
        public long categoryId{get;set;}
    }
}