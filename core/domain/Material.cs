using support.domain.ddd;
using support.utils;
using support.dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
    public class Material : AggregateRoot<string>, DTOAble
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

        /**
        <summary>
            String with the Material's reference.
        </summary>
        */
        [Key]
        private string reference;

        /** 
        <summary>
            String with the Material's designation.
        </summary>
        */
        private string designation;

        /**
        <summary>
            List with all the Material's colors.
        </summary>
         */
        private readonly List<Color> colors = new List<Color>();

        /**
         <summary>
             List with all the Material's finishes.
         </summary>
          */
        private readonly List<Finish> finishes = new List<Finish>();

        /**
        <summary>
            Long with the Material's database ID.
        </summary>
         */
        private long persistence_id { get; set; }

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
            this.colors.AddRange(colors);
            this.finishes.AddRange(finishes);
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
        public void changeReference(string reference)
        {
            if (String.IsNullOrEmpty(reference)) throw new ArgumentException(INVALID_MATERIAL_REFERENCE);
            this.reference = reference;
        }

        /**
            Changes the Material's designation.
         */
        public void changeDesignation(string designation)
        {
            if (String.IsNullOrEmpty(designation)) throw new ArgumentException(INVALID_MATERIAL_DESIGNATION);
            this.designation = designation;
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
            if (color == null || colors.Contains(color)) return false;
            colors.Add(color);
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
            return colors.Remove(color);
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
            return colors.Contains(color);
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
            if (finish == null || finishes.Contains(finish)) return false;
            finishes.Add(finish);
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
            return finishes.Remove(finish);
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
            return finishes.Contains(finish);
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
        public DTO toDTO()
        {
            GenericDTO dto = new GenericDTO(Properties.CONTEXT);

            dto.put(Properties.REFERENCE_PROPERTY, reference);
            dto.put(Properties.DESIGNATION_PROPERTY, designation);
            dto.put(Properties.DATABASE_ID_PROPERTY, persistence_id);

            List<String> dtoColors = new List<String>();
            foreach (Color color in colors)
            {
                dtoColors.Add(color.ToString());
            }
            dto.put(Properties.COLORS_PROPERTY, dtoColors);

            List<String> dtoFinishes = new List<String>();
            foreach (Finish finish in finishes)
            {
                dtoFinishes.Add(finish.ToString());
            }
            dto.put(Properties.FINISHES_PROPERTY, dtoFinishes);

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

