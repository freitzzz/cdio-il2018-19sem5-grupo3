using core.domain;
using core.dto;
using core.persistence;
using support.persistence.repositories;
using System.Collections.Generic;
using System.Linq;

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
    }
}