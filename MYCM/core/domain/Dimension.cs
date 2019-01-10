using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using core.dto;
using Microsoft.EntityFrameworkCore.Infrastructure;
using support.domain.ddd;
using support.dto;

namespace core.domain {
    /// <summary>
    /// Represents a Product dimension
    /// </summary>
    public abstract class Dimension : DTOAble<DimensionDTO> {

        private const double THRESHOLD = 0.0001;

        /// <summary>
        /// Database identifier.
        /// </summary>
        /// <value>Gets/sets the database identifier.</value>
        public long Id { get; set; }

        /// <summary>
        /// LazyLoader being injected by the framework.
        /// </summary>
        /// <value>Protected Gets/Sets the value of the LazyLoader.</value>
        [NotMapped]
        protected ILazyLoader LazyLoader { get; set; }

        /// <summary>
        /// Constructor for injecting the LazyLoader.
        /// </summary>
        /// <param name="lazyLoader">LazyLoader being injected.</param>
        protected Dimension(ILazyLoader lazyLoader) {
            this.LazyLoader = lazyLoader;
        }

        /// <summary>
        /// Empty constructor for ORM.
        /// </summary>
        protected Dimension() { }

        /// <summary>
        /// Checks if the Dimension has the given value.
        /// </summary>
        /// <param name="value">double being checked.</param>
        /// <returns>true, if the Dimension has the given; false, otherwise.</returns>
        public abstract bool hasValue(double value);

        /// <summary>
        /// Retrieves this instance of Dimension's minimum value.
        /// </summary>
        /// <returns>The minimum value.</returns>
        public abstract double getMaxValue();

        /// <summary>
        /// Retrieves this instance of Dimension's maximum value.
        /// </summary>
        /// <returns>The maximum value.</returns>
        public abstract double getMinValue();

        /// <summary>
        /// Retrieves all values of the Dimension as an array
        /// </summary>
        /// <returns>values of the Dimension as an array</returns>
        public abstract double[] getValuesAsArray();

        public abstract DimensionDTO toDTO();

        /// <summary>
        /// Retrieves the Dimension's DTO and converts the value(s) into the specific unit.
        /// </summary>
        /// <param name="unit">Measurement unit to which the values are being converted.</param>
        /// <returns>DimensionDTO with converted value(s).</returns>
        public abstract DimensionDTO toDTO(string unit);
    }
}