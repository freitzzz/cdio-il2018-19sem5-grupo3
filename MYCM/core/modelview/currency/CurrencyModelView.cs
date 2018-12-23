using System.Runtime.Serialization;

namespace core.modelview.currency
{
    [DataContract]
    public class CurrencyModelView
    {
        [DataMember(Name = "currency")]
        public string currency { get; set; }
    }
}