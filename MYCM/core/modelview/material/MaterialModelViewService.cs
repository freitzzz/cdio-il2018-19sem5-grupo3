using core.domain;
using support.dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace core.modelview.material{
    /// <summary>
    /// Service for creating model views based on certain material contexts
    /// </summary>
    public sealed class MaterialModelViewService{

        /// <summary>
        /// Creates a model view with a material basic information
        /// </summary>
        /// <param name="material">Material with the material being created the model view</param>
        /// <returns>GetBasicMaterialModelView with the material basic information model view</returns>
        public static GetBasicMaterialModelView fromEntityAsBasic(Material material){
            GetBasicMaterialModelView basicMaterialModelView=new GetBasicMaterialModelView();
            basicMaterialModelView.id=material.Id;
            basicMaterialModelView.reference=material.reference;
            basicMaterialModelView.designation=material.designation;
            basicMaterialModelView.imageFilename=material.image;
            return basicMaterialModelView;
        }

        /// <summary>
        /// Creates a model view with a material information
        /// </summary>
        /// <param name="material">Material with the material being created the model view</param>
        /// <returns>GetMaterialModelView with the material information model view</returns>
        public static GetMaterialModelView fromEntity(Material material){
            GetMaterialModelView materialModelView=new GetMaterialModelView();
            materialModelView.id=material.Id;
            materialModelView.reference=material.reference;
            materialModelView.designation=material.designation;
            materialModelView.imageFilename=material.image;
            materialModelView.colors=DTOUtils.parseToDTOS(material.Colors).ToList();
            materialModelView.finishes=DTOUtils.parseToDTOS(material.Finishes).ToList();
            return materialModelView;
        }

        /// <summary>
        /// Creates a model view with the information about a collection of materials
        /// </summary>
        /// <param name="materials">IEnumerable with the collection of materials</param>
        /// <returns>GetAllMaterialsModelView with the collection of materials model view</returns>
        public static GetAllMaterialsModelView fromCollection(IEnumerable<Material> materials){
            GetAllMaterialsModelView allMaterialsModelView=new GetAllMaterialsModelView();
            foreach(Material material in materials)allMaterialsModelView.Add(fromEntityAsBasic(material));
            return allMaterialsModelView;
        }
    }
}