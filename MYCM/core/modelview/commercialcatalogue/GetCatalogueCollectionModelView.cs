using System.Collections.Generic;
using System.Runtime.Serialization;
using core.modelview.customizedproduct;

namespace core.modelview.commercialcatalogue
{
    /// <summary>
    /// Class representing a ModelView used for retrieving basic data from instances of Catalogue Collection
    /// </summary>
    [DataContract]
    public class GetCatalogueCollectionModelView
    {


        /// <summary>
        /// Catalogue Collection's Customized Product Collection.
        /// </summary>
        /// <value>Gets/sets the Customized Product Collection.</value>
        [DataMember]
        public GetCustomizedProductCollectionModelView customizedProductCollection;

        /// <summary>
        /// Catalogue Collection's list of Customized Products.
        /// </summary>
        /// <value>Gets/sets the list of Customized Products.</value>
        [DataMember]
        public List<GetCustomizedProductModelView> customizedProducts;
    }
}