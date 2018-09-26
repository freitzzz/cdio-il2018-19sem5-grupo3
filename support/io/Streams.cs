using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace support.io{
    /// <summary>
    /// Helper class for Streams
    /// </summary>
    public sealed class Streams{
        /// <summary>
        /// Reads all lines from an Input Stream
        /// </summary>
        /// <param name="inputStream">StreamReader with the Input Stream where the content is being read from</param>
        /// <returns>List with all the lines from the input stream</returns>
        public static List<string> readAllLines(StreamReader inputStream){
            List<string> lines=new List<string>();
            while(!inputStream.EndOfStream)lines.Add(inputStream.ReadLine());
            return lines;
        }
    }
}