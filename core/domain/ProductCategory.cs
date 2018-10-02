using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using support.domain;
using support.domain.ddd;
using support.dto;
using core.dto;

namespace core.domain
{

    /// <summary>
    /// Class used for categorizing all the available products.
    /// </summary>
    /// <typeparam name="string">ProductCategory's identifier</typeparam>
    public class ProductCategory : AggregateRoot<string>, DTOAble<ProductCategoryDTO>
    {
        /// <summary>
        /// Constant that represents the message being presented when a ProductCategory with an empty name is attempted to be created. 
        /// </summary>
        private const string ERROR_EMPTY_NAME = "The name must not be empty.";

        /// <summary>
        /// Database identifier property
        /// </summary>
        /// <value></value>
        public long Id { get; internal set; }


        /// <summary>
        /// The ProductCategory's name e.g.: "Shelves", Drawers", "Handles".
        /// </summary>
        public string name { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        protected ProductCategory(){}

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

        public bool sameAs(string comparingEntity)
        {
            return name.Equals(comparingEntity, StringComparison.InvariantCultureIgnoreCase);
        }

        public ProductCategoryDTO toDTO()
        {
            ProductCategoryDTO dto = new ProductCategoryDTO();

            dto.id = Id;
            dto.name = name;

            return dto;
        }
    }

}