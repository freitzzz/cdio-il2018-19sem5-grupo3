using System.Runtime.Serialization;
using core.dto;

namespace core.modelview.product
{
    /// <summary>
    /// Class representing the ModelView used for updating a Product's information.
    /// </summary>
    [DataContract]
    public class UpdateProductModelView
    {
        /// <summary>
        /// Product's reference.
        /// </summary>
        /// <value>Gets/sets the Product's reference.</value>
        [DataMember(Name = "reference")]
        public string reference { get; set; }

        /// <summary>
        /// Product's designation.
        /// </summary>
        /// <value>Gets/sets the Product's designation.</value>
        [DataMember(Name = "designation")]
        public string designation { get; set; }

        /// <summary>
        /// Database identifier of the ProductCategory to which the Product belongs. 
        /// </summary>
        /// <value>Gets/sets the ProductCategory's identifier.</value>
        [DataMember (Name = "productCategoryId")]
        public long productCategoryId { get; set; }


        //TODO: handle the update of Slot Dimensions

    }
}