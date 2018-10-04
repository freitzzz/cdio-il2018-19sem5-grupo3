using core.domain;
using support.dto;

namespace core.dto {
    //TODO: add implementation
    public class RestrictionDTO : DTO, DTOParseable<Restriction, RestrictionDTO> {
        public Restriction toEntity() {
            throw new System.NotImplementedException();
        }
    }
}