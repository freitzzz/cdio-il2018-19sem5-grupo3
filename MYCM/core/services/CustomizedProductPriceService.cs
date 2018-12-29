using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using core.domain;
using core.exceptions;
using core.modelview.area;
using core.modelview.color;
using core.modelview.customizeddimensions;
using core.modelview.customizedproduct.customizedproductprice;
using core.modelview.price;
using core.persistence;
using static core.domain.CustomizedProduct;
using System.Linq;

namespace core.services
{
    /// <summary>
    /// Service that helps calculating the price of a customized product
    /// </summary>
    public static class CustomizedProductPriceService
    {
        /// <summary>
        /// Decimal places to use when rounding results
        /// </summary>
        private const int DECIMAL_PLACES = 2;

        /// <summary>
        /// Message that is sent if no customized product is found for the requested identifier
        /// </summary>
        private const string CUSTOMIZED_PRODUCT_NOT_FOUND = "Unable to find customized product with identifier of {0}";

        /// <summary>
        /// Message that is sent if the requested customized product is not finished
        /// </summary>
        private const string CUSTOMIZED_PRODUCT_NOT_FINISHED = "This customized product isn't finished yet!";

        /// <summary>
        /// Message that is sent if a material doesn't have a current price
        /// </summary>
        private const string MATERIAL_HAS_NO_CURRENT_PRICE = "Material with identifier of {0} currently has no price";

        /// <summary>
        /// Message that is sent if a material finish doens't have a current price
        /// </summary>
        private const string FINISH_HAS_NO_CURRENT_PRICE = "Finish with identifier of {0} currently has no price";

        /// <summary>
        /// Calculates the price
        /// </summary>
        /// <param name="fetchCustomizedProductPrice">FetchCustomizedProductModelView with the necessary information to fetch the customized product's price</param>
        /// <returns>CustomizedProductPriceModelView with the price of the customized product</returns>
        public static async Task<CustomizedProductFinalPriceModelView> calculatePrice(FetchCustomizedProductPriceModelView fetchCustomizedProductPrice, IHttpClientFactory clientFactory)
        {
            if (fetchCustomizedProductPrice.currency != null)
            {
                CurrenciesService.checkCurrencySupport(fetchCustomizedProductPrice.currency);
            }

            if (fetchCustomizedProductPrice.area != null)
            {
                AreasService.checkAreaSupport(fetchCustomizedProductPrice.area);
            }

            CustomizedProduct customizedProduct =
                PersistenceContext.repositories().createCustomizedProductRepository().find(fetchCustomizedProductPrice.id);

            if (customizedProduct == null)
            {
                throw new ResourceNotFoundException
                (
                    string.Format(CUSTOMIZED_PRODUCT_NOT_FOUND, fetchCustomizedProductPrice.id)
                );
            }

            if (customizedProduct.status == CustomizationStatus.PENDING)
            {
                throw new ArgumentException
                (
                    CUSTOMIZED_PRODUCT_NOT_FINISHED
                );
            }

            //TODO Should the fetching of prices of customized products that aren't base products be allowed?

            MaterialPriceTableRepository materialPriceTableRepository =
                PersistenceContext.repositories().createMaterialPriceTableRepository();
            FinishPriceTableRepository finishPriceTableRepository =
                PersistenceContext.repositories().createFinishPriceTableRepository();

            CustomizedProductFinalPriceModelView customizedProductTotalPrice = new CustomizedProductFinalPriceModelView();
            List<CustomizedProductPriceModelView> customizedProductPriceList = new List<CustomizedProductPriceModelView>();

            MaterialPriceTableEntry materialPriceTableEntry =
                getCurrentMaterialPrice(materialPriceTableRepository, customizedProduct.customizedMaterial.material.Id);

            FinishPriceTableEntry finishPriceTableEntry = null;

            if (customizedProduct.customizedMaterial.finish != null)
            {
                finishPriceTableEntry = getCurrentFinishPrice(finishPriceTableRepository,
                     customizedProduct.customizedMaterial.material.Finishes.Where(f => f.Equals(customizedProduct.customizedMaterial.finish)).SingleOrDefault().Id);
            }

            //TODO What should we do about doors?
            //TODO Should we consider that every component has a box geometry?
            //!For now the surface area of every product is being calculated as if it the product was a SIX FACED RIGHT RECTANGULAR PRISM

            CustomizedProductPriceModelView parentCustomizedProductModelView =
                await buildCustomizedProductPriceModelView(customizedProduct, fetchCustomizedProductPrice, materialPriceTableEntry, finishPriceTableEntry, clientFactory);

            customizedProductPriceList.Add(parentCustomizedProductModelView);

            if (customizedProduct.hasCustomizedProducts())
            {
                List<CustomizedProductPriceModelView> childCustomizedProducts = new List<CustomizedProductPriceModelView>();
                customizedProductPriceList.AddRange(await
                    calculatePricesOfChildCustomizedProducts(childCustomizedProducts, customizedProduct, fetchCustomizedProductPrice,
                        materialPriceTableRepository, finishPriceTableRepository, clientFactory));
            }

            customizedProductTotalPrice.customizedProducts = customizedProductPriceList;
            customizedProductTotalPrice.finalPrice = calculateFinalPriceOfCustomizedProduct(customizedProductTotalPrice, fetchCustomizedProductPrice);

            return customizedProductTotalPrice;
        }

        /// <summary>
        /// Calculates the final price of a customized product
        /// </summary>
        /// <param name="customizedProductTotalPriceModelView">ModelView containing the detailed info about the customized product's price</param>
        /// <param name="fetchCustomizedProductPriceModelView">ModelView containing information about currency/area conversion</param>
        /// <returns>PriceModelView containing the final price of a customized product</returns>
        private static PriceModelView calculateFinalPriceOfCustomizedProduct(CustomizedProductFinalPriceModelView customizedProductTotalPriceModelView, FetchCustomizedProductPriceModelView fetchCustomizedProductPriceModelView)
        {
            PriceModelView finalPriceModelView = new PriceModelView();
            double finalPrice = 0;
            foreach (CustomizedProductPriceModelView customizedProductPrice in customizedProductTotalPriceModelView.customizedProducts)
            {
                finalPrice += customizedProductPrice.price.value;
            }

            finalPriceModelView.value = finalPrice;
            if (fetchCustomizedProductPriceModelView.currency != null && fetchCustomizedProductPriceModelView.area != null)
            {
                finalPriceModelView.currency = fetchCustomizedProductPriceModelView.currency;
            }
            else
            {
                finalPriceModelView.currency = CurrencyPerAreaConversionService.getBaseCurrency();
            }
            return finalPriceModelView;
        }

        /// <summary>
        /// Calculates the price of all child customized products of a given customized product
        /// </summary>
        /// <param name="customizedProduct">Parent Customized Product</param>
        /// <param name="fetchCustomizedProductPrice">Information about currency/area conversion</param>
        /// <param name="materialPriceTableRepository">MaterialPriceTableRepository instance to fetch current material prices</param>
        /// <param name="finishPriceTableRepository">FinishPriceTableRepository instance to fetch current finish prices</param>
        /// <param name="clientFactory">Injected HTTP Client Factory</param>
        /// <returns>IEnumerable containing the prices of all child customized products</returns>
        private static async Task<IEnumerable<CustomizedProductPriceModelView>> calculatePricesOfChildCustomizedProducts(List<CustomizedProductPriceModelView> childCustomizedProductPrices, CustomizedProduct customizedProduct, FetchCustomizedProductPriceModelView fetchCustomizedProductPrice,
            MaterialPriceTableRepository materialPriceTableRepository, FinishPriceTableRepository finishPriceTableRepository, IHttpClientFactory clientFactory)
        {
            foreach (Slot slot in customizedProduct.slots)
            {
                foreach (CustomizedProduct childCustomizedProduct in slot.customizedProducts)
                {
                    MaterialPriceTableEntry materialPriceTableEntry =
                        getCurrentMaterialPrice(materialPriceTableRepository, childCustomizedProduct.customizedMaterial.material.Id);

                    FinishPriceTableEntry finishPriceTableEntry = null;

                    if (childCustomizedProduct.customizedMaterial.finish != null)
                    {
                        finishPriceTableEntry = getCurrentFinishPrice(finishPriceTableRepository,
                            childCustomizedProduct.customizedMaterial.material.Finishes.Where(f => f.Equals(childCustomizedProduct.customizedMaterial.finish)).SingleOrDefault().Id);
                    }

                    CustomizedProductPriceModelView childCustomizedProductPriceModelView =
                        await buildCustomizedProductPriceModelView(childCustomizedProduct, fetchCustomizedProductPrice,
                            materialPriceTableEntry, finishPriceTableEntry, clientFactory);

                    childCustomizedProductPrices.Add(childCustomizedProductPriceModelView);

                    if (childCustomizedProduct.hasCustomizedProducts())
                    {
                        await calculatePricesOfChildCustomizedProducts(childCustomizedProductPrices, childCustomizedProduct,
                                fetchCustomizedProductPrice, materialPriceTableRepository, finishPriceTableRepository, clientFactory);
                    }
                }
            }

            return childCustomizedProductPrices;
        }

        /// <summary>
        /// Builds a CustomizedProductPriceModelView out of a Customized Product
        /// </summary>
        /// <param name="customizedProduct">Customized Product to build the model view out of</param>
        /// <param name="fetchCustomizedProductPriceModelView">ModelView to know if currency/area conversion is needed</param>
        /// <returns>CustomizedProductPriceModelView</returns>
        private static async Task<CustomizedProductPriceModelView> buildCustomizedProductPriceModelView(CustomizedProduct customizedProduct, FetchCustomizedProductPriceModelView fetchCustomizedProductPriceModelView,
             MaterialPriceTableEntry materialPriceTableEntry, FinishPriceTableEntry finishPriceTableEntry, IHttpClientFactory clientFactory)
        {
            CustomizedProductPriceModelView customizedProductPriceModelView = new CustomizedProductPriceModelView();

            string defaultCurrency = CurrencyPerAreaConversionService.getBaseCurrency();
            string defaultArea = CurrencyPerAreaConversionService.getBaseArea();
            bool convertCurrencyPerArea = fetchCustomizedProductPriceModelView.currency != null && fetchCustomizedProductPriceModelView.area != null;

            customizedProductPriceModelView.customizedProductId = customizedProduct.Id;
            customizedProductPriceModelView.reference = customizedProduct.reference;
            customizedProductPriceModelView.serialNumber = customizedProduct.serialNumber;
            customizedProductPriceModelView.productId = customizedProduct.product.Id;
            string requestedMeasurementUnit = null;
            if (convertCurrencyPerArea)
            {
                requestedMeasurementUnit = new String(fetchCustomizedProductPriceModelView.area.Where(c => Char.IsLetter(c)).ToArray());
            }
            else
            {
                requestedMeasurementUnit = new String(CurrencyPerAreaConversionService.getBaseArea().Where(c => Char.IsLetter(c)).ToArray());
            }
            customizedProductPriceModelView.customizedDimensions = new GetCustomizedDimensionsModelView();
            customizedProductPriceModelView.customizedDimensions.unit = requestedMeasurementUnit;
            customizedProductPriceModelView.customizedDimensions.width =
                MeasurementUnitService.convertToUnit(customizedProduct.customizedDimensions.width, requestedMeasurementUnit);
            customizedProductPriceModelView.customizedDimensions.height =
                MeasurementUnitService.convertToUnit(customizedProduct.customizedDimensions.height, requestedMeasurementUnit);
            customizedProductPriceModelView.customizedDimensions.depth =
                MeasurementUnitService.convertToUnit(customizedProduct.customizedDimensions.depth, requestedMeasurementUnit);

            customizedProductPriceModelView.customizedMaterial = new CustomizedMaterialPriceModelView();
            customizedProductPriceModelView.customizedMaterial.customizedMaterialId = customizedProduct.customizedMaterial.Id;
            customizedProductPriceModelView.customizedMaterial.materialId = customizedProduct.customizedMaterial.material.Id;
            if (customizedProduct.customizedMaterial.finish != null)
            {
                customizedProductPriceModelView.customizedMaterial.finish = new FinishPriceModelView();
                customizedProductPriceModelView.customizedMaterial.finish.finishId = customizedProduct.customizedMaterial.finish.Id;
                customizedProductPriceModelView.customizedMaterial.finish.description = customizedProduct.customizedMaterial.finish.description;
                customizedProductPriceModelView.customizedMaterial.finish.shininess = customizedProduct.customizedMaterial.finish.shininess;
                customizedProductPriceModelView.customizedMaterial.finish.price = new PriceModelView();
                if (convertCurrencyPerArea)
                {
                    customizedProductPriceModelView.customizedMaterial.finish.price.currency =
                        fetchCustomizedProductPriceModelView.currency;
                    customizedProductPriceModelView.customizedMaterial.finish.price.area =
                        fetchCustomizedProductPriceModelView.area;
                    customizedProductPriceModelView.customizedMaterial.finish.price.value =
                        await convertPriceValue(
                                                finishPriceTableEntry.price.value, fetchCustomizedProductPriceModelView.currency,
                                                fetchCustomizedProductPriceModelView.area, clientFactory
                                                );
                }
                else
                {
                    customizedProductPriceModelView.customizedMaterial.finish.price.currency = defaultCurrency;
                    customizedProductPriceModelView.customizedMaterial.finish.price.area = defaultArea;
                    customizedProductPriceModelView.customizedMaterial.finish.price.value = finishPriceTableEntry.price.value;
                }

            }
            if (customizedProduct.customizedMaterial.color != null)
            {
                customizedProductPriceModelView.customizedMaterial.color = ColorModelViewService.fromEntity(customizedProduct.customizedMaterial.color);
            }
            customizedProductPriceModelView.customizedMaterial.price = new PriceModelView();
            if (convertCurrencyPerArea)
            {
                customizedProductPriceModelView.customizedMaterial.price.currency =
                    fetchCustomizedProductPriceModelView.currency;
                customizedProductPriceModelView.customizedMaterial.price.area =
                    fetchCustomizedProductPriceModelView.area;
                customizedProductPriceModelView.customizedMaterial.price.value =
                    await convertPriceValue(
                                            materialPriceTableEntry.price.value, fetchCustomizedProductPriceModelView.currency,
                                            fetchCustomizedProductPriceModelView.area, clientFactory
                                            );
            }
            else
            {
                customizedProductPriceModelView.customizedMaterial.price.currency = defaultCurrency;
                customizedProductPriceModelView.customizedMaterial.price.area = defaultArea;
                customizedProductPriceModelView.customizedMaterial.price.value = materialPriceTableEntry.price.value;
            }

            customizedProductPriceModelView.price = new PriceModelView();
            customizedProductPriceModelView.totalArea = new AreaModelView();

            if (convertCurrencyPerArea)
            {
                customizedProductPriceModelView.price.currency = fetchCustomizedProductPriceModelView.currency;
                customizedProductPriceModelView.totalArea.area = fetchCustomizedProductPriceModelView.area;
            }
            else
            {
                customizedProductPriceModelView.price.currency = defaultCurrency;
                customizedProductPriceModelView.totalArea.area = defaultArea;
            }

            calculateTotalAreaAndPriceOfCustomizedMaterial(customizedProductPriceModelView, customizedProduct);

            return customizedProductPriceModelView;
        }

        /// <summary>
        /// Calculates the surface area of material and material finish used in a customized product and it's respective prices
        /// </summary>
        /// <param name="customizedProductPriceModelView">ModelView of the customized product's price information</param>
        /// <param name="customizedProduct">Customized product whose price is being calculated</param>
        private static void calculateTotalAreaAndPriceOfCustomizedMaterial(CustomizedProductPriceModelView customizedProductPriceModelView, CustomizedProduct customizedProduct)
        {
            customizedProductPriceModelView.totalArea.value =
                    calculateSurfaceAreaOfRightRectangularPrism(
                        customizedProductPriceModelView.customizedDimensions.width,
                        customizedProductPriceModelView.customizedDimensions.height,
                        customizedProductPriceModelView.customizedDimensions.depth);
            customizedProductPriceModelView.price.value =
                calculateMaterialAndFinishPrice(
                    customizedProductPriceModelView.totalArea.value,
                    customizedProductPriceModelView.customizedMaterial
                    );
        }

        /// <summary>
        /// Calculates the price of a material and/or a material finish
        /// </summary>
        /// <param name="totalArea">total area of the material and/or material finish that were used</param>
        /// <param name="customizedMaterialPriceModelView">CustomizedMaterialPriceModelView with the necessary information to perform this operation</param>
        /// <returns></returns>
        private static double calculateMaterialAndFinishPrice(double totalArea, CustomizedMaterialPriceModelView customizedMaterialPriceModelView)
        {
            return customizedMaterialPriceModelView.finish != null ?
                   Math.Round(customizedMaterialPriceModelView.finish.price.value * totalArea
                   + customizedMaterialPriceModelView.price.value * totalArea, DECIMAL_PLACES)
                   :
                   Math.Round(customizedMaterialPriceModelView.price.value * totalArea, DECIMAL_PLACES);
        }

        /// <summary>
        /// Calculates the surface area of a right rectangular prism
        /// The formula is the following: //! A = 2 * (wl + hl + hw)
        /// Where A is the surface area,
        /// w is the prism's width,
        /// l is the prism's length (in our case depth = length) and
        /// h is the prism's height
        /// </summary>
        /// <param name="width">prism's width</param>
        /// <param name="height">prism's height</param>
        /// <param name="depth">prism's length (equivalent to depth)</param>
        /// <returns>surface area of a right rectangular prism</returns>
        private static double calculateSurfaceAreaOfRightRectangularPrism(double width, double height, double depth)
        {
            return Math.Round(2 * (width * depth + height * depth + height * width), DECIMAL_PLACES);
        }

        /// <summary>
        /// Calls CurrencyPerAreaConversionService to convert a price to a requested currency per area
        /// </summary>
        /// <param name="valueToConvert">value to convert</param>
        /// <param name="toCurrency">requested currency</param>
        /// <param name="toArea">requested area</param>
        /// <param name="clientFactory">injected http client factory</param>
        /// <returns>converted value of the price</returns>
        private static async Task<double> convertPriceValue(double valueToConvert, string toCurrency, string toArea, IHttpClientFactory clientFactory)
        {
            return await new CurrencyPerAreaConversionService(clientFactory)
                        .convertDefaultCurrencyPerAreaToCurrencyPerArea(valueToConvert, toCurrency, toArea);
        }

        /// <summary>
        /// Checks if a material from a customized product has a current price
        /// </summary>
        /// <param name="materialPriceTableRepository">Material Price Table Repository</param>
        /// <param name="materialId">Material's PID</param>
        /// <returns>MaterialPriceTableEntry with the material's current price</returns>
        private static MaterialPriceTableEntry getCurrentMaterialPrice(MaterialPriceTableRepository materialPriceTableRepository, long materialId)
        {
            MaterialPriceTableEntry materialPriceTableEntry =
                materialPriceTableRepository.fetchCurrentMaterialPrice(materialId);

            if (materialPriceTableEntry == null)
            {
                throw new ResourceNotFoundException
                (
                    string.Format(MATERIAL_HAS_NO_CURRENT_PRICE, materialId)
                );
            }

            return materialPriceTableEntry;
        }

        /// <summary>
        /// Checks if a material finish from a customized product has a current price
        /// </summary>
        /// <param name="finishPriceTableRepository">Finish Price Table Repository</param>
        /// <param name="finishId">Finish's PID</param>
        /// <returns>FinishPriceTableEntry with the finish's current price</returns>
        private static FinishPriceTableEntry getCurrentFinishPrice(FinishPriceTableRepository finishPriceTableRepository, long finishId)
        {
            FinishPriceTableEntry finishPriceTableEntry =
                finishPriceTableRepository.fetchCurrentMaterialFinishPrice(finishId);

            if (finishPriceTableEntry == null)
            {
                throw new ResourceNotFoundException
                (
                    string.Format(FINISH_HAS_NO_CURRENT_PRICE, finishId)
                );
            }

            return finishPriceTableEntry;
        }
    }
}