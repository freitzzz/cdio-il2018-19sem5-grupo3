using support.options;

namespace support.dto{
    /// <summary>
    /// A generic interface for transforming an object to a DTO with options
    /// </summary>
    /// <typeparam name="D">Generic-Type of the DTO being created</typeparam>
    public interface DTOAbleOptions<D> where D:DTO{
        D toDTO(Options dtoOptions);
    }
}