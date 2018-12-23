using System.Runtime.Serialization;

namespace core.modelview.area
{
    [DataContract]
    public class AreaModelView
    {
        [DataMember(Name = "area")]
        public string area { get; set; }
    }
}