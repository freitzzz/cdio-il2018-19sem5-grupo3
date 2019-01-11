using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using core.domain;

namespace core.modelview.customizedproduct
{

    /// <summary>
    /// Class representing the ModelView used for retrieving a Collection of CustomizedProduct.
    /// </summary>
    [CollectionDataContract]
    public class GetAllCustomizedProductsModelView : List<GetBasicCustomizedProductModelView>
    {
    }
}