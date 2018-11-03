namespace core.dto
{
    /// <summary>
    /// ModelView for deleting a child customized product
    /// </summary>
    public class DeleteChildCustomizedProductModelView
    {
        /// <summary>
        /// PID of the parent Customized Product
        /// </summary>
        /// <value>Gets/Sets the identifier</value>
        public long parentId { get; set; }

        /// <summary>
        /// PID of the slot where the child is inserted in
        /// </summary>
        /// <value>Gets/Sets the identifier</value>
        public long slotId { get; set; }

        /// <summary>
        /// PID of the child Customized Product
        /// </summary>
        /// <value>Gets/Sets the identifer</value>
        public long childId { get; set; }
    }
}