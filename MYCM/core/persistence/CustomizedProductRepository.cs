using core.domain;
using core.dto;
using support.domain.ddd;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace core.persistence
{
    /// <summary>
    /// Interface that represents the repository functionalities for customized products
    /// </summary>
    /// <typeparam name="CustomizedProduct">Generic-Type of the repository aggregate root</typeparam>
    /// <typeparam name="long">Generic-Type of the aggregate entity persistence identifier</typeparam>
    /// <typeparam name="string">Generic-Type of the aggreagate entity identity</typeparam>
    public interface CustomizedProductRepository : Repository<CustomizedProduct, long, string>
    {
        /// <summary>
        /// Fetches all customized products by their persistence IDS
        /// </summary>
        /// <returns>IEnumerable with all customized products by their PIDS</returns>
        IEnumerable<CustomizedProduct> findCustomizedProductsByTheirPIDS(IEnumerable<CustomizedProductDTO> customizedProductDTOS);
        /// <summary>
        /// Finds all instances of CustomizedProduct that are not inserted in any other CustomizedProduct's slot.
        /// </summary>
        /// <returns>IEnumerable of CustomizedProduct with the base instances of CustomizedProduct.</returns>
        IEnumerable<CustomizedProduct> findBaseCustomizedProducts();

        /// <summary>
        /// Finds an instance of CustomizedProduct using the identifier of one of its slots.
        /// </summary>
        /// <param name="slot">Instance of Slot.</param>
        /// <returns>Instance of CustomizedProduct to which the slot belongs.</returns>
        CustomizedProduct findCustomizedProductBySlot(Slot slot);
        /// <summary>
        /// Fetches a given slot of a given customized product
        /// </summary>
        /// <param name="customizedProductId">PID of the CustomizedProduct</param>
        /// <param name="slotId">PID of the Slot</param>
        /// <returns>Slot instance</returns>
        Slot findSlot(long customizedProductId, long slotId);
    }
}