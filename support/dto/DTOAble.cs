namespace support.dto{
    /// <summary>
    /// Represents a domain object that can be transformed into a DTO.
    /// </summary>
    public interface DTOAble<D> where D : DTO{
        /// <summary>
        /// Returns a DTO of the current object.
        /// </summary>
        /// <returns>DTO containing domain object's information.</returns>
        D toDTO();
    }
}