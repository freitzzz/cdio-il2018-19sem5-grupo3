using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.commercialcatalogue
{
    /// <summary>
    /// Class representing a ModelView used for retrieving basic data from instances of Commercial Catalogue
    /// </summary>
    [DataContract]
    public class GetCommercialCatalogueModelView
    {
        /// <summary>
        /// Commercial Catalogue's database identifier.
        /// </summary>
        /// <value>Gets/sets the database identifier.</value>
        [DataMember]
        public long id { get; set; }

        /// <summary>
        /// Commercial Catalogue's reference.
        /// </summary>
        /// <value>Gets/sets the reference.</value>
        [DataMember]
        public string reference { get; set; }

        /// <summary>
        /// Commercial Catalogue's designation.
        /// </summary>
        /// <value>Gets/sets the designation.</value>
        [DataMember]
        public string designation { get; set; }

    
        /// <summary>
        /// List of Commercial Catalogue Catalogue Collection.
        /// </summary>
        /// <value>Gets/sets the name.</value>
        [DataMember]
        public List<GetCatalogueCollectionModelView> commercialCatalogueCatalogueCollectionList { get; set; }

      
    }
}