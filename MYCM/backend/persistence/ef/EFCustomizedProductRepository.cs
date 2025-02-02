using core.domain;
using core.dto;
using core.persistence;
using support.persistence.repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.persistence.ef
{
    public class EFCustomizedProductRepository : EFBaseRepository<CustomizedProduct, long, string>, CustomizedProductRepository
    {
        public EFCustomizedProductRepository(MyCContext dbContext) : base(dbContext) { }

        public IEnumerable<CustomizedProduct> findBaseCustomizedProducts()
        {
            return dbContext.CustomizedProduct.Where(cp => !cp.insertedInSlotId.HasValue).Where(cp => cp.activated).ToList();
        }

        public CustomizedProduct findCustomizedProductBySlot(Slot slot)
        {
            return dbContext.CustomizedProduct.Where(cp => cp.slots.Contains(slot)).SingleOrDefault();
        }

        /// <summary>
        /// Fetches all customized products by their persistence IDS
        /// </summary>
        /// <returns>IEnumerable with all customized products by their PIDS</returns>
        public IEnumerable<CustomizedProduct> findCustomizedProductsByTheirPIDS(IEnumerable<CustomizedProductDTO> customizedProductDTOS)
        {
            List<long> customizedProductsPIDS = new List<long>();
            foreach (CustomizedProductDTO customizedProductDTO in customizedProductDTOS)
                customizedProductsPIDS.Add(customizedProductDTO.id);
            return (from customizedProduct in base.dbContext.Set<CustomizedProduct>()
                    where customizedProductsPIDS.Contains(customizedProduct.Id)
                    select customizedProduct
            );
        }

        /// <summary>
        /// Fetches a slot of a customized product
        /// </summary>
        /// <param name="customizedProductId">PID of a customized product</param>
        /// <param name="slotId">PID of a slot</param>
        /// <returns>Slot of a customized product</returns>
        public Slot findSlot(long customizedProductId, long slotId)
        {
            /* return (
                    from customizedProduct in base.dbContext.CustomizedProduct
                        where customizedProduct.Id == customizedProductId
                            from slot in customizedProduct.slots
                                where slot.Id == slotId
                                    select slot
            ).SingleOrDefault(); */
            Task<CustomizedProduct> fetchedCustomizedProductTask =
                    dbContext.CustomizedProduct.FindAsync(customizedProductId);

            fetchedCustomizedProductTask.Wait();
            CustomizedProduct fetchedCustomizedProduct = fetchedCustomizedProductTask.Result;

            return fetchedCustomizedProduct.slots.
                    Where(s => s.Id == slotId).SingleOrDefault();
        }

        public IEnumerable<CustomizedProduct> findUserCreatedCustomizedProducts(string userAuthToken)
        {
            return dbContext.CustomizedProduct.Where(cp => cp.activated)
                                        .Where(cp => cp.authToken.Equals(userAuthToken))
                                        .Where(cp => cp.insertedInSlot == null).ToList();
        }
    }
}