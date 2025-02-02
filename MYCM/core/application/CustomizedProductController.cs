using core.domain;
using core.exceptions;
using core.modelview.customizeddimensions;
using core.modelview.customizedproduct;
using core.modelview.customizedproduct.customizedproductprice;
using core.modelview.product;
using core.modelview.slot;
using core.persistence;
using core.services;
using support.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static core.domain.CustomizedProduct;

namespace core.application {
    /// <summary>
    /// Application controller for customized products
    /// </summary>
    public sealed class CustomizedProductController {
        /// <summary>
        /// Constant that represents the message presented when no instance of CustomizedProduct was found.
        /// </summary>
        private const string ERROR_NO_CUSTOMIZED_PRODUCTS_FOUND = "Unable to find customized products.";

        /// <summary>
        /// Constant that represents the message presented when the new CustomizedProduct instance could not be saved.
        /// </summary>
        private const string ERROR_UNABLE_TO_SAVE_CUSTOMIZED_PRODUCT = "Unable to save the customized product, make sure the reference is unique.";

        /// <summary>
        /// Constant representing the message presented when no Product is found with a given identifier.
        /// </summary>
        private const string ERROR_UNABLE_TO_FIND_CUSTOMIZED_PRODUCT_BY_ID = "Unable to find a customized product with an identifier of: {0}";

        /// <summary>
        /// Constant representing the message presented when no Slot is found with a given identifier.
        /// </summary>
        private const string ERROR_UNABLE_TO_FIND_SLOT = "Unable to find a slot with an identifier of: {0}";

        /// <summary>
        /// Constant representing the message presented when none of the CustomizedProduct's properties are updated.
        /// </summary>
        private const string ERROR_NO_UPDATE_PERFORMED = "The request did not perform any update.";

        /// <summary>
        /// Constant representing the message presented when a CustomizedProduct that does not belong to the current user is attempted to be altered.
        /// </summary>
        private const string UNABLE_TO_ALTER_CUSTOMIZED_PRODUCT = "Unable to alter the customized product.";

        /// <summary>
        /// Constant that represents the message that occurs when there are no possible components for the selected slot in a customized product
        /// </summary>
        private const string NO_POSSIBLE_COMPONENTS = "There are no possible components for the selected slot!";

        /// <summary>
        /// Fetches all available customized products.
        /// </summary>
        /// <returns>List with all available customized products</returns>
        /// <exception cref="ResourceNotFoundException">Thrown when no instance of CustomizedProduct was found.</exception>
        public GetAllCustomizedProductsModelView findAllCustomizedProducts() {
            IEnumerable<CustomizedProduct> customizedProducts = PersistenceContext.repositories().
                createCustomizedProductRepository().findAll();

            if (!customizedProducts.Any()) {
                throw new ResourceNotFoundException(ERROR_NO_CUSTOMIZED_PRODUCTS_FOUND);
            }

            return CustomizedProductModelViewService.fromCollection(customizedProducts);
        }

        /// <summary>
        /// Fetches all available base customized products.
        /// </summary>
        /// <returns>List with all available customized products</returns>
        /// <exception cref="ResourceNotFoundException">Thrown when no instance of CustomizedProduct was found.</exception>
        public GetAllCustomizedProductsModelView findAllBaseCustomizedProducts() {
            IEnumerable<CustomizedProduct> baseCustomizedProducts = PersistenceContext.repositories().
                createCustomizedProductRepository().findBaseCustomizedProducts();

            if (!baseCustomizedProducts.Any()) {
                throw new ResourceNotFoundException(ERROR_NO_CUSTOMIZED_PRODUCTS_FOUND);
            }

            return CustomizedProductModelViewService.fromCollection(baseCustomizedProducts);
        }


        /// <summary>
        /// Retrieves all the instances of CustomizedProduct created by a given user.
        /// </summary>
        /// <returns>An instance of GetAllCustomizedProductsModelView representing the CustomizedProducts created by the user.</returns>
        /// <exceptions cref="ResourceNotFoundException">Thrown when no instance of CustomizeProduct was found.</exception>
        public GetAllCustomizedProductsModelView findUserCreatedCustomizedProducts(FindUserCreatedCustomizedProductsModelView findUserCreatedCustomizedProductsModelView)
        {
            IEnumerable<CustomizedProduct> userCreatedCustomizedProducts =
                PersistenceContext.repositories().createCustomizedProductRepository()
                    .findUserCreatedCustomizedProducts(findUserCreatedCustomizedProductsModelView.userAuthToken);

            if (!userCreatedCustomizedProducts.Any())
            {
                throw new ResourceNotFoundException(ERROR_NO_CUSTOMIZED_PRODUCTS_FOUND);
            }

            return CustomizedProductModelViewService.fromCollection(userCreatedCustomizedProducts);
        }


        /// <summary>
        /// Retrieves an instance of CustomizedProduct.
        /// </summary>
        /// <param name="findCustomizedProductModelView">Instance of FindCustomizedProductModelView containing the CustomizedProduct's identifier.</param>
        /// <returns>Instance of GetCustomizedProductModelView containing CustomizedProduct information.</returns>
        /// <exception cref="ResourceNotFoundException">Thrown when no CustomizedProduct could be found with the given identifier.</exception>
        public GetCustomizedProductModelView findCustomizedProduct(FindCustomizedProductModelView findCustomizedProductModelView) {
            CustomizedProductRepository customizedProductRepository = PersistenceContext.repositories().createCustomizedProductRepository();

            CustomizedProduct customizedProduct = customizedProductRepository.find(findCustomizedProductModelView.customizedProductId);

            if (customizedProduct == null) {
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_CUSTOMIZED_PRODUCT_BY_ID, findCustomizedProductModelView.customizedProductId));
            }

            return CustomizedProductModelViewService.fromEntity(customizedProduct, findCustomizedProductModelView.options.unit);
        }

        /// <summary>
        /// Gets recommended slots from a certain customized product
        /// </summary>
        /// <param name="findCustomizedProductModelView">Instance of FindCustomizedProductModelView.</param>
        /// <exception cref="ResourceNotFoundException">Thrown when no CustomizedProduct could be found with the given identifier.</exception>
        /// <returns>Instance of GetAllCustomizedDimensions representing the recommended Slots.</returns>
        public GetAllCustomizedDimensionsModelView getRecommendedSlots(FindCustomizedProductModelView findCustomizedProductModelView) {
            CustomizedProductRepository customizedProductRepository = PersistenceContext.repositories().createCustomizedProductRepository();
            CustomizedProduct customizedProduct = customizedProductRepository.find(findCustomizedProductModelView.customizedProductId);

            if (customizedProduct == null) {
                throw new ResourceNotFoundException(
                    string.Format(ERROR_UNABLE_TO_FIND_CUSTOMIZED_PRODUCT_BY_ID, findCustomizedProductModelView.customizedProductId)
                );
            }

            List<CustomizedDimensions> customizedDimensions = customizedProduct.recommendedSlots();
            return CustomizedDimensionsModelViewService.fromCollection(customizedDimensions, findCustomizedProductModelView.options.unit);
        }

        /// <summary>
        /// Gets min slots from a certain customized product
        /// </summary>
        /// <param name="findCustomizedProductModelView">Instance of FindCustomizedProductModelView.</param>
        /// <exception cref="ResourceNotFoundException">Thrown when no CustomizedProduct could be found with the given identifier.</exception>
        /// <returns>Instance of GetAllCustomizedDimensionsModelView representing the minimum Slots.</returns>
        public GetAllCustomizedDimensionsModelView getMinSlots(FindCustomizedProductModelView findCustomizedProductModelView) {
            CustomizedProductRepository customizedProductRepository = PersistenceContext.repositories().createCustomizedProductRepository();
            CustomizedProduct customizedProduct = customizedProductRepository.find(findCustomizedProductModelView.customizedProductId);

            if (customizedProduct == null) {
                throw new ResourceNotFoundException(
                    string.Format(ERROR_UNABLE_TO_FIND_CUSTOMIZED_PRODUCT_BY_ID, findCustomizedProductModelView.customizedProductId)
                );
            }

            List<CustomizedDimensions> customizedDimensions = customizedProduct.minSlots();
            return CustomizedDimensionsModelViewService.fromCollection(customizedDimensions, findCustomizedProductModelView.options.unit);
        }

        /// <summary>
        /// Retrieves an instance of Slot associated to an instance of CustomizedProduct.
        /// </summary>
        /// <param name="findSlotModelView">Instance of FindSlotModelView containing the CustomizedProduct's and the Slot's persistence identifiers.</param>
        /// <returns>Instance of GetSlotModelView containing Slot information.</returns>
        /// <exception cref="ResourceNotFoundException">Thrown when the either the CustomizedProduct or the Slot could not be found.</exception>
        public GetSlotModelView findSlot(FindSlotModelView findSlotModelView) {
            CustomizedProductRepository customizedProductRepository = PersistenceContext.repositories().createCustomizedProductRepository();

            CustomizedProduct customizedProduct = customizedProductRepository.find(findSlotModelView.customizedProductId);

            if (customizedProduct == null) {
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_CUSTOMIZED_PRODUCT_BY_ID, findSlotModelView.customizedProductId));
            }

            Slot slot = customizedProduct.slots.Where(s => s.Id == findSlotModelView.slotId).SingleOrDefault();

            if (slot == null) {
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_SLOT, findSlotModelView.slotId));
            }

            return SlotModelViewService.fromEntity(slot, findSlotModelView.options.unit);
        }

        /// <summary>
        /// Retrieves all possible components for a certain slot of a customized product
        /// </summary>
        /// <param name="customizedProductID">customized product to base restrictions on</param>
        /// <param name="slotID">selected slot</param>
        /// <returns>possible components for selected slot</returns>
        public GetPossibleComponentsModelView fetchPossibleComponents(FindPossibleComponentsModelView findComponentsMV) {
            CustomizedProductRepository customizedProductRepository = PersistenceContext.repositories().createCustomizedProductRepository();
            CustomizedProduct customizedProduct = customizedProductRepository.find(findComponentsMV.customizedProductID);
            if (customizedProduct == null) {
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_CUSTOMIZED_PRODUCT_BY_ID, findComponentsMV.customizedProductID));
            }

            Slot slot = customizedProduct.slots.Where(s => s.Id == findComponentsMV.slotID).SingleOrDefault();

            if (slot == null) {
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_SLOT, findComponentsMV.slotID));
            }
            List<Product> restrictedProducts = (List<Product>)customizedProduct.product.getRestrictedComponents(customizedProduct, slot);
            if (Collections.isEnumerableNullOrEmpty(restrictedProducts)) {
                throw new InvalidOperationException(NO_POSSIBLE_COMPONENTS);
            }
            return ProductModelViewService.possibleComponentsFromCollection(restrictedProducts);
        }

        /// <summary>
        /// Calculates the price of a customized product
        /// </summary>
        /// <param name="fetchCustomizedProductPrice">FetchCustomizedProductPriceModelView with necessary information to fetch the customized product's price</param>
        /// <returns>CustomizedProductPriceModelView with the customized product's price</returns>
        public async Task<CustomizedProductFinalPriceModelView> calculateCustomizedProductPrice(FetchCustomizedProductPriceModelView fetchCustomizedProductPrice, IHttpClientFactory clientFactory) {
            return await CustomizedProductPriceService.calculatePrice(fetchCustomizedProductPrice, clientFactory);
        }

        /// <summary>
        /// Adds an instance of CustomizedProduct.
        /// </summary>
        /// <param name="addCustomizedProductModelView">AddCustomizedProductModelView containing the CustomizedProduct's information.</param>
        /// <returns>GetCustomizedProductModelView with the added CustomizedProduct's information.</returns>
        public GetCustomizedProductModelView addCustomizedProduct(AddCustomizedProductModelView addCustomizedProductModelView) {
            CustomizedProduct customizedProduct = CreateCustomizedProductService.create(addCustomizedProductModelView);

            return CustomizedProductModelViewService.fromEntity(customizedProduct);
        }

        /// <summary>
        /// Adds a Slot to a CustomizedProduct.
        /// </summary>
        /// <param name="addSlotModelView">AddSlotModelView with the Slot's information</param>
        /// <returns>An instance of GetCustomizedProductModelView containing updated CustomizedProduct information.</returns>
        /// <exception cref="ResourceNotFoundException">Thrown when the CustomizedProduct could not be found.</exception>
        public GetCustomizedProductModelView addSlotToCustomizedProduct(AddSlotModelView addSlotModelView) {
            CustomizedProductRepository customizedProductRepository = PersistenceContext.repositories().createCustomizedProductRepository();

            CustomizedProduct customizedProduct = customizedProductRepository.find(addSlotModelView.customizedProductId);

            if (customizedProduct == null) {
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_CUSTOMIZED_PRODUCT_BY_ID, addSlotModelView.customizedProductId));
            }

            checkUserToken(customizedProduct, addSlotModelView.userAuthToken);

            CustomizedDimensions customizedDimensions = CustomizedDimensionsModelViewService.fromModelView(addSlotModelView.slotDimensions);

            customizedProduct.addSlot(customizedDimensions);

            customizedProduct = customizedProductRepository.update(customizedProduct);

            return CustomizedProductModelViewService.fromEntity(customizedProduct);
        }

        /// <summary>
        /// Adds the recommended slot layout to the CustomizedProduct with the given persistence identifier.
        /// </summary>
        /// <param name="addSlotLayoutModelView">Instance of AddSlotLayoutModelView.</param>
        /// <exception cref="ResourceNotFoundException">Thrown when no CustomizedProduct could be found with the given identifier.</exception>
        /// <returns>Instance of GetCustomizedProductModelView with the recommended Slot layout.</returns>
        public GetCustomizedProductModelView addRecommendedSlots(AddSlotLayoutModelView addSlotLayoutModelView)
        {
            CustomizedProductRepository customizedProductRepository = PersistenceContext.repositories().createCustomizedProductRepository();
            CustomizedProduct customizedProduct = customizedProductRepository.find(addSlotLayoutModelView.customizedProductId);

            if (customizedProduct == null) {
                throw new ResourceNotFoundException(
                    string.Format(ERROR_UNABLE_TO_FIND_CUSTOMIZED_PRODUCT_BY_ID, addSlotLayoutModelView.customizedProductId)
                );
            }

            checkUserToken(customizedProduct, addSlotLayoutModelView.userAuthToken);

            customizedProduct.addRecommendedSlots();
            customizedProduct = customizedProductRepository.update(customizedProduct);

            return CustomizedProductModelViewService.fromEntity(customizedProduct, addSlotLayoutModelView.options.unit);
        }

        /// <summary>
        /// Adds the minimum slot layout to the CustomizedProduct with the given persistence identifier.
        /// </summary>
        /// <param name="addSlotLayoutModelView">Instance of AddSlotLayoutModelView.</param>
        /// <exception cref="ResourceNotFoundException">Thrown when no CustomizedProduct could be found with the given identifier.</exception>
        /// <returns>Instance of GetCustomizedProductModelView with the recommended Slot layout.</returns>
        public GetCustomizedProductModelView addMinimumSlots(AddSlotLayoutModelView addSlotLayoutModelView)
        {
            CustomizedProductRepository customizedProductRepository = PersistenceContext.repositories().createCustomizedProductRepository();
            CustomizedProduct customizedProduct = customizedProductRepository.find(addSlotLayoutModelView.customizedProductId);

            if (customizedProduct == null) {
                throw new ResourceNotFoundException(
                    string.Format(ERROR_UNABLE_TO_FIND_CUSTOMIZED_PRODUCT_BY_ID, addSlotLayoutModelView.customizedProductId)
                );
            }

            checkUserToken(customizedProduct, addSlotLayoutModelView.userAuthToken);

            customizedProduct.addMinimumSlots();
            customizedProduct = customizedProductRepository.update(customizedProduct);

            return CustomizedProductModelViewService.fromEntity(customizedProduct, addSlotLayoutModelView.options.unit);
        }

        /// <summary>
        /// Updates an instance of CustomizedProduct with the data provided in the given UpdateCustomizedProductModelView.
        /// </summary>
        /// <param name="updateCustomizedProductModelView">Instance of UpdateCustomizedProductModelView containing updated CustomizedProduct information.</param>
        /// <returns>An instance of GetCustomizedProductModelView with the updated CustomizedProduct information.</returns>
        /// <exception cref="ResourceNotFoundException">Thrown when the CustomizedProduct could not be found.</exception>
        /// <exception cref="System.ArgumentException">Thrown when none of the CustomizedProduct's properties are updated.</exception>
        public GetCustomizedProductModelView updateCustomizedProduct(UpdateCustomizedProductModelView updateCustomizedProductModelView) {
            CustomizedProductRepository customizedProductRepository = PersistenceContext.repositories().createCustomizedProductRepository();

            CustomizedProduct customizedProduct = customizedProductRepository.find(updateCustomizedProductModelView.customizedProductId);

            if (customizedProduct == null) {
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_CUSTOMIZED_PRODUCT_BY_ID, updateCustomizedProductModelView.customizedProductId));
            }

            checkUserToken(customizedProduct, updateCustomizedProductModelView.userAuthToken);

            bool performedAtLeastOneUpdate = false;

            if (updateCustomizedProductModelView.reference != null) {
                customizedProduct.changeReference(updateCustomizedProductModelView.reference);
                performedAtLeastOneUpdate = true;
            }

            if (updateCustomizedProductModelView.designation != null) {
                customizedProduct.changeDesignation(updateCustomizedProductModelView.designation);
                performedAtLeastOneUpdate = true;
            }

            if (updateCustomizedProductModelView.customizedMaterial != null) {
                //TODO: check if only the finish or the color are being updated
                CustomizedMaterial customizedMaterial = CreateCustomizedMaterialService.create(updateCustomizedProductModelView.customizedMaterial);

                customizedProduct.changeCustomizedMaterial(customizedMaterial);
                performedAtLeastOneUpdate = true;
            }

            if (updateCustomizedProductModelView.customizedDimensions != null) {
                customizedProduct.changeDimensions(CustomizedDimensionsModelViewService.fromModelView(updateCustomizedProductModelView.customizedDimensions));
                performedAtLeastOneUpdate = true;
            }

            if (updateCustomizedProductModelView.customizationStatus == CustomizationStatus.FINISHED) {
                customizedProduct.finalizeCustomization();
                performedAtLeastOneUpdate = true;
            }

            if (!performedAtLeastOneUpdate) {
                throw new ArgumentException(ERROR_NO_UPDATE_PERFORMED);
            }

            customizedProduct = customizedProductRepository.update(customizedProduct);

            if (customizedProduct == null) {
                throw new ArgumentException(ERROR_UNABLE_TO_SAVE_CUSTOMIZED_PRODUCT);
            }

            return CustomizedProductModelViewService.fromEntity(customizedProduct);
        }

        /// <summary>
        /// Updates an instance of Slot.
        /// </summary>
        /// <param name="updateSlotModelView">Instance of UpdateSlotModelView.</param>
        /// <returns>Instance of GetCustomizedProductModelView with updated CustomizedProduct information.</returns>
        /// <exception cref="ResourceNotFoundException">Thrown when the CustomizedProduct or the Slot could not be found.</exception>
        /// <exception cref="System.ArgumentException">Thrown when none of the CustomizedProduct's properties are updated.</exception> 
        public GetCustomizedProductModelView updateSlot(UpdateSlotModelView updateSlotModelView) {
            CustomizedProductRepository customizedProductRepository = PersistenceContext.repositories().createCustomizedProductRepository();

            CustomizedProduct customizedProduct = customizedProductRepository.find(updateSlotModelView.customizedProductId);

            if (customizedProduct == null) {
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_CUSTOMIZED_PRODUCT_BY_ID, updateSlotModelView.customizedProductId));
            }

            checkUserToken(customizedProduct, updateSlotModelView.userAuthToken);

            Slot slot = customizedProduct.slots.Where(s => s.Id == updateSlotModelView.slotId).SingleOrDefault();

            if (slot == null) {
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_SLOT, updateSlotModelView.slotId));
            }

            bool performedAtLeastOneUpdate = false;

            if (updateSlotModelView.dimensions != null) {
                CustomizedDimensions updatedDimensions = CustomizedDimensionsModelViewService.fromModelView(updateSlotModelView.dimensions);

                customizedProduct.resizeSlot(slot, updatedDimensions);

                performedAtLeastOneUpdate = true;
            }

            if (!performedAtLeastOneUpdate) {
                throw new ArgumentException(ERROR_NO_UPDATE_PERFORMED);
            }

            customizedProduct = customizedProductRepository.update(customizedProduct);

            return CustomizedProductModelViewService.fromEntity(customizedProduct);
        }

        /// <summary>
        /// Deletes an instance of CustomizedProduct.
        /// </summary>
        /// <param name="deleteCustomizedProductModelView">Instance of DeleteCustomizedProductModelView containing the CustomizedProduct's identifier.</param>
        /// <exception cref="ResourceNotFoundException">Thrown when no CustomizedProduct could be found with the given identifier.</exception>
        public void deleteCustomizedProduct(DeleteCustomizedProductModelView deleteCustomizedProductModelView) {
            CustomizedProductRepository customizedProductRepository = PersistenceContext.repositories().createCustomizedProductRepository();

            CustomizedProduct customizedProduct = customizedProductRepository.find(deleteCustomizedProductModelView.customizedProductId);

            if (customizedProduct == null) {
                throw new ResourceNotFoundException(
                    string.Format(ERROR_UNABLE_TO_FIND_CUSTOMIZED_PRODUCT_BY_ID, deleteCustomizedProductModelView.customizedProductId)
                );
            }

            checkUserToken(customizedProduct, deleteCustomizedProductModelView.userAuthToken);

            //check if it's a sub customized product
            if (customizedProduct.insertedInSlotId.HasValue) {
                CustomizedProduct parent = customizedProductRepository.findCustomizedProductBySlot(customizedProduct.insertedInSlot);
                Slot insertedInSlot = customizedProduct.insertedInSlot;

                //remove the sub customized product and update parent
                parent.removeCustomizedProduct(customizedProduct, insertedInSlot);
                customizedProductRepository.update(parent);
            } else {
                //if it's a base product, just deactivate it
                customizedProductRepository.remove(customizedProduct);
            }
        }

        /// <summary>
        /// Deletes a slot from a CustomizedProduct.
        /// </summary>
        /// <param name="deleteSlotModelView">Instance of DeleteSlotModelView containing both the CustomizedProduct's and Slot's identifiers.</param>
        /// <exception cref="ResourceNotFoundException">Thrown when either no CustomizedProduct or Slot could be found with the given identifiers.</exception>
        public void deleteSlot(DeleteSlotModelView deleteSlotModelView) {
            CustomizedProductRepository customizedProductRepository = PersistenceContext.repositories().createCustomizedProductRepository();

            CustomizedProduct customizedProduct = customizedProductRepository.find(deleteSlotModelView.customizedProductId);

            if (customizedProduct == null) {
                throw new ResourceNotFoundException(
                    string.Format(ERROR_UNABLE_TO_FIND_CUSTOMIZED_PRODUCT_BY_ID, deleteSlotModelView.customizedProductId)
                );
            }

            checkUserToken(customizedProduct, deleteSlotModelView.userAuthToken);

            Slot slot = customizedProduct.slots.Where(s => s.Id == deleteSlotModelView.slotId).SingleOrDefault();

            if (slot == null) {
                throw new ResourceNotFoundException(string.Format(ERROR_UNABLE_TO_FIND_SLOT, deleteSlotModelView.slotId));
            }

            customizedProduct.removeSlot(slot);

            customizedProductRepository.update(customizedProduct);
        }

        /// <summary>
        /// Checks if the CustomizedProduct's user token matches the provided user token.
        /// </summary>
        /// <param name="customizedProduct">Instance of CustomizedProduct.</param>
        /// <param name="userToken">User's authentication token.</param>
        /// <exception cref="NotAuthorizedException">
        /// Thrown when the CustomizedProduct's user token is set and does not match the provided user token.
        /// </exception>
        private void checkUserToken(CustomizedProduct customizedProduct, string userToken)
        {
            if (customizedProduct.authToken != null && !customizedProduct.authToken.Equals(userToken))
            {
                throw new NotAuthorizedException(UNABLE_TO_ALTER_CUSTOMIZED_PRODUCT);
            }
        }
    }
}