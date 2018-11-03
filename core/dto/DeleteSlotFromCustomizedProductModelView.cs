namespace core.dto
{
    /// <summary>
    /// ModelView representing the necessary information to delete a slot from a customized product
    /// </summary>
    public class DeleteSlotFromCustomizedProductModelView
    {
        /// <summary>
        /// Slot's identifier
        /// </summary>
        /// <value>Gets/Sets identifier</value>
        public long slotId {get; set;}

        /// <summary>
        /// Customized Product's identifier
        /// </summary>
        /// <value>Gets/Sets identifier</value>
        public long customizedProductId{get; set;}
    }
}