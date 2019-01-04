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
                    double convertedValue = await new CurrencyPerAreaConversionService(clientFactory)
                                                    .convertCurrencyPerAreaToDefaultCurrencyPerArea(
                                                        modelView.priceTableEntry.price.currency,
                                                        modelView.priceTableEntry.price.area,
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
            createdPriceModelView.currency = defaultCurrency;
            createdPriceModelView.area = defaultArea;
            createdPriceModelView.id = savedMaterialPriceTableEntry.Id;

            return createdPriceModelView;
        }
    }
}