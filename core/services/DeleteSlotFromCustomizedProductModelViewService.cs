using core.domain;
using core.dto;
using core.persistence;

namespace core.services
{
    /// <summary>
    /// Service to help delete a slot from a customized product
    /// </summary>
    public static class DeleteSlotFromCustomizedProductModelViewService
    {
        /// <summary>
        /// Hard deletes a slot from a customized product
        /// </summary>
        /// <param name="deleteSlotFromCustomizedProductModelView">ModelView with the necessary information to remove a slot from a customized product</param>
        /// <returns>true if the slot was removed successfully, false if otherwise</returns>
        public static bool delete(DeleteSlotFromCustomizedProductModelView deleteSlotFromCustomizedProductModelView)
        {
            bool slotRemoved = false;
            CustomizedProductRepository customizedProductRepository =
                PersistenceContext.repositories()
                                    .createCustomizedProductRepository();

            CustomizedProduct customizedProduct =
                customizedProductRepository.find(deleteSlotFromCustomizedProductModelView.customizedProductId);

            if (!customizedProduct.product.supportsSlots)
            {
                return false;
            }
            else
            {
                //TODO Replace foreach with query
                //!Temporary solution using for each instead of queries due to thread safe issue discovered in PostCustomizedProductToSlotModelViewService
                foreach (Slot slot in customizedProduct.slots)
                {
                    if (slot.Id == deleteSlotFromCustomizedProductModelView.slotId)
                    {
                        slotRemoved = customizedProduct.slots.Remove(slot)
                                        && customizedProductRepository.update(customizedProduct) != null;
                        break;
                    }
                }
            }
            return slotRemoved;
        }
    }
}