using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.dto{
    [DataContract]
    /// <summary>
    /// Simple DTO class used to transpor data required for products updates
    /// </summary>
    public class UpdateProductDTO{
        /// <summary>
        /// ID of the product being updated
        /// </summary>
        [DataMember]
        public long id{get;set;}

        /// <summary>
        /// String with the product reference being updated
        /// </summary>
        [DataMember]
        public string reference{get;set;}
        /// <summary>
        /// String with the product designation being updated
        /// </summary>
        [DataMember]
        public string designation{get;set;}
        /// <summary>
        /// ProductCategoryDTO with the category being updated
        /// </summary>
        [DataMember(Name="category")]
        public ProductCategoryDTO productCategoryToUpdate{get;set;}
        
        /// <summary>
        /// List with the components being added
        /// </summary>
        [DataMember(Name="addComponents")]
        public List<ComponentDTO> componentsToAdd{get;set;}
        /// <summary>
        /// List with the materials being added
        /// </summary>
        [DataMember(Name="addMaterials")]
        public List<MaterialDTO> materialsToAdd{get;set;}
        /// <summary>
        /// DimensionListDTO with the dimensions being added
        /// </summary>
        [DataMember(Name="addDimensions")]
        public DimensionsListDTO dimensionsToAdd{get;set;}

        /// <summary>
        /// List with the components being removed
        /// </summary>
        [DataMember(Name="removeComponents")]
        public List<ComponentDTO> componentsToRemove{get;set;}
        /// <summary>
        /// List with the materials being removed
        /// </summary>
        [DataMember(Name="removeMaterials")]
        public List<MaterialDTO> materialsToRemove{get;set;}
        /// <summary>
        /// DimensionListDTO with the dimensions being removed
        /// </summary>
        [DataMember(Name="removeDimensions")]
        public DimensionsListDTO dimensionsToRemove{get;set;}
    }
}