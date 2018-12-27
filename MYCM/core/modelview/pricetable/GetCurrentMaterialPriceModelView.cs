using System.Runtime.Serialization;
using core.modelview.material;
using core.modelview.price;

namespace core.modelview.pricetable
{
    /// <summary>
    /// ModelView to represent the fetch of a material's current price
    /// </summary>
    [DataContract]
    public class GetCurrentMaterialPriceModelView
    {
        /// <summary>
        /// Requested material
        /// </summary>
        /// <value>Gets/Sets the model view</value>
        [DataMember(Name = "material")]
        public GetBasicMaterialModelView material { get; set; }

        /// <summary>
        /// Material's current price
        /// </summary>
        /// <value>Gets/Sets the current price</value>
        [DataMember(Name = "currentPrice")]
        public PriceModelView currentPrice { get; set; }
    }
}