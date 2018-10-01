namespace support.dto{
    /// <summary>
    /// Abstract class used for indicating that a class 
    /// </summary>
    /// <typeparam name="T">Type of object being tranformed into a DTO.</typeparam>
    /// <typeparam name="D">Type of DTO in which object tranformation will result.</typeparam>
    public interface DTOParseable<T,D> where T : DTOAble<D> where D : DTO{

        /// <summary>
        /// Converts an instance of DTO into an instance of the given object type.
        /// </summary>
        /// <param name="dto">DTO containing object information</param>
        /// <returns></returns>
        T toEntity();
    }
}