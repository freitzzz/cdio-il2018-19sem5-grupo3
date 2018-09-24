namespace framework.dto{
    public interface DTO{
        /// <summary>
        /// Puts an object into the DTO with a certain key which is mapping it
        /// </summary>
        /// <param name="key">String with the object key reference</param>
        /// <param name="data">Object with the data being inserted in the DTO</param>
        /// <returns>boolean true if the object was inserted with success, false if not</returns>
        bool put(string key,object data);
        
        /// <summary>
        /// Returns an object which is being held on the DTO by a certain key
        /// </summary>
        /// <param name="key">String with the key which references the object</param>
        /// <returns>Object with the object which is being referenced by the key</returns>
        object get(string key);
    }
}