using support.domain.ddd;
using support.utils;
using support.dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using core.dto;

namespace core.domain
{
    /**
    <summary>
        Class that represents a Material.
        <br>Material is an entity;
        <br>Material is an aggregate root.
    </summary>
    <typeparam name = "string">Generic-Type of the Material entity identifier</typeparam>
    */
    public class Material : AggregateRoot<string>, DTOAble<MaterialDTO>
    {
        /**
        <summary>
            Constant that represents the message that ocurrs if the Material's reference is not valid.
        </summary>
        */
        private const string INVALID_MATERIAL_REFERENCE = "The Material's reference is not valid!";

        /**
        <summary>
            Constant that represents the message that ocurrs if the Material's designation is not valid.
        </summary>
        */
        private const string INVALID_MATERIAL_DESIGNATION = "The Material's designation is not valid!";

        /**
        <summary>
            Constant that represents the message that ocurrs if the Material's colors are not valid.
        </summary>
         */
        private const string INVALID_MATERIAL_COLORS = "The Material's colors are not valid!";

        /**
        <summary>
            Constant that represents the message that ocurrs if the Material's finishes are not valid.
        </summary>
         */
        private const string INVALID_FINISHES_COLORS = "The Material's finishes are not valid!";

        public long Id { get; internal set; } //the id should have an internal set, since DTO's have to be able to set them

        /**
        <summary>
            String with the Material's reference.
        </summary>
        */
        public string reference { get; protected set; }

        /** 
        <summary>
            String with the Material's designation.
        </summary>
        */
        public string designation { get; protected set; }

        /**
        <summary>
            List with all the Material's colors.
        </summary>
        **/
        public virtual List<Color> Colors { get; protected set; }

        /**
         <summary>
             List with all the Material's finishes.
         </summary>
         **/
        public virtual List<Finish> Finishes { get; protected set; }
        /// <summary>
        /// Boolean that controls if the current material is available or not
        /// </summary>
        public bool isAvailable { get; protected set; }


        /// <summary>
        /// Empty constructor used by ORM.
        /// </summary>
        protected Material() { }

        /**
        <summary>
            Builds a new instance of Material, receiving its reference and designation.
        </summary>
        <param name = "reference">string with the new Material's reference</param>
        <param name = "designation">string with the new Material's designation</param>
        <param name = "colors">List with the new Material's colors</param>
        <param name = "finishes">List with the new Material's finishes</param>
         */
        public Material(string reference, string designation,
        List<Color> colors, List<Finish> finishes)
        {
            checkMaterialProperties(reference, designation, colors, finishes);
            this.reference = reference;
            this.designation = designation;
            this.Colors = new List<Color>();
            this.Finishes = new List<Finish>();
            this.Colors.AddRange(colors);
            this.Finishes.AddRange(finishes);
            isAvailable = true;
        }

        /**
        <summary>
            Checks if the Material's properties are valid.
        </summary>
        <param name = "reference">String with the Material's reference</param>
        <param name = "designation">String with the Material's designation</param>
        <param name = "colors">List with the Material's colors</param>
        <param name = "finishes">List with the Material's finishes</param>
        */
        private void checkMaterialProperties(string reference,
        string designation, List<Color> colors, List<Finish> finishes)
        {
            if (Strings.isNullOrEmpty(reference)) throw new ArgumentException(INVALID_MATERIAL_REFERENCE);
            if (Strings.isNullOrEmpty(designation)) throw new ArgumentException(INVALID_MATERIAL_DESIGNATION);
            if (Collections.isListNull(colors) || Collections.isListEmpty(colors)) throw new ArgumentException(INVALID_MATERIAL_COLORS);
            if (Collections.isListNull(finishes) || Collections.isListEmpty(finishes)) throw new ArgumentException(INVALID_FINISHES_COLORS);
        }

        /**
            Changes the Material's reference.
         */
        public bool changeReference(string reference)
        {
            if(Strings.isNullOrEmpty(reference)||this.reference.Equals(reference))return false;
            this.reference = reference;
            return true;
        }

        /**
            Changes the Material's designation.
         */
        public bool changeDesignation(string designation)
        {
            if(Strings.isNullOrEmpty(designation)||this.designation.Equals(designation))return false;
            this.designation = designation;
            return true;
        }

        /// <summary>
        /// Disables the current material
        /// </summary>
        /// <returns>boolean true if the material was disabled with success, false if not</returns>
        public bool disable()
        {
            if (!isAvailable) return false;
            isAvailable = false;
            return true;
        }
        /**
        <summary>
            Returns the Material's identity.
        </summary>
        <returns>String with the Material's identity</returns>
         */
        public string id()
        {
            return reference;
        }

        /**
        <summary>
            Adds a new color to the Material's list of colors.
        </summary>
        <param name = "color">Color to add</param>
        <returns>True if the color is successfully added, false if not</returns>
        */
        public bool addColor(Color color)
        {
            if (color == null || Colors.Contains(color)) return false;
            Colors.Add(color);
            return true;
        }

        /**
        <summary>
            Removes a color from the Material's list of colors.
        </summary>
        <param name = "color">Color to remove</param>
        <returns>True if the color is successfully removed, false if not</returns>
        */
        public bool removeColor(Color color)
        {
            if (color == null) return false;
            return Colors.Remove(color);
        }

        /**
        <summary>
            Checks if the Material has a certain color on its list of colors.
        </summary>
        <param name = "color">Color to check</param>
        <returns>True if the color exists, false if not</returns>
        */
        public bool hasColor(Color color)
        {
            if (color == null) return false;
            return Colors.Contains(color);
        }

        /**
        <summary>
            Adds a new finish to the Material's list of finishes.
        </summary>
        <param name = "finish">Finish to add</param>
        <returns>True if the finish is successfully added, false if not</returns>
        */
        public bool addFinish(Finish finish)
        {
            if (finish == null || Finishes.Contains(finish)) return false;
            Finishes.Add(finish);
            return true;
        }

        /**
        <summary>
            Removes a finish from the Material's list of finishes.
        </summary>
        <param name = "finish">Finish to remove</param>
        <returns>True if the finish is successfully removed, false if not</returns>
        */
        public bool removeFinish(Finish finish)
        {
            if (finish == null) return false;
            return Finishes.Remove(finish);
        }

        /**
        <summary>
            Checks if the Material has a certain finish on its list of finishes.
        </summary>
        <param name = "color">Finish to check</param>
        <returns>True if the finish exists, false if not</returns>
        */
        public bool hasFinish(Finish finish)
        {
            if (finish == null) return false;
            return Finishes.Contains(finish);
        }


        /**
        <summary>
            Checks if a certain Material's identity is the same as the current Material.
        </summary>
        <param name = "comparingEntity">string with the Material's identity to compare</param>
        <returns>True if both Materials' identities are the same, false if not</returns>
        */
        public bool sameAs(string comparingEntity)
        {
            return id().Equals(comparingEntity);
        }

        /** <summary>
            Returns the current Material as a DTO.
        </summary>
        <returns>DTO with the current DTO representation of the Material</returns>
        */
        public MaterialDTO toDTO()
        {
            MaterialDTO dto = new MaterialDTO();

            dto.reference = this.reference;
            dto.designation = this.designation;
            dto.id = this.Id;

            List<ColorDTO> dtoColors = new List<ColorDTO>();
            foreach (Color color in Colors)
            {
                dtoColors.Add(color.toDTO());
            }
            dto.colors = dtoColors;

            List<FinishDTO> dtoFinishes = new List<FinishDTO>();
            foreach (Finish finish in Finishes)
            {
                dtoFinishes.Add(finish.toDTO());
            }
            dto.finishes = dtoFinishes;

            return dto;
        }

        /**
        <summary>
            Returns a textual description of the Material.
        </summary>
         */
        public override string ToString()
        {
            return string.Format("Designation: {0}, Reference {1}", designation, reference);
        }

        /**
        <summary>
            Returns the generated hash code of the Material.
        </summary>
         */
        public override int GetHashCode()
        {
            return reference.GetHashCode();
        }

        /**
        <summary>
            Checks if a certain Material is the same as a received object.
        </summary>
        <param name = "obj">object to compare to the current Material</param>
         */
        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Material material = (Material)obj;
                return reference.Equals(material.reference);
            }
        }

        MaterialDTO DTOAble<MaterialDTO>.toDTO()
        {
            throw new NotImplementedException();
        }

        /**
        <summary>
            Inner static class which represents the Material's properties used to map on data holders (e.g. DTO)
        </summary>
         */
        public static class Properties
        {
            /**
           <summary>
                Constant that represents the context of the Properties.
            </summary>
            */
            public const string CONTEXT = "Material";

            /**
            <summary>
                Constant that represents the name of the Property which maps the Material's database ID.
            </summary>
             */
            public const string DATABASE_ID_PROPERTY = "id";

            /**
            <summary>
                Constant that represents the name of the Property which maps the Material's reference.
            </summary>
             */
            public const string REFERENCE_PROPERTY = "reference";

            /**
            <summary>
                Constant that represents the name of the Property which maps the Material's designation.
            </summary>
             */
            public const string DESIGNATION_PROPERTY = "designation";

            /**
            <summary>
                Constant that represents the name of the Property which maps the Material's colors.
            </summary>
             */
            public const string COLORS_PROPERTY = "colors";

            /**
            <summary>
                Constant that represents the name of the Property which maps the Material's finishes.
            </summary>
             */
            public const string FINISHES_PROPERTY = "finishes";
        }
    }
}

