namespace core.modelview.customizedproduct.customizedproductprice
{
    /// <summary>
    /// ModelView that represents the necessary information to fetch a customized product's price
    /// </summary>
    public class FetchCustomizedProductPriceModelView
    {

        /// <summary>
        /// CustomizedProduct's PID
        /// </summary>
        /// <value></value>
        public long id {get; set;}

        /// <summary>
        /// Requested currency to present the price in
        /// </summary>
        /// <value>Gets/Sets the currency</value>
        public string currency {get; set;}

        /// <summary>
        /// Requested area to present the price in
        /// </summary>
        /// <value>Gets/Sets the area</value>
        public string area {get; set;}
        
    }
}