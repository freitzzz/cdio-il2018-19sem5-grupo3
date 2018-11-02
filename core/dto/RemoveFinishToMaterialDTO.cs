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
    public class RemoveFinishToMaterialDTO : DTO
    {
        /// <summary>
        /// Material's database identifier.
        /// </summary>
        /// <value></value>
        [DataMember]
        public long id { get; set; }

        /// <summary>
        /// Finish to remove
        /// </summary>
        [DataMember]
        public FinishDTO finishToRemove { get; set; }

    }
}