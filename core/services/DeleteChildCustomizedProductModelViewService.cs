using core.domain;
using core.dto;
using core.modelview.customizedproduct;
using core.persistence;

namespace core.services
{
    /// <summary>
    /// Service to help hard deleting a child customized product
    /// </summary>
    public static class DeleteChildCustomizedProductModelViewService
    {
        /// <summary>
        /// Hard deletes a child Customized Product
        /// </summary>
        /// <param name="deleteChildCustomizedProductModelView">ModelView with the necessary information to hard delete a child customized product</param>
        /// <returns>true if the child customized product is deleted successfully, false if otherwise</returns>
        public static bool delete(DeleteChildCustomizedProductModelView deleteChildCustomizedProductModelView)
        {
            bool deletedWithSuccess = false;

            CustomizedProductRepository customizedProductRepository =
                PersistenceContext.repositories()
                                    .createCustomizedProductRepository();

            CustomizedProduct fatherCustomizedProduct =
                customizedProductRepository.find(deleteChildCustomizedProductModelView.parentId);

            CustomizedProduct childCustomizedProduct =
                customizedProductRepository.find(deleteChildCustomizedProductModelView.childId);

            if (childCustomizedProduct == null || fatherCustomizedProduct == null)
            {
                return false;
            }

            foreach (Slot slot in fatherCustomizedProduct.slots)
            {
                if (slot.Id == deleteChildCustomizedProductModelView.slotId)
                {
                    deletedWithSuccess = slot.removeCustomizedProduct(childCustomizedProduct);
                    break;
                }
            }

            deletedWithSuccess = customizedProductRepository.remove(childCustomizedProduct) != null;
            return deletedWithSuccess;
        }
    }
}