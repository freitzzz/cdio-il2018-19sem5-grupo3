using System.Runtime.Serialization;
using core.domain;
using support.dto;

namespace core.dto
{
    
    /// <summary>
    /// DTO that represents a Configured Product instance
    /// </summary>
    [DataContract]
    public class CustomizedProductDTO : DTO,DTOParseable<CustomizedProduct,CustomizedProductDTO>
    {
        public CustomizedProduct toEntity() {
            throw new System.NotImplementedException();
        }
    }
}