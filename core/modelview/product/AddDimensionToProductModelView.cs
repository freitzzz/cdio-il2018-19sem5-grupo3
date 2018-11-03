using core.dto;
using System.Runtime.Serialization;

namespace core.modelview.product{
    [DataContract]
    /// <summary>
    /// A Model View DTO representation for the add dimension to a product context
    /// </summary>
    public sealed class AddDimensionToProductModelView{
        /// <summary>
        /// Long with the resource ID of the product which will be complemented
        /// </summary>
        public long productID{get;set;}
        
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