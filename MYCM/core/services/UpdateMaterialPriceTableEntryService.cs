using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using core.domain;
using core.dto;
using core.exceptions;
using core.modelview.pricetable;
using core.modelview.pricetableentries;
using core.persistence;
using NodaTime;
using NodaTime.Text;

namespace core.services
{
    /// <summary>
    /// Service to help update a material's price table entry
    /// </summary>
    public static class UpdateMaterialPriceTableEntryService
    {
        /// <summary>
        /// Message that occurs if the requested material for the price table entry isn't found
        /// </summary>
        private const string MATERIAL_NOT_FOUND = "Requested material wasn't found";

        /// <summary>
        /// Message that occurs if the requested price table entry to update isn't found
        /// </summary>
        private const string ENTRY_NOT_FOUND = "Requested price table entry wasn't found";

        /// <summary>
        /// Message that occurs if the requested price table entry doesn't belong to the requested material
        /// </summary>
        private const string ENTRY_DOESNT_BELONG_TO_MATERIAL = "The requested price table entry does not belong to the requested material";

        /// <summary>
        /// Message that occurs if the request price table entry to update isn't updated successfully
        /// </summary>
        private const string UPDATE_NOT_SUCCESSFUL = "Update of the price table entry wasn't successful";

        /// <summary>
        /// Message the occurs if the request material doesn't have any price table entries
        /// </summary>
        private const string NO_ENTRIES_FOUND = "The inserted material has no price table entries yet";

        /// <summary>
        /// Message that occurs if one of the dates of the time period doesn't follow the General ISO format
        /// </summary>
        private const string DATES_WRONG_FORMAT = "Make sure all dates follow the General ISO Format: ";

        /// <summary>
        /// Message that occurs if the new currency isn't supported
        /// </summary>
        private const string UNSUPPORTED_CURRENCY = "The inserted currency is not being supported at the moment!";

        /// <summary>
        /// Message to help the user know which currencies to use
        /// </summary>
        private const string SUPPORTED_CURRENCIES_MESSAGE = "Please use one of the currencies that are currently being supported";

        /// <summary>
        /// Message that occurs if the new area isn't supported
        /// </summary>
        private const string UNSUPPORTED_AREA = "The inserted area is not being supported at the moment!";

        /// <summary>
        /// Message to help the user know which areas to use
        /// </summary>
        private const string SUPPORTED_AREAS_MESSAGE = "Please use one of the areas that are currently being supported";

        /// <summary>
        /// Updates a material's price table entry
        /// </summary>
        /// <param name="modelView">model view with the update information</param>
        /// <returns></returns>
        public static async Task<GetMaterialPriceModelView> update(UpdatePriceTableEntryModelView modelView, IHttpClientFactory clientFactory)
        {
            string defaultCurrency = CurrencyPerAreaConversionService.getBaseCurrency();
            string defaultArea = CurrencyPerAreaConversionService.getBaseArea();
            MaterialRepository materialRepository = PersistenceContext.repositories().createMaterialRepository();
            long materialId = modelView.entityId;

            bool performedAtLeastOneUpdate = false;

            Material material = materialRepository.find(materialId);

            if (material == null)
            {
                throw new ResourceNotFoundException(MATERIAL_NOT_FOUND);
            }

            MaterialPriceTableRepository materialPriceTableRepository = PersistenceContext.repositories().createMaterialPriceTableRepository();
            long materialPriceTableEntryId = modelView.tableEntryId;

            IEnumerable<MaterialPriceTableEntry> allEntries = materialPriceTableRepository.findAll();

            if (allEntries == null || !allEntries.Any())
            {
                throw new ResourceNotFoundException(NO_ENTRIES_FOUND);
            }

            MaterialPriceTableEntry tableEntryToUpdate = materialPriceTableRepository.find(materialPriceTableEntryId);

            if (tableEntryToUpdate == null)
            {
                throw new ResourceNotFoundException(ENTRY_NOT_FOUND);
            }

            if (tableEntryToUpdate.entity.Id != modelView.entityId)
            {
                throw new InvalidOperationException(ENTRY_DOESNT_BELONG_TO_MATERIAL);
            }

            if (modelView.priceTableEntry.price != null)
            {
                List<string> availableCurrencies = (List<string>)CurrenciesService.getAvailableCurrencies();
                List<string> availableAreas = (List<string>)AreasService.getAvailableAreas();

                if (!availableCurrencies.Contains(modelView.priceTableEntry.price.currency))
                {
                    throw new ArgumentException
                    (
                        string.Format
                        (
                            "{0} {1}: {2}",
                            UNSUPPORTED_CURRENCY, SUPPORTED_CURRENCIES_MESSAGE, string.Join(", ", availableCurrencies)
                        )
                    );
                }

                if (!availableAreas.Contains(modelView.priceTableEntry.price.area))
                {
                    throw new ArgumentException
                    (
                        string.Format
                        (
                         "{0} {1}: {2}",
                         UNSUPPORTED_AREA, SUPPORTED_AREAS_MESSAGE, string.Join(", ", availableAreas)
                        )
                    );
                }

                Price newPrice = null;
                try
                {
                    if (defaultCurrency.Equals(modelView.priceTableEntry.price.currency) && defaultArea.Equals(modelView.priceTableEntry.price.area))
                    {
                        newPrice = Price.valueOf(modelView.priceTableEntry.price.value);
                    }
                    else
                    {
                        double convertedValue = await new CurrencyPerAreaConversionService(clientFactory)
                                                        .convertCurrencyToDefaultCurrency(modelView.priceTableEntry.price.currency,
                                                             modelView.priceTableEntry.price.value);
                        newPrice = Price.valueOf(convertedValue);
                    }
                }
                catch (HttpRequestException)
                {
                    newPrice = Price.valueOf(modelView.priceTableEntry.price.value);
                }

                tableEntryToUpdate.changePrice(newPrice);
                performedAtLeastOneUpdate = true;
            }

            if (modelView.priceTableEntry.startingDate != null)
            {
                LocalDateTime newStartingDate;
                try
                {
                    string newStartingDateAsString = modelView.priceTableEntry.startingDate;
                    newStartingDate = LocalDateTimePattern.GeneralIso.Parse(newStartingDateAsString).GetValueOrThrow();
                    tableEntryToUpdate.changeTimePeriod(TimePeriod.valueOf(newStartingDate, tableEntryToUpdate.timePeriod.endingDate));
                    performedAtLeastOneUpdate = true;
                }
                catch (UnparsableValueException)
                {
                    throw new UnparsableValueException(DATES_WRONG_FORMAT + LocalDateTimePattern.GeneralIso.PatternText);
                }
            }

            if (modelView.priceTableEntry.endingDate != null)
            {
                LocalDateTime newEndingDate;
                try
                {
                    string newEndingDateAsString = modelView.priceTableEntry.endingDate;
                    newEndingDate = LocalDateTimePattern.GeneralIso.Parse(newEndingDateAsString).GetValueOrThrow();
                    tableEntryToUpdate.changeTimePeriod(TimePeriod.valueOf(tableEntryToUpdate.timePeriod.startingDate, newEndingDate));
                    performedAtLeastOneUpdate = true;
                }
                catch (UnparsableValueException)
                {
                    throw new UnparsableValueException(DATES_WRONG_FORMAT + LocalDateTimePattern.GeneralIso.PatternText);
                }
            }

            if (performedAtLeastOneUpdate)
            {
                MaterialPriceTableEntry updatedTableEntry = materialPriceTableRepository.update(tableEntryToUpdate);

                if (updatedTableEntry == null)
                {
                    throw new InvalidOperationException(UPDATE_NOT_SUCCESSFUL);
                }

                GetMaterialPriceModelView updatedTableEntryModelView = new GetMaterialPriceModelView();

                updatedTableEntryModelView.id = updatedTableEntry.Id;
                updatedTableEntryModelView.materialId = updatedTableEntry.entity.Id;
                updatedTableEntryModelView.value = updatedTableEntry.price.value;
                updatedTableEntryModelView.currency = defaultCurrency;
                updatedTableEntryModelView.area = defaultArea;
                updatedTableEntryModelView.startingDate = LocalDateTimePattern.GeneralIso.Format(updatedTableEntry.timePeriod.startingDate);
                updatedTableEntryModelView.endingDate = LocalDateTimePattern.GeneralIso.Format(updatedTableEntry.timePeriod.endingDate);

                return updatedTableEntryModelView;
            }

            throw new InvalidOperationException(UPDATE_NOT_SUCCESSFUL);
        }
    }
}