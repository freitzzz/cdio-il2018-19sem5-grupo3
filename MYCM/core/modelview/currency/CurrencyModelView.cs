using System.Runtime.Serialization;

namespace core.modelview.currency
{
    /// <summary>
    /// ModelView that represents a currency
    /// </summary>
    [DataContract]
    public class CurrencyModelView
    {
        /// <summary>
        /// Currency's symbol
        /// </summary>
        /// <value>Gets/Sets the currency</value>
        [DataMember(Name = "currency")]
        public string currency { get; set; }
    }
}