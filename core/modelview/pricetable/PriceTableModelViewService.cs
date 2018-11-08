using core.domain;
using System;
using System.Collections.Generic;

namespace core.modelview.pricetable{
    /// <summary>
    /// Service for creating model views based on certain price table contexts
    /// </summary>
    public sealed class PriceTableModelViewService{

        /// <summary>
        /// Creates a model view with a material price information
        /// </summary>
        /// <param name="materialPriceTableEntry">MaterialPriceTableEntry with the material price</param>
        /// <returns>GetMaterialPriceModelView with the material price information model view</returns>
        public static GetMaterialPriceModelView fromMaterialEntity(MaterialPriceTableEntry materialPriceTableEntry){
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a model view with a material finish price information
        /// </summary>
        /// <param name="finishPriceTableEntry">MaterialFinishPriceTableEntry with the material finish price</param>
        /// <returns>GetMaterialFinishPriceModelView with the material finish price information model view</returns>
        public static GetMaterialPriceModelView fromMaterialFinishEntity(FinishPriceTableEntry finishPriceTableEntry){
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a model view with a collection of material prices information
        /// </summary>
        /// <param name="materialFinishPriceTableEntries">IEnumerable with the material prices</param>
        /// <returns>GetAllMaterialPriceHistoryModelView with the material price history information</returns>
        public static GetAllMaterialPriceHistoryModelView fromMaterialCollection(IEnumerable<MaterialPriceTableEntry> materialPriceTableEntries){
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a model view with a collection of material finish prices information
        /// </summary>
        /// <param name="materialFinishPriceTableEntries">IEnumerable with the material finish prices</param>
        /// <returns>GetAllMaterialFinishPriceHistoryModelView with the material finish price history information</returns>
        public static GetAllMaterialPriceHistoryModelView fromMaterialFinishCollection(IEnumerable<FinishPriceTableEntry> materialFinishPriceTableEntries){
            throw new NotImplementedException();
        }
    }
}