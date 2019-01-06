namespace core.modelview.customizedproduct
{
    /// <summary>
    /// Class representing the ModelView used for retrieving an instance of CustomizedProduct.
    /// </summary>
    public class FindCustomizedProductModelView
    {
        /// <summary>
        /// Identifier of the CustomizedProduct being retrieved.
        /// </summary>
        /// <value>Gets/Sets the persistence identifier.</value>
        public long customizedProductId { get; set; }

        /// <summary>
        /// Additional options used for retrieving a CustomizedProduct.
        /// </summary>
        /// <returns>Gets/Sets the options.</returns>
        public FindCustomizedProductModelViewOptions options { get; set; } = new FindCustomizedProductModelViewOptions();
    }


    /// <summary>
    /// Class representing the options used for retrieving and instance of CustomizedProduct.
    /// </summary>
    public class FindCustomizedProductModelViewOptions
    {
        /// <summary>
        /// Unit to which the CustomizedProduct's dimensions will be converted.
        /// </summary>
        /// <value>Gets/Sets the unit.</value>
        public string unit { get; set; }
    }

}