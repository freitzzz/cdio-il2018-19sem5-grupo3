using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace core.domain
{
    /// <summary>
    /// Class that restricts the material and finish of a component to the same as its father product
    /// </summary>
    public class SameMaterialAndFinishAlgorithm : Algorithm
    {

        /// <summary>
        /// Constant representing the SameMaterialAndFinishAlgorithm's name.
        /// </summary>
        private const string SAME_MATERIAL_AND_FINISH_ALGORITHM_NAME = "Same Material and Finish Algorithm";

        /// <summary>
        /// Constant representing the SameMaterialAndFinishAlgorithm's description.
        /// </summary>
        private const string SAME_MATERIAL_AND_FINISH_ALGORITHM_DESCRIPTION = "Limits the component's material and finish to the same material and finish as its father product.";

        protected SameMaterialAndFinishAlgorithm(ILazyLoader lazyLoader) : base(lazyLoader) { }

        public SameMaterialAndFinishAlgorithm()
        {
            checkName(SAME_MATERIAL_AND_FINISH_ALGORITHM_NAME);
            checkDescription(SAME_MATERIAL_AND_FINISH_ALGORITHM_DESCRIPTION);
            this.name = SAME_MATERIAL_AND_FINISH_ALGORITHM_NAME;
            this.description = SAME_MATERIAL_AND_FINISH_ALGORITHM_DESCRIPTION;
            this.restrictionAlgorithm = RestrictionAlgorithm.SAME_MATERIAL_AND_FINISH_ALGORITHM;
            this.inputValues = new List<InputValue>();
        }

        protected override void checkValue(Input input, string value)
        {
            //since this algorithm does not take in any inputs, simply do nothing
        }


        /// <summary>
        /// Restricts the product received by parameter to have the same material and finish as the customized product received
        /// </summary>
        /// <param name="customProduct">customized product</param>
        /// <param name="product">product</param>
        /// <returns>Product with only one material and one finish, null if Product did not contain the material or the finish</returns>
        public override Product apply(CustomizedProduct customProduct, Product product)
        {
            Finish restrictedFinish = customProduct.customizedMaterial.finish;
            Material restrictedMaterial = customProduct.customizedMaterial.material;
            List<ProductMaterial> productMaterialsToRemove = new List<ProductMaterial>();
            if (product.containsMaterial(restrictedMaterial))
            {
                foreach (ProductMaterial prodMat in product.productMaterials)
                {
                    if (!prodMat.hasMaterial(restrictedMaterial))
                    {
                        productMaterialsToRemove.Add(prodMat);
                    }
                }
                foreach (ProductMaterial prodMat in productMaterialsToRemove)
                {
                    product.removeMaterial(prodMat.material);
                }
            }
            else
            {
                return null;
            }
            List<Finish> finishesToRemove = new List<Finish>();
            foreach (Finish finish in product.productMaterials[0].material.Finishes)
            {
                if (!finish.Equals(restrictedFinish))
                {
                    finishesToRemove.Add(finish);
                }
            }
            foreach (Finish finish in finishesToRemove)
            {
                product.productMaterials[0].material.removeFinish(finish);
            }
            if (product.productMaterials[0].material.Finishes.Count() == 0)
            {
                return null;
            }
            return product;
        }
    }
}
