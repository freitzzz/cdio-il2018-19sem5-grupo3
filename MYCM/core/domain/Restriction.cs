using support.builders;
using support.domain;
using support.domain.ddd;
using support.dto;
using support.utils;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using core.dto;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace core.domain
{
    /// <summary>
    /// Represents a Product Restriction
    /// </summary>
    public class Restriction : DTOAble<RestrictionDTO>
    {

        /// <summary>
        /// Constant with the message that is presented when the restriction being instantiated has an invalid description
        /// </summary>
        private const string INVALID_DESCRIPTION = "Description can't be null or empty.";

        /// <summary>
        /// Constant with the message that is presented when the restriction being instantiated has an invalid algorithm
        /// </summary>
        private const string INVALID_ALGORITHM = "The provided algorithm is not valid.";

        /// <summary>
        /// Long property with the persistence iD
        /// </summary>
        public long Id { get; internal set; }

        /// <summary>
        /// Description of the restriction
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// Algorithm being applied by the Restriction.
        /// </summary>
        private Algorithm _algorithm;   //!private field used for lazy loading, do not use this for storing or fetching data
        public Algorithm algorithm { get => LazyLoader.Load(this, ref _algorithm); set => _algorithm = value; }

        /// <summary>
        /// Injected LazyLoader.
        /// </summary>
        /// <value>Gets/sets the value of the injected LazyLoader.</value>
        private ILazyLoader LazyLoader { get; set; }

        /// <summary>
        /// Constructor used for injecting the LazyLoader.
        /// </summary>
        /// <param name="lazyLoader">LazyLoader being injected.</param>
        private Restriction(ILazyLoader lazyLoader)
        {
            this.LazyLoader = lazyLoader;
        }

        /// <summary>
        /// Empty constructor for ORM
        /// </summary>
        protected Restriction() { }

        /// <summary>
        /// Creates a new instance of Restriction with a given description, applying a given Algorithm with given values.
        /// </summary>
        /// <param name="description">Restriction's description</param>
        /// <param name="algorithm">Restriction's Algorithm</param>
        public Restriction(string description, Algorithm algorithm)
        {
            checkDescription(description);
            checkAlgorithm(algorithm);
            this.description = description;
            this.algorithm = algorithm;
        }

        /// <summary>
        /// Checks if the Restriction's description is valid.
        /// </summary>
        /// <param name="description">String being checked.</param>
        /// <exception cref="System.ArgumentException"></exception>
        private void checkDescription(string description)
        {
            if (String.IsNullOrEmpty(description) || description.Trim().Length == 0)
            {
                throw new ArgumentException(INVALID_DESCRIPTION);
            }
        }

        /// <summary>
        /// Checks if the Algorithm is valid.
        /// </summary>
        /// <param name="algorithm">Instance of Algorithm being checked.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided instance of Algorithm is null.</exception>
        private void checkAlgorithm(Algorithm algorithm)
        {
            if (algorithm == null) throw new ArgumentNullException(INVALID_ALGORITHM);
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

            Restriction other = (Restriction)obj;

            return this.description.Equals(other.description);
        }

        public override int GetHashCode()
        {
            return description.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("Description: {0}", description);
        }

        /// <summary>
        /// Returns DTO equivalent of the Entity
        /// </summary>
        /// <returns>DTO equivalent of the Entity</returns>
        public RestrictionDTO toDTO()
        {
            RestrictionDTO dto = new RestrictionDTO();
            dto.id = Id;
            dto.description = description;
            //dto.algorithm = algorithm;
            //dto.inputs = (List<InputDTO>)DTOUtils.parseToDTOS(inputValues);
            return dto;
        }
    }
}
