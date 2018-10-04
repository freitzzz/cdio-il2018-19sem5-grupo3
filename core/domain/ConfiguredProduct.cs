using core.dto;
using support.domain.ddd;
using support.dto;
using System.Collections.Generic;

namespace core.domain
{
    /**
    <summary>
        Class that represents a Configured Product.
        <br> Configured Product is value object;
    </summary>
    */
    public class ConfiguredProduct : ValueObject,DTOAble<ConfiguredProductDTO>
    {
        /**
        <summary>
            The ConfiguredProduct Customized Material
        </summary>
         */
        private readonly CustomizedMaterial customizedMaterial;

        /**
        <summary>
            The ConfiguredProduct Customized Dimensions
        </summary>
         */
        private readonly CustomizedDimensions customizedDimensions;

        /**
        <summary>
            List of Products from ConfiguredProduct
        </summary>
         */
         private  List<Product> list ;
    }
}