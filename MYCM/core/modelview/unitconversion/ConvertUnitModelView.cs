using System.Runtime.Serialization;

namespace core.modelview.unitconversion
{
    /// <summary>
    /// Class representing the model
    /// </summary>
    [DataContract]
    public class ConvertUnitModelView
    {
        /// <summary>
        /// Unit to which the value will be converted.
        /// </summary>
        /// <value>Gets/Sets the unit to which the value will be converted.</value>
        [DataMember(Name="to")]
        public string toUnit { get; set; }

        /// <summary>
        /// Unit from which the value will be converted.
        /// </summary>
        /// <value>Gets/Sets the unit from which the value will be converted.</value>
        [DataMember(Name="from")]
        public string fromUnit { get; set; }

        /// <summary>
        /// Value being converted.
        /// </summary>
        /// <value>Gets/Sets the value being converted.</value>
        [DataMember]
        public double value { get; set; }
    }
}