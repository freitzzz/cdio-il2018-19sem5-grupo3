using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using support.domain.ddd;
using support.domain;
using support.utils;
using support.dto;
using core.dto;
using System;

namespace core.domain
{
    /// <summary>
    /// Class that represents a Material.
    /// <br>Material is an entity;
    /// <br>Material is an aggregate root.
    /// </summary>
    /// <typeparam name="string">Generic-Type of the Material entity identifier</typeparam>
    public class Material : Activatable, AggregateRoot<string>, DTOAble<MaterialDTO>
    {
        ///<summary>
        /// Constant that represents the message that ocurrs if the material's reference is not valid.
        ///</summary>
        private const string INVALID_MATERIAL_REFERENCE = "The material's reference is not valid!";

        /// <summary>
        /// Constant that represents the message that ocurrs if the material's designation is not valid.
        /// </summary>
        private const string INVALID_MATERIAL_DESIGNATION = "The material's designation is not valid!";

        /// <summary>
        /// Constant that represents the message that ocurrs if the material's colors are not valid.
        /// </summary>
        private const string INVALID_MATERIAL_COLORS = "The material's colors are not valid!";

        /// <summary>
        /// Constant that represents the message that ocurrs if the material's finishes are not valid.
        /// </summary>
        private const string INVALID_FINISHES_COLORS = "The material's finishes are not valid!";

        /// <summary>
        /// Constant that represents the message that ocurrs if the material's image file name is not valid.
        /// </summary>
        private const string INVALID_MATERIAL_IMAGE_FILE_NAME = "The material's image file name is not valid!";

        public long Id { get; internal set; } //the id should have an internal set, since DTO's have to be able to set them

        /// <summary>
        /// String with the Material's reference.
        /// </summary>
        public string reference { get; protected set; }

        /// <summary>
        /// String with the Material's designation.
        /// </summary>
        public string designation { get; protected set; }

        /// <summary>
        /// String with the Material's image file name.
        /// </summary>
        public string image { get; protected set; }

        /// <summary>
        /// List with all the Material's colors.
        /// </summary>
        private List<Color> _colors;     //!private field used for lazy loading, do not use this for storing or fetching data
        public List<Color> Colors { get => LazyLoader.Load(this, ref _colors); protected set => _colors = value; }

        /// <summary>
        /// List with all the Material's finishes.
        /// </summary>
        private List<Finish> _finishes;  //!private field used for lazy loading, do not use this for storing or fetching data
        public List<Finish> Finishes { get => LazyLoader.Load(this, ref _finishes); protected set => _finishes = value; }

        /// <summary>
        /// Empty constructor used by ORM.
        /// </summary>
        protected Material() { }

        /// <summary>
        /// LazyLoader injected by the framework.
        /// </summary>
        /// <value>Private Gets/Sets the LazyLoader.</value>
        private ILazyLoader LazyLoader { get; set; }

        /// <summary>
        /// Constructor used for injecting the LazyLoader.
        /// </summary>
        /// <param name="lazyLoader">LazyLoader being injected.</param>
        private Material(ILazyLoader lazyLoader)
        {
            this.LazyLoader = lazyLoader;
        }

        /// <summary>
        /// Builds a new instance of Material, receiving its reference and designation.
        /// </summary>
        /// <param name="reference">String with the Material's reference</param>
        /// <param name="designation">String with the Material's designation</param>
        /// <param name="image">String with the Material's image file name</param>
        /// <param name="colors">List with the Material's colors</param>
        /// <param name="finishes">List with the Material's finishes</param>
        public Material(string reference, string designation, string image,
        List<Color> colors, List<Finish> finishes)
        {
            checkIfReferenceIsValid(reference);
            checkIfDesignationIsValid(designation);
            checkIfImageIsValid(image);
            checkIfColorsAreValid(colors);
            checkIfFinishesAreValid(finishes);

            this.reference = reference;
            this.designation = designation;
            this.image = image;
            this.Colors = new List<Color>();
            this.Finishes = new List<Finish>();
            this.Colors.AddRange(colors);
            this.Finishes.AddRange(finishes);
        }

        /// <summary>
        /// Changes the Material's reference.
        /// </summary>
        /// <param name="reference">String with the Material's new reference</param>
        /// <returns>bool true if the change was successful, false if not</returns>
        public bool changeReference(string reference)
        {
            if (Strings.isNullOrEmpty(reference) || this.reference.Equals(reference)) return false;
            this.reference = reference;
            return true;
        }

        /// <summary>
        /// Changes the Material's designation.
        /// </summary>
        /// <param name="designation">String with the Material's new designation</param>
        /// <returns>bool true if the change was successful, false if not</returns>
        public bool changeDesignation(string designation)
        {
            if (Strings.isNullOrEmpty(designation) || this.designation.Equals(designation)) return false;
            this.designation = designation;
            return true;
        }

        /// <summary>
        /// Changes the Material's image file name.
        /// </summary>
        /// <param name="image">String with the Material's new image file name</param>
        /// <returns>bool true if the change was successful, false if not</returns>
        public bool changeImage(string image)
        {
            if (Strings.isNullOrEmpty(image) || !Regex.Match(image, @"^[\w\-. ]+(.png|.jpg|.jpeg|.dds|.gif)$").Success) return false;
            this.image = image;
            return true;
        }

        /// <summary>
        /// Returns the Material's identity.
        /// </summary>
        /// <returns>String with the Material's identity</returns>
        public string id()
        {
            return reference;
        }

        /// <summary>
        /// Adds a new color to the Material's list of colors.
        /// </summary>
        /// <param name="color">Color to add</param>
        /// <returns>bool true if the color is successfully added, false if not</returns>
        public bool addColor(Color color)
        {
            if (color == null || Colors.Contains(color)) return false;
            Colors.Add(color);
            return true;
        }

        /// <summary>
        /// Removes a color from the Material's list of colors.
        /// </summary>
        /// <param name="color">Color to remove</param>
        /// <returns>bool true if the color is successfully removed, false if not</returns>
        public bool removeColor(Color color)
        {
            if (color == null) return false;
            return Colors.Remove(color);
        }

        /// <summary>
        /// Checks if the Material has a certain color on its list of colors.
        /// </summary>
        /// <param name="color">Color to check</param>
        /// <returns>bool true if the color exists, false if not</returns>
        public bool hasColor(Color color)
        {
            if (color == null) return false;
            return Colors.Contains(color);
        }

        /// <summary>
        /// Adds a new finish to the Material's list of finishes.
        /// </summary>
        /// <param name="finish">Finish to add</param>
        /// <returns>bool true if the finish is successfully added, false if not</returns>
        public bool addFinish(Finish finish)
        {
            if (finish == null || Finishes.Contains(finish)) return false;
            Finishes.Add(finish);
            return true;
        }

        /// <summary>
        /// Removes a finish from the Material's list of finishes.
        /// </summary>
        /// <param name="finish">Finish to remove</param>
        /// <returns>bool true if the finish is successfully removed, false if not</returns>
        public bool removeFinish(Finish finish)
        {
            if (finish == null) return false;
            return Finishes.Remove(finish);
        }

        /// <summary>
        /// Checks if the Material has a certain finish on its list of finishes.
        /// </summary>
        /// <param name="finish">Finish to check</param>
        /// <returns>bool true if the finish exists, false if not</returns>
        public bool hasFinish(Finish finish)
        {
            if (finish == null) return false;
            return Finishes.Contains(finish);
        }

        /// <summary>
        /// Checks if the Material's reference is valid.
        /// </summary>
        /// <param name="reference">String with the Material's reference</param>
        private void checkIfReferenceIsValid(string reference)
        {
            if (Strings.isNullOrEmpty(reference)) throw new ArgumentException(INVALID_MATERIAL_REFERENCE);
        }

        /// <summary>
        /// Checks if the Material's designation is valid.
        /// </summary>
        /// <param name="designation">String with the Material's designation</param>
        private void checkIfDesignationIsValid(string designation)
        {
            if (Strings.isNullOrEmpty(designation)) throw new ArgumentException(INVALID_MATERIAL_DESIGNATION);
        }

        /// <summary>
        /// Checks if the Material's image is valid.
        /// </summary>
        /// <param name="image">String with the Material's image file name</param>
        private void checkIfImageIsValid(string image)
        {
            if (Strings.isNullOrEmpty(image) || !Regex.Match(image, @"^[\w\-. ]+(.png|.jpg|.jpeg|.dds|.gif)$").Success) throw new ArgumentException(INVALID_MATERIAL_IMAGE_FILE_NAME);
        }
        
        /// <summary>
        /// Checks if the Material's colors are valid.
        /// </summary>
        /// <param name="colors">List with the Material's colors</param>
        private void checkIfColorsAreValid(List<Color> colors)
        {
            if (Collections.isListNull(colors) || Collections.isListEmpty(colors)) throw new ArgumentException(INVALID_MATERIAL_COLORS);
        }

        /// <summary>
        /// Checks if the Material's finishes are valid.
        /// </summary>
        /// <param name="finishes">List with the Material's finishes</param>
        private void checkIfFinishesAreValid(List<Finish> finishes)
        {
            if (Collections.isListNull(finishes) || Collections.isListEmpty(finishes)) throw new ArgumentException(INVALID_FINISHES_COLORS);
        }

        /// <summary>
        /// Checks if a certain Material's identity is the same as the current Material.
        /// </summary>
        /// <param name="comparingEntity">string with the Material's identity to compare</param>
        /// <returns>bool true if both Materials' identities are the same, false if not</returns>
        public bool sameAs(string comparingEntity)
        {
            return id().Equals(comparingEntity);
        }

        /// <summary>
        /// Returns the current Material as a DTO.
        /// </summary>
        /// <returns>DTO with the current DTO representation of the Material</returns>
        public MaterialDTO toDTO()
        {
            MaterialDTO dto = new MaterialDTO();

            dto.reference = this.reference;
            dto.designation = this.designation;
            dto.image = this.image;
            dto.id = this.Id;

            List<ColorDTO> dtoColors = new List<ColorDTO>();

            if (Colors != null)
            {
                foreach (Color color in Colors)
                {
                    dtoColors.Add(color.toDTO());
                }
                dto.colors = dtoColors;
            }

            List<FinishDTO> dtoFinishes = new List<FinishDTO>();
            if (Finishes != null)
            {
                foreach (Finish finish in Finishes)
                {
                    dtoFinishes.Add(finish.toDTO());
                }
                dto.finishes = dtoFinishes;
            }
            return dto;
        }

        /// <summary>
        /// Returns a textual description of the Material. 
        /// </summary>
        /// <returns>String with the Material's information</returns>
        public override string ToString()
        {
            return string.Format("Designation: {0}, Reference {1}", designation, reference);
        }

        /// <summary>
        /// Returns the generated hash code of the Material.
        /// </summary>
        /// <returns>the generated hash code</returns>
        public override int GetHashCode()
        {
            return reference.GetHashCode();
        }

        /// <summary>
        /// Checks if a certain Material is the same as a received object.
        /// </summary>
        /// <param name="obj">Object to compare to the current Material</param>
        /// <returns>bool true if both objects are equal, false if not</returns>
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
    }
}