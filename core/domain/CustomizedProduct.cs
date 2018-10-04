using System.Collections.Generic;
using support.domain.ddd;
using System;

namespace core.domain
{
    /**
    <summary>
        Class that represents a Configured Product.
        <br> Configured Product is value object;
    </summary>
    */
    public class CustomizedProduct : ValueObject
    {
        /**
        <summary>
            Constant that represents the message that ocurrs if the CustomizedMaterial's  are not valid.
        </summary>
         */
        private const string INVALID_CONFIGURED_PRODUCT_MATERIAL = "The CustomizedMaterial is not valid!";
        
        /**
        <summary>
            Constant that represents the message that ocurrs if the CustomizedMaterial's  are not valid.
        </summary>
         */
        private const string INVALID_CONFIGURED_PRODUCT_DIMENSIONS = "The CustomizedDimension is  not valid!";
        
        /**
        <summary>
            The CustomizedProduct Customized Material
        </summary>
         */
        private readonly CustomizedMaterial customizedMaterial;

        /**
        <summary>
            The CustomizedProduct Customized Dimensions
        </summary>
         */
        private readonly CustomizedDimensions customizedDimensions;

        /**
        <summary>
            List of Products from CustomizedProduct
        </summary>
         */
        private List<Product> list;

        public static CustomizedProduct valueOf(CustomizedMaterial customizedMaterial, CustomizedDimensions customizedDimensions,Product product)
        {
            return new CustomizedProduct(customizedMaterial,customizedDimensions,product);
        }
        /** 
        <summary>
            Builds a new instance of CustomizedProduct receiving its color and finish.
        </summary>
        <param name = "customizedMaterial,">The new CustomizedProduct's customizedMaterial</param>
        <param name = "customizedDimensions">The new CustomizedProduct's customizedMaterial</param>
        <param name = "Product">The new CustomizedProduct's product</param>
         */
        private CustomizedProduct(CustomizedMaterial customizedMaterial, CustomizedDimensions customizedDimensions,Product product)
        {
            checkCustomizedMaterial(customizedMaterial);
            checkCustomizedDimensions(customizedDimensions);
            this.customizedDimensions = customizedDimensions;
            this.customizedMaterial = customizedMaterial;
            this.list.Add(product);
        }

        /**
        <summary>
            Checks if the CustomizedMaterial's  is valid.
        </summary>
        <param name = "customizedMaterial">The CustomizedMaterial</param>
        */
        private void checkCustomizedMaterial(CustomizedMaterial customizedMaterial)
        {
            if (String.IsNullOrEmpty(customizedMaterial.ToString())) throw new ArgumentException(INVALID_CONFIGURED_PRODUCT_MATERIAL);

        }

        /**
        <summary>
            Checks if the CustomizedDimension is valid.
        </summary>
        <param name = "customizedDimension">The CustomizedDimension</param>
        */
        private void checkCustomizedDimensions(CustomizedDimensions customizedDimensions)
        {
            if (String.IsNullOrEmpty(customizedDimensions.ToString())) throw new ArgumentException(INVALID_CONFIGURED_PRODUCT_DIMENSIONS);

        }

        /**
        <summary>
            Returns a textual with the Customized Dimensions and CustomizedMaterial of the Customized Product.
        </summary>
         */
         public override string ToString()
        {
            return string.Format("Customized Material {0}, Customized Dimensions {1}", customizedMaterial.ToString(), customizedDimensions.ToString());
        }

        /**
        <summary>
            Returns the generated hash code of the Customized Material.
        </summary>
         */
        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = (hashCode * 23) + this.customizedDimensions.GetHashCode();
            hashCode = (hashCode * 23) + this.customizedMaterial.GetHashCode();

            return hashCode.GetHashCode();
        }

        /**
        <summary>
            Checks if a certain Customized Material is the same as a received object.
        </summary>
        <param name = "obj">object to compare to the current Customized Material</param>
         */
        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                CustomizedProduct configProduct = (CustomizedProduct)obj;
                return customizedDimensions.Equals(configProduct.customizedDimensions) && customizedMaterial.Equals(configProduct.customizedMaterial);
            }
        }
        
    }
}