using core.modelview.pricetableentries;
using core.persistence;
using core.dto;
using core.domain;
using NodaTime;
using NodaTime.Text;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using core.modelview.pricetable;
using core.exceptions;
using System.Collections.Generic;

namespace core.services
{
    /// <summary>
    /// Service that helps transforming and persisting a Finish Price Table Entry
    /// </summary>
    public static class AddFinishPriceTableEntryService
    {
        /// <summary>
        /// Message that occurs if the requested material for the price table entry isn't found
        /// </summary>
        private const string MATERIAL_NOT_FOUND = "Requested material wasn't found";

        /// <summary>
        /// Message that occurs if the requested finish for the price table entry isn't found
        /// </summary>
        private const string FINISH_NOT_FOUND = "Requested finish wasn't found";

        /// <summary>
        /// Message that occurs if the requested material does not have a price
        /// </summary>
        private const string MATERIAL_HAS_NO_PRICE = "The requested material doesn't have any price. Please add one before inserting the prices of it's finishes";

        /// <summary>
        /// Message that occurs if one of the dates of the time period doesn't follow the General ISO format
        /// </summary>
        private const string DATES_WRONG_FORMAT = "Make sure all dates follow the General ISO Format: ";

        /// <summary>
        /// Message that occurs if the price table entry isn't created
        /// </summary>
        private const string PRICE_TABLE_ENTRY_NOT_CREATED = "A price table entry with the same values already exists for this finish. Please try again with different values";

        /// <summary>
        /// Transforms and creates a finish price table entry
        /// </summary>
        /// <param name="modelView">model view with the necessary info to create a finish price table entry</param>
        /// <param name="clientFactory">injected client factory</param>
        /// <returns></returns>
        public static GetMaterialFinishPriceModelView create(AddFinishPriceTableEntryModelView modelView, IHttpClientFactory clientFactory)
        {
            string defaultCurrency = CurrencyPerAreaConversionService.getBaseCurrency();
            string defaultArea = CurrencyPerAreaConversionService.getBaseArea();
            MaterialRepository materialRepository = PersistenceContext.repositories().createMaterialRepository();
            long materialId = modelView.entityId;

            Material material = materialRepository.find(materialId);

            if (material == null)
            {
                throw new ResourceNotFoundException(MATERIAL_NOT_FOUND);
            }

            //TODO Is this null check enough? Should we check ,if an entry exists, that the time period of the price entry is valid?
            MaterialPriceTableEntry materialPriceTableEntry = PersistenceContext.repositories().createMaterialPriceTableRepository().find(materialId);

            if (materialPriceTableEntry == null)
            {
                throw new InvalidOperationException(MATERIAL_HAS_NO_PRICE);
            }

            foreach (Finish finish in material.Finishes)
            {
                if (finish.Id == modelView.finishId)
                {
                    string startingDateAsString = modelView.priceTableEntry.startingDate;
                    string endingDateAsString = modelView.priceTableEntry.endingDate;

                    LocalDateTime startingDate;

                    try
                    {
                        startingDate = LocalDateTimePattern.GeneralIso.Parse(startingDateAsString).GetValueOrThrow();
                    }
                    catch (UnparsableValueException)
                    {
                        throw new UnparsableValueException(DATES_WRONG_FORMAT + LocalDateTimePattern.GeneralIso.PatternText);
                    }

                    TimePeriod timePeriod = null;

                    if (endingDateAsString != null)
                    {
                        try
                        {
                            timePeriod = TimePeriod.valueOf(startingDate, LocalDateTimePattern.GeneralIso.Parse(endingDateAsString).GetValueOrThrow());
                        }
                        catch (UnparsableValueException)
                        {
                            throw new UnparsableValueException(DATES_WRONG_FORMAT + LocalDateTimePattern.GeneralIso.PatternText);
                        }
                    }
                    else
                    {
                        timePeriod = TimePeriod.valueOf(startingDate);
                    }

                    CurrenciesService.checkCurrencySupport(modelView.priceTableEntry.price.currency);
                    AreasService.checkAreaSupport(modelView.priceTableEntry.price.area);

                    Price price = null;
                    try
                    {
                        if (defaultCurrency.Equals(modelView.priceTableEntry.price.currency) && defaultArea.Equals(modelView.priceTableEntry.price.area))
                        {
                            price = Price.valueOf(modelView.priceTableEntry.price.value);
                        }
                        else
                        {
                            Task<double> convertedValueTask = new CurrencyPerAreaConversionService(clientFactory)
                                                            .convertCurrencyPerAreaToDefaultCurrencyPerArea(
                                                                modelView.priceTableEntry.price.currency,
                                                                modelView.priceTableEntry.price.area,
                                                                modelView.priceTableEntry.price.value);
                            convertedValueTask.Wait();
                            double convertedValue = convertedValueTask.Result;
                            price = Price.valueOf(convertedValue);
                        }
                    }
                    catch (HttpRequestException)
                    {
                        price = Price.valueOf(modelView.priceTableEntry.price.value);
                    }

                    FinishPriceTableEntry finishPriceTableEntry = new FinishPriceTableEntry(material.id(), finish, price, timePeriod);
                    FinishPriceTableEntry savedFinishPriceTableEntry = PersistenceContext.repositories()
                                            .createFinishPriceTableRepository().save(finishPriceTableEntry);

                    if (savedFinishPriceTableEntry == null)
                    {
                        throw new InvalidOperationException(PRICE_TABLE_ENTRY_NOT_CREATED);
                    }

                    GetMaterialFinishPriceModelView createdPriceModelView = new GetMaterialFinishPriceModelView();

                    createdPriceModelView.startingDate = LocalDateTimePattern.GeneralIso.Format(savedFinishPriceTableEntry.timePeriod.startingDate);
                    createdPriceModelView.endingDate = LocalDateTimePattern.GeneralIso.Format(savedFinishPriceTableEntry.timePeriod.endingDate);
                    createdPriceModelView.value = savedFinishPriceTableEntry.price.value;
                    createdPriceModelView.currency = defaultCurrency;
                    createdPriceModelView.area = defaultArea;
                    createdPriceModelView.finishId = finish.Id;
                    createdPriceModelView.id = savedFinishPriceTableEntry.Id;

                    return createdPriceModelView;
                }
            }
            throw new ResourceNotFoundException(FINISH_NOT_FOUND);
        }
    }
}