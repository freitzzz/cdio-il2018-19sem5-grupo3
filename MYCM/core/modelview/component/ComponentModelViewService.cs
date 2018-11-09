using core.domain;
using core.modelview.restriction;
using System;
using System.Collections.Generic;

namespace core.modelview.component{
    /// <summary>
    /// Service for creating model views based on certain component contexts
    /// </summary>
    public sealed class ComponentModelViewService{

        /// <summary>
        /// Creates a model view with a component basic information
        /// </summary>
        /// <param name="component">Component with the component being created the model view</param>
        /// <returns>GetBasicComponentModelView with the component basic information model view</returns>
        public static GetBasicComponentModelView fromEntityAsBasic(Component component){
            GetBasicComponentModelView basicComponentModelView=new GetBasicComponentModelView();
            basicComponentModelView.id=component.complementedProductId;
            basicComponentModelView.fatherProductID=component.fatherProductId;
            basicComponentModelView.mandatory=component.mandatory;
            return basicComponentModelView;
        }

        /// <summary>
        /// Creates a model view with a component information
        /// </summary>
        /// <param name="component">Component with the component being created the model view</param>
        /// <returns>GetComponentModelView with the component information model view</returns>
        public static GetComponentModelView fromEntity(Component component){
            GetComponentModelView componentModelView=new GetComponentModelView();
            componentModelView.id=component.complementedProductId;
            componentModelView.fatherProductID=component.fatherProductId;
            componentModelView.mandatory=component.mandatory;
            componentModelView.restrictions=RestrictionModelViewService.fromCollection(component.restrictions);
            return componentModelView;
        }

        /// <summary>
        /// Creates a model view with the information about a collection of components
        /// </summary>
        /// <param name="components">IEnumerable with the collection of components</param>
        /// <returns>GetAllComponentsModelView with the collection of components model view</returns>
        public static GetAllComponentsModelView fromCollection(IEnumerable<Component> components){
            GetAllComponentsModelView allComponentsModelView=new GetAllComponentsModelView();
            foreach(Component component in components)allComponentsModelView.Add(fromEntityAsBasic(component));
            return allComponentsModelView;
        }
    }
}