using core.domain;
using core.dto;
using core.persistence;
using Microsoft.EntityFrameworkCore;
using support.persistence.repositories;
using support.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.persistence.ef
{
    public class EFProductRepository : EFBaseRepository<Product, long, string>, ProductRepository
    {
        public EFProductRepository(MyCContext dbContext) : base(dbContext){}

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

        public IEnumerable<Product> findBaseProducts()
        {
            List<long> childrenIdentifiers = dbContext.Set<Component>().Select(c => c.complementaryProduct.Id).ToList();

            return dbContext.Product.Where(p => p.activated).Where(p => !childrenIdentifiers.Contains(p.Id)).Distinct();
        }
    }
}