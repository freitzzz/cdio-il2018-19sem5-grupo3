using System;
using System.Collections.Generic;
using core.domain;
using core.modelview.customizedproduct;
using support.utils;

namespace core.modelview.customizedproductcollection
{
    /// <summary>
    /// Class representing the service for converting instances of CustomizedProductCollection into Model Views
    /// </summary>
    public static class CustomizedProductCollectionModelViewService
    {

        /// <summary>
        /// Constant that represents the message that occurs if a customized product collection is null
        /// </summary>
        private static string INVALID_CUSTOMIZED_PRODUCT_COLLECTION = "Null customized product collection";

        public static GetBasicCustomizedProductCollectionModelView fromEntityAsBasic(CustomizedProductCollection customizedProductCollection)
        {
            if (customizedProductCollection == null)
            {
                throw new ArgumentNullException(INVALID_CUSTOMIZED_PRODUCT_COLLECTION);
            }

            GetBasicCustomizedProductCollectionModelView basicModelView = new GetBasicCustomizedProductCollectionModelView();

            basicModelView.id = customizedProductCollection.Id;
            basicModelView.name = customizedProductCollection.name;

            return basicModelView;
        }

        public static GetCustomizedProductCollectionModelView fromEntity(CustomizedProductCollection customizedProductCollection)
        {
            if (customizedProductCollection == null)
            {
                throw new ArgumentNullException(INVALID_CUSTOMIZED_PRODUCT_COLLECTION);
            }

            GetCustomizedProductCollectionModelView customizedProductCollectionModelView = new GetCustomizedProductCollectionModelView();

            customizedProductCollectionModelView.id = customizedProductCollection.Id;
            customizedProductCollectionModelView.name = customizedProductCollection.name;

            if (!Collections.isEnumerableNullOrEmpty(customizedProductCollection.collectionProducts))
            {
                customizedProductCollectionModelView.customizedProducts = new List<GetBasicCustomizedProductModelView>();

                foreach (CollectionProduct collectionProduct in customizedProductCollection.collectionProducts)
                {
                    GetBasicCustomizedProductModelView basicCustomizedProductModelView = new GetBasicCustomizedProductModelView();

                    basicCustomizedProductModelView.customizedProductId = collectionProduct.customizedProduct.Id;
                    basicCustomizedProductModelView.designation = collectionProduct.customizedProduct.designation;
                    basicCustomizedProductModelView.reference = collectionProduct.customizedProduct.reference;
                    basicCustomizedProductModelView.serialNumber = collectionProduct.customizedProduct.serialNumber;
                    basicCustomizedProductModelView.productId = collectionProduct.customizedProduct.product.Id;

                    customizedProductCollectionModelView.customizedProducts.Add(basicCustomizedProductModelView);
                }
            }

            return customizedProductCollectionModelView;
        }

        public static GetAllCustomizedProductCollectionsModelView fromCollection(IEnumerable<CustomizedProductCollection> customizedProductCollections)
        {
            if (customizedProductCollections == null)
            {
                throw new ArgumentNullException(nameof(customizedProductCollections));
            }

            GetAllCustomizedProductCollectionsModelView customizedProductCollectionsModelView = new GetAllCustomizedProductCollectionsModelView();
            foreach (CustomizedProductCollection customizedProductCollection in customizedProductCollections)
            {
                customizedProductCollectionsModelView.Add(fromEntityAsBasic(customizedProductCollection));
            }

            return customizedProductCollectionsModelView;
        }

    }
}