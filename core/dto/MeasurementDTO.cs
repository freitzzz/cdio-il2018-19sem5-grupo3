using System.Runtime.Serialization;
using core.domain;
using support.dto;

namespace core.dto
{
    [DataContract]
    public class MeasurementDTO : DTO, DTOParseable<Measurement, MeasurementDTO>
    {
        [DataMember(Name = "id")]
        public long id { get; set; }

        [DataMember(Name = "height")]
        public DimensionDTO height { get; set; }

        [DataMember(Name = "width")]
        public DimensionDTO width { get; set; }

        [DataMember(Name = "depth")]
        public DimensionDTO depth { get; set; }

        public Measurement toEntity()
        {
            throw new System.NotImplementedException();
        }
    }
}