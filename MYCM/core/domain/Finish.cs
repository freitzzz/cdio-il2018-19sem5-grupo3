using core.dto;
using support.domain.ddd;
using support.dto;
using support.utils;
using System;
using System.ComponentModel.DataAnnotations;

namespace core.domain
{

    /// <summary>
    /// Class that represents a finish of the material.
    /// <br>Finish is a value object.
    /// </summary>
    public class Finish : ValueObject, DTOAble<FinishDTO>
    {

        public long Id { get; internal set; }

        /// <summary>
        /// Constant that represents the message that ocurrs if the description of the Finish is not valid
        /// </summary>
        private static readonly string INVALID_FINISH_DESCRIPTION =
        "The inserted description is not valid!";

        /// <summary>
        /// Constant that represents the message that ocurrs if the shininess of the Finish is not valid
        /// </summary>
        private static readonly string INVALID_FINISH_SHININESS =
        "The inserted shininess is not valid!";

        /// <summary>
        /// String with the description of the Finish
        /// </summary>
        public string description { get; internal set; }

        /// <summary>
        /// Float with the shininess of the Finish
        /// </summary>
        public float shininess { get; internal set; }

        /// <summary>
        /// Empty constructor for ORM.
        /// </summary>
        protected Finish() { }

        /// <summary>
        /// Returns a new instance of Finish, receiving its description and shininess
        /// </summary>
        /// <param name="description">String with the description</param>
        /// <param name="shininess">Float with the shininess</param>
        /// <returns></returns>
        public static Finish valueOf(string description, float shininess)
        {
            return new Finish(description, shininess);
        }

        /// <summary>
        /// Builds a new instance of Finish, receiving its description and shininess
        /// </summary>
        /// <param name="description">String with the description</param>
        /// <param name="shininess">Float with the shininess</param>
        private Finish(string description, float shininess)
        {
            checkDescription(description);
            checkShininess(shininess);

            this.description = description;
            this.shininess = shininess;
        }

        /// <summary>
        /// Checks if the description of the Finish is valid
        /// </summary>
        /// <param name="description">String with the description</param>
        private void checkDescription(string description)
        {
            if (Strings.isNullOrEmpty(description)) throw new ArgumentException(INVALID_FINISH_DESCRIPTION);
        }

        /// <summary>
        /// Checks if the shininess of the Finish is valid
        /// </summary>
        /// <param name="shininess">Float with the shininess</param>
        private void checkShininess(float shininess)
        {
            if (shininess < 0 || shininess > 100) throw new ArgumentException(INVALID_FINISH_SHININESS);
        }
        
         /// <summary>
         /// Checks if a certain Finish is the same as a received object
         /// </summary>
         /// <param name="obj">Object to compare to the current Finish</param>
         /// <returns>bool true if both objects are equal, false if not</returns>
        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            //Check for null and compare run-time types.
            if ((obj == null) || obj.GetType().ToString() != "core.domain.Finish")
            {
                return false;
            }
            else
            {
                Finish finish = (Finish) obj;
                return description.Equals(finish.description) && shininess.Equals(finish.shininess);
            }
        }
        
         /// <summary>
         /// Returns a textual description of the Finish
         /// </summary>
         /// <returns>String with the information of the Finish</returns>
        public override string ToString()
        {
            return string.Format("Description: {0} Shininess: {1}", description, shininess);
        }
        
         /// <summary>
         /// Generates a unique hash code that represents the Finish
         /// </summary>
         /// <returns>The generated hash code</returns>
        public override int GetHashCode()
        {
            return description.GetHashCode() ^ shininess.GetHashCode();
        }

        /// <summary>
        /// Returns the DTO equivalent of the current instance
        /// </summary>
        /// <returns>DTO equivalent of the current instance</returns>
        public FinishDTO toDTO()
        {
            FinishDTO dto = new FinishDTO();
            dto.id = this.Id;
            dto.description = this.description;
            dto.shininess = this.shininess;
            return dto;
        }
    }
}


