using System;
using System.Collections.Generic;
using core.dto;
using Microsoft.EntityFrameworkCore.Infrastructure;
using support.domain.ddd;
using support.dto;

namespace core.domain
{
    ///<summary>
    ///Class that represents a Customized Material.
    ///<br> Customized Material is value object;
    ///</summary>
    public class CustomizedMaterial : ValueObject, DTOAble<CustomizedMaterialDTO>
    {
        /// <summary>
        /// Constant that represents the message that ocurrs if the CustomizedMaterial's matterial are not valid.
        /// </summary>
        private const string INVALID_CUSTOMIZED_MATERIAL_MATERIAL = "The CustomizedMaterial's material are not valid!";


        /// <summary>
        /// Constant that represents the message that ocurrs if the CustomizedMaterial's color are not valid.
        /// </summary>
        private const string INVALID_CUSTOMIZED_MATERIAL_COLOR = "The CustomizedMaterial's color are not valid!";

        /// <summary>
        /// Constant that represents the message that ocurrs if the CustomizedMaterial's finish are not valid.
        /// </summary>
        private const string INVALID_CUSTOMIZED_MATERIAL_FINISH = "The CustomizedMaterial's finish are not valid!";

        /// <summary>
        /// Database identifier.
        /// </summary>
        /// <value>Gets/sets the value of the database identifier.</value>
        public long Id { get; internal set; }

        ///<summary>
        ///The CustomizedMaterial's material.
        ///</summary>
        private Material _material;   //!private field used for lazy loading, do not use this for storing or fetching data
        public Material material { get => LazyLoader.Load(this, ref _material); protected set => _material = value; }

        ///<summary>
        ///The CustomizedMaterial's color.
        ///</summary>
        private Color _color;   //!private field used for lazy loading, do not use this for storing or fetching data
        public Color color { get => LazyLoader.Load(this, ref _color); protected set => _color = value; }

        ///<summary>
        ///The CustomizedMaterial's finishe.
        ///</summary>
        private Finish _finish; //!private field used for lazy loading, do not use this for storing or fetching data
        public Finish finish { get => LazyLoader.Load(this, ref _finish); protected set => _finish = value; }

        /// <summary>
        /// LazyLoader injected by the framework.
        /// </summary>
        /// <value>Gets/sets the value of the injected LazyLoader.</value>
        private ILazyLoader LazyLoader { get; set; }

        /// <summary>
        /// Private constructor used for injecting the LazyLoader.
        /// </summary>
        /// <param name="lazyLoader">LazyLoader instance being injected.</param>
        private CustomizedMaterial(ILazyLoader lazyLoader)
        {
            this.LazyLoader = lazyLoader;
        }

        /// <summary>
        /// Empty constructor for ORM.
        /// </summary>
        protected CustomizedMaterial() { }


        ///<summary>
        ///Builds a new instance of CustomizedMaterial, receiving its color and finish.
        ///</summary>
        ///<param name = "material">material with the new CustomizedMaterial's material</param>
        ///<param name = "color">string with the new CustomizedMaterial's color</param>
        ///<param name = "finish">string with the new CustomizedMaterial's finish</param>
        public static CustomizedMaterial valueOf(Material material, Color color, Finish finish)
        {
            return new CustomizedMaterial(material, color, finish);
        }

        ///<summary>
        ///Builds a new instance of CustomizedMaterial, receiving its color.
        ///</summary>
        ///<param name = "material">material with the new CustomizedMaterial's material</param>
        ///<param name = "color">string with the new CustomizedMaterial's color</param>
        public static CustomizedMaterial valueOf(Material material, Color color)
        {
            return new CustomizedMaterial(material, color);
        }

        ///<summary>
        ///Builds a new instance of CustomizedMaterial, receiving its finish.
        ///</summary>
        ///<param name = "material">material with the new CustomizedMaterial's material</param>
        ///<param name = "finish">string with the new CustomizedMaterial's finish</param>
        public static CustomizedMaterial valueOf(Material material, Finish finish)
        {
            return new CustomizedMaterial(material, finish);
        }

        ///<summary>
        ///Builds a new instance of CustomizedMaterial, receiving its color and finish.
        ///</summary>
        ///<param name = "material">material with the new CustomizedMaterial's material</param>
        ///<param name = "color">The new CustomizedMaterial's color</param>
        ///<param name = "finish">The new CustomizedMaterial's finishe</param>
        private CustomizedMaterial(Material material, Color color, Finish finish)
        {
            checkCustomizedMaterialMaterial(material);
            checkCustomizedMaterialColor(material, color);
            checkCustomizedMaterialFinish(material, finish);
            this.material = material;
            this.color = color;
            this.finish = finish;
        }

        ///<summary>
        ///Builds a new instance of CustomizedMaterial, receiving its color.
        ///</summary>
        ///<param name = "material">material with the new CustomizedMaterial's material</param>
        ///<param name = "color">The new CustomizedMaterial's color</param>
        private CustomizedMaterial(Material material, Color color)
        {
            checkCustomizedMaterialMaterial(material);
            checkCustomizedMaterialColor(material, color);
            this.material = material;
            this.color = color;
            this.finish = null;
        }

        ///<summary>
        ///Builds a new instance of ConfiguredMaterial, receiving its finish.
        ///</summary>
        ///<param name = "material">material with the new CustomizedMaterial's material</param>
        ///<param name = "finish">The new ConfiguredMaterial's finishe</param>
        private CustomizedMaterial(Material material, Finish finish)
        {
            checkCustomizedMaterialMaterial(material);
            checkCustomizedMaterialFinish(material, finish);
            this.material = material;
            this.finish = finish;
            this.color = null;
        }

        ///<summary>
        ///Checks if the CustomizedMaterial's material is valid.
        ///</summary>
        ///<param name = "material">The CustomizedMaterial's material</param>
        private void checkCustomizedMaterialMaterial(Material material)
        {
            if (material == null) throw new ArgumentException(INVALID_CUSTOMIZED_MATERIAL_MATERIAL);
        }
        ///<summary>
        ///Checks if the CustomizedMaterial's color is valid.
        ///</summary>
        ///<param name = "color">The CustomizedMaterial's color</param>
        private void checkCustomizedMaterialColor(Material material, Color color)
        {
             if (color == null) throw new ArgumentException(INVALID_CUSTOMIZED_MATERIAL_COLOR);
            if (!material.hasColor(color)) throw new ArgumentException(INVALID_CUSTOMIZED_MATERIAL_COLOR);
        }

        ///<summary>
        ///Checks if the CustomizedMaterial's finish is valid.
        ///</summary>
        ///<param name = "finish">The CustomizedMaterial's finish</param>
        private void checkCustomizedMaterialFinish(Material material, Finish finish)
        {
            if (finish == null) throw new ArgumentException(INVALID_CUSTOMIZED_MATERIAL_FINISH);
            if (!material.hasFinish(finish)) throw new ArgumentException(INVALID_CUSTOMIZED_MATERIAL_FINISH);
        }

        /// <summary>
        /// Updates the finish of a customized material
        /// </summary>
        /// <param name="finish">new finish</param>
        /// <returns>true if the finish was updated successfully</returns>
        public bool changeFinish(Finish finish){
            checkCustomizedMaterialFinish(this.material,finish);
            this.finish = finish;
            return true;
        }

        /// <summary>
        /// Updates the color of a customized material
        /// </summary>
        /// <param name="color">new color</param>
        /// <returns>true if the color was updated successfully</returns>
        public bool changeColor(Color color){
            checkCustomizedMaterialColor(this.material,color);
            this.color = color;
            return true;
        }

        ///<summary>
        ///Returns a textual with the material, color and finish of the Customized Material.
        ///</summary>
        public override string ToString()
        {
            return string.Format("Material: {0}, Color: {1}, Finish {2}", material, color, finish);
        }

        ///<summary>
        ///Returns the generated hash code of the Customized Material.
        ///</summary>
        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = (hashCode * 23) + this.material.GetHashCode();
            hashCode = (hashCode * 23) + this.color.GetHashCode();
            hashCode = (hashCode * 23) + this.finish.GetHashCode();

            return hashCode.GetHashCode();
        }

        ///<summary>
        ///Checks if a certain Customized Material is the same as a received object.
        ///</summary>
        ///<param name = "obj">object to compare to the current Customized Material</param>
        public override bool Equals(object obj)
        {

            if (this == obj)
            {
                return true;
            }

            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                CustomizedMaterial configMaterial = (CustomizedMaterial)obj;

                if (this.color != null && configMaterial.color != null && configMaterial.finish == null)
                {
                    return material.Equals(configMaterial.material) && color.Equals(configMaterial.color);
                }

                if (configMaterial.color == null && configMaterial.finish != null && this.finish != null)
                {
                    return material.Equals(configMaterial.material) && finish.Equals(configMaterial.finish);
                }

                return material.Equals(configMaterial.material) && finish.Equals(configMaterial.finish) && color.Equals(configMaterial.color);
            }
        }

        /// <summary>
        /// Returns the DTO equivalent of the current instance
        /// </summary>
        /// <returns>DTO equivalent of the current instance</returns>
        public CustomizedMaterialDTO toDTO()
        {
            CustomizedMaterialDTO dto = new CustomizedMaterialDTO();
            dto.id = this.Id;
            dto.material = this.material.toDTO();
            dto.color = this.color.toDTO();
            dto.finish = this.finish.toDTO();
            return dto;
        }
    }
}