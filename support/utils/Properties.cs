using System.IO;
using support.io;
using System.Collections.Generic;

namespace support.system{
    /// <summary>
    /// Class that holds a set of properties
    /// <br>It can be seen as a migration from java.util.Properties
    /// </summary>
    public class Properties{
        /// <summary>
        /// Constant that represents the character used to separate keys from values
        /// </summary>
        private static readonly char PROPERTIES_SEPARATOR='=';
        /// <summary>
        /// Holder of the current properties
        /// </summary>
        private readonly IDictionary<object,object> holder=new Dictionary<object,object>();
        /// <summary>
        /// Creates an empty properties holder
        /// </summary>
        public Properties(){}

        /// <summary>
        /// Puts a new property
        /// </summary>
        /// <param name="key">object with the propertie key</param>
        /// <param name="value">object with the propertie value</param>
        public void put(object key,object value){holder.Add(key,value);}
        
        /// <summary>
        /// Returns the property value by its key
        /// </summary>
        /// <param name="key">object with the propertie key</param>
        /// <returns>object with the property value</returns>
        public object get(object key){
            object value=null;
            holder.TryGetValue(key,out value);
            return value; 
        }
        
        /// <summary>
        /// Loads a set of properties from an input stream
        /// </summary>
        /// <param name="inputStream">StreamReader with the input stream</param>
        public void load(StreamReader inputStream){
            List<string> streamContent=Streams.readAllLines(inputStream);
            streamContent.ForEach(line =>{
                string[] keyValue=line.Split(PROPERTIES_SEPARATOR);
                holder.Add(keyValue[0],keyValue[1]);
            });
        }
    }
}