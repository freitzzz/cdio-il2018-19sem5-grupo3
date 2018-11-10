using System;
using System.Runtime.Serialization;
using core.domain;
using support.dto;

namespace core.dto {
    /// <summary>
    /// Represents a Finish's Data Transfer Object.
    /// </summary>
    [DataContract]
    public class FinishDTO : DTO, DTOParseable<Finish, FinishDTO> {
        /// <summary>
        /// Finish's database identifier.
        /// </summary>
        /// <value>Gets/sets the value of the database identifier field.</value>
        [DataMember]
        public long id { get; set; }

        /// <summary>
        /// Finish's description.
        /// </summary>
        /// <value>Gets/sets the value of the description field.</value>
        [DataMember]
        public string description { get; set; }

        /// <summary>
        /// Returns the Entity equivalent of the DTO
        /// </summary>
        /// <returns>Entity equivalent of the DTO</returns>
        public Finish toEntity() {
            Finish finish = Finish.valueOf(description);
            finish.Id = this.id;
            return finish;
        }
    }
}