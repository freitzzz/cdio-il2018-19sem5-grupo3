using core.domain;
using support.dto;

namespace core.dto {
    /// <summary>
    /// DTO of Restriction
    /// </summary>
    public class RestrictionDTO : DTO, DTOParseable<Restriction, RestrictionDTO> {
        /// <summary>
        /// Restriction's id
        /// </summary>
        public long id;
        /// <summary>
        /// Restriction's description
        /// </summary>
        public string description;
        /// <summary>
        /// Returns Entity equivalent of the DTO
        /// </summary>
        /// <returns>Entity equivalent of the DTO</returns>
        public Restriction toEntity() {
            Restriction restriction = new Restriction();
            restriction.Id = id;
            restriction.description = description;
            return restriction;
        }
    }
}