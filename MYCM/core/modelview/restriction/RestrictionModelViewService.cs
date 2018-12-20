using core.domain;
using core.modelview.algorithm;
using System;
using System.Collections.Generic;

namespace core.modelview.restriction
{
    /// <summary>
    /// Service for creating model views based on certain restriction contexts
    /// </summary>
    public static class RestrictionModelViewService
    {
        /// <summary>
        /// Constant representing the message presented when the provided instance of Restriction is null.
        /// </summary>
        private const string NULL_RESTRICTION = "Unable to convert the provided restriction into a view.";

        /// <summary>
        /// Constant representing the message presented when the provided IEnumerable of Restriction is null.
        /// </summary>
        private const string NULL_RESTRICTION_COLLECTION = "Unable to convert the provided restrictions into views.";

        /// <summary>
        /// Creates a model view with a restriction basic information
        /// </summary>
        /// <param name="restriction">Restriction with the restriction being created the model view</param>
        /// <returns>GetBasicRestrictionModelView with the restriction basic information model view</returns>
        public static GetBasicRestrictionModelView fromEntityAsBasic(Restriction restriction)
        {
            if (restriction == null)
            {
                throw new ArgumentNullException(NULL_RESTRICTION);
            }

            GetBasicRestrictionModelView basicRestrictionModelView = new GetBasicRestrictionModelView();
            basicRestrictionModelView.id = restriction.Id;
            basicRestrictionModelView.description = restriction.description;
            basicRestrictionModelView.algorithm = AlgorithmModelViewService.fromEntityAsBasic(restriction.algorithm);
            return basicRestrictionModelView;
        }

        /// <summary>
        /// Creates a model view with a restriction information
        /// </summary>
        /// <param name="restriction">Restriction with the restriction being created the model view</param>
        /// <returns>GetRestrictionModelView with the restriction information model view</returns>
        public static GetRestrictionModelView fromEntity(Restriction restriction)
        {
            if (restriction == null)
            {
                throw new ArgumentNullException(NULL_RESTRICTION);
            }

            GetRestrictionModelView restrictionModelView = new GetRestrictionModelView();
            restrictionModelView.id = restriction.Id;
            restrictionModelView.description = restriction.description;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a model view with the information about a collection of restrictions
        /// </summary>
        /// <param name="restrictions">IEnumerable with the collection of restrictions</param>
        /// <returns>GetAllRestrictionsModelView with the collection of restrictions model view</returns>
        public static GetAllRestrictionsModelView fromCollection(IEnumerable<Restriction> restrictions)
        {
            if (restrictions == null)
            {
                throw new ArgumentNullException(NULL_RESTRICTION_COLLECTION);
            }

            GetAllRestrictionsModelView allRestrictionsModelView = new GetAllRestrictionsModelView();
            foreach (Restriction restriction in restrictions) allRestrictionsModelView.Add(fromEntityAsBasic(restriction));
            return allRestrictionsModelView;
        }
    }
}