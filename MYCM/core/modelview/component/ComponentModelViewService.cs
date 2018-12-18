using System;
using System.Collections.Generic;
using System.Linq;
using core.domain;
using core.modelview.measurement;
using core.modelview.product;
using core.modelview.productcategory;
using core.modelview.productmaterial;
using core.modelview.productslotwidths;
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
        public static GetComponentModelView fromEntity(Component component, string unit)
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
            componentModelView.category = ProductCategoryModelViewService.fromEntityAsBasic(component.complementaryProduct.productCategory);
            if (component.complementaryProduct.components.Any())
            {
                componentModelView.components = ComponentModelViewService.fromCollection(component.complementaryProduct.components);
            }
            //no need to check if the product has materials and measurements, since they're mandatory
            componentModelView.materials = ProductMaterialModelViewService.fromCollection(component.complementaryProduct.productMaterials);
            componentModelView.measurements = MeasurementModelViewService.fromCollection(component.complementaryProduct.productMeasurements.Select(pm => pm.measurement), unit);
            if (component.complementaryProduct.supportsSlots)
            {
                componentModelView.slotWidths = ProductSlotWidthsModelViewService.fromEntity(component.complementaryProduct.slotWidths, unit);
            }
            /*Skip converting Restrictions if the Component has none,
            since null GetAllRestrictionsModelView won't be serialized */
            if (component.restrictions.Any())
            {
                componentModelView.restrictions = RestrictionModelViewService.fromCollection(component.restrictions);
            }
            return componentModelView;
        }

        /// <summary>
        /// Converts an IEnumerable of Component into an instance of GetAllComponentsListModelView.
        /// </summary>
        /// <param name="components">IEnumerable of Component.</param>
        /// <returns>An instance of GetAllComponentsModelView.</returns>        
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when the provided IEnumerable of Component is null.
        /// </exception>
        public static GetAllComponentsListModelView fromCollection(IEnumerable<Component> components)
        {
            if (components == null)
            {
                throw new ArgumentNullException(ERROR_NULL_COMPONENT_COLLECTION);
            }

            GetAllComponentsListModelView allComponentsModelView = new GetAllComponentsListModelView();
            foreach (Component component in components)
            {
                allComponentsModelView.Add(fromEntityAsBasic(component));
            }

            return allComponentsModelView;
        }

        /// <summary>
        /// Converts an IEnumerable of Component into an instance of GetAllComponentsGroupedByCategoryModelView.
        /// </summary>
        /// <param name="components">IEnumerable of Component being converted.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when the provided IEnumerable of Component is null.
        /// </exception>
        public static GetAllComponentsDictionaryModelView fromCollectionGroupedByCategory(IEnumerable<Component> components)
        {
            if (components == null)
            {
                throw new ArgumentNullException(ERROR_NULL_COMPONENT_COLLECTION);
            }

            GetAllComponentsDictionaryModelView allComponentsGroupedByCategoryModelView = new GetAllComponentsDictionaryModelView();

            foreach (Component component in components)
            {
                string categoryName = component.complementaryProduct.productCategory.name;

                if (!allComponentsGroupedByCategoryModelView.ContainsKey(categoryName))
                {
                    allComponentsGroupedByCategoryModelView.Add(categoryName, new GetAllComponentsListModelView());
                }

                GetBasicComponentModelView basicComponentModelView = fromEntityAsBasic(component);

                allComponentsGroupedByCategoryModelView[categoryName].Add(basicComponentModelView);
            }

            return allComponentsGroupedByCategoryModelView;
        }
    }
}