using System.Collections.Generic;
using System.Runtime.Serialization;
using core.dto;

namespace core.modelview.productmaterial
{
    /// <summary>
    /// Class representing the ModelView used for adding a Material to a Product's collection of Material.
    /// </summary>
    [DataContract]
    public class AddProductMaterialModelView
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
        [DataMember(Name = "id")]
        public long materialId { get; set; }


        /// <summary>
        /// List containing all the Component's restrictions' information.
        /// </summary>
        /// <value>Get/sets the Component's restrictions' information list.</value>
        [DataMember]
        public List<RestrictionDTO> restrictions { get; set; }
        //TODO: change this to ModelView ASAP
    }
}