using System.Collections.Generic;
using System.Runtime.Serialization;
using core.domain;
using support.dto;
using System;

namespace core.dto {
    /// <summary>
    /// Represents a Product's Data Transfer Object.
    /// </summary>
    /// <typeparam name="Product">Type of entity</typeparam>
    /// <typeparam name="ProductDTO">Type of DTO</typeparam>
    [DataContract]
    public class ProductDTO : DTO, DTOParseable<Product, ProductDTO> {
        /// <summary>
        /// Product's database identifier.
        /// </summary>
        /// <value>Gets/sets the value of the database identifier field.</value>
        [DataMember]
        public long id { get; set; }

        /// <summary>
        /// Product's reference.
        /// </summary>
        /// <value>Gets/sets the value of the reference field.</value>
        [DataMember]
        public string reference { get; set; }

        /// <summary>
        /// Product's designation.
        /// </summary>
        /// <value>Gets/sets the value of the designation field.</value>
        [DataMember]
        public string designation { get; set; }

        /// <summary>
        /// List of product-material relations
        /// </summary>
        /// <value>Gets/sets the value of the materials field.</value>
        [DataMember(Name = "materials", EmitDefaultValue = false)]  //no data field data is presented if it's null
        public List<MaterialDTO> productMaterials { get; set; }

        /// <summary>
        /// Product's list of complementary products.
        /// </summary>
        /// <value>Gets/sets the value of the complements field.</value>
        [DataMember(Name = "components", EmitDefaultValue = false)]  //no data field data is presented if it's null
        public List<ComponentDTO> complements { get; set; }

        /// <summary>
        /// Product category
        /// </summary>
        [DataMember(Name = "category")]
        public ProductCategoryDTO productCategory { get; set; }

        /// <summary>
        /// Product dimensions
        /// </summary>
        [DataMember(Name = "dimensions")]
        public DimensionsListDTO dimensions { get; set; }

        /// <summary>
        /// SlotDimensionSetDTO with the Product's maximum, minimum and recommended Slot dimensions
        /// </summary>
        [DataMember(Name = "slots")]
        public SlotDimensionSetDTO slotDimensions;

        public ProductDTO() {
            //it is necessary to instantiate the DimensionsListDTO property
            dimensions = new DimensionsListDTO();
            productMaterials = new List<MaterialDTO>();
        }

        /// <summary>
        /// Returns this DTO's equivalent Entity
        /// </summary>
        /// <returns>DTO's equivalent Entity</returns>
        public Product toEntity() {
            /*        IEnumerable<Component> productComponents=complements!=null ? DTOUtils.reverseDTOS(complements) : null;
                   if(productComponents==null){
                       Console.WriteLine("->>>>>>>>>>>>>>>"+reference);
                       Console.WriteLine("->>>>>>>>>>>>>>>"+productCategory);
                       return new Product(reference
                                           ,designation
                                           ,productCategory.toEntity()
                                           ,DTOUtils.reverseDTOS(productMaterials)
                                           ,DTOUtils.reverseDTOS(heightDimensions)
                                           ,DTOUtils.reverseDTOS(widthDimensions)
                                           ,DTOUtils.reverseDTOS(depthDimensions)
                                         );
                   }else{
                       return new Product(reference
                                           ,designation
                                           ,productCategory.toEntity()
                                           ,DTOUtils.reverseDTOS(productMaterials)
                                           ,productComponents
                                           ,DTOUtils.reverseDTOS(heightDimensions)
                                           ,DTOUtils.reverseDTOS(widthDimensions)
                                           ,DTOUtils.reverseDTOS(depthDimensions)
                                         );
                   } */
            return null;
        }
        //TODO: Add RestrictionDTO's & Slot's Max, Min, Recommended Dimensions
    }
}