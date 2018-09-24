namespace framework.dto{
    /// <summary>
    /// Functional Interface which allow the parse of a DTO into its original object
    /// Its useful for concrete implementations of DTO
    /// </summary>
    /// <typeparam name="T">Generic-Type of the object which created the DTO</typeparam>
    public interface DTOParseable<T> where T:DTO{
        /// <summary>
        /// Parses a DTO into its original object
        /// </summary>
        /// <returns>T with the original object which created the DTO</returns>
        T valueOf();
    }
}