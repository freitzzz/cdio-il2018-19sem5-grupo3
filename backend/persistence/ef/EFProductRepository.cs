using core.domain;
using core.dto;
using core.persistence;
using support.persistence.repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace backend.persistence.ef
{
    public class EFProductRepository : EFBaseRepository<Product, long, string>, ProductRepository
    {
        public EFProductRepository(MyCContext dbContext) : base(dbContext){}
        /// <summary>
        /// Fetches product component by their ids
        /// </summary>
        /// <param name="productID">product id</param>
        /// <param name="componentID">component id</param>
        /// <returns>product component with respective id</returns>
        public Component fetchProductComponent(long productID, long componentID) {
            Product product = find(productID);
            return product.complementedProducts.SingleOrDefault(c => c.complementedProductId == componentID);
        }

        /// <summary>
        /// Fetches an enumerable of products by their ids
        /// </summary>
        /// <param name="productsDTO">IEnumerable with the products information</param>
        /// <returns>IEnumerable with the fetched products</returns>
        public IEnumerable<Product> fetchProductsByID(IEnumerable<ProductDTO> productsDTO){
            List<long> productsID=new List<long>();
            foreach(ProductDTO productDTO in productsDTO)productsID.Add(productDTO.id);
            return (from product in base.dbContext.Set<Product>()
                    where productsID.Contains(product.Id)
                    select product
                    );
        }
        /// <summary>
        /// Updates a product
        /// <br>Returns null if the update affects the entity identifier and the changing entity identifier already exists on the database
        /// </summary>
        /// <param name="entity">Product with the product being updated</param>
        /// <returns>Product with the updated product</returns>
        public override Product update(Product entity){
            if(base.find(entity.id()).Id!=entity.Id)
                return null;
            return update(entity);
        }
    }
}