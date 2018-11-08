using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using core.domain;
using core.dto;
using core.modelview.pricetableentries;
using core.persistence;
using NodaTime;
using NodaTime.Text;

namespace core.services
{
    /// <summary>
    /// Service to help update a material's price table entry
    /// </summary>
    public static class UpdateMaterialPriceTableEntryModelViewService
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
        /// Represents abbreviation of EURO currency
        /// </summary>
        private const string EURO_CURRENCY_ABV = "EUR";

        /// <summary>
        /// Updates a material's price table entry
        /// </summary>
        /// <param name="modelView">model view with the update information</param>
        /// <returns></returns>
        public static async Task<bool> update(UpdatePriceTableEntryModelView modelView, IHttpClientFactory clientFactory)
        {
            MaterialRepository materialRepository = PersistenceContext.repositories().createMaterialRepository();
            long materialId = modelView.entityId;

            bool performedAtLeastOneUpdate = false;

            Material material = materialRepository.find(materialId);

            if (material == null)
            {
                throw new NullReferenceException(MATERIAL_NOT_FOUND);
            }

            MaterialPriceTableRepository materialPriceTableRepository = PersistenceContext.repositories().createMaterialPriceTableRepository();
            long materialPriceTableEntryId = modelView.tableEntryId;

            IEnumerable<MaterialPriceTableEntry> allEntries = materialPriceTableRepository.findAll();

            if (allEntries == null || !allEntries.Any())
            {
                throw new NullReferenceException(NO_ENTRIES_FOUND);
            }

            MaterialPriceTableEntry tableEntryToUpdate = materialPriceTableRepository.find(materialPriceTableEntryId);

            if (tableEntryToUpdate == null)
            {
                throw new NullReferenceException(ENTRY_NOT_FOUND);
            }

            //TODO Is this if statement correct?
            if (tableEntryToUpdate.entity.Id != modelView.entityId)
            {
                throw new InvalidOperationException(ENTRY_DOESNT_BELONG_TO_MATERIAL);
            }

            if (modelView.priceTableEntry.price != null)
            {
                //TODO Take area conversion into account
                //!For now we are considering all prices are in €/m2
                Price newPrice = null;
                if (!modelView.priceTableEntry.price.currency.Equals(EURO_CURRENCY_ABV))
                {
                    try
                    {
                        double convertedValue = await new CurrencyConversionService(clientFactory)
                                                            .convertCurrencyToEuro(modelView.priceTableEntry.price.currency,
                                                                 modelView.priceTableEntry.price.value);
                        newPrice = Price.valueOf(convertedValue);
                    }
                    catch (HttpRequestException)
                    {
                        newPrice = Price.valueOf(modelView.priceTableEntry.price.value);
                    }
                }
                tableEntryToUpdate.changePrice(newPrice);
                performedAtLeastOneUpdate = true;
            }

            //TODO check what's more efficient: always create 2 VOs or chain ifs to know what dates can be ignored
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


            MaterialPriceTableEntry updatedTableEntry = materialPriceTableRepository.update(tableEntryToUpdate);

            if (updatedTableEntry == null)
            {
                throw new InvalidOperationException(UPDATE_NOT_SUCCESSFUL);
            }

            return performedAtLeastOneUpdate;
        }
    }
}