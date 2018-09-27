using support.domain.ddd;
using support.utils;
using System;
using System.ComponentModel.DataAnnotations;

namespace core.domain
{
    /**
    <summary>
        Class that represents a Material.
        <br>Material is an entity;
        <br>Material is an aggregate root.
    </summary>
    <typeparam name="string">Generic-Type of the Material entity identifier</typeparam>
    */
    public class Material : AggregateRoot<string>
    {
        /**
        <summary>
            String with the Material's reference
        </summary>
        */
        [Key]
        private readonly string reference;

        /** 
        <summary>
            String with the Material's designation
        </summary>
        */
        private readonly string designation;

        /**
        <summary>
            Constant that represents the message that ocurrs if the Material's reference is not valid.
        </summary>
        */
        private static readonly string INVALID_MATERIAL_REFERENCE = "The Material's reference is not valid!";

        /**
        <summary>
            Constant that represents the message that ocurrs if the Material's designation is not valid.
        </summary>
        */
        private static readonly string INVALID_MATERIAL_DESIGNATION = "The Material's designation is not valid!";


        /**
        <summary>
            Builds a new instance of Material, receiving its reference and designation.
        </summary>
        <param name = "reference">string with the new Material's reference</param>
        <param name = "designation">string with the new Material's designation</param>
         */
        public Material(string reference, string designation)
        {
            checkMaterialProperties(reference, designation);
            this.reference = reference;
            this.designation = designation;
        }

        /**
        <summary>
            Checks if the Material's reference and designation are valid.
        </summary>
        <param name = "reference">String with the Material's reference</param>
        <param name = "designation">String with the Material's designation</param>
        */
        private void checkMaterialProperties(string reference, string designation)
        {
            if (Strings.isNullOrEmpty(reference)) throw new ArgumentException(INVALID_MATERIAL_REFERENCE);
            if (Strings.isNullOrEmpty(designation)) throw new ArgumentException(INVALID_MATERIAL_DESIGNATION);
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
            Checks if a certain Material's identity is the same as the current Material.
        </summary>
        <param name = "comparingEntity">string with the Material's identity to compare</param>
        <returns>True if both Materials' identities are the same, false if not</returns>
        */
        public bool sameAs(string comparingEntity)
        {
            return id().Equals(comparingEntity);
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
    }
}

