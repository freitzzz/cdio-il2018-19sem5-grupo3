namespace core.modelview.component
{
    /// <summary>
    /// Class representing the ModelView used for retrieving a Product's collection of Component.
    /// </summary>
    public class FindComponentsModelView
    {
        /// <summary>
        /// Parent Product's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the persistence identifier.</value>
        public long fatherProductId { get; set; }

        /// <summary>
        /// Currently selected option.
        /// </summary>
        /// <value>Gets/Sets the selected option.</value>
        public FindComponentsOptions option { get; set; }
    }

    /// <summary>
    /// Enumerate representing the options available when retrieving Components.
    /// </summary>
    public enum FindComponentsOptions
    {
        DEFAULT = 0, CATEGORY = 1
    }
}