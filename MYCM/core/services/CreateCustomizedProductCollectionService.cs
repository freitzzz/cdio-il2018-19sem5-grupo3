using System;
using System.Collections.Generic;
using core.domain;
using core.modelview.customizedproduct;
using core.modelview.customizedproductcollection;
using core.persistence;
using support.utils;

namespace core.services
{
    /// <summary>
    /// Service to create customized product collections
    /// </summary>
    public static class CreateCustomizedProductCollectionService
    {
        /// <summary>
        /// Constant representing the message that is presented if a customized product isn't found when creating a customized product collection
        /// </summary>
        private const string UNABLE_TO_FIND_CUSTOMIZED_PRODUCT = "Unable to find a customized product with the identifier of: {0}";

        /// <summary>
        /// Creates a customized product collection
        /// </summary>
        /// <param name="modelView"> model view with the information about the customized product collection to create</param>
        /// <returns>newly created customized product collection or throws exceptions if an error occurs</returns>
        public static CustomizedProductCollection create(AddCustomizedProductCollectionModelView modelView)
        {
            if (Collections.isEnumerableNullOrEmpty(modelView.customizedProducts))
            {
                CustomizedProductCollection customizedProductCollection =
                    new CustomizedProductCollection(modelView.name);

                return PersistenceContext.repositories()
                        .createCustomizedProductCollectionRepository()
                            .save(customizedProductCollection);
            }
            else
            {
                List<CustomizedProduct> customizedProducts =
                    new List<CustomizedProduct>();

                CustomizedProductRepository customizedProductRepository =
                    PersistenceContext.repositories()
                        .createCustomizedProductRepository();

                foreach (GetBasicCustomizedProductModelView customizedProductModelView in modelView.customizedProducts)
                {
                    CustomizedProduct customizedProduct =
                        customizedProductRepository.find(customizedProductModelView.customizedProductId);

                    if (customizedProduct == null)
                    {
                        throw new ArgumentException(
                            string.Format(UNABLE_TO_FIND_CUSTOMIZED_PRODUCT,
                                 customizedProductModelView.customizedProductId
                            ));
                    }

                    customizedProducts.Add(customizedProduct);
                }

                CustomizedProductCollection customizedProductCollection =
                    new CustomizedProductCollection(modelView.name, customizedProducts);

                return PersistenceContext.repositories()
                        .createCustomizedProductCollectionRepository()
                            .save(customizedProductCollection);
            }
        }
    }
}