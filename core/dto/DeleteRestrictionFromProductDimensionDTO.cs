using System.Runtime.Serialization;

namespace core.dto{
    /// <summary>
    /// Model View representation for the delete restriction from a product dimension context
    /// </summary>
    public sealed class DeleteRestrictionFromProductDimensionDTO{
        /// <summary>
        /// Long with the product resource ID which restriction will be deleted from its dimension
        /// </summary>
        public long productID{get;set;}

        /// <summary>
        /// Long with the dimension resource ID which restriction will be deleted from
        /// </summary>
        public long dimensionID{get;set;}

        /// <summary>
        /// Long with the restriction resource ID which will be deleted
        /// </summary>
        public long restrictionID{get;set;}
    }
}