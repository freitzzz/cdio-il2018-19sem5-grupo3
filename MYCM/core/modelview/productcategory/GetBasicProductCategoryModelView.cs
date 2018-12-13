using System.Runtime.Serialization;

namespace core.modelview.productcategory
{
    /// <summary>
    /// Class representing a ModelView used for retrieving basic data from instances of ProductCategory.
    /// </summary>
    [DataContract]
    public class GetBasicProductCategoryModelView
    {
        /// <summary>
        /// ProductCategory's database identifier.
        /// </summary>
        /// <value>Gets/sets the database identifier.</value>
        [DataMember]
        public long id { get; set; }

        /// <summary>
        /// ProductCategory's name.
        /// </summary>
        /// <value>Gets/sets the name.</value>
        [DataMember]
        public string name { get; set; }

        /// <summary>
        /// ProductCategory's parent id.
        /// </summary>
        /// <value>Gets/sets the parent id.</value>
        [DataMember(EmitDefaultValue = false)]
        public long? parentId{get;set;}
    }
}