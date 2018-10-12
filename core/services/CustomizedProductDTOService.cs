using core.domain;
using core.dto;
using core.persistence;
using System;
using System.Collections.Generic;

namespace core.services{

    /// <summary>
    /// Service class that helps the transformation of CustomizedProductDTO into CustomizedProduct since some information needs to be accessed on the persistence context
    /// </summary>
    public sealed class CustomizedProductDTOService{
        /// <summary>
        /// Transforms a customized product dto into a customized product via service
        /// </summary>
        /// <param name="customizedProductDTO">CustomizedProductDTO with the customized product dto being transformed</param>
        /// <returns>CustomizedProduct with the of customized products transformed from the dto</returns>
        public CustomizedProduct transform(CustomizedProductDTO customizedProductDTO){
            throw new NotImplementedException();
        }
    }
}