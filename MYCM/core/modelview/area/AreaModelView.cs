using System.Runtime.Serialization;

namespace core.modelview.area
{
    /// <summary>
    /// Model View that represents an Area
    /// </summary>
    [DataContract]
    public class AreaModelView
    {
        /// <summary>
        /// String containing the unit of area
        /// </summary>
        /// <value>Gets/Sets the unit of area</value>
        [DataMember(Name = "area")]
        public string area { get; set; }
    }
}