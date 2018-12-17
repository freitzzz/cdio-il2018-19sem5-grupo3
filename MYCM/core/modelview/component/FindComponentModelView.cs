namespace core.modelview.component
{
    /// <summary>
    /// Class representing the ModelView used for finding a Component.
    /// </summary>
    //*This ModelView is only used for data transportation and so it should not be serialized */
    public class FindComponentModelView
    {
        /// <summary>
        /// Father Product's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the father's persistence identifier.</value>
        public long fatherProductId { get; set; }

        /// <summary>
        /// Child Product's persistence identifier.
        /// </summary>
        /// <value>Gets/sets the child's persistence identifier.</value>
        public long childProductId { get; set; }

        /// <summary>
        /// Unit to show measurements in
        /// </summary>
        /// <value>Gets/sets the measurement unit</value>
        public string unit {get; set;}
    }
}