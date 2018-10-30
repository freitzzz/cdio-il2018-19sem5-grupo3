using System.Runtime.Serialization;

namespace core.dto{
    [DataContract]
    /// <summary>
    /// A Model View DTO representation for the add dimension to a product context
    /// </summary>
    public sealed class AddDimensionToProductDTO{
        /// <summary>
        /// DimensionDTO with the width dimension which will be added to the product
        /// </summary>
        [DataMember(Name="widthDimension")]
        public DimensionDTO widthDimension{get;set;}

        /// <summary>
        /// DimensionDTO with the height dimension which will be added to the product
        /// </summary>
        [DataMember(Name="heightDimension")]
        public DimensionDTO heightDimension{get;set;}

        /// <summary>
        /// DimensionDTO with the depth dimension which will be added to the product
        /// </summary>
        [DataMember(Name="depthDimension")]
        public DimensionDTO depthDimension{get;set;}
    }
}