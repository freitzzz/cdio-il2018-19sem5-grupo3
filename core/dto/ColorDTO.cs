using System.Runtime.Serialization;
using core.domain;
using support.dto;

namespace core.dto
{
    /// <summary>
    /// Represents a Color's Data Tranfer Object.
    /// </summary>
    [DataContract]
    public class ColorDTO : DTO, DTOParseable<Color, ColorDTO>
    {
        /// <summary>
        /// Color's database identifier.
        /// </summary>
        /// <value>Gets/sets the value of the database identifier field.</value>
        [DataMember]
        public long id { get; set; }

        /// <summary>
        /// Color's name.
        /// </summary>
        /// <value>Gets/sets the value of the name field.</value>
        [DataMember]
        public string name { get; set; }

        /// <summary>
        /// Color's red value.
        /// </summary>
        /// <value>Gets/sets the value of the red value field.</value>
        [DataMember]
        public int red { get; set; }

        /// <summary>
        /// Color's green value.
        /// </summary>
        /// <value>Gets/sets the value of the green value field.</value>
        [DataMember]
        public int green { get; set; }

        /// <summary>
        /// Color's blue value.
        /// </summary>
        /// <value>Gets/sets the value of the blue value field.</value>
        [DataMember]
        public int blue { get; set; }

        /// <summary>
        /// Color's alpha value.
        /// </summary>
        /// <value>Gets/sets the value of the alpha value field.</value>
        [DataMember]
        public int alpha { get; set; }

        public Color toEntity()
        {
            throw new System.NotImplementedException();
        }
    }
}