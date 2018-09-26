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
        public static bool isListEmpty(List<object> list)
        {
            return list != null && list.Count == 0;
        }

        /// <summary>
        ///  Checks if a list is null.
        /// </summary>
        /// <param name="list"></param>
        /// <returns>true if it's null, false if otherwise</returns>
        public static bool isListNull(List<object> list)
        {
            return list == null;
        }
    }
}