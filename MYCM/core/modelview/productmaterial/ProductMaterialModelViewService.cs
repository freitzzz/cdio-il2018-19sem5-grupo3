using System;
using System.Collections.Generic;
using System.Linq;
using core.domain;
using core.modelview.material;
using core.modelview.restriction;

namespace core.modelview.productmaterial
{
    public static class ProductMaterialModelViewService
    {
        /// <summary>
        /// Constant representing the message presented when the provided ProductMaterial is null.
        /// </summary>
        private const string ERROR_NULL_PRODUCT_MATERIAL = "The provided product material is invalid.";
        /// <summary>
        /// Constant representing the message presented when the provided collection of ProductMaterial is null.
        /// </summary>
        private const string ERROR_NULL_PRODUCT_MATERIAL_COLLECTION = "The provided product material collection is invalid.";

        /// <summary>
        /// Converts an instance of ProductMaterial into an instance of GetBasicProductMaterialModelView.
        /// </summary>
        /// <param name="productMaterial">Instance of ProductMaterial.</param>
        /// <returns>Instance of GetBasicProductMaterialModelView.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the provided instance of ProductMaterial is null.</exception>
        public static GetBasicProductMaterialModelView fromEntityAsBasic(ProductMaterial productMaterial)
        {
            if (productMaterial == null)
            {
                throw new ArgumentNullException(ERROR_NULL_PRODUCT_MATERIAL);
            }

            GetBasicProductMaterialModelView basicProductMaterialModelView = new GetBasicProductMaterialModelView();
            basicProductMaterialModelView.id = productMaterial.materialId;
            basicProductMaterialModelView.reference = productMaterial.material.reference;
            basicProductMaterialModelView.designation = productMaterial.material.designation;
            basicProductMaterialModelView.imageFilename = productMaterial.material.image;

            return basicProductMaterialModelView;
        }

        /// <summary>
        /// Converts an instance of ProductMaterial into an instance of GetProductMaterialModelView.
        /// </summary>
        /// <param name="productMaterial">Instance of ProductMaterial.</param>
        /// <returns>Instance of GetProductMaterialModelView.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the provided instance of ProductMaterial is null.</exception>
        public static GetProductMaterialModelView fromEntity(ProductMaterial productMaterial)
        {
            if (productMaterial == null)
            {
                throw new ArgumentNullException(ERROR_NULL_PRODUCT_MATERIAL);
            }

            GetProductMaterialModelView productMaterialModelView = new GetProductMaterialModelView();

            productMaterialModelView.id = productMaterial.materialId;
            productMaterialModelView.reference = productMaterial.material.reference;
            productMaterialModelView.designation = productMaterial.material.designation;
            productMaterialModelView.imageFilename = productMaterial.material.image;

            /*Skip converting Restrictions if the ProductMaterial has none,
            since null GetAllRestrictionsModelView won't be serialized */
            if (productMaterial.restrictions.Any())
            {
                productMaterialModelView.restrictions = RestrictionModelViewService.fromCollection(productMaterial.restrictions);
            }

            return productMaterialModelView;
        }


        /// <summary>
        /// Converts an IEnumerable of ProductMaterial into an instance of GetAllProductMaterialsModelView.
        /// </summary>
        /// <param name="productMaterials">IEnumerable of ProductMaterial.</param>
        /// <returns>Instance of GetAllProductMaterialsModelView.</returns>
        ///<exception cref="System.ArgumentException">Thrown when the provided IEnumerable is null.</exception>
        public static GetAllProductMaterialsModelView fromCollection(IEnumerable<ProductMaterial> productMaterials)
        {
            if (productMaterials == null)
            {
                throw new ArgumentNullException(ERROR_NULL_PRODUCT_MATERIAL_COLLECTION);
            }

            GetAllProductMaterialsModelView allProductMaterialsModelView = new GetAllProductMaterialsModelView();

            foreach (ProductMaterial productMaterial in productMaterials)
            {
                allProductMaterialsModelView.Add(fromEntityAsBasic(productMaterial));
            }

            return allProductMaterialsModelView;
        }
    }
}