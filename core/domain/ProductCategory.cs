using System;
using System.Text;
using support.domain.ddd;
using support.dto;
using core.dto;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

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
        /// Constant that represents the message being presented when a ProductCategory with an invalid parent ProductCategory is attempted to be created.
        /// </summary>
        private const string ERROR_NULL_PARENT = "The parent category must not be null";

        /// <summary>
        /// Constant that represents the message being presented when a ProductCategory shares the same name as its parent ProductCategory.
        /// </summary>
        private const string ERROR_SAME_CATEGORY = "The category can't be its own parent";

        /// <summary>
        /// Database identifier property
        /// </summary>
        /// <value></value>
        public long Id { get; internal set; } //the id should have an internal set, since DTO's have to be able to set them

        /// <summary>
        /// Boolean that represents the ProductCategory's state (for soft delete purposes)
        /// </summary>
        public bool active { get; protected set; }

        /// <summary>
        /// The ProductCategory's name e.g.: "Shelves", Drawers", "Handles".
        /// </summary>
        public string name { get; protected set; }

        /// <summary>
        /// Long that represents the parent's ID.
        /// </summary>
        /// <value></value>
        public long? parentId { get; internal set; }

        /// <summary>
        /// The ProductCategory's parent ProductCategory
        /// </summary>
        private ProductCategory _parent;
        
        [ForeignKey("parentId")]
        public ProductCategory parent { get => LazyLoader.Load(this, ref _parent); protected set => _parent = value; }

        /// <summary>
        /// Injected LazyLoader.
        /// </summary>
        /// <value>Gets/Sets the value of the LazyLoader</value>
        private ILazyLoader LazyLoader {get; set;}

        /// <summary>
        /// Constructor used for injecting the LazyLoader.
        /// </summary>
        /// <param name="lazyLoader">LazyLoader being injected.</param>
        private ProductCategory(ILazyLoader lazyLoader){
            this.LazyLoader = lazyLoader;
        }

        /// <summary>
        /// Empty constructor for ORM.
        /// </summary>
        protected ProductCategory() { }

        /// <summary>
        /// Creates a new instance of ProductCategory with a given name and an empty collection of Product.
        /// </summary>
        /// <param name="name">ProductCategory's name</param>
        public ProductCategory(string name)
        {
            if (!isNameValid(name))
            {
                throw new ArgumentException(ERROR_EMPTY_NAME);
            }
            this.name = name;
            this.active = true;
        }

        /// <summary>
        /// Creates a new instance of ProductCategory with a given name, an empty collection of Product and its parent.
        /// </summary>
        /// <param name="name">ProductCategory's name</param>
        public ProductCategory(string name, ProductCategory parent) : this(name)
        {
            if (parent == null)
            {
                throw new ArgumentException(ERROR_NULL_PARENT);
            }
            if (parent.sameAs(name))
            {
                throw new ArgumentException(ERROR_SAME_CATEGORY);
            }

            this.parent = parent;
            this.parentId = parent.Id;
        }

        /// <summary>
        /// Checks if a given name has a valid length.
        /// </summary>
        /// <param name="name">Name being checked</param>
        /// <returns>true if the name has more than one non-whitespace character, otherwise false is returned.</returns>
        private bool isNameValid(string name)
        {
            return name != null && name.Trim().Length > 0;
        }

        /// <summary>
        /// Changes the ProductCategory's name.
        /// </summary>
        /// <param name="newName">ProductCategory's new name</param>
        /// <returns>true if the new name is valid, otherwise false is returned.</returns>
        public bool changeName(string newName)
        {
            if (!isNameValid(newName))
            {
                return false;
            }

            this.name = newName;

            return true;
        }

        /// <summary>
        /// Checks if the ProductCategory is active 
        /// </summary>
        /// <returns>true if the ProductCategory is active, false if not</returns>
        public bool isActive()
        {
            return this.active == true;
        }

        /// <summary>
        /// Deactivates the ProductCategory
        /// </summary>
        public void deactivate()
        {
            this.active = false;
        }

        /// <summary>
        /// Activates the ProductCategory
        /// </summary>
        public void activate()
        {
            this.active = true;
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

            dto.parentId = parentId;
            dto.id = Id;
            dto.name = name;

            return dto;
        }
    }

}