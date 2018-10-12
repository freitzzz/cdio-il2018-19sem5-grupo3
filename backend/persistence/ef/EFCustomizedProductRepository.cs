using core.domain;
using core.dto;
using core.persistence;
using support.persistence.repositories;
using System.Collections.Generic;
using System.Linq;

namespace backend.persistence.ef
{
    public class EFCustomizedProductRepository : EFBaseRepository<CustomizedProduct, long, string>, CustomizedProductRepository
    {
        public EFCustomizedProductRepository(MyCContext dbContext) : base(dbContext){}

        /// <summary>
        /// Fetches all available customized products
        /// </summary>
        /// <returns>IEnumerable with all available customized products</returns>
        public IEnumerable<CustomizedProduct> findAllCustomizedProducts()
        {
            return findAll();
        }

        /// <summary>
        /// Fetches all customized products by their persistence IDS
        /// </summary>
        /// <returns>IEnumerable with all customized products by their PIDS</returns>
        public IEnumerable<CustomizedProduct> findCustomizedProductsByTheirPIDS(IEnumerable<CustomizedProductDTO> customizedProductDTOS)
        {
            List<long> customizedProductsPIDS=new List<long>();
            foreach(CustomizedProductDTO customizedProductDTO in customizedProductDTOS)
                customizedProductsPIDS.Add(customizedProductDTO.id);
            return (from customizedProduct in base.dbContext.Set<CustomizedProduct>()
                    where customizedProductsPIDS.Contains(customizedProduct.Id)
                    select customizedProduct
            );
        }
    }
}