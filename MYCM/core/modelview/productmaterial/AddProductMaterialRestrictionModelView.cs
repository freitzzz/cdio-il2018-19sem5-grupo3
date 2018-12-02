using System.Runtime.Serialization;
using core.dto;

namespace core.modelview.productmaterial
{
    /// <summary>
    /// Class representing the ModelView used for adding a Restriction to a Product's Material.
    /// </summary>
    [DataContract]
    public class AddProductMaterialRestrictionModelView
    {
        /// <summary>
        /// Product's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the Product's persistence identifier.</value>
        //*This property should not be included in serialization, it's only used for data transportation */
        [IgnoreDataMember]
        public long productId { get; set; }

        /// <summary>
        /// Material's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the Material's persistence identifier.</value>
        //*This property should not be included in serialization, it's only used for data transportation */
        [IgnoreDataMember]
        public long materialId { get; set; }

        /// <summary>
        /// RestrictionDTO containing the Restriction's information.
        /// </summary>
        /// <value>Gets/sets the instance of RestrictionDTO.</value>
        [DataMember]
        public RestrictionDTO restriction { get; set; }
        //TODO: change this to ModelView ASAP
    }
}