using core.dto;
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
        /// <value></value>
        public long Id {get; set;}

        /// <summary>
        /// Empty constructor for ORM.
        /// </summary>
        protected Dimension(){}

        public abstract DimensionDTO toDTO();

        /// <summary>
        /// Retrieves the Dimension's DTO and converts the value(s) into the specific unit.
        /// </summary>
        /// <param name="unit">Measurement unit to which the values are being converted.</param>
        /// <returns>DimensionDTO with converted value(s).</returns>
        public abstract DimensionDTO toDTO(string unit);
    }
}