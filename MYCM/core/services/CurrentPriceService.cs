using System.Net.Http;
using System.Threading.Tasks;
using core.domain;
using core.exceptions;
using core.modelview.material;
using core.modelview.price;
using core.modelview.pricetable;
using core.persistence;
using NodaTime.Text;

namespace core.services
{
    /// <summary>
    /// Service to help fetch the current price of materials and material finishes
    /// </summary>
    public static class CurrentPriceService
    {
        /// <summary>
        /// Message that is presented if the requested material isn't found
        /// </summary>
        private const string MATERIAL_NOT_FOUND = "Unable to find material with identifier of {0}";

        /// <summary>
        /// Message that is presented if the requested material finish isn't found
        /// </summary>
        private const string FINISH_NOT_FOUND = "Unable to find finish with identifier of {0} of material with identifier of {1}";

        /// <summary>
        /// Message that is presented if the requested material doesn't have a current price
        /// </summary>
        private const string NO_CURRENT_MATERIAL_PRICE = "Currently, the material with the identifier of {0} doesn't have a price";

        /// <summary>
        /// Message that is presented if the requested material finishd doesn't have a current price
        /// </summary>
        private const string NO_CURRENT_FINISH_PRICE = "Currently, the finish with the identifier of {0} of material with identifier {1} doesn't have a price";

        /// <summary>
        /// Fetches the current price of a material
        /// </summary>
        /// <param name="modelView">ModelView with the necessary information to fetch a material's current price</param>
        /// <returns>GetCurrentMaterialPriceModelView with a material's current price</returns>
        public static GetCurrentMaterialPriceModelView fromMaterial(GetCurrentMaterialPriceModelView modelView, IHttpClientFactory clientFactory)
        {
            Material material = PersistenceContext.repositories().createMaterialRepository().find(modelView.material.id);

            if (material == null)
            {
                throw new ResourceNotFoundException(string.Format(MATERIAL_NOT_FOUND, modelView.material.id));
            }

            MaterialPriceTableEntry currentPrice = PersistenceContext.repositories().createMaterialPriceTableRepository().fetchCurrentMaterialPrice(modelView.material.id);

            if (currentPrice == null)
            {
                throw new ResourceNotFoundException(string.Format(NO_CURRENT_MATERIAL_PRICE, modelView.material.id));
            }

            GetCurrentMaterialPriceModelView currentMaterialPriceModelView = new GetCurrentMaterialPriceModelView();

            currentMaterialPriceModelView.material = new GetBasicMaterialModelView();
            currentMaterialPriceModelView.currentPrice = new PriceModelView();
            currentMaterialPriceModelView.timePeriod = new TimePeriodModelView();
            currentMaterialPriceModelView.tableEntryId = currentPrice.Id;
            currentMaterialPriceModelView.material.id = material.Id;
            currentMaterialPriceModelView.material.designation = material.designation;
            currentMaterialPriceModelView.material.reference = material.reference;
            currentMaterialPriceModelView.material.imageFilename = material.image;
            currentMaterialPriceModelView.timePeriod.startingDate = LocalDateTimePattern.GeneralIso.Format(currentPrice.timePeriod.startingDate);
            currentMaterialPriceModelView.timePeriod.endingDate = LocalDateTimePattern.GeneralIso.Format(currentPrice.timePeriod.endingDate);
            if (modelView.currentPrice.currency == null || modelView.currentPrice.area == null)
            {
                currentMaterialPriceModelView.currentPrice.value = currentPrice.price.value;
                currentMaterialPriceModelView.currentPrice.area = CurrencyPerAreaConversionService.getBaseArea();
                currentMaterialPriceModelView.currentPrice.currency = CurrencyPerAreaConversionService.getBaseCurrency();
            }
            else
            {
                Task<double> convertedValueTask =
                    new CurrencyPerAreaConversionService(clientFactory)
                        .convertDefaultCurrencyPerAreaToCurrencyPerArea(currentPrice.price.value, modelView.currentPrice.currency, modelView.currentPrice.area);
                convertedValueTask.Wait();
                currentMaterialPriceModelView.currentPrice.value = convertedValueTask.Result;
                currentMaterialPriceModelView.currentPrice.currency = modelView.currentPrice.currency;
                currentMaterialPriceModelView.currentPrice.area = modelView.currentPrice.area;
            }

            return currentMaterialPriceModelView;
        }

        public static GetCurrentMaterialFinishPriceModelView fromMaterialFinish(GetCurrentMaterialFinishPriceModelView modelView, IHttpClientFactory clientFactory)
        {
            Material material = PersistenceContext.repositories().createMaterialRepository().find(modelView.finish.materialId);

            if (material == null)
            {
                throw new ResourceNotFoundException(string.Format(MATERIAL_NOT_FOUND, modelView.finish.materialId));
            }

            MaterialPriceTableEntry currentMaterialPrice = PersistenceContext.repositories().createMaterialPriceTableRepository().fetchCurrentMaterialPrice(modelView.finish.materialId);

            if (currentMaterialPrice == null)
            {
                throw new ResourceNotFoundException(string.Format(NO_CURRENT_MATERIAL_PRICE, modelView.finish.materialId));
            }

            foreach (Finish finish in material.Finishes)
            {
                if (finish.Id == modelView.finish.id)
                {
                    FinishPriceTableEntry currentMaterialFinishPrice = PersistenceContext.repositories().createFinishPriceTableRepository().fetchCurrentMaterialFinishPrice(modelView.finish.id);

                    if (currentMaterialFinishPrice == null)
                    {
                        throw new ResourceNotFoundException(string.Format(NO_CURRENT_FINISH_PRICE, modelView.finish.id, modelView.finish.materialId));
                    }

                    GetCurrentMaterialFinishPriceModelView currentMaterialFinishPriceModelView = new GetCurrentMaterialFinishPriceModelView();
                    currentMaterialFinishPriceModelView.finish = new GetMaterialFinishModelView();
                    currentMaterialFinishPriceModelView.currentPrice = new PriceModelView();
                    currentMaterialFinishPriceModelView.timePeriod = new TimePeriodModelView();
                    currentMaterialFinishPriceModelView.tableEntryId = currentMaterialFinishPrice.Id;
                    currentMaterialFinishPriceModelView.finish.materialId = material.Id;
                    currentMaterialFinishPriceModelView.finish.id = finish.Id;
                    currentMaterialFinishPriceModelView.finish.description = finish.description;
                    currentMaterialFinishPriceModelView.finish.shininess = finish.shininess;
                    currentMaterialFinishPriceModelView.timePeriod.startingDate = LocalDateTimePattern.GeneralIso.Format(currentMaterialFinishPrice.timePeriod.startingDate);
                    currentMaterialFinishPriceModelView.timePeriod.endingDate = LocalDateTimePattern.GeneralIso.Format(currentMaterialFinishPrice.timePeriod.endingDate);
                    if (modelView.currentPrice.currency == null || modelView.currentPrice.area == null)
                    {
                        currentMaterialFinishPriceModelView.currentPrice.value = currentMaterialFinishPrice.price.value;
                        currentMaterialFinishPriceModelView.currentPrice.area = CurrencyPerAreaConversionService.getBaseArea();
                        currentMaterialFinishPriceModelView.currentPrice.currency = CurrencyPerAreaConversionService.getBaseCurrency();
                    }
                    else
                    {
                        Task<double> convertedValueTask =
                            new CurrencyPerAreaConversionService(clientFactory)
                                .convertDefaultCurrencyPerAreaToCurrencyPerArea(currentMaterialFinishPrice.price.value, modelView.currentPrice.currency, modelView.currentPrice.area);
                        convertedValueTask.Wait();
                        currentMaterialFinishPriceModelView.currentPrice.value = convertedValueTask.Result;
                        currentMaterialFinishPriceModelView.currentPrice.currency = modelView.currentPrice.currency;
                        currentMaterialFinishPriceModelView.currentPrice.area = modelView.currentPrice.area;
                    }
                    return currentMaterialFinishPriceModelView;
                }
            }
            throw new ResourceNotFoundException(string.Format(FINISH_NOT_FOUND, modelView.finish.id, modelView.finish.materialId));
        }
    }
}