using core.modelview.pricetableentries;
using core.persistence;
using core.dto;
using core.domain;
using NodaTime;
using NodaTime.Text;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using core.exceptions;
using core.modelview.pricetable;
using System.Collections.Generic;

namespace core.services
{
    /// <summary>
    /// Service that helps in transforming an AddMaterialPriceTableEntry into a MaterialPriceTableEntry and saving it to the database
    /// </summary>
    public static class AddMaterialPriceTableEntryService
    {
        /// <summary>
        /// Message that occurs if the requested material for the price table entry isn't found
        /// </summary>
        private const string MATERIAL_NOT_FOUND = "Requested material wasn't found";

        /// <summary>
        /// Message that occurs if one of the dates of the time period doesn't follow the General ISO format
        /// </summary>
        private const string DATES_WRONG_FORMAT = "Make sure all dates follow the General ISO Format: ";

        /// <summary>
        /// Message that occurs if the price table entry isn't created
        /// </summary>
        private const string PRICE_TABLE_ENTRY_NOT_CREATED = "A price table entry for the requested material with the same values already exists. Please try again";

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
        /// Transforms an AddMaterialPriceTableEntry into a MaterialPriceTableEntry and saves it to the database
        /// </summary>
        /// <param name="modelView">material price table entry to transform and persist</param>
        /// <returns>created instance or null in case the creation wasn't successfull</returns>
        public static async Task<GetMaterialPriceModelView> create(AddPriceTableEntryModelView modelView, IHttpClientFactory clientFactory)
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

            MaterialPriceTableEntry materialPriceTableEntry = new MaterialPriceTableEntry(material, price, timePeriod);
            MaterialPriceTableEntry savedMaterialPriceTableEntry =
                PersistenceContext.repositories().createMaterialPriceTableRepository().save(materialPriceTableEntry);

            if (savedMaterialPriceTableEntry == null)
            {
                throw new InvalidOperationException(PRICE_TABLE_ENTRY_NOT_CREATED);
            }

            GetMaterialPriceModelView createdPriceModelView = new GetMaterialPriceModelView();

            createdPriceModelView.materialId = material.Id;
            createdPriceModelView.startingDate = LocalDateTimePattern.GeneralIso.Format(savedMaterialPriceTableEntry.timePeriod.startingDate);
            createdPriceModelView.endingDate = LocalDateTimePattern.GeneralIso.Format(savedMaterialPriceTableEntry.timePeriod.endingDate);
            createdPriceModelView.value = savedMaterialPriceTableEntry.price.value;
            createdPriceModelView.currency = CurrencyPerAreaConversionService.getBaseCurrency();
            createdPriceModelView.area = CurrencyPerAreaConversionService.getBaseArea();
            createdPriceModelView.id = savedMaterialPriceTableEntry.Id;

            return createdPriceModelView;
        }
    }
}