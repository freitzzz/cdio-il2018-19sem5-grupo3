using System;
using System.Collections.Generic;

namespace support.utils
{

    /// <summary>
    ///  Auxiliary class for operations that work with collections.
    /// </summary>

    public sealed class Collections
    {

        /// <summary>
        /// Checks if a collection is empty.
        /// </summary>
        /// <param name="list">List to check</param>
        /// <returns>true if it's empty, false if otherwise</returns>
        public static bool isListEmpty<T>(List<T> list)
        {
            return list == null || list.Count == 0;
        }

        /// <summary>
        ///  Checks if a list is null.
        /// </summary>
        /// <param name="list">List to check</param>
        /// <returns>true if it's null, false if otherwise</returns>
        public static bool isListNull<T>(List<T> list)
        {
            return list == null;
        }

        /// <summary>
        /// Checks if an enumerable is null or empty
        /// </summary>
        /// <param name="enumerable">IEnumerable with the enumerable being checked</param>
        /// <typeparam name="T">Generic-Type of the enumerable</typeparam>
        /// <returns>boolean true if the enumerable is null or empty, false if not</returns>
        public static bool isEnumerableNullOrEmpty<T>(IEnumerable<T> enumerable){
            return enumerable==null||!enumerable.GetEnumerator().MoveNext();
        }

        /// <summary>
        /// Returns the current size of an enumerable
        /// </summary>
        /// <param name="enumerable">IEnumerable with the enumerable which size is being retrieved</param>
        /// <typeparam name="T">Generic-Type param of the enumerable</typeparam>
        /// <returns>Integer with the enumerable size</returns>
        public static int getEnumerableSize<T>(IEnumerable<T> enumerable){
            if(enumerable.GetType()==typeof(List<T>)){
                return ((List<T>)enumerable).Count;
            }else{
                int size=0;
                foreach(T item in enumerable)
                    size++;
                return size;
            }
        }

        /// <summary>
        /// Parses an enumerable to a list
        /// </summary>
        /// <param name="enumerable">IEnumerable with the enumerable being parsed</param>
        /// <typeparam name="T">Generic-Type param of the enumerable</typeparam>
        /// <returns>List with the parsed enumerable</returns>
        public static List<T> enumerableAsList<T>(IEnumerable<T> enumerable){
            if(enumerable.GetType()==typeof(List<T>)){
                return (List<T>)enumerable;
            }else{
                return new List<T>(enumerable);
            }
        }
    }
}