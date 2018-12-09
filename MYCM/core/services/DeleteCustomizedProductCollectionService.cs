using core.modelview.customizedproductcollection;
using core.persistence;
using core.domain;
using core.exceptions;
using System;

namespace core.services
{
    /// <summary>
    /// Service to help with deactivating and removal operations related to Customized Product Collections
    /// </summary>   
    public static class DeleteCustomizedProductCollectionService
    {

        /// <summary>
        /// Constant that represents the message that's presented if the requested customized product collection can't be found
        /// </summary>
        /// <value></value>
        private const string UNABLE_TO_FIND_CUSTOMIZED_PRODUCT_COLLECTION = "Unable to find customized product collection with the identifier of: {0}";

        /// <summary>
        /// Constant that represents the error message presented when the requested customized product wasn't found
        /// </summary>
        private const string UNABLE_TO_FIND_CUSTOMIZED_PRODUCT = "Unable to find customized product with identifier of: {0}";

        /// <summary>
        /// Deactivates a customized product collection
        /// </summary>
        /// <param name="modelView"> model view with the necessary information to deactivate a customized product collection</param>
        public static void deactivate(DeleteCustomizedProductCollectionModelView modelView)
        {
            CustomizedProductCollectionRepository customizedProductCollectionRepository =
                PersistenceContext.repositories()
                    .createCustomizedProductCollectionRepository();

            CustomizedProductCollection customizedProductCollection =
                customizedProductCollectionRepository.find(modelView.customizedProductCollectionId);

            checkIfCustomizedProductCollectionWasFound(customizedProductCollection, modelView.customizedProductCollectionId);

            customizedProductCollectionRepository.remove(customizedProductCollection);
        }

        /// <summary>
        /// Deletes a customized product from a customized product collection
        /// </summary>
        /// <param name="modelView"> model view with the necessary information to perform the request</param>
        public static void delete(DeleteCustomizedProductFromCustomizedProductCollectionModelView modelView)
        {
            CustomizedProductCollectionRepository customizedProductCollectionRepository =
                PersistenceContext.repositories()
                    .createCustomizedProductCollectionRepository();

            CustomizedProductCollection customizedProductCollection =
                customizedProductCollectionRepository.find(modelView.customizedProductCollectionId);

            checkIfCustomizedProductCollectionWasFound(customizedProductCollection, modelView.customizedProductCollectionId);

            CustomizedProduct customizedProduct =
                PersistenceContext.repositories()
                    .createCustomizedProductRepository()
                        .find(modelView.customizedProductId);

            if (customizedProduct == null)
            {
                throw new ArgumentException(
                    string.Format(
                                UNABLE_TO_FIND_CUSTOMIZED_PRODUCT,
                                modelView.customizedProductId)
                );
            }

            customizedProductCollection.removeCustomizedProduct(customizedProduct);

            customizedProductCollectionRepository.update(customizedProductCollection);
        }

        /// <summary>
        /// Checks if a customized product collection was found
        /// </summary>
        /// <param name="customizedProductCollection">Customized Product Collection to be checked</param>
        /// <param name="customizedProductCollectionId">PID of the requested customized product collection</param>
        private static void checkIfCustomizedProductCollectionWasFound(CustomizedProductCollection customizedProductCollection, long customizedProductCollectionId)
        {
            if (customizedProductCollection == null)
            {
                throw new ResourceNotFoundException(
                    string.Format(
                        UNABLE_TO_FIND_CUSTOMIZED_PRODUCT_COLLECTION,
                         customizedProductCollectionId)
                );
            }
        }
    }
}