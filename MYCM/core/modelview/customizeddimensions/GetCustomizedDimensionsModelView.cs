using System.Runtime.Serialization;

namespace core.modelview.customizeddimensions
{
    /// <summary>
    /// Class representing the ModelView used for fetching CustomizedDimensions' data.
    /// </summary>
    [DataContract]
    public class GetCustomizedDimensionsModelView
    {
        /// <summary>
        /// CustomizedDimensions' height value.
        /// </summary>
        /// <value>Gets/sets the height value.</value>
        [DataMember]
        public double height { get; set; }

        /// <summary>
        /// CustomizedDimensions' width value.
        /// </summary>
        /// <value>Gets/sets the width value.</value>
        [DataMember]
        public double width { get; set; }

        /// <summary>
        /// CustomizedDimensions' depth value.
        /// </summary>
        /// <value>Gets/sets the depth value.</value>
        [DataMember]
        public double depth { get; set; }

        /// <summary>
        /// CustomizedDimensions' measurement unit.
        /// </summary>
        /// <value>Gets/sets the unit value.</value>
        [DataMember]
        public string unit { get; set; }
    }
}