using System.Collections.Generic;
using System.Runtime.Serialization;
using core.modelview.cataloguecollection;

namespace core.modelview.commercialcatalogue
{
    /// <summary>
    /// Class representing a ModelView used for adding a instance of a Commercial Catalogue
    /// </summary>
    [DataContract]
    public class AddCommercialCatalogueModelView
    {
        /// <summary>
        /// Commercial Catalogue reference.
        /// </summary>
        /// <value>Gets/sets the reference.</value>
        [DataMember]
        public string reference { get; set; }

        /// <summary>
        /// Commercial Catalogue designation.
        /// </summary>
        /// <value>Gets/sets the designation.</value>
        [DataMember]
        public string designation { get; set; }

        /// <summary>
        /// List of Commercial Catalogue Catalogue Collection.
        /// </summary>
        /// <value>Gets/sets the name.</value>
        [DataMember(Name = "collections")]
        public List<AddCatalogueCollectionModelView> catalogueCollections { get; set; } = new List<AddCatalogueCollectionModelView>();

    }
}