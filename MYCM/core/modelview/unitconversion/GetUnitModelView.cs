using System.Runtime.Serialization;

namespace core.modelview.unitconversion
{
    /// <summary>
    /// Class representing the ModelView used for fetching a unit.
    /// </summary>
    [DataContract]
    public class GetUnitModelView
    {
        /// <summary>
        /// String representing the unit.
        /// </summary>
        /// <value>Gets/Sets the value of the unit.</value>
        [DataMember]
        public string unit { get; set; }
    }
}