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
        /// <param name="list"></param>
        /// <returns>true if it's empty, false if otherwise</returns>
        public static bool isListEmpty<T>(List<T> list)
        {
            return list != null && list.Count == 0;
        }

        /// <summary>
        ///  Checks if a list is null.
        /// </summary>
        /// <param name="list">List with the list being checked</param>
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

    }
}