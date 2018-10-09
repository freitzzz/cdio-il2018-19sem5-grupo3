using System;
using System.Collections.Generic;
using core.dto;
using support.domain.ddd;
using support.dto;
using support.utils;

namespace core.domain
{
    /**
    <summary>
        Class that represents a Collection-
        <br> Collection is a value object;
    </summary>
    */
    public class Collection : ValueObject, DTOAble<CollectionDTO>
    {
        /**
       <summary>
           Constant that represents the id of a certain collection.
       </summary>
        */
        public long Id { get; set; }

        /**
       <summary>
           Constant that represents a list of Customized Products of a collection.
       </summary>
        */
        private List<CustomizedProduct> list;


        /**
        <summary>
            String with the Collection's reference.
        </summary>
        */
        public string reference { get; protected set; }

        /** 
        <summary>
            String with the Collection's designation.
        </summary>
        */
        public string designation { get; set; }

        /**
        <summary>
            Constant that represents the message that ocurrs if the Customized Product
        </summary>
         */
        private const string INVALID_CUSTOMIZED_PRODUCT = "The Customized Product is invalid!";

        /**
        <summary>
            Constant that represents the message that ocurrs if the list of Customized Products is invalid.
        </summary>
         */
        private const string INVALID_LIST_CONFIGURED_PRODUCT = "The list of Customized Product is invalid!";


        /**
        <summary>
            Constant that represents the message that ocurrs if a string is invalid.
        </summary>
         */
        private const string INVALID_STRING = "The String is invalid!";


        public static Collection valueOf(string reference, string designation, List<CustomizedProduct> list)
        {
            return new Collection(reference, designation, list);
        }

        public static Collection valueOf(string reference, string designation, CustomizedProduct customizedProduct)
        {
            return new Collection(reference, designation, customizedProduct);
        }
        /**
        <summary>
            Builds a new instance of Collection, receiving its reference, designation and list
        </summary>
        <param name = "reference">The new Collection reference</param>
        <param name = "designation">The new Collection designation</param>
        <param name ="list">The new Collection list of Costumized Products</param>
         */
        private Collection(string reference, string designation, List<CustomizedProduct> list)
        {
            checkList(list);
            checkString(reference);
            checkString(designation);

            this.designation = designation;
            this.reference = reference;
            this.list = list;
        }

        /**
        <summary>
            Builds a new instance of Collection, receiving its reference, designation and list
        </summary>
        <param name = "reference">The new Collection reference</param>
        <param name = "designation">The new Collection designation</param>
        <param name ="list">The new Collection list of Costumized Products</param>
         */
        private Collection(string reference, string designation, CustomizedProduct customizedProduct)
        {
            checkCustomizedProduct(customizedProduct);
            checkString(reference);
            checkString(designation);

            this.designation = designation;
            this.reference = reference;
            //this.list = new List<CustomizedProduct>();
            this.list.Add(customizedProduct);
        }

        /**
        <summary>
            Checks if the Customized Product is invalid
        </summary>
        <param name = "customizedProductl">The customized product</param>
        */
        private void checkCustomizedProduct(CustomizedProduct customizedProduct)
        {
            if (customizedProduct == null) throw new ArgumentException(INVALID_CUSTOMIZED_PRODUCT);
        }

        /**
        <summary>
            Checks if the String is invalid
        </summary>
        <param name = "string1">Any string inserted</param>
        */
        private void checkString(string string1)
        {
            if (String.IsNullOrEmpty(string1)) throw new ArgumentException(INVALID_STRING);
        }

        private void checkList(List<CustomizedProduct> list)
        {
            if (Collections.isListNull(list) || Collections.isListEmpty(list)) throw new ArgumentException(INVALID_LIST_CONFIGURED_PRODUCT);
        }

        /**
       <summary>
           Returns a textual description of the Collection.
       </summary>
        */
        public override string ToString()
        {
            return string.Format("Designation: {0}, Reference: {1}, List of Customized Products: {2} ", designation, reference, list.ToString());
        }
        /**
        <summary>
            Returns the generated hash code of the Customized Material.
        </summary>
         */
        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = (hashCode * 23) + this.reference.GetHashCode();
            hashCode = (hashCode * 23) + this.designation.GetHashCode();
            hashCode = (hashCode * 23) + this.list.GetHashCode();

            return hashCode.GetHashCode();
        }

        public CollectionDTO toDTO()
        {
            CollectionDTO dto = new CollectionDTO();
            dto.reference = this.reference;
            dto.designation = this.designation;
            dto.list = this.list;
            dto.id = this.Id;
            return dto;
        }

        /**
       <summary>
           Method that adds to the list another Customized Product
       </summary>
        */
        public bool addCustomizedProduct(CustomizedProduct customizedProduct)
        {
            try
            {
                checkCustomizedProduct(customizedProduct);
                if (this.list.Contains(customizedProduct))
                {
                    return false;
                }
                list.Add(customizedProduct);

            }
            catch (ArgumentException) // The customized Product is not valid
            {
                return false;

            }
            return true;

        }

        /* 
        
        public bool removeCustomizedProduct(long id)
        {
            try
            {
                
                if (this.list.Contains(customizedProduct))
                {
                    return false;
                }
                list.Add(customizedProduct);

            }
            catch (ArgumentException) // The customized Product is not valid
            {
                return false;

            }
            return true;

        } **/

        
        /**
        <summary>
            Checks if a certain Collection is the same as a received object.
        </summary>
        <param name = "obj">object to compare to the current Collection</param>
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
                Collection collection = (Collection)obj;
                return reference.Equals(collection.reference) && 
                designation.Equals(collection.designation) && 
                list.Equals(collection.list);
            }
        }

         /**
       <summary>
           Inner static class which represents the Collections's properties used to map on data holders (e.g. DTO)
       </summary>
        */
        public static class Properties
        {
            /**
           <summary>
                Constant that represents the context of the Properties.
            </summary>
            */
            public const string CONTEXT = "CollectionDTO";

            /**
            <summary>
                Constant that represents the name of the Property which maps the Collection's database ID.
            </summary>
             */
            public const string DATABASE_ID_PROPERTY = "id";

            /**
            <summary>
                Constant that represents the name of the Property which maps the Collection's reference.
            </summary>
             */
            public const string REFERENCE_PROPERTY = "reference";

            /**
            <summary>
                Constant that represents the name of the Property which maps the Collection's designation.
            </summary>
             */
            public const string DESIGNATION_PROPERTY = "designation";

            /**
            <summary>
                Constant that represents the name of the Property which maps the Collection's list.
            </summary>
             */
            public const string DESIGNATION_LIST = "list";
        }
    }
}