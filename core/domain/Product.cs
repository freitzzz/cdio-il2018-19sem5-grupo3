using support.domain;
using support.domain.ddd;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace core.domain{
    /// <summary>
    /// Class that represents a Product
    /// <br>Product is an entity
    /// <br>Product is an aggregate root
    /// </summary>
    /// <typeparam name="string">Generic-Type of the Product entity identifier</typeparam>
    public class Product:AggregateRoot<string>{
        /// <summary>
        /// String with the product reference
        /// </summary>
        [Key]
        private readonly string reference;
        /// <summary>
        /// String with the product designation
        /// </summary>
        private readonly string designation;

        /// <summary>
        /// Returns the product identity
        /// </summary>
        /// <returns>string with the product identity</returns>
        public string id(){return reference;}

        /// <summary>
        /// Checks if a certain product entity is the same as the current product
        /// </summary>
        /// <param name="comparingEntity">string with the comparing product identity</param>
        /// <returns>boolean true if both entities identity are the same, false if not</returns>
        public bool sameAs(string comparingEntity){return id().Equals(comparingEntity);}
    }
}