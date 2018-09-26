
using System;
using System.Collections.Generic;
using support.domain.ddd;

namespace core.domain
{
    /// <summary>
    /// Class that represents a Color
    /// <br>Color is an Value Object
    /// </summary>
    /// <typeparam name="name">Generic-Type of the Color identifier</typeparam>
    public class Color : ValueObject
    {
        /// <summary>
        /// String with the color name.
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Respective RGBA coordinates.
        /// </summary>
        public int Red { get; }
        public int Green { get; }
        public int Blue { get; }
        public int Alpha { get; }

        private Color() { }

        /// <summary>
        /// Checks if a certain color is the same as the current color.
        /// </summary>
        public Color(String name, int red, int green, int blue, int alpha)
        {
            Name = name;
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }

        /// <summary>
        /// Checks if a certain color is the same as the current color.
        /// </summary>
        /// <param name="comparingValueObject">string with the comparing product identity</param>
        /// <returns>boolean true if both value objects are the same, false if not</returns>
        public bool sameAs(string comparingEntity) { return Name.Equals(comparingEntity); }

        public override int GetHashCode()
        {
            unchecked
            {
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
        /// toString override method
        /// </summary>
        public override String ToString()
        {
            return "Name: " + Name + " R:" + Red + " G:" + Green + " B:" + Blue + " Alpha:" + Alpha + ".\n";
        }
    }
}