using System;
using core.domain;
using core.exceptions;
using core.modelview.customizedproductcollection;
using core.persistence;

namespace core.services
{
    /// <summary>
    /// Service to help updating a Customized Product Collection
    /// </summary>
    public static class UpdateCustomizedProductCollectionService
    {

        /// <summary>
        /// Constant that represents the error message presented when the requested customized product collection wasn't found
        /// </summary>
        private const string UNABLE_TO_FIND_CUSTOMIZED_PRODUCT_COLLECTION = "Unable to find customized product collection with identifier of: {0}";

        /// <summary>
        /// Constant that represents the error message presented when the requested customized product collection wasn't updated
        /// </summary>
        private const string UNABLE_TO_UPDATE_CUSTOMIZED_PRODUCT_COLLECTION = "Unable to update customized product collection with identifier of: {0}";

        /// <summary>
        /// Constant that represents the error message presented when the requested customized product wasn't found
        /// </summary>
        private const string UNABLE_TO_FIND_CUSTOMIZED_PRODUCT = "Unable to find customized product with identifier of: {0}";


        /// <summary>
        /// Updates a customized product collection's basic information
        /// </summary>
        /// <param name="modelView">model view with the updates for a customized product collection</param>
        /// <returns>Updated customized product collection or throws an exception if an error occurs</returns>
        public static CustomizedProductCollection update(UpdateCustomizedProductCollectionModelView modelView)
        {
            CustomizedProductCollectionRepository customizedProductCollectionRepository =
                PersistenceContext.repositories()
                    .createCustomizedProductCollectionRepository();


            CustomizedProductCollection customizedProductCollection =
                customizedProductCollectionRepository.find(modelView.customizedProductCollectionId);

            checkIfCustomizedProductCollectionWasFound(
                customizedProductCollection, modelView.customizedProductCollectionId
            );

            customizedProductCollection.changeName(modelView.name);

            CustomizedProductCollection updatedCustomizedProductCollection =
                customizedProductCollectionRepository.update(customizedProductCollection);

            checkIfUpdatedCustomizedProductCollectionWasSaved(
                updatedCustomizedProductCollection, modelView.customizedProductCollectionId
            );

            return updatedCustomizedProductCollection;
        }

        /// <summary>
        /// Updates a customized product collection by adding a new customized product to it
        /// </summary>
        /// <param name="modelView">model view with the necessary update information</param>
        /// <returns>Updated customized product collection or throws an exception if an error occurs</returns>
        public static CustomizedProductCollection update(AddCustomizedProductToCustomizedProductCollectionModelView modelView)
        {
            CustomizedProductCollectionRepository customizedProductCollectionRepository =
                PersistenceContext.repositories()
                    .createCustomizedProductCollectionRepository();

            CustomizedProductCollection customizedProductCollection =
                customizedProductCollectionRepository.find(modelView.customizedProductCollectionId);

            checkIfCustomizedProductCollectionWasFound(
                customizedProductCollection, modelView.customizedProductCollectionId
            );

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

            customizedProductCollection.addCustomizedProduct(customizedProduct);

            CustomizedProductCollection updatedCustomizedProductCollection =
                customizedProductCollectionRepository.update(customizedProductCollection);

            checkIfUpdatedCustomizedProductCollectionWasSaved(
                updatedCustomizedProductCollection, modelView.customizedProductCollectionId
            );

            return updatedCustomizedProductCollection;
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

        /// <summary>
        /// Checks if a customized product collection was updated successfully
        /// </summary>
        /// <param name="updatedCustomizedProductCollection">Updated Customized Product Collection to be checked</param>
        /// <param name="customizedProductCollectionId">PID of the updated customized product collection</param>
        private static void checkIfUpdatedCustomizedProductCollectionWasSaved(CustomizedProductCollection updatedCustomizedProductCollection, long customizedProductCollectionId)
        {
            if (updatedCustomizedProductCollection == null)
            {
                throw new ArgumentException(
                    string.Format(
                        UNABLE_TO_UPDATE_CUSTOMIZED_PRODUCT_COLLECTION,
                        customizedProductCollectionId)
                );
            }
        }
    }
}