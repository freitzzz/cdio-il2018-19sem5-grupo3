using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.commercialcatalogue
{
    /// <summary>
    /// Class representing a ModelView used for adding a instance of catalogue collection to commercial catalogue.
    /// </summary>
    [DataContract]
    public class AddCatalogueCollectionModelView
    {
        /// <summary>
        /// Customized Product Collection id
        /// </summary>
        /// <value>Gets/sets the database identifier.</value>
        [DataMember]
        public long customizedProductCollectionId { get; set; }

        /// <summary>
        /// List of all the Customized Product ids
        /// </summary>
        /// <value>Gets/sets the database identifier.</value>
        [DataMember]
        public List<CatalogueCollectionProductId> listOfCatalogueCollectionProductId { get; set; }


    }
    /// <summary>
    /// Class representing an object of an id Customized Product.
    /// </summary>
    [DataContract]
    public class CatalogueCollectionProductId
    {
        /// <summary>
        /// Customized Product id
        /// </summary>
        /// <value>Gets/sets the database identifier.</value>
        [DataMember]
        public long customizedProductId { get; set; }
    }

}