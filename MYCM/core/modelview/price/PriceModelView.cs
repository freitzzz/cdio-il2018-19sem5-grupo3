using System.Runtime.Serialization;

namespace core.modelview.price
{
    /// <summary>
    /// ModelView that represents the current price per area of something
    /// </summary>
    [DataContract]
    public class PriceModelView
    {
        /// <summary>
        /// Value of the price itself
        /// </summary>
        /// <value>Gets/Sets the value</value>
        [DataMember(Name = "value")]
        public double value { get; set; }

        /// <summary>
        /// The currency in which the price is presented
        /// </summary>
        /// <value>Gets/Sets the currency</value>
        [DataMember(Name = "currency")]
        public string currency { get; set; }

        /// <summary>
        /// The area in which the price is presented
        /// </summary>
        /// <value>Gets/Sets the Area</value>
        [DataMember(Name = "area", EmitDefaultValue = false)]
        public string area { get; set; }
    }
}