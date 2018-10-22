using support.options;

namespace support.dto{
    /// <summary>
    /// A generic interface for transforming an object to a DTO with options
    /// </summary>
    /// <typeparam name="D">Generic-Type of the DTO being created</typeparam>
    public interface DTOAbleOptions<D,O> where D:DTO where O:Options{
        /// <summary>
        /// Returns the DTO representation of the object with set of options
        /// </summary>
        /// <param name="dtoOptions">O with the set of options being applied</param>
        /// <returns>D with the DTO of the current object with the applied options</returns>
        D toDTO(O dtoOptions);
    }
}