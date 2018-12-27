namespace core.dto{
    /// <summary>
    /// DTO for holding the necessary properties for the fetch material price history
    /// </summary>
    public class FetchMaterialPriceHistoryDTO{
        /// <summary>
        /// Long with the material resource ID being fetched the price history
        /// </summary>
        public long materialID{get;set;}
        /// <summary>
        /// Requested currency to present the price in
        /// </summary>
        public string currency{get;set;}
        /// <summary>
        /// Requested area to present the price in
        /// </summary>
        public string area{get;set;}
    }
}