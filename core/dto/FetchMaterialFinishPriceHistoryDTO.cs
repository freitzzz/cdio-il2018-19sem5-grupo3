namespace core.dto{
    /// <summary>
    /// DTO for holding the necessary properties for the fetch material finish price history
    /// </summary>
    public sealed class FetchMaterialFinishPriceHistoryDTO:FetchMaterialPriceHistoryDTO{

        /// <summary>
        /// Long with the material finish resource ID being fetched the price history
        /// </summary>
        public long finishID{get;set;}
    }
}