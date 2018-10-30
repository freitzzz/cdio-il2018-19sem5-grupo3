using System.Runtime.Serialization;

namespace core.dto{
    /// <summary>
    /// Model View representation for the add restriction to a product dimension context
    /// </summary>
    [DataContract]
    public sealed class AddRestrictionToProductDimensionDTO{
        /// <summary>
        /// Long with the product resource ID which restriction will be applied to its dimension
        /// </summary>
        public long productID{get;set;}

        /// <summary>
        /// Long with the dimension resource ID which restriction will be applied to
        /// </summary>
        public long dimensionID{get;set;}
        
        /// <summary>
        /// RestrictionDTO with the restriction information which will be applied to the product dimension
        /// </summary>
        public RestrictionDTO restriction{get;set;}
    }
}