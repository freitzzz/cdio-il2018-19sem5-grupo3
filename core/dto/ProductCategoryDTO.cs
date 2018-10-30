using System.Runtime.Serialization;
using core.domain;
using support.dto;

namespace core.dto
{
    /// <summary>
    /// Represents a ProductCategory's Data Transfer Object. 
    /// </summary>
    /// <typeparam name="ProductCategory">Type of domain entity</typeparam>
    /// <typeparam name="ProductCategoryDTO">Type of DTO</typeparam>
    [DataContract]
    public class ProductCategoryDTO : DTO, DTOParseable<ProductCategory, ProductCategoryDTO>
    {
        /// <summary>
        /// ProductCategory's database identifier.
        /// </summary>
        /// <value>Gets/sets the value of the database identifier.</value>
        [DataMember]
        public long id { get; set; }

        /// <summary>
        /// ProductCategory's name.
        /// </summary>
        /// <value>Gets/sets the value of the name.</value>
        [DataMember]
        public string name { get; set; }

        public ProductCategory toEntity()
        {
            ProductCategory category = new ProductCategory(this.name);
            category.Id = id;
            return category;
        }
    }
}