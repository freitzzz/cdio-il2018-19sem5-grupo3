using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using support.domain;
using support.domain.ddd;
using support.dto;

namespace core.domain
{

    /// <summary>
    /// Class used for categorizing all the available products.
    /// </summary>
    /// <typeparam name="string">ProductCategory's identifier</typeparam>
    public class ProductCategory : AggregateRoot<string>, DTOAble
    {
        /// <summary>
        /// Constant that represents the message being presented when a ProductCategory with an empty name is attempted to be created. 
        /// </summary>
        private const string ERROR_EMPTY_NAME = "The name must not be empty.";

        /// <summary>
        /// Database identifier property
        /// </summary>
        /// <value></value>
        public long ProductCategoryId { get; set; }


        /// <summary>
        /// The ProductCategory's name e.g.: "Shelves", Drawers", "Handles".
        /// </summary>
        private string name;

        /// <summary>
        /// Creates a new instance of ProductCategory with a given name and an empty collection of Product.
        /// </summary>
        /// <param name="name">ProductCategory's name</param>
        public ProductCategory(string name)
        {
            this.name = name;

            if (!isNameLengthValid(name))
            {
                throw new ArgumentException(ERROR_EMPTY_NAME);
            }
        }

        /// <summary>
        /// Checks if a given name has a valid length.
        /// </summary>
        /// <param name="name">Name being checked</param>
        /// <returns>true if the name has more than one non-whitespace character, otherwise false is returned.</returns>
        private bool isNameLengthValid(string name)
        {
            return name.Trim().Length > 0;
        }

        /// <summary>
        /// Changes the ProductCategory's name.
        /// </summary>
        /// <param name="newName">ProductCategory's new name</param>
        /// <returns>true if the new name is valid, otherwise false is returned.</returns>
        public bool changeName(string newName)
        {
            if (!isNameLengthValid(newName))
            {
                return false;
            }

            this.name = newName;

            return true;
        }

        public override bool Equals(object obj)
        {


            if (this == obj)
            {
                return true;
            }


            if (obj == null || !obj.GetType().Equals(this.GetType()))
            {
                return false;
            }

            var otherCategory = (ProductCategory)obj;

            return name.Equals(otherCategory.name, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            int hash = 13;

            hash = hash * 29 + name.ToLowerInvariant().GetHashCode();

            return hash;
        }

        public string id()
        {
            return name;
        }

        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();

            sb.Append("Name: ");
            sb.Append(name);

            return sb.ToString();
        }


        public DTO toDTO()
        {
            DTO dto = new GenericDTO(Properties.CONTEXT);

            dto.put(Properties.CATEGORY_NAME, name);
            dto.put(Properties.CATEGORY_ID, ProductCategoryId);

            return dto;
        }
        public bool sameAs(string comparingEntity)
        {
            return name.Equals(comparingEntity, StringComparison.InvariantCultureIgnoreCase);
        }

        //DTO mapping keys

        /// <summary>
        /// Static inner class that represents the keys by which ProductCategory's attributes are mapped.
        /// </summary>
        public static class Properties
        {

            /// <summary>
            /// Constant that represents the mapping context.
            /// </summary>
            public const string CONTEXT = "ProductCategory";

            /// <summary>
            /// Key for the ProductCategory's name attribute.
            /// </summary>
            public const string CATEGORY_NAME = "name";

            /// <summary>
            /// Key for the ProducCategory's database identifier.
            /// </summary>
            public const string CATEGORY_ID = "id";

        }

    }

}