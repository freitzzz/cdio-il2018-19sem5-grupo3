using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using core.dto;
using support.domain.ddd;
using support.dto;

namespace core.domain {
    /// <summary>
    /// Class that represents a Color
    /// <br>Color is an Value Object
    /// </summary>
    /// <typeparam name="name">Generic-Type of the Color identifier</typeparam>
    public class Color : ValueObject, DTOAble<ColorDTO> {
        public long Id { get; set; }

        /// <summary>
        /// String with the color name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Respective RGBA coordinates.
        /// </summary>
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
        public byte Alpha { get; set; }

        /// <summary>
        /// Returns a new ContinuousDimensionInterval instance
        /// </summary>
        /// <param name="minValue">minimum value of the interval</param>
        /// <param name="maxValue">maximum value of the interval</param>
        /// <param name="increment">increment value of the interval</param>
        /// <returns>ContinuousDimensionInterval instance</returns>
        public static Color valueOf(string name, byte red, byte green, byte blue, byte alpha) {
            return new Color(name, red, green, blue, alpha);
        }

        /// <summary>
        /// Empty constructor for ORM.
        /// </summary>
        protected Color() { }

        /// <summary>
        /// Checks if a certain color is the same as the current color.
        /// </summary>
        private Color(string name,byte red, byte green, byte blue, byte alpha) {
            this.Name = name;
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
            this.Alpha = alpha;
        }

        /// <summary>
        /// Checks if a certain color is the same as the current color.
        /// </summary>
        /// <param name="comparingValueObject">string with the comparing product identity</param>
        /// <returns>boolean true if both value objects are the same, false if not</returns>
        public bool sameAs(string comparingEntity) { return Name.Equals(comparingEntity); }

        public override int GetHashCode() {
            unchecked {
                int hashCode = 17;
                hashCode = (hashCode * 23) + (Name == null ? 0 : this.Name.GetHashCode());
                hashCode = (hashCode * 23) + this.Red;
                hashCode = (hashCode * 23) + this.Green;
                hashCode = (hashCode * 23) + this.Blue;
                hashCode = (hashCode * 23) + this.Alpha;
                return hashCode;
            }
        }

        /// <summary>
        /// Equals method of Color.null 
        /// Two objects are the same if the name and RGBA coordinates are the same.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>true if the objects are equal, false if otherwise</returns>
        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }

            Color other = (Color)obj;

            if (!(this.Red == other.Red) || !(this.Blue == other.Blue) || !(this.Green == other.Green) || !(this.Alpha == other.Alpha)) {
                return false;
            }

            return String.Equals(this.Name, other.Name);
        }
        /// <summary>
        /// toString override method
        /// </summary>
        public override String ToString() {
            return "Name: " + Name + " R:" + Red + " G:" + Green + " B:" + Blue + " A:" + Alpha + ".\n";
        }
        /// <summary>
        /// Returns the DTO equivalent of the current instance
        /// </summary>
        /// <returns>DTO equivalent of the current instance</returns>
        public ColorDTO toDTO() {
            ColorDTO dto = new ColorDTO();
            dto.id = this.Id;
            dto.red = this.Red;
            dto.green = this.Green;
            dto.blue = this.Blue;
            dto.alpha = this.Alpha;
            dto.name = this.Name;
            return dto;
        }
    }
}