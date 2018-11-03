using System.Runtime.Serialization;
using core.domain;

namespace core.modelview.productcategory
{
    /// <summary>
    /// Class representing a ModelView used for retriving all data from instances of ProductCategory.
    /// </summary>
    [DataContract]
    public class GetProductCategoryModelView
    {
        /// <summary>
        /// ProductCategory's database identifier.
        /// </summary>
        /// <value>Gets/sets the database identifier.</value>
        [DataMember]
        public long id { get; set; }

        /// <summary>
        /// ProductCategory's parent ProductCategory database identifier.
        /// </summary>
        /// <value>Gets/sets the parent's database identifier.</value>
        [DataMember(EmitDefaultValue = false)]
        public long? parentId { get; set; }

        /// <summary>
        /// ProductCategory's name.
        /// </summary>
        /// <value>Gets/sets the name.</value>
        [DataMember]
        public string name { get; set; }

    }
}