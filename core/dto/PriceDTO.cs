using System.Runtime.Serialization;

namespace core.dto
{
    /// <summary>
    /// DTO that represents a Price
    /// </summary>
    [DataContract]
    public class PriceDTO
    {
        /// <summary>
        /// Price's monetary value
        /// </summary>
        /// <value>Gets/Sets the value</value>
        [DataMember(Name = "value")]
        public double value { get; set; }

        /// <summary>
        /// Currency in which the value is presented in
        /// </summary>
        /// <value>Gets/Sets the currency</value>
        [DataMember(Name = "currency")]
        public string currency { get; set; }

        /// <summary>
        /// Unit of area that the price is applied to (e.g. euro per squared meter)
        /// </summary>
        /// <value>Gets/Sets the unit</value>
        [DataMember(Name = "area")]
        public string area { get; set; }
    }
}