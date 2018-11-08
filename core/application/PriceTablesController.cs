using core;
using core.domain;
using core.dto;
using System.Net.Http;
using System.Threading.Tasks;
using core.modelview.pricetableentries;
using core.modelview.pricetable;
using core.persistence;
using core.services;
using core.services.ensurance;
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
            FetchEnsurance.ensureMaterialPriceHistoryFetchWasSuccessful(materialPriceHistory);
            return PriceTableModelViewService.fromMaterialCollection(materialPriceHistory);
        }

        /// <summary>
        /// Fetches the price history of a material finish
        /// </summary>
        /// <param name="fetchMaterialFinishPriceHistoryDTO">FetchMaterialFinishPriceHistoryDTO with the information about the fetch</param>
        /// <returns>GetAllMaterialFinishPriceHistoryModelView with the material finish price history fetch information</returns>
        public GetAllMaterialFinishPriceHistoryModelView fetchMaterialFinishPriceHistory(FetchMaterialFinishPriceHistoryDTO fetchMaterialFinishPriceHistoryDTO){
            IEnumerable<FinishPriceTableEntry> materialFinishPriceHistory=PersistenceContext.repositories().createFinishPriceTableRepository().fetchMaterialFinishPriceHistory(fetchMaterialFinishPriceHistoryDTO);
            FetchEnsurance.ensureMaterialFinishPriceHistoryFetchWasSuccessful(materialFinishPriceHistory);
            return PriceTableModelViewService.fromMaterialFinishCollection(materialFinishPriceHistory);
        }

        /// <summary>
        /// Adds a new price table entry for a material
        /// </summary>
        /// <param name="modelView">model view with the price table entry's information</param>
        public async Task<AddPriceTableEntryModelView> addMaterialPriceTableEntry(AddPriceTableEntryModelView modelView, IHttpClientFactory clientFactory)
        {
            return await AddMaterialPriceTableEntryModelViewService.transform(modelView, clientFactory);
        }

        /// <summary>
        /// Adds new price table entry for a material's finish
        /// </summary>
        /// <param name="modelView">model view with the price table entry's information</param>
        public async Task<AddFinishPriceTableEntryModelView> addFinishPriceTableEntry(AddFinishPriceTableEntryModelView modelView, IHttpClientFactory clientFactory)
        {
            return await AddFinishPriceTableEntryModelViewService.transform(modelView, clientFactory);
        }

        /// <summary>
        /// Updates a material's price table entry
        /// </summary>
        /// <param name="modelView">model view with the necessary update information</param>
        public async Task<bool> updateMaterialPriceTableEntry(UpdatePriceTableEntryModelView modelView, IHttpClientFactory clientFactory)
        {
            return await UpdateMaterialPriceTableEntryModelViewService.update(modelView, clientFactory);
        }

        /// <summary>
        /// Updates a finish's price table entry
        /// </summary>
        /// <param name="modelView">model view with the necessary update information</param>
        public async Task<bool> updateFinishPriceTableEntry(UpdateFinishPriceTableEntryModelView modelView, IHttpClientFactory clientFactory)
        {
            return await UpdateFinishPriceTableEntryModelViewService.update(modelView, clientFactory);
        }
    }
}