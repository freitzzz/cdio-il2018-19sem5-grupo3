using System.Collections.Generic;
using System.Runtime.Serialization;
using core.domain;
using support.dto;

namespace core.dto {
    /// <summary>
    /// Represents a Material's Data Transfer Object. 
    /// </summary>
    [DataContract]
    public class MaterialDTO : DTO, DTOParseable<Material, MaterialDTO> {
        /// <summary>
        /// Material's database identifier.
        /// </summary>
        /// <value></value>
        [DataMember]
        public long id { get; set; }

        /// <summary>
        /// Material's reference.
        /// </summary>
        /// <value>Gets/sets the value of the reference field.</value>
        [DataMember]
        public string reference { get; set; }

        /// <summary>
        /// Material's designation.
        /// </summary>
        /// <value>Gets/sets the value of the designation field.</value>
        [DataMember]
        public string designation { get; set; }

        /// <summary>
        /// Material's list of available colors.
        /// </summary>
        /// <value>Gets/sets the value of the colors field.</value>
        [DataMember]
        public List<ColorDTO> colors { get; set; }

        /// <summary>
        /// Material's list of available finishes.
        /// </summary>
        /// <value>Gets/sets the value of the finishes field.</value>
        [DataMember]
        public List<FinishDTO> finishes { get; set; }
        /// <summary>
        /// Returns this DTO's equivalent Entity
        /// </summary>
        /// <returns>DTO's equivalent Entity</returns>
        public Material toEntity() {

            List<Color> colors = new List<Color>();

            foreach(ColorDTO dto in this.colors){
                colors.Add(dto.toEntity());
            }

            List<Finish> finishes = new List<Finish>();

            foreach(FinishDTO dto in this.finishes){
                finishes.Add(dto.toEntity());
            }

            Material material = new Material(this.reference, this.designation, colors, finishes);
            material.Id = this.id;

            return material;
        }
    }
}