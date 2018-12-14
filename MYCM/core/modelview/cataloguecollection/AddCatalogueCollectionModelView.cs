using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.modelview.cataloguecollection
{
    /// <summary>
    /// Class representing a ModelView used for adding a instance of catalogue collection to commercial catalogue.
    /// </summary>
    [DataContract]
    public class AddCatalogueCollectionModelView
    {
        /// <summary>
        /// CommercialCatalogue's persistence identifier.
        /// </summary>
        /// <value>Gets/Sets the persistence identifier.</value>
        [IgnoreDataMember]
        public long commercialCatalogueId { get; set; }

        /// <summary>
        /// Customized Product Collection id
        /// </summary>
        /// <value>Gets/sets the database identifier.</value>
        [DataMember(Name = "collectionId")]
        public long customizedProductCollectionId { get; set; }

        /// <summary>
        /// List of all the Customized Product ids
        /// </summary>
        /// <value>Gets/sets the database identifier.</value>
        [DataMember(Name = "customizedProducts")]
        public List<CustomizedProductIdModelView> customizedProductIds { get; set; } = new List<CustomizedProductIdModelView>();
    }

    /// <summary>
    /// Class representing an object of an id Customized Product.
    /// </summary>
    [DataContract]
    public class CustomizedProductIdModelView
    {
        /// <summary>
        /// Customized Product id
        /// </summary>
        /// <value>Gets/sets the database identifier.</value>
        [DataMember(Name = "id")]
        public long customizedProductId { get; set; }
    }

}