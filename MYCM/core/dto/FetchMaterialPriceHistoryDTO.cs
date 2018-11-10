namespace core.dto{
    /// <summary>
    /// DTO for holding the necessary properties for the fetch material price history
    /// </summary>
    public class FetchMaterialPriceHistoryDTO{
        /// <summary>
        /// Long with the material resource ID being fetched the price history
        /// </summary>
        public long materialID{get;set;}
    }
}