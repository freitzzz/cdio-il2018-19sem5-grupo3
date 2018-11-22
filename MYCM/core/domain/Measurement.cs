using System;
using System.Collections.Generic;
using core.dto;
using Microsoft.EntityFrameworkCore.Infrastructure;
using support.dto;

namespace core.domain
{
    public class Measurement : DTOAble<MeasurementDTO>
    {
        /// <summary>
        /// Constant representing the error message being presented when an instance is attempted to be created with a null height Dimension.
        /// </summary>
        private const string ERROR_NULL_HEIGHT_DIMENSION = "The height dimension can not be null.";

        /// <summary>
        /// Constant representing the error message being presented when an instance is attempted to be created with a null width Dimension.
        /// </summary>
        private const string ERROR_NULL_WIDTH_DIMENSION = "The width dimension can not be null.";

        /// <summary>
        /// Constant representing the error message being presented when an instance is attempted to be created with a null depth Dimension.
        /// </summary>
        private const string ERROR_NULL_DEPTH_DIMENSION = "The depth dimension can not be null";

        /// <summary>
        /// Measurement's database identifier.
        /// </summary>
        /// <value>Gets/protected sets the value of the database identifier.</value>
        public long Id { get; protected set; }

        /// <summary>
        /// List containing instances of Restriction.
        /// </summary>
        /// <value>Gets/ protected sets the value of the list.</value>
        private List<Restriction> _restrictions;    //!private field used for lazy loading, do not use this for storing or fetching data
        public List<Restriction> restrictions { get => LazyLoader.Load(this, ref _restrictions); protected set => _restrictions = value; }

        /// <summary>
        /// Measurement's height Dimension.
        /// </summary>
        /// <value>Gets/protected sets the value of the height Dimension.</value>
        private Dimension _height;  //!private field used for lazy loading, do not use this for storing or fetching data
        public Dimension height { get => LazyLoader.Load(this, ref _height); protected set => _height = value; }

        /// <summary>
        /// Measurement's width Dimension.
        /// </summary>
        /// <value>Gets/protected sets the value of the width Dimension.</value>
        private Dimension _width;   //!private field used for lazy loading, do not use this for storing or fetching data
        public Dimension width { get => LazyLoader.Load(this, ref _width); protected set => _width = value; }

        /// <summary>
        /// Measurement's depth Dimension.
        /// </summary>
        /// <value>Gets/protected sets the value of the depth Dimension.</value>
        private Dimension _depth;   //!private field used for lazy loading, do not use this for storing or fetching data
        public Dimension depth { get => LazyLoader.Load(this, ref _depth); protected set => _depth = value; }


        /// <summary>
        /// Injected ILazyLoader instance.
        /// </summary>
        /// <value>Gets/sets the ILazyLoader.</value>
        private ILazyLoader LazyLoader { get; set; }

        /// <summary>
        /// Private constructor used by the framework in order to inject an instace of ILazyLoader.
        /// </summary>
        /// <param name="lazyLoader">LazyLoader being injected.</param>
        private Measurement(ILazyLoader lazyLoader)
        {
            this.LazyLoader = lazyLoader;
        }

        /// <summary>
        /// Empty constructor used by ORM.
        /// </summary>
        protected Measurement() { }

        /// <summary>
        /// Creates a new instance of Measurement with the given instances of Dimension.
        /// </summary>
        /// <param name="height">Instance of Dimension attributed to the Measurement's height Dimension.</param>
        /// <param name="depth">Instance of Dimension attributed to the Measurement's depth Dimension.</param>
        /// <param name="width">Instance of Dimension attributed to the Measurement's width Dimension.</param>
        /// <exception cref="System.ArgumentException">Throws an ArgumentException if any of the arguments are null.</exception>
        public Measurement(Dimension height, Dimension width, Dimension depth)
        {
            checkAttributes(height, width, depth);
            this.height = height;
            this.width = width;
            this.depth = depth;
            this.restrictions = new List<Restriction>();
        }

        /// <summary>
        /// Private method used for checking if the given dimensions are not null.
        /// </summary>
        /// <param name="height">Height Dimension being checked.</param>
        /// <param name="width">Width Dimension being checked.</param>
        /// <param name="depth">Depth Dimension being checked.</param>
        /// <exception cref="System.ArgumentException">Throws an ArgumentException if any of the arguments are null.</exception>
        private void checkAttributes(Dimension height, Dimension width, Dimension depth)
        {
            if (height == null)
            {
                throw new ArgumentException(ERROR_NULL_HEIGHT_DIMENSION);
            }
            else if (width == null)
            {
                throw new ArgumentException(ERROR_NULL_WIDTH_DIMENSION);
            }
            else if (depth == null)
            {
                throw new ArgumentException(ERROR_NULL_DEPTH_DIMENSION);
            }
        }

        /// <summary>
        /// Adds an instance of Restriction to the Measurement's list of Restrictions.
        /// </summary>
        /// <param name="restriction">Instance of Restriction being added.</param>
        /// <returns>Returns true if the Restriction was successfully added; false can also be returned if the Restriction is null</returns>
        public bool addRestriction(Restriction restriction)
        {
            if (restriction == null)
            {
                return false;
            }
            int beforeCount = restrictions.Count;
            restrictions.Add(restriction);
            int afterCount = restrictions.Count;

            return beforeCount + 1 == afterCount;
        }

        /// <summary>
        /// Removes an instance of Restriction from the Dimension's list of Restrictions.
        /// </summary>
        /// <param name="restriction">Instance of Restriction being removed.</param>
        /// <returns>Returns true if the Restriction was succesfully removed; otherwise false.</returns>
        public bool removeRestriction(Restriction restriction)
        {
            return restrictions.Remove(restriction);
        }

        /// <summary>
        /// Changes the Measurement's height Dimension.
        /// </summary>
        /// <param name="newHeight">New height Dimension.</param>
        /// <returns>true if the instance of Dimension is not null; false otherwise</returns>
        public bool changeHeightDimension(Dimension newHeight)
        {
            if (newHeight == null)
            {
                return false;
            }
            this.height = newHeight;
            return true;
        }

        /// <summary>
        /// Changes the Measurement's width Dimension.
        /// </summary>
        /// <param name="newWidth">New width Dimension.</param>
        /// <returns>true if the instance of Dimension is not null; false otherwise</returns>
        public bool changeWidthDimension(Dimension newWidth)
        {
            if (newWidth == null)
            {
                return false;
            }
            this.width = newWidth;
            return true;
        }

        /// <summary>
        /// Changes the Measurement's depth Dimension.
        /// </summary>
        /// <param name="newDepth">New depth Dimension.</param>
        /// <returns>true if the instance of Dimension is not null; false otherwise</returns>
        public bool changeDepthDimension(Dimension newDepth)
        {
            if (newDepth == null)
            {
                return false;
            }
            this.depth = newDepth;
            return true;
        }

        /// <summary>
        /// Checks if all the specified values are contained in this instance.
        /// </summary>
        /// <param name="height">Height value being checked.</param>
        /// <param name="width">Width value being checked.</param>
        /// <param name="depth">Depth value being checked.</param>
        /// <returns>true if all the given values are available in this instance; false, otherwise.</returns>
        public bool hasValues(double height, double width, double depth)
        {
            bool hasHeightValue = this.height.hasValue(height);
            bool hasWidthvalue = this.width.hasValue(width);
            bool hasDepthValue = this.depth.hasValue(depth);

            return (hasHeightValue && hasWidthvalue && hasDepthValue);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !obj.GetType().Equals(this.GetType()))
            {
                return false;
            }

            if (obj == this)
            {
                return true;
            }

            Measurement other = (Measurement)obj;

            return height.Equals(other.height) && width.Equals(other.width) && depth.Equals(other.depth);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 59;

                hash = hash * 31 + height.GetHashCode();
                hash = hash * 31 + width.GetHashCode();
                hash = hash * 31 + depth.GetHashCode();

                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format("Height: {0}; Width: {1}; Depth: {2}", height.ToString(), width.ToString(), depth.ToString());
        }

        public MeasurementDTO toDTO()
        {
            MeasurementDTO dto = new MeasurementDTO();

            dto.id = Id;
            dto.height = height.toDTO();
            dto.width = width.toDTO();
            dto.depth = depth.toDTO();

            return dto;
        }

        public MeasurementDTO toDTO(string unit)
        {
            MeasurementDTO dto = new MeasurementDTO();

            dto.id = Id;
            dto.height = height.toDTO(unit);
            dto.width = width.toDTO(unit);
            dto.depth = depth.toDTO(unit);

            return dto;
        }
    }
}