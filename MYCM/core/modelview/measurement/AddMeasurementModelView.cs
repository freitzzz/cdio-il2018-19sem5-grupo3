using System.Runtime.Serialization;
using core.modelview.dimension;

namespace core.modelview.measurement
{
    /// <summary>
    /// Class representing the ModelView used for adding instances of Measurement.
    /// </summary>
    [DataContract]
    public class AddMeasurementModelView
    {
        /// <summary>
        /// AddDimensionModelView with the width dimension which will be added to the product
        /// </summary>
        [DataMember(Name="width")]
        public AddDimensionModelView widthDimension{get;set;}

        /// <summary>
        /// AddDimensionModelView with the height dimension which will be added to the product
        /// </summary>
        [DataMember(Name="height")]
        public AddDimensionModelView heightDimension{get;set;}

        /// <summary>
        /// AddDimensionModelView with the depth dimension which will be added to the product
        /// </summary>
        [DataMember(Name="depth")]
        public AddDimensionModelView depthDimension{get;set;}
    }
}