using System;

namespace support.utils{
    /// <summary>
    /// Utility class for Strings
    /// </summary>
    public sealed class Strings{
        /// <summary>
        /// Checks if a String is either null or empty
        /// </summary>
        /// <param name="stringBeingChecked">String with the string being checked</param>
        /// <returns>boolean true if the string is either null or empty, false if not</returns>
        public static bool isNullOrEmpty(string stringBeingChecked){return stringBeingChecked==null||stringBeingChecked.Trim().Length==0;}
    }
}