
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
        string reference {get; set;}
        
        /// <summary>
        /// Commercial Catalogue designation.
        /// </summary>
        /// <value>Gets/sets the designation.</value>
        [DataMember]
        string designation {get; set;}
    }
}