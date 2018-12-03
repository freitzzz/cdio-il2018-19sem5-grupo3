using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace core.domain
{
    /// <summary>
    /// Abstract class used for indicating that an Entity can hold instances of Restriction.
    /// </summary>
    public abstract class Restrictable
    {
        /// <summary>
        /// Constant that represents the message presented when an instance of Restriction could not be added.
        /// </summary>
        private const string ERROR_UNABLE_TO_ADD_RESTRICTION = "The provided restriction could not be added. Please, make sure it's not a duplicate.";
        /// <summary>
        /// Constant that represents the message presented when an instance of Restriction could not be removed.
        /// </summary>
        private const string ERROR_UNABLE_TO_REMOVE_RESTRICTION = "The provided restriction could not be removed.";

        /// <summary>
        /// Instance of ILazyLoader.
        /// </summary>
        /// <value>Gets/sets the instance of ILazyLoader.</value>
        [NotMapped]
        protected ILazyLoader LazyLoader { get; set; }

        /// <summary>
        /// Constructor used by the framework for injecting an instance of ILazyLoader.
        /// </summary>
        /// <param name="lazyLoader">Instance of ILazyLoader.</param>
        protected Restrictable(ILazyLoader lazyLoader)
        {
            this.LazyLoader = lazyLoader;
        }

        /// <summary>
        /// Empty constructor used for ORM.
        /// </summary>
        protected Restrictable() { }

        /// <summary>
        /// List containing instances of Restriction.
        /// </summary>
        /// <value>Gets/ protected sets the value of the list.</value>
        private List<Restriction> _restrictions;    //!private field used for lazy loading, do not use this for storing or fetching data
        public List<Restriction> restrictions
        {
            get => LazyLoader.Load(this, ref _restrictions); protected set => _restrictions = value;
        }

        /// <summary>
        /// Adds an instance of Restriction.
        /// </summary>
        /// <param name="restriction">Instance of Restriction being added.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when the provided Restriction is null or could not be added.
        /// </exception>
        public void addRestriction(Restriction restriction)
        {
            if (restriction == null || restrictions.Contains(restriction))
            {
                throw new ArgumentException(ERROR_UNABLE_TO_ADD_RESTRICTION);
            }
            restrictions.Add(restriction);
        }

        /// <summary>
        /// Removes an instance of Restriction.
        /// </summary>
        /// <param name="restriction">Instance of Restriction being removed.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when the provided Restriction is null or could not be removed.
        /// </exception>
        public void removeRestriction(Restriction restriction)
        {
            if (!restrictions.Remove(restriction))
            {
                throw new ArgumentException(ERROR_UNABLE_TO_REMOVE_RESTRICTION);
            }
        }

        /// <summary>
        /// Checks if an instance of Restriction exists.
        /// </summary>
        /// <param name="restriction">Instance of Restriction being checked.</param>
        /// <returns>true if the provided Restriction had previously been added; false, otherwise.</returns>
        public bool hasRestriction(Restriction restriction)
        {
            return restrictions.Contains(restriction);
        }
    }
}