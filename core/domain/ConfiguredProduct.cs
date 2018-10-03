using support.domain.ddd;

namespace core.domain
{
    /**
    <summary>
        Class that represents a Configured Product.
        <br> Configured Product is value object;
    </summary>
    */
    public class ConfiguredProduct : ValueObject
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