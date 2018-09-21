namespace framework.dto{
    public interface DTO{
        /// <summary>
        /// Returns an object which is being held on the DTO by a certain key
        /// </summary>
        /// <param name="key">String with the key which references the object</param>
        /// <returns>Object with the object which is being referenced by the key</returns>
        object get(string key);
    }
}