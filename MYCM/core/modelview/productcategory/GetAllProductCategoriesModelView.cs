using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.productcategory
{
    /// <summary>
    /// Class representing a collection of GetBasicProductCategoryModelView.
    /// </summary>
    [CollectionDataContract]
    public class GetAllProductCategoriesModelView : List<GetBasicProductCategoryModelView>
    {
        
    }
}