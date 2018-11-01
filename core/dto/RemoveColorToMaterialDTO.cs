using support.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
namespace core.dto
{
    [DataContract]
    /// <summary>
    /// Simple DTO class used to transpor data required for material updates
    /// </summary>
    public class RemoveColorToMaterialDTO : DTO
    {
        /// <summary>
        /// Material's database identifier.
        /// </summary>
        /// <value></value>
        [DataMember]
        public long id { get; set; }

        /// <summary>
        /// Color to remove
        /// </summary>
        [DataMember]
        public ColorDTO colorToRemove { get; set; }

    }
}