using System;
using core.domain;

namespace core.services.ensurance{
    /// <summary>
    /// Service class for ensuring that a variety of updates were succesful
    /// </summary>
    public sealed class UpdateEnsurance{

        /// <summary>
        /// Constant that represents the message that ocurrs if the product update wasn't successful
        /// </summary>
        private const string INVALID_PRODUCT_UPDATE="An error ocurrd while updating the product";

        /// <summary>
        /// Constant that represents the message that ocurrs if no updates were performed
        /// </summary>
        private const string NO_UPDATES_PERFORMED="No updated were performed";

        /// <summary>
        /// Ensures that the update of a product was successful
        /// </summary>
        /// <param name="product">Product with the updated product</param>
        public static void ensureProductUpdateWasSuccessful(Product product){
            if(product==null)
                throw new InvalidOperationException(INVALID_PRODUCT_UPDATE);
        }

        /// <summary>
        /// Ensures that at least one update was performed
        /// </summary>
        public static void ensureAtLeastOneUpdateWasPerformed(bool performedAtLeastOneUpdate){
            if(!performedAtLeastOneUpdate)
                throw new InvalidOperationException(NO_UPDATES_PERFORMED);
        }

    }
}