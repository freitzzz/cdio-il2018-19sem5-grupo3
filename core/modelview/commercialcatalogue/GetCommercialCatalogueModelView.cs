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
        /// Commercial Catalogue's name.
        /// </summary>
        /// <value>Gets/sets the name.</value>
        [DataMember]
        public string reference { get; set; }

        /// <summary>
        /// Commercial Catalogue's name.
        /// </summary>
        /// <value>Gets/sets the name.</value>
        [DataMember]
        public string designation { get; set; }

     
    }
}