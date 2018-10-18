using System.Collections.Generic;
using core.dto;
using support.domain.ddd;
using support.dto;

namespace core.domain
{
    /// <summary>
    /// Represents a Product dimension
    /// </summary>
    public abstract class Dimension : DTOAble<DimensionDTO>
    {
        /// <summary>
        /// Database identifier.
        /// </summary>
        /// <value>Gets/sets the database identifier.</value>
        public long Id { get; set; }

        /// <summary>
        /// List containing instances of Restriction.
        /// </summary>
        /// <value>Gets/sets the list.</value>
        public virtual List<Restriction> restrictions { get; protected set; }

        /// <summary>
        /// Empty constructor for ORM.
        /// </summary>
        protected Dimension() { }

        /// <summary>
        /// Adds an instance of Restriction to the Dimension's list of Restrictions.
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


        public abstract DimensionDTO toDTO();

        /// <summary>
        /// Retrieves the Dimension's DTO and converts the value(s) into the specific unit.
        /// </summary>
        /// <param name="unit">Measurement unit to which the values are being converted.</param>
        /// <returns>DimensionDTO with converted value(s).</returns>
        public abstract DimensionDTO toDTO(string unit);
    }
}