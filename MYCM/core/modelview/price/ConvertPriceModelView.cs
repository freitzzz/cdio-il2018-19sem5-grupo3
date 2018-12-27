namespace core.modelview.price
{
    /// <summary>
    /// ModelView to help when converting prices
    /// </summary>
    public class ConvertPriceModelView
    {
        /// <summary>
        /// Currency to convert from
        /// </summary>
        /// <value>Gets/Sets the currency</value>
        public string fromCurrency { get; set; }

        /// <summary>
        /// Currency to convert to
        /// </summary>
        /// <value>Gets/Sets the currency</value>
        public string toCurrency { get; set; }

        /// <summary>
        /// Area to convert from
        /// </summary>
        /// <value>Gets/Sets the area</value>
        public string fromArea { get; set; }

        /// <summary>
        /// Area to convert to
        /// </summary>
        /// <value>Gets/Sets the area</value>
        public string toArea { get; set; }

        /// <summary>
        /// Value to convert
        /// </summary>
        /// <value>Gets/Sets the value</value>
        public double value { get; set; }
    }
}