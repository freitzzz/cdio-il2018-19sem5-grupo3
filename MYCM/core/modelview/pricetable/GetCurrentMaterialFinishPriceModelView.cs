using System.Runtime.Serialization;
using core.modelview.finish;
using core.modelview.material;
using core.modelview.price;

namespace core.modelview.pricetable
{
    /// <summary>
    /// ModelView to represent the fetch of a material finish's current price
    /// </summary>
    [DataContract]
    public class GetCurrentMaterialFinishPriceModelView
    {
        /// <summary>
        /// Requested Material Finish
        /// </summary>
        /// <value></value>
        [DataMember(Name = "finish")]
        public GetMaterialFinishModelView finish { get; set; }

        /// <summary>
        /// Material Finish's current price
        /// </summary>
        /// <value></value>
        [DataMember(Name = "currentPrice")]
        public PriceModelView currentPrice { get; set; }

        /// <summary>
        /// Time Period for which the price is active
        /// </summary>
        /// <value>Gets/Sets the time period</value>
        [DataMember(Name = "timePeriod")]
        public TimePeriodModelView timePeriod { get; set; }
    }
}