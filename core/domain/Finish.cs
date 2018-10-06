using core.dto;
using support.domain.ddd;
using support.dto;
using support.utils;
using System;
using System.ComponentModel.DataAnnotations;

namespace core.domain {
    /**
    <summary>
        Class that represents a finish of the material.
        <br>Finish is a value object.
    </summary>
    */
    public class Finish : ValueObject, DTOAble<FinishDTO> {

        public long Id { get; internal set;}

        /**
        <summary>
            Constant that represents the message that ocurrs if the Finish's description is not valid.
        </summary>
        */
        private static readonly string INVALID_FINISH_DESCRIPTION =
        "The Finish's description is not valid!";
        /**
        <summary>
            String with the Finish's description
        </summary>
        */
        public string description { get; internal set; }

        /// <summary>
        /// Empty constructor for ORM.
        /// </summary>
        protected Finish(){}

        /**
        <summary>
            Builds a new instance of Finish, receiving its description.
        </summary>
        <param name = "description">string with the new Finish's description</param>
         */
        public static Finish valueOf(string description) {
            return new Finish(description);
        }
        /**
        <summary>
            Builds a new instance of Finish, receiving its description.
        </summary>
        <param name = "description">string with the new Finish's description</param>
         */
        private Finish(string description) {
            checkDescription(description);
            this.description = description;
        }
        /**
        <summary>
            Checks if the Finish's description are valid.
        </summary>
        <param name = "description">String with the Finish's description</param>
        */
        private void checkDescription(string description) {
            if (Strings.isNullOrEmpty(description)) throw new
            ArgumentException(INVALID_FINISH_DESCRIPTION);
        }

        /**
        <summary>
            Checks if a certain Finish is the same as a received object.
        </summary>
        <param name = "obj">object to compare to the current Finish</param>
         */
        public override bool Equals(object obj) {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType())) {
                return false;
            } else {
                Finish finish = (Finish)obj;
                return description.Equals(finish.description);
            }
        }
        /**
        <summary>
            Returns a description of the Finish.
        </summary>
         */
        public override string ToString() {
            return string.Format("Description: {0}", description);
        }

        /**
        <summary>
            Returns the generated hash code of the Finish.
        </summary>
         */
        public override int GetHashCode() {
            return description.GetHashCode();
        }
        /// <summary>
        /// Returns the DTO equivalent of the current instance
        /// </summary>
        /// <returns>DTO equivalent of the current instance</returns>
        public FinishDTO toDTO() {
            FinishDTO dto = new FinishDTO();
            dto.id = this.Id;
            dto.description = this.description;
            return dto;
        }
    }
}


