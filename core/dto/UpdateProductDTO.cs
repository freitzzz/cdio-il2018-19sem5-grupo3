using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.dto{
    [DataContract]
    /// <summary>
    /// Simple DTO class used to transpor data required for products updates
    /// </summary>
    public sealed class UpdateProductDTO{
        /// <summary>
        /// ID of the product being updated
        /// </summary>
        public long id;

        /// <summary>
        /// String with the product reference being updated
        /// </summary>
        public string reference;
        /// <summary>
        /// String with the product designation being updated
        /// </summary>
        public string designation;

        
        /// <summary>
        /// List with the components being added
        /// </summary>
        [DataMember(Name="add")]
        public List<ComponentDTO> componentsToAdd{get;set;}
        /// <summary>
        /// List with the materials being added
        /// </summary>
        [DataMember(Name="add")]
        public List<MaterialDTO> materialsToAdd{get;set;}
        /// <summary>
        /// DimensionListDTO with the dimensions being added
        /// </summary>
        [DataMember(Name="add")]
        public DimensionsListDTO dimensionsToAdd{get;set;}

        /// <summary>
        /// List with the components being removed
        /// </summary>
        [DataMember(Name="remove")]
        public List<ComponentDTO> componentsToRemove{get;set;}
        /// <summary>
        /// List with the materials being removed
        /// </summary>
        [DataMember(Name="remove")]
        public List<MaterialDTO> materialsToRemove{get;set;}
        /// <summary>
        /// DimensionListDTO with the dimensions being removed
        /// </summary>
        [DataMember(Name="remove")]
        public DimensionsListDTO dimensionsToRemove{get;set;}
    }
}