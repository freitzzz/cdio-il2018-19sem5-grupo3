using support.dto;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.dto{
    /// <summary>
    /// DTO used for the deserialization of the customized product collection updates
    /// </summary>
    [DataContract]
    public class UpdateCustomizedProductCollectionDTO:DTO{
        /// <summary>
        /// Long with the id of the customized product collection being updated
        /// </summary>
        [DataMember(Name="id")]
        public long id{get;set;}
        /// <summary>
        /// String with the customized product collection name
        /// </summary>
        [DataMember(Name="name")]
        public string name{get;set;}
        /// <summary>
        /// List with the customized products to add to the collection
        /// </summary>
        [DataMember(Name="addCustomizedProducts")]
        public List<CustomizedProductDTO> customizedProductsToAdd{get;set;}
        /// <summary>
        /// List with the customized products to remove from the collection
        /// </summary>
        [DataMember(Name="removeCustomizedProducts")]
        public List<CustomizedProductDTO> customizedProductsToRemove{get;set;}
    }
}