using System.Runtime.Serialization;
using core.domain;
using support.dto;

namespace core.dto
{
    
    /// <summary>
    /// DTO that represents a Configured Product instance
    /// </summary>
    [DataContract]
    public class ConfiguredProductDTO : DTO,DTOParseable<ConfiguredProduct,ConfiguredProductDTO>
    {
        public ConfiguredProduct toEntity() {
            throw new System.NotImplementedException();
        }
    }
}