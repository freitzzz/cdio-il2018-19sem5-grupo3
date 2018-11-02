using support.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
namespace core.dto
{
    [DataContract]
    public class AddFinishToMaterialDTO : DTO
    {
    
    /// <summary>
    /// Simple DTO class used to transpor data required for material updates
    /// </summary>
        /// <summary>
        /// Material's database identifier.
        /// </summary>
        /// <value></value>
        [DataMember]
        public long id { get; set; }

        /// <summary>
        /// Finish to add
        /// </summary>
        [DataMember]
        public FinishDTO finishToAdd { get; set; }

    }
}