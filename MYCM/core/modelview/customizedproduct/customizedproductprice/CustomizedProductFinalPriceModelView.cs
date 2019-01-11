using System.Collections.Generic;
using System.Runtime.Serialization;
using core.modelview.price;

namespace core.modelview.customizedproduct.customizedproductprice
{
    /// <summary>
    /// ModelView that represents the price of a customized product
    /// </summary>
    [DataContract]
    public class CustomizedProductFinalPriceModelView
    {
        /// <summary>
        /// List of all customized products within a customized product and their details
        /// </summary>
        /// <value>Gets/Sets the list</value>
        [DataMember(Name = "customizedProducts")]
        public List<CustomizedProductPriceModelView> customizedProducts {get; set;}
        
        /// <summary>
        /// Total price of the customized product
        /// </summary>
        /// <value>Gets/Sets the price</value>
        [DataMember(Name = "finalPrice")]
        public PriceModelView finalPrice {get; set;}
    }
}