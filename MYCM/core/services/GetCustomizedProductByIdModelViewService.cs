using System.Collections.Generic;
using System.Linq;
using core.domain;
using core.modelview.customizeddimensions;
using core.modelview.customizedmaterial;
using core.modelview.customizedproduct;
using core.persistence;
using support.utils;

namespace core.services
{
    /// <summary>
    /// Service that helps transforming GetCustomizedProductByIdModelViews into entities and vice versa
    /// </summary>
    public static class GetCustomizedProductByIdModelViewService
    {
        /// <summary>
        /// Transforms a model view into a CustomizedProduct entity and saves it to the database
        /// </summary>
        /// <param name="customizedProductModelView">model view to transform</param>
        public static GetCustomizedProductByIdModelView transform(GetCustomizedProductByIdModelView customizedProductModelView)
        {
            CustomizedProduct fetchedCustomizedProduct =
                             PersistenceContext.repositories().
                                createCustomizedProductRepository().
                                    find(customizedProductModelView.id);

            GetCustomizedProductByIdModelView fetchedCustomizedProductModelView = new GetCustomizedProductByIdModelView();
            fetchedCustomizedProductModelView.customizedMaterial = new GetCustomizedMaterialModelView();

            fetchedCustomizedProductModelView.id = fetchedCustomizedProduct.Id;
            fetchedCustomizedProductModelView.productId = fetchedCustomizedProduct.product.Id;
            fetchedCustomizedProductModelView.reference = fetchedCustomizedProduct.reference;
            fetchedCustomizedProductModelView.designation = fetchedCustomizedProduct.designation;

            fetchedCustomizedProductModelView.customizedMaterial.id = fetchedCustomizedProduct.customizedMaterial.Id;
            fetchedCustomizedProductModelView.customizedMaterial.materialId = fetchedCustomizedProduct.customizedMaterial.material.Id;
            fetchedCustomizedProductModelView.customizedMaterial.color = fetchedCustomizedProduct.customizedMaterial.color.toDTO();
            fetchedCustomizedProductModelView.customizedMaterial.finish = fetchedCustomizedProduct.customizedMaterial.finish.toDTO();
            fetchedCustomizedProductModelView.customizedDimensions = CustomizedDimensionsModelViewService.
                                                                        fromEntity(fetchedCustomizedProduct.customizedDimensions);
            fetchedCustomizedProductModelView.slots = new List<GetCustomizedProductByIdSlotModelView>();

            if (!Collections.isEnumerableNullOrEmpty(fetchedCustomizedProduct.slots))
            {
                foreach (Slot slot in fetchedCustomizedProduct.slots)
                {
                    GetCustomizedProductByIdSlotModelView slotModelView = new GetCustomizedProductByIdSlotModelView();
                    slotModelView.id = slot.Id;
                    slotModelView.slotDimensions = CustomizedDimensionsModelViewService.fromEntity(slot.slotDimensions);
                    slotModelView.customizedProducts = new List<BasicCustomizedProductModelView>();

                    if (!Collections.isEnumerableNullOrEmpty(slot.customizedProducts))
                    {
                        foreach (CustomizedProduct customizedProduct in slot.customizedProducts)
                        {
                            BasicCustomizedProductModelView childCustomizedProductModelView = new BasicCustomizedProductModelView();
                            childCustomizedProductModelView.id = customizedProduct.Id;
                            childCustomizedProductModelView.reference = customizedProduct.reference;
                            childCustomizedProductModelView.designation = customizedProduct.designation;
                            childCustomizedProductModelView.productId = customizedProduct.product.Id;
                            slotModelView.customizedProducts.Add(childCustomizedProductModelView);
                        }
                    }
                    fetchedCustomizedProductModelView.slots.Add(slotModelView);
                }
            }
            return fetchedCustomizedProductModelView;
        }

        /// <summary>
        /// Transforms a CustomizedProduct entity into a ModelView
        /// </summary>
        /// <param name="customizedProduct">customized product to transform</param>
        public static GetCustomizedProductByIdModelView transform(CustomizedProduct customizedProduct)
        {
            GetCustomizedProductByIdModelView fetchedCustomizedProductModelView = new GetCustomizedProductByIdModelView();
            fetchedCustomizedProductModelView.customizedMaterial = new GetCustomizedMaterialModelView();

            fetchedCustomizedProductModelView.id = customizedProduct.Id;
            fetchedCustomizedProductModelView.productId = customizedProduct.product.Id;
            fetchedCustomizedProductModelView.reference = customizedProduct.reference;
            fetchedCustomizedProductModelView.designation = customizedProduct.designation;

            fetchedCustomizedProductModelView.customizedMaterial.id = customizedProduct.customizedMaterial.Id;
            fetchedCustomizedProductModelView.customizedMaterial.materialId = customizedProduct.customizedMaterial.material.Id;
            fetchedCustomizedProductModelView.customizedMaterial.color = customizedProduct.customizedMaterial.color.toDTO();
            fetchedCustomizedProductModelView.customizedMaterial.finish = customizedProduct.customizedMaterial.finish.toDTO();
            fetchedCustomizedProductModelView.customizedDimensions = CustomizedDimensionsModelViewService.
                                                                        fromEntity(customizedProduct.customizedDimensions);
            fetchedCustomizedProductModelView.slots = new List<GetCustomizedProductByIdSlotModelView>();

            if (!Collections.isEnumerableNullOrEmpty(customizedProduct.slots))
            {
                foreach (Slot slot in customizedProduct.slots.ToList())
                {
                    GetCustomizedProductByIdSlotModelView slotModelView = new GetCustomizedProductByIdSlotModelView();
                    slotModelView.id = slot.Id;
                    slotModelView.slotDimensions = CustomizedDimensionsModelViewService.fromEntity(slot.slotDimensions);
                    slotModelView.customizedProducts = new List<BasicCustomizedProductModelView>();

                    if (!Collections.isEnumerableNullOrEmpty(slot.customizedProducts))
                    {
                        foreach (CustomizedProduct childCustomizedProduct in slot.customizedProducts.ToList())
                        {
                            BasicCustomizedProductModelView childCustomizedProductModelView = new BasicCustomizedProductModelView();
                            childCustomizedProductModelView.id = childCustomizedProduct.Id;
                            childCustomizedProductModelView.reference = childCustomizedProduct.reference;
                            childCustomizedProductModelView.designation = childCustomizedProduct.designation;
                            childCustomizedProductModelView.productId = childCustomizedProduct.product.Id;
                        }
                    }
                    fetchedCustomizedProductModelView.slots.Add(slotModelView);
                }
            }
            return fetchedCustomizedProductModelView;
        }
    }
}