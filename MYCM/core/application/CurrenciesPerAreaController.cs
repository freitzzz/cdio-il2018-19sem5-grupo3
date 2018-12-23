using System.Collections.Generic;
using core.modelview.area;
using core.modelview.currency;
using core.services;

namespace core.application
{
    public class CurrenciesPerAreaController
    {
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