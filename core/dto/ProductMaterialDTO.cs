using core.domain;
using support.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace core.dto {
    /// <summary>
    /// Represents ProductMaterial's data transfer object
    /// </summary>
    [DataContract]
    public class ProductMaterialDTO : DTO, DTOParseable<ProductMaterial, ProductMaterialDTO> {
        /// <summary>
        /// ProductMaterial's database identifier.
        /// </summary>
        /// <value>Gets/sets the value of the database identifier field.</value>
        [DataMember]
        public long id { get; set; }
        /// <summary>
        /// Material the Product is connected to
        /// </summary>
        [DataMember]
        public MaterialDTO material { get; set; }
        /// <summary>
        /// Product
        /// </summary>
        [DataMember]
        public ProductDTO product { get; set; }
        /// <summary>
        /// List of restrictions in the product-material relation
        /// </summary>
        [DataMember]
        public List<RestrictionDTO> restrictions { get; set; }
        /// <summary>
        /// Creates a ProductMaterial Entity from the DTO
        /// </summary>
        /// <returns>new ProductMaterial</returns>
        public ProductMaterial toEntity() {
            throw new System.NotImplementedException();
        }
    }
}
