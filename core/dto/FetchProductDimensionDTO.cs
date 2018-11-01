namespace core.dto{
    /// <summary>
    /// DTO for holding information about the fetch of a product dimension
    /// </summary>
    public sealed class FetchProductDimensionDTO{
        /// <summary>
        /// Long with the product ID
        /// </summary>
        public long productID{get;set;}

        /// <summary>
        /// Long with the product dimension ID
        /// </summary>
        public long dimensionID{get;set;}
    }
}