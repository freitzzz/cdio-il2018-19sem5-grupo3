using support.options;

namespace core.dto.options{
    /// <summary>
    /// Class which holds all the options which a ProductDTO can have 
    /// </summary>
    public sealed class ProductDTOOptions:Options{
        /// <summary>
        /// Specifies the unit which the dimensions of a product will be represented
        /// </summary>
        public string requiredUnit{get;set;}
    }
}