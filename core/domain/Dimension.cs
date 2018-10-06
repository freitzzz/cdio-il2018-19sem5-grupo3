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
    }
}