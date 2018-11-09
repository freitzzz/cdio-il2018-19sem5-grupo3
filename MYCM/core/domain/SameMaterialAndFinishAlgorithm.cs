using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace core.domain {
    /// <summary>
    /// Class that restricts the material and finish of a component to the same as its father product
    /// </summary>
    public class SameMaterialAndFinishAlgorithm : Algorithm {
        /// <summary>
        /// Restricts the product received by parameter to have the same material and finish as the customized product received
        /// </summary>
        /// <param name="customProduct">customized product</param>
        /// <param name="product">product</param>
        /// <returns>Product with only one material and one finish, null if Product did not contain the material or the finish</returns>
        public Product apply(CustomizedProduct customProduct, Product product) {
            Finish restrictedFinish = customProduct.customizedMaterial.finish;
            Material restrictedMaterial = customProduct.customizedMaterial.material;
            List<ProductMaterial> productMaterialsToRemove = new List<ProductMaterial>();
            if (product.containsMaterial(restrictedMaterial)) {
                foreach (ProductMaterial prodMat in product.productMaterials) {
                    if (!prodMat.hasMaterial(restrictedMaterial)) {
                        productMaterialsToRemove.Add(prodMat);
                    }
                }
                foreach (ProductMaterial prodMat in productMaterialsToRemove) {
                    product.removeMaterial(prodMat.material);
                }
            } else {
                return null;
            }
            List<Finish> finishesToRemove = new List<Finish>();
            foreach (Finish finish in product.productMaterials[0].material.Finishes) {
                if (!finish.Equals(restrictedFinish)) {
                    finishesToRemove.Add(finish);
                }
            }
            foreach (Finish finish in finishesToRemove) {
                product.productMaterials[0].material.removeFinish(finish);
            }
            if (product.productMaterials[0].material.Finishes.Count() == 0) {
                return null;
            }
            return product;
        }
        /// <summary>
        /// Returns the list of inputs needed (this algorithm does not need any input)
        /// </summary>
        /// <returns>null</returns>
        public List<Input> getRequiredInputs() {
            return new List<Input>();
        }
        /// <summary>
        /// Checks if Inputs are allowed (this algorithm does not need any input)
        /// </summary>
        /// <param name="inputs">list of inputs to check</param>
        /// <returns>true</returns>
        public bool isWithinDataRange(List<Input> inputs) {
            return true;
        }
        /// <summary>
        /// Sets the input values for the algorithm to work with (this algorithm does not need any input)
        /// </summary>
        /// <param name="inputs">list of inputs to set</param>
        /// <returns>true</returns>
        public bool setInputValues(List<Input> inputs) {
            return true;
        }
    }
}
