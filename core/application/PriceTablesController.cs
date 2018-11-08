using core;
using core.domain;
using core.dto;
using core.modelview.pricetableentries;
using core.modelview.pricetable;
using core.persistence;
using core.services;
using System;
using System.Collections.Generic;

namespace core.application
{
    /// <summary>
    /// Application controller for Price Table Entries
    /// </summary>
    public class PriceTablesController
    {

        /// <summary>
        /// Fetches the price history of a material
        /// </summary>
        /// <param name="fetchMaterialFinishPriceHistoryDTO">FetchMaterialPriceHistoryDTO with the information about the fetch</param>
        /// <returns>GetAllMaterialPriceHistoryModelView with the material price history fetch information</returns>
        public GetAllMaterialPriceHistoryModelView fetchMaterialPriceHistory(FetchMaterialPriceHistoryDTO fetchMaterialPriceHistoryDTO){
            IEnumerable<MaterialPriceTableEntry> materialPriceHistory=PersistenceContext.repositories().createMaterialPriceTableRepository().fetchMaterialPriceHistory(fetchMaterialPriceHistoryDTO);
            //TODO: ENSURE PRICE HISTORY WAS FETCHED WITH SUCCESS
            return PriceTableModelViewService.fromMaterialCollection(materialPriceHistory);
        }

        /// <summary>
        /// Fetches the price history of a material finish
        /// </summary>
        /// <param name="fetchMaterialFinishPriceHistoryDTO">FetchMaterialFinishPriceHistoryDTO with the information about the fetch</param>
        /// <returns>GetAllMaterialFinishPriceHistoryModelView with the material finish price history fetch information</returns>
        public GetAllMaterialFinishPriceHistoryModelView fetchMaterialFinishPriceHistory(FetchMaterialFinishPriceHistoryDTO fetchMaterialFinishPriceHistoryDTO){
            IEnumerable<FinishPriceTableEntry> materialFinishPriceHistory=PersistenceContext.repositories().createFinishPriceTableRepository().fetchMaterialFinishPriceHistory(fetchMaterialFinishPriceHistoryDTO);
            //TODO: ENSURE PRICE HISTORY WAS FETCHED WITH SUCCESS
            return PriceTableModelViewService.fromMaterialFinishCollection(materialFinishPriceHistory);
        }

        /// <summary>
        /// Adds a new price table entry for a material
        /// </summary>
        /// <param name="modelView">model view with the price table entry's information</param>
        public AddPriceTableEntryModelView addMaterialPriceTableEntry(AddPriceTableEntryModelView modelView)
        {
            return AddMaterialPriceTableEntryModelViewService.transform(modelView);
        }

        /// <summary>
        /// Adds new price table entry for a material's finish
        /// </summary>
        /// <param name="modelView">model view with the price table entry's information</param>
        public AddFinishPriceTableEntryModelView addFinishPriceTableEntry(AddFinishPriceTableEntryModelView modelView)
        {
            return AddFinishPriceTableEntryModelViewService.transform(modelView);
        }

        /// <summary>
        /// Updates a material's price table entry
        /// </summary>
        /// <param name="modelView">model view with the necessary update information</param>
        public bool updateMaterialPriceTableEntry(UpdatePriceTableEntryModelView modelView)
        {
            return UpdateMaterialPriceTableEntryModelViewService.update(modelView);
        }

        /// <summary>
        /// Updates a finish's price table entry
        /// </summary>
        /// <param name="modelView">model view with the necessary update information</param>
        public bool updateFinishPriceTableEntry(UpdateFinishPriceTableEntryModelView modelView)
        {
            return UpdateFinishPriceTableEntryModelViewService.update(modelView);
        }
    }
}