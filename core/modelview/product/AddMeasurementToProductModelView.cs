using core.dto;
using core.modelview.dimension;
using System.Runtime.Serialization;

namespace core.modelview.product{
    [DataContract]
    /// <summary>
    /// A Model View DTO representation for the add dimension to a product context
    /// </summary>
    public sealed class AddMeasurementToProductModelView{
        /// <summary>
        /// Long with the resource ID of the product which will be complemented
        /// </summary>
        public long productID{get;set;}
        
        /// <summary>
        /// DimensionDTO with the width dimension which will be added to the product
        /// </summary>
        [DataMember(Name="width")]
        public AddDimensionModelView widthDimension{get;set;}

        /// <summary>
        /// DimensionDTO with the height dimension which will be added to the product
        /// </summary>
        [DataMember(Name="height")]
        public AddDimensionModelView heightDimension{get;set;}

        /// <summary>
        /// DimensionDTO with the depth dimension which will be added to the product
        /// </summary>
        [DataMember(Name="depth")]
        public AddDimensionModelView depthDimension{get;set;}
    }
}