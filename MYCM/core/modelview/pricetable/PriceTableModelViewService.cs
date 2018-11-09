using core.domain;
using core.services;
using NodaTime.Text;
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
            GetMaterialPriceModelView getMaterialPriceModelView=new GetMaterialPriceModelView();
            getMaterialPriceModelView.id=materialPriceTableEntry.Id;
            getMaterialPriceModelView.value=materialPriceTableEntry.price.value;
            getMaterialPriceModelView.currency=CurrencyPerAreaConversionService.getBaseCurrency();
            getMaterialPriceModelView.area=CurrencyPerAreaConversionService.getBaseArea();
            getMaterialPriceModelView.startingDate=materialPriceTableEntry.timePeriod.startingDate.ToString();
            getMaterialPriceModelView.endingDate=materialPriceTableEntry.timePeriod.endingDate.ToString();
            return getMaterialPriceModelView;
        }

        /// <summary>
        /// Creates a model view with a material finish price information
        /// </summary>
        /// <param name="materialFinishPriceTableEntry">MaterialFinishPriceTableEntry with the material finish price</param>
        /// <returns>GetMaterialFinishPriceModelView with the material finish price information model view</returns>
        public static GetMaterialFinishPriceModelView fromMaterialFinishEntity(FinishPriceTableEntry materialFinishPriceTableEntry){
            GetMaterialFinishPriceModelView getMaterialFinishPriceModelView=new GetMaterialFinishPriceModelView();
            getMaterialFinishPriceModelView.id=materialFinishPriceTableEntry.Id;
            getMaterialFinishPriceModelView.value=materialFinishPriceTableEntry.price.value;
            getMaterialFinishPriceModelView.currency=CurrencyPerAreaConversionService.getBaseCurrency();
            getMaterialFinishPriceModelView.area=CurrencyPerAreaConversionService.getBaseArea();
            getMaterialFinishPriceModelView.startingDate=LocalDateTimePattern.GeneralIso.Format(materialFinishPriceTableEntry.timePeriod.startingDate);
            getMaterialFinishPriceModelView.endingDate=LocalDateTimePattern.GeneralIso.Format(materialFinishPriceTableEntry.timePeriod.endingDate);
            return getMaterialFinishPriceModelView;
        }

        /// <summary>
        /// Creates a model view with a collection of material prices information
        /// </summary>
        /// <param name="materialFinishPriceTableEntries">IEnumerable with the material prices</param>
        /// <returns>GetAllMaterialPriceHistoryModelView with the material price history information</returns>
        public static GetAllMaterialPriceHistoryModelView fromMaterialCollection(IEnumerable<MaterialPriceTableEntry> materialPriceTableEntries){
            GetAllMaterialPriceHistoryModelView getAllMaterialPrices=new GetAllMaterialPriceHistoryModelView();
            foreach(MaterialPriceTableEntry materialPriceTableEntry in materialPriceTableEntries)getAllMaterialPrices.Add(fromMaterialEntity(materialPriceTableEntry));
            return getAllMaterialPrices;
        }

        /// <summary>
        /// Creates a model view with a collection of material finish prices information
        /// </summary>
        /// <param name="materialFinishPriceTableEntries">IEnumerable with the material finish prices</param>
        /// <returns>GetAllMaterialFinishPriceHistoryModelView with the material finish price history information</returns>
        public static GetAllMaterialFinishPriceHistoryModelView fromMaterialFinishCollection(IEnumerable<FinishPriceTableEntry> materialFinishPriceTableEntries){
            GetAllMaterialFinishPriceHistoryModelView getAllMaterialFinishPrices=new GetAllMaterialFinishPriceHistoryModelView();
            foreach(FinishPriceTableEntry materialFinishPriceTableEntry in materialFinishPriceTableEntries)getAllMaterialFinishPrices.Add(fromMaterialFinishEntity(materialFinishPriceTableEntry));
            return getAllMaterialFinishPrices;
        }
    }
}