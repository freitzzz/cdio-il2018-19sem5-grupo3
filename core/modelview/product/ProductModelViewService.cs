using core.domain;
using core.modelview.component;
using core.modelview.customizeddimensions;
using core.modelview.material;
using core.modelview.measurement;
using core.modelview.productcategory;
using core.modelview.slotdimensions;
using System.Collections.Generic;
using System.Linq;

namespace core.modelview.product{
    /// <summary>
    /// Service for creating model views based on certain product contexts
    /// </summary>
    public sealed class ProductModelViewService{
        /// <summary>
        /// Creates a model view with a product basic information
        /// </summary>
        /// <param name="product">Product with the product being created the model view</param>
        /// <returns>GetBasicProductModelView with the product basic information model view</returns>
        public static GetBasicProductModelView fromEntityAsBasic(Product product){
            GetBasicProductModelView basicProductModelView=new GetBasicProductModelView();
            basicProductModelView.id=product.Id;
            basicProductModelView.reference=product.reference;
            basicProductModelView.designation=product.designation;
            return basicProductModelView;
        }

        /// <summary>
        /// Creates a model view with a product information
        /// </summary>
        /// <param name="product">Product with the product being created the model view</param>
        /// <returns>GetProductModelView with the product information model view</returns>
        public static GetProductModelView fromEntity(Product product){
            GetProductModelView productModelView=new GetProductModelView();
            productModelView.id=product.Id;
            productModelView.reference=product.reference;
            productModelView.designation=product.designation;
            productModelView.category=ProductCategoryModelViewService.fromEntityAsBasic(product.productCategory);
            productModelView.components=ComponentModelViewService.fromCollection(product.complementedProducts);
            productModelView.materials=MaterialModelViewService.fromCollection(product.productMaterials.Select(pm => pm.material));
            productModelView.measurements=MeasurementModelViewService.fromCollection(product.measurements.Select(pm => pm.measurement)).ToList();
            productModelView.slotSizes = new GetSlotDimensionsModelView();
            productModelView.slotSizes.minSize = CustomizedDimensionsModelViewService.fromEntity(product.minSlotSize);
            productModelView.slotSizes.maxSize = CustomizedDimensionsModelViewService.fromEntity(product.maxSlotSize);
            productModelView.slotSizes.recommendedSize = CustomizedDimensionsModelViewService.fromEntity(product.recommendedSlotSize);
            return productModelView;
        }

        /// <summary>
        /// Creates a model view with the information about a collection of products
        /// </summary>
        /// <param name="products">IEnumerable with the collection of products</param>
        /// <returns>GetAllProductsModelView with the collection of products model view</returns>
        public static GetAllProductsModelView fromCollection(IEnumerable<Product> products){
            GetAllProductsModelView allProductsModelView=new GetAllProductsModelView();
            foreach(Product product in products)allProductsModelView.Add(fromEntityAsBasic(product));
            return allProductsModelView;
        }
    }
}