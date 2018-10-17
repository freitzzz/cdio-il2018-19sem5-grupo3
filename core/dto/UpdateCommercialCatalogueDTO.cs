using System.Collections.Generic;
using System.Runtime.Serialization;

namespace core.dto
{
    public class UpdateCommercialCatalogueDTO
    {
        /// <summary>
        /// ID of the product being updated
        /// </summary>
        [DataMember]
        public long id { get; set; }

        /// <summary>
        /// List with the catalogue Collection being added
        /// </summary>
        [DataMember(Name = "addCatalogueCollections")]
        public List<CatalogueCollectionDTO> catalogueCollectionDTOToAdd { get; set; }

        /// <summary>
        /// List with the catalogue Collection being removed
        /// </summary>
        [DataMember(Name = "removeCatalogueCollections")]
        public List<CatalogueCollectionDTO> catalogueCollectionDTOToRemove { get; set; }

        /// <summary>
        /// String with the product reference being updated
        /// </summary>
        [DataMember]
        public string reference{get;set;}
        
        /// <summary>
        /// String with the product designation being updated
        /// </summary>
        [DataMember]
        public string designation{get;set;}

    }
}