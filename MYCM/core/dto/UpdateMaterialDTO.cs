using support.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace core.dto {
    [DataContract]
    /// <summary>
    /// Simple DTO class used to transpor data required for material updates
    /// </summary>
    public class UpdateMaterialDTO : DTO {
        /// <summary>
        /// Material's database identifier.
        /// </summary>
        /// <value></value>
        [DataMember]
        public long id { get; set; }

        /// <summary>
        /// Material's reference.
        /// </summary>
        /// <value>Gets/sets the value of the reference field.</value>
        [DataMember]
        public string reference { get; set; }

        /// <summary>
        /// Material's designation.
        /// </summary>
        /// <value>Gets/sets the value of the designation field.</value>
        [DataMember]
        public string designation { get; set; }

        /// <summary>
        /// Material's image file name.
        /// </summary>
        /// <value>Gets/sets the value of the image field.</value>
        [DataMember]
        public string image {get; set;}

        /// <summary>
        /// List of colors to add
        /// </summary>
        [DataMember]
        public List<ColorDTO> colorsToAdd { get; set; }

        /// <summary>
        /// List of finishes to add
        /// </summary>
        [DataMember]
        public List<FinishDTO> finishesToAdd { get; set; }

        /// <summary>
        /// List of colors to remove
        /// </summary>
        [DataMember]
        public List<ColorDTO> colorsToRemove { get; set; }

        /// <summary>
        /// List of finishes to remove
        /// </summary>
        [DataMember]
        public List<FinishDTO> finishesToRemove { get; set; }
    }
}
