using core.modelview.pricetableentries;
using core.persistence;
using core.dto;
using core.domain;
using NodaTime;
using NodaTime.Text;
using System;

namespace core.services
{
    /// <summary>
    /// Service that helps in transforming an AddMaterialPriceTableEntry into a MaterialPriceTableEntry and saving it to the database
    /// </summary>
    //TODO This Service and AddFinishPriceTableEntryModelViewService are very similar. Should we look into a way of decreasing duplicated code?
    public static class AddMaterialPriceTableEntryModelViewService
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
        public static AddPriceTableEntryModelView transform(AddPriceTableEntryModelView modelView)
        {

            MaterialRepository materialRepository = PersistenceContext.repositories().createMaterialRepository();
            long materialId = modelView.entityId;

            Material material = materialRepository.find(materialId);

            if (material == null)
            {
                throw new NullReferenceException(MATERIAL_NOT_FOUND);
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

            //TODO Take area conversion into account
            //!For now we are considering all prices are in currency/m2
            Price price = null;
            if (!modelView.priceTableEntry.price.currency.Equals(EURO_CURRENCY_ABV))
            {
                try
                {
                    double convertedValue = await new CurrencyConversionService(clientFactory)
                                                        .convertCurrencyToEuro(modelView.priceTableEntry.price.currency,
                                                             modelView.priceTableEntry.price.value);
                    price = Price.valueOf(convertedValue);
                }
                catch (HttpRequestException)
                {
                    price = Price.valueOf(modelView.priceTableEntry.price.value);
                }
            }
            MaterialPriceTableEntry materialPriceTableEntry = new MaterialPriceTableEntry(material, price, timePeriod);
            MaterialPriceTableEntry savedMaterialPriceTableEntry =
                PersistenceContext.repositories().createMaterialPriceTableRepository().save(materialPriceTableEntry);

            if (savedMaterialPriceTableEntry == null)
            {
                throw new InvalidOperationException(PRICE_TABLE_ENTRY_NOT_CREATED);
            }

            AddPriceTableEntryModelView createdPriceModelView = new AddPriceTableEntryModelView();

            createdPriceModelView.entityId = material.Id;

            PriceTableEntryDTO createdPriceTableEntryDTO = new PriceTableEntryDTO();
            createdPriceTableEntryDTO.startingDate = LocalDateTimePattern.GeneralIso.Format(savedMaterialPriceTableEntry.timePeriod.startingDate);
            createdPriceTableEntryDTO.endingDate = LocalDateTimePattern.GeneralIso.Format(savedMaterialPriceTableEntry.timePeriod.endingDate);
            createdPriceTableEntryDTO.price = new PriceDTO();
            createdPriceTableEntryDTO.price.value = savedMaterialPriceTableEntry.price.value;
            //TODO Take into account currency and area conversion
            //!For now we are considering all prices are in €/m2
            createdPriceTableEntryDTO.price.currency = "";
            createdPriceTableEntryDTO.price.area = "";

            createdPriceModelView.priceTableEntry = createdPriceTableEntryDTO;
            createdPriceModelView.tableEntryId = savedMaterialPriceTableEntry.Id;

            return createdPriceModelView;
        }
    }
}