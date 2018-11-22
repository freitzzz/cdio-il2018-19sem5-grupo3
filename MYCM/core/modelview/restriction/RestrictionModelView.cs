using core.domain;
using core.modelview.algorithm;
using System;
using System.Collections.Generic;

namespace core.modelview.restriction{
    /// <summary>
    /// Service for creating model views based on certain restriction contexts
    /// </summary>
    public sealed class RestrictionModelViewService{

        /// <summary>
        /// Creates a model view with a restriction basic information
        /// </summary>
        /// <param name="restriction">Restriction with the restriction being created the model view</param>
        /// <returns>GetBasicRestrictionModelView with the restriction basic information model view</returns>
        public static GetBasicRestrictionModelView fromEntityAsBasic(Restriction restriction){
            GetBasicRestrictionModelView basicRestrictionModelView=new GetBasicRestrictionModelView();
            basicRestrictionModelView.id=restriction.Id;
            basicRestrictionModelView.description=restriction.description;
            basicRestrictionModelView.algorithm = new GetBasicAlgorithmModelView();
            basicRestrictionModelView.algorithm.id = restriction.algorithm;
            return basicRestrictionModelView;
        }

        /// <summary>
        /// Creates a model view with a restriction information
        /// </summary>
        /// <param name="restriction">Restriction with the restriction being created the model view</param>
        /// <returns>GetRestrictionModelView with the restriction information model view</returns>
        public static GetRestrictionModelView fromEntity(Restriction restriction){
            GetRestrictionModelView restrictionModelView=new GetRestrictionModelView();
            restrictionModelView.id=restriction.Id;
            restrictionModelView.description=restriction.description;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a model view with the information about a collection of restrictions
        /// </summary>
        /// <param name="restrictions">IEnumerable with the collection of restrictions</param>
        /// <returns>GetAllRestrictionsModelView with the collection of restrictions model view</returns>
        public static GetAllRestrictionsModelView fromCollection(IEnumerable<Restriction> restrictions){
            GetAllRestrictionsModelView allRestrictionsModelView=new GetAllRestrictionsModelView();
            foreach(Restriction restriction in restrictions)allRestrictionsModelView.Add(fromEntityAsBasic(restriction));
            return allRestrictionsModelView;
        }
    }
}