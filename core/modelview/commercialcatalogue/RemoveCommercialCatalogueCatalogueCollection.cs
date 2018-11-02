using System.Runtime.Serialization;
namespace core.modelview.commercialcatalogue
{
    /// <summary>
    /// Class representing a ModelView used for removing a instance of a Catalogue Collection.
    /// </summary>
    [DataContract]

    public class RemoveCommercialCatalogueCatalogueCollection
    {
         /// <summary>
        /// Commercial Catalogue reference.
        /// </summary>
        /// <value>Gets/sets the reference.</value>
        [DataMember]
        long  commercialCatalogueCatalogueCollectionId {get; set;}
    }
}