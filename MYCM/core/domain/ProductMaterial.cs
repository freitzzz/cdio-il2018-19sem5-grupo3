using core.dto;
using Microsoft.EntityFrameworkCore.Infrastructure;
using support.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace core.domain
{
    /// <summary>
    /// Represents the relation between a Product and a Material
    /// </summary>
    public class ProductMaterial : Restrictable
    {

        /// <summary>
        /// Material's database identifier (Foreign Key, part of this class's Primary Key).
        /// </summary>
        /// <value>Gets/protected sets the Material's database identifier.</value>
        public long materialId { get; protected set; }

        /// <summary>
        /// Material
        /// </summary>
        /// 
        private Material _material;//!private field used for lazy loading, do not use this for storing or fetching data
        public Material material { get => LazyLoader.Load(this, ref _material); protected set => _material = value; }

        /// <summary>
        /// Product's database identifier (Foreign Key, part of this class's Primary Key).
        /// </summary>
        /// <value>Get/protected sets the Product's database identifier.</value>
        public long productId {get; protected set;}

        /// <summary>
        /// Product
        /// </summary>
        private Product _product;//!private field used for lazy loading, do not use this for storing or fetching data
        public Product product { get => LazyLoader.Load(this, ref _product); protected set => _product = value; }


        /// <summary>
        /// Protected constructor in order to allow ORM mapping
        /// </summary>
        protected ProductMaterial() { }

        /// <summary>
        /// Constructor used for injecting the LazyLoader.
        /// </summary>
        /// <param name="lazyLoader">LazyLoader being injected.</param>
        private ProductMaterial(ILazyLoader lazyLoader)
        {
            this.LazyLoader = lazyLoader;
        }

        /// <summary>
        /// Creates a new instance of ProductMaterial from the data received as parameter
        /// </summary>
        /// <param name="product">Product instance in the new relation</param>
        /// <param name="material">Material instance in the new relation</param>
        public ProductMaterial(Product product, Material material)
        {
            this.product = product;
            this.material = material;
            restrictions = new List<Restriction>();
        }

        /// <summary>
        /// Creates a new instance of ProductMaterial from the data received as parameter
        /// </summary>
        /// <param name="material">Material instance in the new relation</param>
        /// <param name="restrictions">List of restrictions in the new relation</param>
        /// <param name="product">Product instance in the new relation</param>
        public ProductMaterial(Product product, Material material, List<Restriction> restrictions)
        {
            this.material = material;
            this.restrictions = restrictions;
            this.product = product;
        }


        /// <summary>
        /// Checks if this relation has a Material
        /// </summary>
        /// <param name="material">Material to be checked for</param>
        /// <returns>true if this relation is about the Material, false if not</returns>
        public bool hasMaterial(Material material)
        {
            return this.material.Equals(material);
        }
    }
}
