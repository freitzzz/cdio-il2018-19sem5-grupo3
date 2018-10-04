using System.Collections.Generic;
using support.domain.ddd;
using System;
using support.dto;
using core.dto;

namespace core.domain
{
    /**
    <summary>
        Class that represents a Configured Product.
        <br> Configured Product is value object;
    </summary>
    */
    public class CustomizedProduct : ValueObject, DTOAble<CustomizedProductDTO>
    {
        public long Id { get; set; }
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
            Constant that represents the message that ocurrs if the string is not valid.
        </summary>
         */
        private const string INVALID_STRING = "The String inserted is not valid!";

        /**
       <summary>
           Constant that represents the message that ocurrs if the CustomizedMaterial's  are not valid.
       </summary>
        */
        private const string INVALID_PRODUCT_REFERENCE = "The inserted product reference is invalid!";

        /**
               <summary>
                   Constant that represents the message that ocurrs if the CustomizedMaterial's  are not valid.
               </summary>
                */
        private const string INVALID_PRODUCT_DESIGNATION = "The inserted designation is invalid!";


        /**
        <summary>
            String with the ConfiguredProduct's reference.
        </summary>
        */
        public string reference { get; protected set; }

        /** 
        <summary>
            String with the ConfiguredProduct's designation.
        </summary>
        */
        public string designation { get; set; }

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

        /**
        <summary>
            Long with the ConfiguredProduct's database ID.
        </summary>
         */
        private long persistence_id { get; set; }


        public static CustomizedProduct valueOf(string reference, string designation, CustomizedMaterial customizedMaterial, CustomizedDimensions customizedDimensions, Product product)
        {
            return new CustomizedProduct(reference, designation, customizedMaterial, customizedDimensions, product);
        }
        /**
       <summary>
           Builds a new instance of ConfiguredProduct, receiving its reference, designation, 
           customizedDimensions, customizedMaterial and product.
       </summary>
       <param name = "reference">string with the new ConfiguredProduct's reference</param>
       <param name = "designation">string with the new ConfiguredProduct's designation</param>
       <param name = "customizedDimensions">string with the new ConfiguredProduct's customizedDimensions</param>
       <param name = "customizedMaterial">string with the new ConfiguredProduct's customizedMaterial</param>
       <param name = "product">string with the new ConfiguredProduct's product</param>
        */
        private CustomizedProduct(string reference, string designation, CustomizedMaterial customizedMaterial, CustomizedDimensions customizedDimensions, Product product)
        {
            checkCustomizedMaterial(customizedMaterial);
            checkCustomizedDimensions(customizedDimensions);
            checkString(reference);
            checkString(designation);
            this.reference = reference;
            this.designation = designation;
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
            Returns a textual description of the ConfiguredProduct.
        </summary>
         */
        public override string ToString()
        {
            return string.Format("Designation: {0}, Reference {1}", designation, reference);
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


        CustomizedProductDTO DTOAble<CustomizedProductDTO>.toDTO()
        {
            throw new NotImplementedException();
        }

        /**
        <summary>
            Checks if string is valid
        </summary>
        <param name = "string">The string</param>
        */
        private void checkString(string obj)
        {
            if (String.IsNullOrEmpty(obj)) throw new ArgumentException(INVALID_STRING);

        }


        /**
            Changes the ConfiguredProduct's reference.
         */
        public void changeReference(string reference)
        {
            if (String.IsNullOrEmpty(reference)) throw new ArgumentException(INVALID_PRODUCT_REFERENCE);
            this.reference = reference;
        }

        /**
            Changes the ConfiguredProduct's designation.
         */
        public void changeDesignation(string designation)
        {
            if (String.IsNullOrEmpty(designation)) throw new ArgumentException(INVALID_PRODUCT_DESIGNATION);
            this.designation = designation;
        }

        /**
        <summary>
            Returns the ConfiguredProduct's identity.
        </summary>
        <returns>String with the ConfiguredProduct's identity</returns>
         */
        public string id()
        {
            return reference;
        }

        /**
       <summary>
           Inner static class which represents the ConfiguredProduct's properties used to map on data holders (e.g. DTO)
       </summary>
        */
        public static class Properties
        {
            /**
           <summary>
                Constant that represents the context of the Properties.
            </summary>
            */
            public const string CONTEXT = "ConfiguredProductDTO";

            /**
            <summary>
                Constant that represents the name of the Property which maps the Material's database ID.
            </summary>
             */
            public const string DATABASE_ID_PROPERTY = "id";

            /**
            <summary>
                Constant that represents the name of the Property which maps the Material's reference.
            </summary>
             */
            public const string REFERENCE_PROPERTY = "reference";

            /**
            <summary>
                Constant that represents the name of the Property which maps the Material's designation.
            </summary>
             */
            public const string DESIGNATION_PROPERTY = "designation";
        }
    }
}