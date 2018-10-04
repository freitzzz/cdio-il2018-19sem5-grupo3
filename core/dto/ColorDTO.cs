using System.Runtime.Serialization;
using core.domain;
using support.dto;

namespace core.dto {
    /// <summary>
    /// Represents a Color's Data Tranfer Object.
    /// </summary>
    [DataContract]
    public class ColorDTO : DTO, DTOParseable<Color, ColorDTO> {
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
        public byte red { get; set; }

        /// <summary>
        /// Color's green value.
        /// </summary>
        /// <value>Gets/sets the value of the green value field.</value>
        [DataMember]
        public byte green { get; set; }

        /// <summary>
        /// Color's blue value.
        /// </summary>
        /// <value>Gets/sets the value of the blue value field.</value>
        [DataMember]
        public byte blue { get; set; }

        /// <summary>
        /// Color's alpha value.
        /// </summary>
        /// <value>Gets/sets the value of the alpha value field.</value>
        [DataMember]
        public byte alpha { get; set; }
        /// <summary>
        /// Returns this DTO's equivalent Entity
        /// </summary>
        /// <returns>DTO's equivalent Entity</returns>
        public Color toEntity() {
            Color color = Color.valueOf(name, red, green, blue, alpha);
            color.Id = this.id;
            return color;
        }
    }
}