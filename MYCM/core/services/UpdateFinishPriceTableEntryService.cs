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
    /// Service to help update a finish' price table entry
    /// </summary>
    public static class UpdateFinishPriceTableEntryService
    {
        /// <summary>
        /// Message that occurs if the requested material for the price table entry isn't found
        /// </summary>
        private const string MATERIAL_NOT_FOUND = "Requested material wasn't found";

        /// <summary>
        /// Message that occurs if the request finish for the price table entry isn't found or if the finish doesn't belong to the requested material
        /// </summary>
        private const string FINISH_NOT_FOUND_OR_DOESNT_BELONG_TO_MATERIAL = "Requested finish wasn't found or doesn't belong to the requested material";

        /// <summary>
        /// Message that occurs if the requested price table entry to update isn't found
        /// </summary>
        private const string ENTRY_NOT_FOUND = "Requested price table entry wasn't found";

        /// <summary>
        /// Message that occurs if the requested price table entry doesn't belong to the requested finish
        /// </summary>
        private const string ENTRY_DOESNT_BELONG_TO_FINISH = "Requested price table entry doesn't belong to the requested finish";

        /// <summary>
        /// Message that occurs if the request price table entry to update isn't updated successfully
        /// </summary>
        private const string UPDATE_NOT_SUCCESSFUL = "Update of the price table entry wasn't successful";

        /// <summary>
        /// Message that occurs if one of the dates of the time period doesn't follow the General ISO format
        /// </summary>
        private const string DATES_WRONG_FORMAT = "Make sure all dates follow the General ISO Format: ";

        /// <summary>
        /// Updates a finish's price table entry
        /// </summary>
        /// <param name="modelView">model view containing updatable information</param>
        /// <returns>True if the update is successful</returns>
        public static async Task<GetMaterialFinishPriceModelView> update(UpdateFinishPriceTableEntryModelView modelView, IHttpClientFactory clientFactory)
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

            foreach (Finish finish in material.Finishes)
            {
                if (finish.Id == modelView.finishId)
                {
                    FinishPriceTableRepository finishPriceTableRepository = PersistenceContext.repositories().createFinishPriceTableRepository();
                    long finishPriceTableEntryId = modelView.tableEntryId;

                    FinishPriceTableEntry tableEntryToUpdate = finishPriceTableRepository.find(finishPriceTableEntryId);

                    if (tableEntryToUpdate == null)
                    {
                        throw new ResourceNotFoundException(ENTRY_NOT_FOUND);
                    }

                    if (tableEntryToUpdate.entity.Id != modelView.finishId)
                    {
                        throw new InvalidOperationException(ENTRY_DOESNT_BELONG_TO_FINISH);
                    }

                    if (modelView.priceTableEntry.price != null)
                    {

                        CurrenciesService.checkCurrencySupport(modelView.priceTableEntry.price.currency);
                        AreasService.checkAreaSupport(modelView.priceTableEntry.price.area);

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
                                                                .convertCurrencyPerAreaToDefaultCurrencyPerArea(
                                                                    modelView.priceTableEntry.price.currency,
                                                                    modelView.priceTableEntry.price.area,
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
                        FinishPriceTableEntry updatedTableEntry = finishPriceTableRepository.update(tableEntryToUpdate);

                        if (updatedTableEntry == null)
                        {
                            throw new InvalidOperationException(UPDATE_NOT_SUCCESSFUL);
                        }

                        GetMaterialFinishPriceModelView updatedTableEntryModelView = new GetMaterialFinishPriceModelView();

                        updatedTableEntryModelView.id = updatedTableEntry.Id;
                        updatedTableEntryModelView.finishId = updatedTableEntry.entity.Id;
                        updatedTableEntryModelView.value = updatedTableEntry.price.value;
                        updatedTableEntryModelView.currency = defaultCurrency;
                        updatedTableEntryModelView.area = defaultArea;
                        updatedTableEntryModelView.startingDate = LocalDateTimePattern.GeneralIso.Format(updatedTableEntry.timePeriod.startingDate);
                        updatedTableEntryModelView.endingDate = LocalDateTimePattern.GeneralIso.Format(updatedTableEntry.timePeriod.endingDate);

                        return updatedTableEntryModelView;
                    }
                }
            }
            throw new ResourceNotFoundException(FINISH_NOT_FOUND_OR_DOESNT_BELONG_TO_MATERIAL);
        }
    }
}