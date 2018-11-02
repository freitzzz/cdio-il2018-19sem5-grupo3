using core.domain;
using System;
using System.Collections.Generic;

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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a model view with a material information
        /// </summary>
        /// <param name="material">Material with the material being created the model view</param>
        /// <returns>GetMaterialModelView with the material information model view</returns>
        public static GetMaterialModelView fromEntity(Material material){
            throw new NotImplementedException();
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