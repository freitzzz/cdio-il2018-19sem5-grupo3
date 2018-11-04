using core.domain;
using System;
using System.Collections.Generic;

namespace core.modelview.dimension{
    /// <summary>
    /// Service for creating model views based on certain dimension contexts
    /// </summary>
    public sealed class DimensionModelViewService{
        
        /// <summary>
        /// Creates a model view with a dimension information
        /// </summary>
        /// <param name="dimension">Dimension with the dimension being created the model view</param>
        /// <returns>GetDimensionModelView with the dimension information model view</returns>
        public static GetDimensionModelView fromEntity(Dimension dimension){
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a model view with the information about a collection of dimension
        /// </summary>
        /// <param name="dimensions">IEnumerable with the collection of dimensions</param>
        /// <returns>GetAllDimensionsModelView with the collection of dimensions model view</returns>
        public static GetAllDimensionsModelView fromCollection(IEnumerable<Dimension> dimensions){
            GetAllDimensionsModelView allDimensionsModelView=new GetAllDimensionsModelView();
            foreach(Dimension dimension in dimensions)allDimensionsModelView.Add(fromEntity(dimension));
            return allDimensionsModelView;
        }
    }
}