
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
        public String Name { get; set; }

        /// <summary>
        /// Respective RGBA coordinates.
        /// </summary>
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
        public int Alpha { get; set; }

        /// <summary>
        /// Minimum for the coordinate color (R/G/B/A).
        /// </summary>
        private static int MIN_VALUE = 0;
        /// <summary>
        /// Maximum for the coordinate color (R/G/B/A).
        /// </summary>
        private static int MAX_VALUE = 255;

        /// <summary>
        /// Constant that represents the message that occurs if the coordinate is bigger than the interval
        /// </summary>
        private static readonly string COORDINATE_BIGGER_THAN_INTERVAL = "The inserted value is bigger than" + MAX_VALUE;

        /// <summary>
        /// Constant that represents the message that occurs if the coordinate is bigger than the interval
        /// </summary>"
        private static readonly string COORDINATE_LOWER_THAN_INTERVAL = "The inserted value is less than" + MIN_VALUE;

        /// <summary>
        /// Returns a new ContinuousDimensionInterval instance
        /// </summary>
        /// <param name="minValue">minimum value of the interval</param>
        /// <param name="maxValue">maximum value of the interval</param>
        /// <param name="increment">increment value of the interval</param>
        /// <returns>ContinuousDimensionInterval instance</returns>
        public static Color valueOf(String name, int red, int green, int blue, int alpha)
        {
            return new Color(name,red,green,blue,alpha);
        }

        public Color() { }

        /// <summary>
        /// Checks if a certain color is the same as the current color.
        /// </summary>
        private Color(String name, int red, int green, int blue, int alpha)
        {
            if (red > MAX_VALUE || green > MAX_VALUE || blue > MAX_VALUE || alpha > MAX_VALUE)
            {
                throw new ArgumentException(COORDINATE_BIGGER_THAN_INTERVAL);
            }

            if (red < MIN_VALUE || green < MIN_VALUE || blue < MIN_VALUE || alpha < MIN_VALUE)
            {
                throw new ArgumentException(COORDINATE_LOWER_THAN_INTERVAL);
            }

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
        /// Equals method of Color.null 
        /// Two objects are the same if the name and RGBA coordinates are the same.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>true if the objects are equal, false if otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Color other = (Color)obj;

            if (!(this.Red == other.Red) || !(this.Blue== other.Blue)||!(this.Green == other.Green)||!(this.Alpha == other.Alpha))
            {
                return false;
            }

            return String.Equals(this.Name, other.Name);
        }
        /// <summary>
        /// toString override method
        /// </summary>
        public override String ToString()
        {
            return "Name: " + Name + " R:" + Red + " G:" + Green + " B:" + Blue + " A:" + Alpha + ".\n";
        }
    }
}