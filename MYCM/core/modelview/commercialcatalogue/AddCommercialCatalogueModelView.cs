
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace core.modelview.commercialcatalogue
{
    /// <summary>
    /// Class representing a ModelView used for adding a instance of a Commercial Catalogue
    /// </summary>
    [DataContract]

    public class AddCommercialCatalogue
    {
        /// <summary>
        /// Commercial Catalogue reference.
        /// </summary>
        /// <value>Gets/sets the reference.</value>
        [DataMember]
        string reference { get; set; }

        /// <summary>
        /// Commercial Catalogue designation.
        /// </summary>
        /// <value>Gets/sets the designation.</value>
        [DataMember]
        string designation { get; set; }

        /// <summary>
        /// List of Commercial Catalogue Catalogue Collection.
        /// </summary>
        /// <value>Gets/sets the name.</value>
        [DataMember]
        public List<CatalogueCollectionModelViewId> catalogueCollectionList { get; set; }

    }

    /// <summary>
    /// Class representing a ModelView used for retrieving basic data from instances of GetCommercialCatalogueCatalogueCollection
    /// </summary>
    [DataContract]
    public class CatalogueCollectionModelViewId
    {

        /// <summary>
        /// Catalogue Collection id
        /// </summary>
        /// <value>Gets/sets the reference.</value>
        [DataMember]
        long catalogueCollectionId { get; set; }
    }
}