using System.Runtime.Serialization;
using core.domain;
using support.dto;

namespace core.dto
{
    [DataContract]
    public class CollectionDTO : DTO, DTOParseable<Collection, CollectionDTO>
    {
        public Collection toEntity()
        {
            throw new System.NotImplementedException();
        }
    }
}