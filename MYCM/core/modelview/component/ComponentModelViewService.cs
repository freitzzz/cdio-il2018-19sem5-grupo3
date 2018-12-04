using System;
using System.Collections.Generic;
using System.Linq;
using core.domain;
using core.modelview.product;
using core.modelview.restriction;

namespace core.modelview.component
{
    public static class ComponentModelViewService
    {
        /// <summary>
        /// Constant representing the message presented when the provided Component is null.
        /// </summary>
        private const string ERROR_NULL_COMPONENT = "The provided component is invalid.";
        /// <summary>
        /// Constant representing the message presented when the provided Collection of Component is null.
        /// </summary>
        private const string ERROR_NULL_COMPONENT_COLLECTION = "The provided component collection is invalid.";


        /// <summary>
        /// Converts an instance of Component into an instance of GetBasicComponentModelView.
        /// </summary>
        /// <param name="component">Instance of Component.</param>
        /// <returns>An instance of GetBasicComponentModelView.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when the provided instance of Component is null.
        /// </exception>
        public static GetBasicComponentModelView fromEntityAsBasic(Component component)
        {
            if (component == null)
            {
                throw new ArgumentNullException(ERROR_NULL_COMPONENT);
            }

            GetBasicComponentModelView basicComponentModelView = new GetBasicComponentModelView();
            basicComponentModelView.productId = component.complementaryProductId;
            basicComponentModelView.reference = component.complementaryProduct.reference;
            basicComponentModelView.designation = component.complementaryProduct.designation;
            basicComponentModelView.modelFilename = component.complementaryProduct.modelFilename;
            basicComponentModelView.supportsSlots = component.complementaryProduct.supportsSlots;
            basicComponentModelView.hasComponents = component.complementaryProduct.components.Any();
            basicComponentModelView.mandatory = component.mandatory;

            return basicComponentModelView;
        }

        /// <summary>
        /// Converts an instance of Component into an instance of GetComponentModelView.
        /// </summary>
        /// <param name="component">Instance of Component.</param>
        /// <returns>An instance of GetComponentModelView.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when the provided instance of Component is null.
        /// </exception>
        public static GetComponentModelView fromEntity(Component component)
        {
            if (component == null)
            {
                throw new ArgumentNullException(ERROR_NULL_COMPONENT);
            }

            GetComponentModelView componentModelView = new GetComponentModelView();
            componentModelView.productId = component.complementaryProductId;
            componentModelView.reference = component.complementaryProduct.reference;
            componentModelView.designation = component.complementaryProduct.designation;
            componentModelView.modelFilename = component.complementaryProduct.modelFilename;
            componentModelView.mandatory = component.mandatory;
            /*Skip converting Restrictions if the Component has none,
            since null GetAllRestrictionsModelView won't be serialized */
            if (component.restrictions.Any())
            {
                componentModelView.restrictions = RestrictionModelViewService.fromCollection(component.restrictions);
            }
            return componentModelView;
        }

        /// <summary>
        /// Converts an IEnumerable of Component into an instance of GetAllComponentsModelView.
        /// </summary>
        /// <param name="components">IEnumerable of Component.</param>
        /// <returns>An instance of GetAllComponentsModelView.</returns>        
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when the provided IEnumerable of Component is null.
        /// </exception>
        public static GetAllComponentsModelView fromCollection(IEnumerable<Component> components)
        {
            if (components == null)
            {
                throw new ArgumentNullException(ERROR_NULL_COMPONENT_COLLECTION);
            }

            GetAllComponentsModelView allComponentsModelView = new GetAllComponentsModelView();
            foreach (Component component in components)
            {
                allComponentsModelView.Add(fromEntityAsBasic(component));
            }

            return allComponentsModelView;
        }
    }
}