using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.currency
{
    [CollectionDataContract]
    public class GetAllCurrenciesModelView : List<CurrencyModelView>
    {

    }
}