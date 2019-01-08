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
        /// Fetches the price history of all materials
        /// </summary>
        /// <returns>GetAllMaterialPriceHistoryModelView with the price history of all materials</returns>
        public GetAllMaterialPriceHistoryModelView fetchPriceHistoryOfAllMaterials(string currency, string area, IHttpClientFactory clientFactory)
        {
            IEnumerable<MaterialPriceTableEntry> materialPriceTableEntries = PersistenceContext.repositories().createMaterialPriceTableRepository().findAll();
            FetchEnsurance.ensureMaterialPriceHistoryFetchWasSuccessful(materialPriceTableEntries);
            if (currency != null && area != null)
            {
                CurrenciesService.checkCurrencySupport(currency);
                AreasService.checkAreaSupport(area);
                foreach (MaterialPriceTableEntry materialPriceTableEntry in materialPriceTableEntries)
                {
                    Task<double> convertedValueTask =
                        new CurrencyPerAreaConversionService(clientFactory)
                            .convertDefaultCurrencyPerAreaToCurrencyPerArea(materialPriceTableEntry.price.value, currency, area);
                    convertedValueTask.Wait();
                    materialPriceTableEntry.price.value = convertedValueTask.Result;
                }
                return PriceTableModelViewService.fromMaterialCollection(materialPriceTableEntries, currency, area);
            }
            return PriceTableModelViewService.fromMaterialCollection(materialPriceTableEntries, CurrencyPerAreaConversionService.getBaseCurrency(), CurrencyPerAreaConversionService.getBaseArea());
        }

        /// <summary>
        /// Fetches the price history of all finishes of a material
        /// </summary>
        /// <param name="materialFinishPriceHistoryDTO">FetchMaterialFinishPriceHistoryDTO containing the material's PID</param>
        /// <returns>GetAllMaterialFinishPriceHistoryModelView with the price history of all finishes of the material</returns>
        public GetAllMaterialFinishPriceHistoryModelView fetchPriceHistoryOfAllMaterialFinishes(FetchMaterialFinishPriceHistoryDTO materialFinishPriceHistoryDTO, IHttpClientFactory clientFactory)
        {
            IEnumerable<FinishPriceTableEntry> materialFinishPriceHistory = PersistenceContext.repositories().createFinishPriceTableRepository().fetchAllMaterialFinishesPriceHistory(materialFinishPriceHistoryDTO);
            FetchEnsurance.ensureMaterialFinishPriceHistoryFetchWasSuccessful(materialFinishPriceHistory);
            if (materialFinishPriceHistoryDTO.currency != null && materialFinishPriceHistoryDTO.area != null)
            {
                CurrenciesService.checkCurrencySupport(materialFinishPriceHistoryDTO.currency);
                AreasService.checkAreaSupport(materialFinishPriceHistoryDTO.area);
                foreach (FinishPriceTableEntry finishPriceTableEntry in materialFinishPriceHistory)
                {
                    Task<double> convertedValueTask =
                        new CurrencyPerAreaConversionService(clientFactory)
                            .convertDefaultCurrencyPerAreaToCurrencyPerArea(finishPriceTableEntry.price.value, materialFinishPriceHistoryDTO.currency, materialFinishPriceHistoryDTO.area);
                    convertedValueTask.Wait();
                    finishPriceTableEntry.price.value = convertedValueTask.Result;
                }
                return PriceTableModelViewService.fromMaterialFinishCollection(materialFinishPriceHistory, materialFinishPriceHistoryDTO.currency, materialFinishPriceHistoryDTO.area);
            }
            return PriceTableModelViewService.fromMaterialFinishCollection(materialFinishPriceHistory, CurrencyPerAreaConversionService.getBaseCurrency(), CurrencyPerAreaConversionService.getBaseArea());
        }

        /// <summary>
        /// Fetches the price history of a material
        /// </summary>
        /// <param name="fetchMaterialFinishPriceHistoryDTO">FetchMaterialPriceHistoryDTO with the information about the fetch</param>
        /// <returns>GetAllMaterialPriceHistoryModelView with the material price history fetch information</returns>
        public GetAllMaterialPriceHistoryModelView fetchMaterialPriceHistory(FetchMaterialPriceHistoryDTO fetchMaterialPriceHistoryDTO, IHttpClientFactory clientFactory)
        {
            IEnumerable<MaterialPriceTableEntry> materialPriceHistory = PersistenceContext.repositories().createMaterialPriceTableRepository().fetchMaterialPriceHistory(fetchMaterialPriceHistoryDTO);
            FetchEnsurance.ensureMaterialPriceHistoryFetchWasSuccessful(materialPriceHistory);
            if (fetchMaterialPriceHistoryDTO.currency != null && fetchMaterialPriceHistoryDTO.area != null)
            {
                CurrenciesService.checkCurrencySupport(fetchMaterialPriceHistoryDTO.currency);
                AreasService.checkAreaSupport(fetchMaterialPriceHistoryDTO.area);
                foreach (MaterialPriceTableEntry materialPriceTableEntry in materialPriceHistory)
                {
                    Task<double> convertedValueTask =
                        new CurrencyPerAreaConversionService(clientFactory)
                            .convertDefaultCurrencyPerAreaToCurrencyPerArea(materialPriceTableEntry.price.value, fetchMaterialPriceHistoryDTO.currency, fetchMaterialPriceHistoryDTO.area);
                    convertedValueTask.Wait();
                    materialPriceTableEntry.price.value = convertedValueTask.Result;
                }
                return PriceTableModelViewService.fromMaterialCollection(materialPriceHistory, fetchMaterialPriceHistoryDTO.currency, fetchMaterialPriceHistoryDTO.area);
            }
            return PriceTableModelViewService.fromMaterialCollection(materialPriceHistory, CurrencyPerAreaConversionService.getBaseCurrency(), CurrencyPerAreaConversionService.getBaseArea());
        }

        /// <summary>
        /// Fetches the price history of a material finish
        /// </summary>
        /// <param name="fetchMaterialFinishPriceHistoryDTO">FetchMaterialFinishPriceHistoryDTO with the information about the fetch</param>
        /// <returns>GetAllMaterialFinishPriceHistoryModelView with the material finish price history fetch information</returns>
        public GetAllMaterialFinishPriceHistoryModelView fetchMaterialFinishPriceHistory(FetchMaterialFinishPriceHistoryDTO fetchMaterialFinishPriceHistoryDTO, IHttpClientFactory clientFactory)
        {
            IEnumerable<FinishPriceTableEntry> materialFinishPriceHistory = PersistenceContext.repositories().createFinishPriceTableRepository().fetchMaterialFinishPriceHistory(fetchMaterialFinishPriceHistoryDTO);
            FetchEnsurance.ensureMaterialFinishPriceHistoryFetchWasSuccessful(materialFinishPriceHistory);
            if (fetchMaterialFinishPriceHistoryDTO.currency != null && fetchMaterialFinishPriceHistoryDTO.area != null)
            {
                CurrenciesService.checkCurrencySupport(fetchMaterialFinishPriceHistoryDTO.currency);
                AreasService.checkAreaSupport(fetchMaterialFinishPriceHistoryDTO.area);
                foreach (FinishPriceTableEntry finishPriceTableEntry in materialFinishPriceHistory)
                {
                    Task<double> convertedValueTask =
                        new CurrencyPerAreaConversionService(clientFactory)
                            .convertDefaultCurrencyPerAreaToCurrencyPerArea(finishPriceTableEntry.price.value, fetchMaterialFinishPriceHistoryDTO.currency, fetchMaterialFinishPriceHistoryDTO.area);
                    convertedValueTask.Wait();
                    finishPriceTableEntry.price.value = convertedValueTask.Result;
                }
                return PriceTableModelViewService.fromMaterialFinishCollection(materialFinishPriceHistory, fetchMaterialFinishPriceHistoryDTO.currency, fetchMaterialFinishPriceHistoryDTO.area);
            }
            return PriceTableModelViewService.fromMaterialFinishCollection(materialFinishPriceHistory, CurrencyPerAreaConversionService.getBaseCurrency(), CurrencyPerAreaConversionService.getBaseArea());
        }

        /// <summary>
        /// Fetches the current price of a material
        /// </summary>
        /// <param name="modelView">GetCurrentMaterialPriceModelView with info to fetch the current price</param>
        /// <returns>GetCurrentMaterialPriceModelView with the material's current price</returns>
        public GetCurrentMaterialPriceModelView fetchCurrentMaterialPrice(GetCurrentMaterialPriceModelView modelView, IHttpClientFactory clientFactory)
        {
            return CurrentPriceService.fromMaterial(modelView, clientFactory);
        }

        /// <summary>
        /// Fetches the current price of a material finish
        /// </summary>
        /// <param name="modelView">GetCurrentMaterialFinishPriceModelView with info to fetch the current price</param>
        /// <returns>GetCurrentMaterialFinishPriceModelView with the material's finish current price</returns>
        public GetCurrentMaterialFinishPriceModelView fetchCurrentMaterialFinishPrice(GetCurrentMaterialFinishPriceModelView modelView, IHttpClientFactory clientFactory)
        {
            return CurrentPriceService.fromMaterialFinish(modelView, clientFactory);
        }

        /// <summary>
        /// Adds a new price table entry for a material
        /// </summary>
        /// <param name="modelView">model view with the price table entry's information</param>
        public GetMaterialPriceModelView addMaterialPriceTableEntry(AddPriceTableEntryModelView modelView, IHttpClientFactory clientFactory)
        {
            return AddMaterialPriceTableEntryService.create(modelView, clientFactory);
        }

        /// <summary>
        /// Adds new price table entry for a material's finish
        /// </summary>
        /// <param name="modelView">model view with the price table entry's information</param>
        public GetMaterialFinishPriceModelView addFinishPriceTableEntry(AddFinishPriceTableEntryModelView modelView, IHttpClientFactory clientFactory)
        {
            return AddFinishPriceTableEntryService.create(modelView, clientFactory);
        }

        /// <summary>
        /// Updates a material's price table entry
        /// </summary>
        /// <param name="modelView">model view with the necessary update information</param>
        public GetMaterialPriceModelView updateMaterialPriceTableEntry(UpdatePriceTableEntryModelView modelView, IHttpClientFactory clientFactory)
        {
            return UpdateMaterialPriceTableEntryService.update(modelView, clientFactory);
        }

        /// <summary>
        /// Updates a finish's price table entry
        /// </summary>
        /// <param name="modelView">model view with the necessary update information</param>
        public GetMaterialFinishPriceModelView updateFinishPriceTableEntry(UpdateFinishPriceTableEntryModelView modelView, IHttpClientFactory clientFactory)
        {
            return UpdateFinishPriceTableEntryService.update(modelView, clientFactory);
        }
    }
}