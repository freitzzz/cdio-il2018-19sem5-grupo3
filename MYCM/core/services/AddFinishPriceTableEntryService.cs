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
        /// Transforms and creates a finish price table entry
        /// </summary>
        /// <param name="modelView">model view with the necessary info to create a finish price table entry</param>
        /// <param name="clientFactory">injected client factory</param>
        /// <returns></returns>
        public static async Task<GetMaterialFinishPriceModelView> create(AddFinishPriceTableEntryModelView modelView, IHttpClientFactory clientFactory)
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
                    LocalDateTime endingDate;

                    try
                    {
                        startingDate = LocalDateTimePattern.GeneralIso.Parse(startingDateAsString).GetValueOrThrow();
                        endingDate = LocalDateTimePattern.GeneralIso.Parse(endingDateAsString).GetValueOrThrow();
                    }
                    catch (UnparsableValueException)
                    {
                        throw new UnparsableValueException(DATES_WRONG_FORMAT + LocalDateTimePattern.GeneralIso.PatternText);
                    }

                    TimePeriod timePeriod = TimePeriod.valueOf(startingDate, endingDate);

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

                    Price price = null;
                    try
                    {
                        if (defaultCurrency.Equals(modelView.priceTableEntry.price.currency) && defaultArea.Equals(modelView.priceTableEntry.price.area))
                        {
                            price = Price.valueOf(modelView.priceTableEntry.price.value);
                        }
                        else
                        {
                            double convertedValue = await new CurrencyPerAreaConversionService(clientFactory)
                                                            .convertCurrencyToDefaultCurrency(modelView.priceTableEntry.price.currency,
                                                                 modelView.priceTableEntry.price.value);
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