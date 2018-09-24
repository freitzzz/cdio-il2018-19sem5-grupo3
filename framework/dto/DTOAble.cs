namespace framework.dto{
    /// <summary>
    /// Functional interface which allows the creation of a DTO
    /// </summary>
    public interface DTOAble{
        /// <summary>
        /// Returns a DTO of the current object
        /// </summary>
        /// <returns>DTO with the DTO of the current object</returns>
        DTO toDTO();
    }
}