using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.currency
{
    /// <summary>
    /// ModelView that represents all available currencies
    /// </summary>
    /// <typeparam name="CurrencyModelView">ModelView that represents a currency</typeparam>
    [CollectionDataContract]
    public class GetAllCurrenciesModelView : List<CurrencyModelView>
    {

    }
}