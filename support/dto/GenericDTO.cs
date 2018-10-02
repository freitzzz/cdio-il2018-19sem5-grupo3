using support.dto;
using System;
using System.Collections;
using System.Collections.Generic;
namespace support.dto{
    /// <summary>
    /// Represents a Generic DTO implementation for a certain context
    /// </summary>
    /// <typeparam name="string">Generic-Type of the DTO key mapper</typeparam>
    /// <typeparam name="object">Generic-Type of the DTO objects</typeparam>
    [Obsolete("Use concrete DTO's instead")]
    public sealed class GenericDTO:DTO,IDictionary<string,object>{
        /// <summary>
        /// Constant that repesents the message that occures if the DTO context is invalid
        /// </summary>
        private static readonly string INVALID_DTO_CONTEXT="DTO context is invalid!";
        /// <summary>
        /// String with the DTO context
        /// </summary>
        private readonly string dtoContext;
        /// <summary>
        /// IDictionary with the DTO mapper
        /// </summary>
        private readonly IDictionary<string,object> dtoMapper=new Dictionary<string,object>();
        /// <summary>
        /// Builds a new GenericDTO with the DTO context
        /// </summary>
        /// <param name="context">String with the DTO context</param>
        public GenericDTO(string context){
            checkGenericDTO(context);
            this.dtoContext=context;
        }
    
        //Attention: For DTO mapping, this method is the one who should be used since 
        //Dictionary implementation throws exceptions if the data being inserted is invalid

        /// <summary>
        /// Puts an object into the DTO with a certain key which is mapping it
        /// </summary>
        /// <param name="key">String with the object key reference</param>
        /// <param name="data">Object with the data being inserted in the DTO</param>
        /// <returns>boolean true if the object was inserted with success, false if not</returns>
        public bool put(string key,object data){
            try{Add(key,data);}catch(SystemException){return false;}
            return true;
        }

        /// <summary>
        /// Returns an object which is being held on the DTO by a certain key
        /// </summary>
        /// <param name="key">String with the key which references the object</param>
        /// <returns>Object with the object which is being referenced by the key</returns>
        public object get(string key){try{return dtoMapper[key];}catch(SystemException){return null;}}

        //IDictionary implementation
        
        public object this[string key] { get => dtoMapper[key]; set => dtoMapper[key] = value; }

        public ICollection<string> Keys => dtoMapper.Keys;

        public ICollection<object> Values => dtoMapper.Values;

        public int Count => dtoMapper.Count;

        public bool IsReadOnly => dtoMapper.IsReadOnly;

        /// <summary>
        /// Puts a new object into the DTO mapper
        /// </summary>
        /// <param name="key">String with the object key mapper</param>
        /// <param name="value">Object with the object being mapped</param>
        public void Add(string key, object value){dtoMapper.Add(key, value);}
        
        /// <summary>
        /// Puts an entry into the DTO mapper
        /// </summary>
        /// <param name="item">Entry with the entry being mapped on the DTO</param>
        public void Add(KeyValuePair<string, object> item){dtoMapper.Add(item);}

        /// <summary>
        /// Clears the current DTO mapper
        /// </summary>
        public void Clear(){dtoMapper.Clear();}

        /// <summary>
        /// Checks if an entry exists on the current DTO mapper
        /// </summary>
        /// <param name="item">Entry with the entry being checked</param>
        /// <returns>boolean true if the DTO mapper contains the entry, false if not</returns>
        public bool Contains(KeyValuePair<string, object> item){return dtoMapper.Contains(item);}

        /// <summary>
        /// Checks if an entry exists on the current DTO mapper by its key
        /// </summary>
        /// <param name="key">String with the entry key</param>
        /// <returns>boolean true if the DTO mapper contains the entry, false if not</returns>
        public bool ContainsKey(string key){return dtoMapper.ContainsKey(key);}

        /// <summary>
        /// Copies the current DTO mapper into an array
        /// </summary>
        /// <param name="array">Array with the array being filled with the DTO mapper</param>
        /// <param name="arrayIndex">Integer with the starting index where the copy will start</param>
        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex){dtoMapper.CopyTo(array, arrayIndex);}

        /// <summary>
        /// Returns an enumerator of the current DTO mapper
        /// </summary>
        /// <returns>IEnumerator with the enumerator of the current DTO mapper</returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator(){return dtoMapper.GetEnumerator();}
        
        /// <summary>
        /// Removes an entry from the DTO mapper by its key
        /// </summary>
        /// <param name="key">String with the entry key</param>
        /// <returns>boolean true if the entry was removed, false if not</returns>
        public bool Remove(string key){return dtoMapper.Remove(key);}

        /// <summary>
        /// Removes an entry from the DTO mapper
        /// </summary>
        /// <param name="item">Entry with the entry being removed</param>
        /// <returns>boolean true if the entry was removed, false if not</returns>
        public bool Remove(KeyValuePair<string, object> item){return dtoMapper.Remove(item);}

        /// <summary>
        /// Checks if an entry exists on the current DTO mapper by its key and value
        /// </summary>
        /// <param name="key">String with the entry key</param>
        /// <param name="value">Object with the entry object</param>
        /// <returns>boolean true if the entry exists, false if not</returns>
        public bool TryGetValue(string key, out object value){return dtoMapper.TryGetValue(key, out value);}
        
        /// <summary>
        /// Returns an enumerator of the current DTO mapper
        /// </summary>
        /// <returns>IEnumerator with the DTO mapper enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator(){return dtoMapper.GetEnumerator();}

        /// <summary>
        /// Checks if the GenericDTO being build is valid
        /// </summary>
        /// <param name="context">string with the DTO context</param>
        private void checkGenericDTO(string context){
            if(context==null||context.Trim().Length==0){
                throw new ArgumentException(INVALID_DTO_CONTEXT);
            }
        }
    }
}