using System.Runtime.Serialization;
using core.modelview.finish;
using core.modelview.price;

namespace core.modelview.customizedproduct.customizedproductprice
{
    /// <summary>
    /// ModelView to represent a finish's price
    /// </summary>
    [DataContract]
    public class FinishPriceModelView : GetFinishModelView
    {
        /// <summary>
        /// Finish's price
        /// </summary>
        /// <value>Gets/Sets the price</value>
        [DataMember(Name = "price")]
        public PriceModelView price { get; set; }
    }
}