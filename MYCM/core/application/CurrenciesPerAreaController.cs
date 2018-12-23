using System.Collections.Generic;
using core.modelview.area;
using core.modelview.currency;
using core.services;

namespace core.application
{
    /// <summary>
    /// Application CurrenciesPerArea Controller
    /// </summary>
    public class CurrenciesPerAreaController
    {
        /// <summary>
        /// Fetches all available currencies
        /// </summary>
        /// <returns>GetAllCurrenciesModelView containing all available currencies</returns>
        public GetAllCurrenciesModelView getAllCurrencies()
        {
            IEnumerable<string> availableCurrencies = CurrenciesService.getAvailableCurrencies();
            GetAllCurrenciesModelView allCurrenciesModelView = new GetAllCurrenciesModelView();

            foreach (string currency in availableCurrencies)
            {
                CurrencyModelView currencyModelView = new CurrencyModelView();
                currencyModelView.currency = currency;
                allCurrenciesModelView.Add(currencyModelView);
            }

            return allCurrenciesModelView;
        }

        /// <summary>
        /// Fetches all available areas
        /// </summary>
        /// <returns>GetAllAreasModelView with all available areas</returns>
        public GetAllAreasModelView getAllAreas()
        {
            IEnumerable<string> availableAreas = AreasService.getAvailableAreas();
            GetAllAreasModelView allAreasModelView = new GetAllAreasModelView();

            foreach (string area in availableAreas)
            {
                AreaModelView areaModelView = new AreaModelView();
                areaModelView.area = area;
                allAreasModelView.Add(areaModelView);
            }

            return allAreasModelView;
        }
    }
}