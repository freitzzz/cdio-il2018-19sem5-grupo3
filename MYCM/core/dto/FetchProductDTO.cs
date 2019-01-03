using core.dto.options;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.dto{
    /// <summary>
    /// DTO for holding the information which will be used to fetch a product
    /// </summary>
    [DataContract]
    public sealed class FetchProductDTO{
        /// <summary>
        /// Long with the persistence ID of the product being fetched
        /// </summary>
        [DataMember(Name="id")]
        public long id{get;set;}

        /// <summary>
        /// String with the reference of the product being fetched
        /// </summary>
        [DataMember(Name="reference")]
        public string reference{get;set;}

        /// <summary>
        /// Boolean to know if only priced materials are to be fetched
        /// </summary>
        [DataMember(Name="pricedMaterialsOnly")]
        public bool pricedMaterialsOnly{get;set;}
        
        /// <summary>
        /// ProductDTOOptions with the set of options which the product information can have 
        /// </summary>
        public ProductDTOOptions productDTOOptions{get;set;} = new ProductDTOOptions();
    }
}