using System;
using System.Runtime.Serialization;

namespace core.modelview.customizedproduct
{
    /// <summary>
    /// ModelView used for the POST Request to add a customized product to a Slot
    /// </summary>
    [DataContract]
    public class PostCustomizedProductToSlotModelView : PostCustomizedProductModelView
    {
        /// <summary>
        /// Slot Identifier
        /// </summary>
        /// <value>Gets/Sets the identifer</value>
        [DataMember(Name = "addToSlot", EmitDefaultValue = false)]
        public long slotId { get; set; }

        /// <summary>
        /// Identifier of the CustomizedProducts base
        /// </summary>
        /// <value>Gets/Sets the identifier</value>
        [DataMember(Name = "baseId", EmitDefaultValue = false)]
        public long baseId { get; set; }
    }
}