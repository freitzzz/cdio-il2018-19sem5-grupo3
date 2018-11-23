using core.domain;
using core.dto;
using core.modelview.customizedproduct;
using core.persistence;
using core.services;
using support.dto;
using support.utils;
using System;
using System.Collections.Generic;

namespace core.application
{
    /// <summary>
    /// Application controller for customized products
    /// </summary>
    public sealed class CustomizedProductController
    {

        /// <summary>
        /// Constant that represents the message that occurs when a customized product that already exists in the database is saved again
        /// </summary>
        private const string INVALID_CUSTOMIZED_PRODUCT_CREATION = "This customized product already exists";

        /// Fetches all available customized products
        /// </summary>        
        /// <summary>
        /// <returns>List with all available customized products</returns>
        public GetAllCustomizedProductsModelView findAllCustomizedProducts()
        {
            return GetAllCustomizedProductsModelView.fromEntities(
                PersistenceContext.repositories().
                createCustomizedProductRepository().
                findAllCustomizedProducts()
            );
        }

        /// <summary>
        /// Fetches a customized product by its id
        /// </summary>
        /// <param name="customizedProductModelView">CustomizedProductDTO with the customized product id information</param>
        /// <returns>CustomizedProductDTO with the fetched customized product information</returns>
        public GetCustomizedProductByIdModelView findCustomizedProductByID(GetCustomizedProductByIdModelView customizedProductModelView)
        {
            return GetCustomizedProductByIdModelViewService.transform(customizedProductModelView);
        }

        /// <summary>
        /// Adds a new customized product
        /// </summary>
        /// <param name="customizedProductModelView">CustomizedProductDTO with the customized product information</param>
        /// <returns>CustomizedProductDTO with the created customized product information</returns>
        public GetCustomizedProductByIdModelView addCustomizedProduct(PostCustomizedProductModelView customizedProductModelView)
        {
            CustomizedProduct customizedProduct = PostCustomizedProductModelViewService.transform(customizedProductModelView);
            CustomizedProduct savedCustomizedProduct = PersistenceContext.repositories().createCustomizedProductRepository().save(customizedProduct);
            return
                savedCustomizedProduct == null ? throw new InvalidOperationException(INVALID_CUSTOMIZED_PRODUCT_CREATION)
                        : GetCustomizedProductByIdModelViewService.transform(savedCustomizedProduct);
        }

        /// <summary>
        /// Adds a new customized product to a slot of another customized product
        /// </summary>
        /// <param name="addCustomizedProductToSlotModelView">ModelView representing the customized product to be added to a slot</param>
        /// <returns>ModelView representing the newly created customized product</returns>
        public PostCustomizedProductModelView addCustomizedProductToSlot(PostCustomizedProductToSlotModelView addCustomizedProductToSlotModelView)
        {
            CustomizedProduct customizedProduct = PostCustomizedProductToSlotModelViewService.transform(addCustomizedProductToSlotModelView);
            CustomizedProductRepository repo = PersistenceContext
                .repositories()
                    .createCustomizedProductRepository();
            CustomizedProduct savedCustomizedProduct = repo.save(customizedProduct);
            CustomizedProduct customizedProductFather = repo.find(addCustomizedProductToSlotModelView.baseId);
            repo.update(customizedProductFather);
            return
                savedCustomizedProduct == null ? throw new InvalidOperationException(INVALID_CUSTOMIZED_PRODUCT_CREATION)
                        : PostCustomizedProductModelViewService.transform(savedCustomizedProduct);
        }

        /// <summary>
        /// Updates a customized product
        /// </summary>
        /// <param name="updateCustomizedProductModelView">ModelView containing the update info</param>
        /// <returns>true if the customized product was updated successfully, false if otherwise</returns>
        public bool updateCustomizedProduct(UpdateCustomizedProductModelView updateCustomizedProductModelView)
        {
            return UpdateCustomizedProductModelViewService.update(updateCustomizedProductModelView);
        }

        /// <summary>
        /// Hard deletes a slot from a customized product
        /// </summary>
        /// <param name="deleteSlotFromCustomizedProductModelView">ModelView with necessary information to delete the slot</param>
        /// <returns>true if the slot was deleted from the customized product successfully, false if otherwise</returns>
        public bool deleteSlotFromCustomizedProduct(DeleteSlotFromCustomizedProductModelView deleteSlotFromCustomizedProductModelView)
        {
            return DeleteSlotFromCustomizedProductModelViewService.delete(deleteSlotFromCustomizedProductModelView);
        }

        /// <summary>
        /// Hard deletes a child customized product from a slot that belongs to the parent customized product
        /// </summary>
        /// <param name="deleteChildCustomizedProductModelView">ModelView with necessary information to delete the child customized product</param>
        /// <returns>true if the child customized product was deleted successfully, false if otherwise</returns>
        public bool deleteChildCustomizedProduct(DeleteChildCustomizedProductModelView deleteChildCustomizedProductModelView)
        {
            return DeleteChildCustomizedProductModelViewService.delete(deleteChildCustomizedProductModelView);
        }
    }
}