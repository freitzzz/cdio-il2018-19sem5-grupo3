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
    /// Service that helps transforming and persisting a Finish Price Table Entry
    /// </summary>
    //TODO This Service and AddMaterialPriceTableEntryModelViewService are very similar. Should we look into a way of decreasing duplicated code?
    public class AddFinishPriceTableEntryModelViewService
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

        public static AddFinishPriceTableEntryModelView transform(AddFinishPriceTableEntryModelView modelView)
        {
            MaterialRepository materialRepository = PersistenceContext.repositories().createMaterialRepository();
            long materialId = modelView.entityId;

            Material material = materialRepository.find(materialId);

            if (material == null)
            {
                throw new NullReferenceException(MATERIAL_NOT_FOUND);
            }

            //TODO Is this null check enough? Should we check ,if an entry exists, that the time period of the price entry is valid?
            MaterialPriceTableEntry materialPriceTableEntry = PersistenceContext.repositories().createMaterialPriceTableRepository().find(materialId);

            if (materialPriceTableEntry == null)
            {
                throw new InvalidOperationException(MATERIAL_HAS_NO_PRICE);
            }

            //TODO Should this be done with a query instead of a foreach?
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
                    catch (UnparsableValueException unparsableValueException)
                    {
                        throw new UnparsableValueException(DATES_WRONG_FORMAT + LocalDateTimePattern.GeneralIso.PatternText);
                    }

                    TimePeriod timePeriod = TimePeriod.valueOf(startingDate, endingDate);

                    //TODO Take into account currency and area conversion
                    //!For now we are considering all prices are in €/m2
                    Price price = Price.valueOf(modelView.priceTableEntry.price.value);
                    FinishPriceTableEntry finishPriceTableEntry = new FinishPriceTableEntry(material.id(), finish, price, timePeriod);
                    FinishPriceTableEntry savedFinishPriceTableEntry = PersistenceContext.repositories()
                                            .createFinishPriceTableRepository().save(finishPriceTableEntry);

                    if (savedFinishPriceTableEntry == null)
                    {
                        throw new InvalidOperationException(PRICE_TABLE_ENTRY_NOT_CREATED);
                    }

                    PriceTableEntryDTO createdPriceTableEntryDTO = new PriceTableEntryDTO();
                    createdPriceTableEntryDTO.startingDate = LocalDateTimePattern.GeneralIso.Format(savedFinishPriceTableEntry.timePeriod.startingDate);
                    createdPriceTableEntryDTO.endingDate = LocalDateTimePattern.GeneralIso.Format(savedFinishPriceTableEntry.timePeriod.endingDate);
                    createdPriceTableEntryDTO.price = new PriceDTO();
                    createdPriceTableEntryDTO.price.value = savedFinishPriceTableEntry.price.value;
                    //TODO Take into account currency and area conversion
                    //!For now we are considering all prices are in €/m2
                    createdPriceTableEntryDTO.price.currency = "";
                    createdPriceTableEntryDTO.price.area = "";

                    AddFinishPriceTableEntryModelView createdPriceModelView = new AddFinishPriceTableEntryModelView();
                    createdPriceModelView.priceTableEntry = createdPriceTableEntryDTO;
                    createdPriceModelView.entityId = material.Id;
                    createdPriceModelView.finishId = finish.Id;

                    return createdPriceModelView;
                }
            }

            throw new NullReferenceException(FINISH_NOT_FOUND);
        }
    }
}