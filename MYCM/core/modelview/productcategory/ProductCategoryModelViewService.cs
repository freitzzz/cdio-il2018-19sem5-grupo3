using System.Collections.Generic;
using core.domain;

namespace core.modelview.productcategory
{
    /// <summary>
    /// Service class used for converting instances of ProductCategory to ModelViews.
    /// </summary>
    public static class ProductCategoryModelViewService
    {
        /// <summary>
        /// Builds an instance of GetBasicProductCategoryModelView from an instance of ProductCategory.
        /// </summary>
        /// <param name="productcategory">Instance of ProductCategory from which the ModelView will be built.</param>
        /// <returns>An instance of GetBasicProductCategoryModelView.</returns>
        public static GetBasicProductCategoryModelView fromEntityAsBasic(ProductCategory productCategory){
            GetBasicProductCategoryModelView basicProductCategoryModelView=new GetBasicProductCategoryModelView();
            basicProductCategoryModelView.id=productCategory.Id;
            basicProductCategoryModelView.name=productCategory.name;
            return basicProductCategoryModelView;
        }

        /// <summary>
        /// Builds an instance of GetProductCategoryModelView from an instance of ProductCategory.
        /// </summary>
        /// <param name="productcategory">Instance of ProductCategory from which the ModelView will be built.</param>
        /// <returns>An instance of GetProductCategoryModelView.</returns>
        public static GetProductCategoryModelView fromEntity(ProductCategory productcategory)
        {
            GetProductCategoryModelView modelView = new GetProductCategoryModelView();

            modelView.id = productcategory.Id;
            modelView.parentId = productcategory.parentId;
            modelView.name = productcategory.name;

            return modelView;
        }

        /// <summary>
        /// Builds a List of GetBasicProductCategoryModelView from an IEnumerable of ProductCategory.
        /// </summary>
        /// <param name="productCategories">IEnumerable of instances of ProductCategory from which the ModelView List will be built.</param>
        /// <returns>A List of GetBasicProductCategoryModelView.</returns>
        public static GetAllProductCategoriesModelView fromCollection(IEnumerable<ProductCategory> productCategories)
        {
            GetAllProductCategoriesModelView result = new GetAllProductCategoriesModelView();

            foreach (ProductCategory productCategory in productCategories)
            {
                GetBasicProductCategoryModelView basicInfoModelView = new GetBasicProductCategoryModelView();
                basicInfoModelView.id = productCategory.Id;
                basicInfoModelView.name = productCategory.name;
                basicInfoModelView.parentId = productCategory.parentId;

                result.Add(basicInfoModelView);
            }

            return result;
        }
    }
}