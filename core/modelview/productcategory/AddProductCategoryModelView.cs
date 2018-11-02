using System.Runtime.Serialization;

namespace core.modelview.productcategory
{
    /// <summary>
    /// Class representing the ModelView used for adding instances of ProductCategory. 
    /// </summary>
    [DataContract]
    public class AddProductCategoryModelView
    {
        /// <summary>
        /// ProductCategory's name.
        /// </summary>
        /// <value>Gets/sets the name.</value>
        [DataMember]
        public string name {get; set;}
    }
}