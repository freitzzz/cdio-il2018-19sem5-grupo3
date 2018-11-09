using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace support.io{
    /// <summary>
    /// Helper class for Streams
    /// </summary>
    public sealed class Streams{
        /// <summary>
        /// Constant that represents the message that ocurres if a stream is invalid for read
        /// </summary>
        private static readonly string INVALID_STREAM_FOR_READ="The stream is invalid for read";
        /// <summary>
        /// Reads all lines from an Input Stream
        /// </summary>
        /// <param name="inputStream">Stream with the Input Stream where the content is being read from</param>
        /// <returns>List with all the lines from the input stream</returns>
        public static List<string> readAllLines(Stream inputStream){
            grantStreamIsValidForRead(inputStream);
            StreamReader inputStreamReader=new StreamReader(inputStream);
            List<string> lines=new List<string>();
            while(!inputStreamReader.EndOfStream)lines.Add(inputStreamReader.ReadLine());
            return lines;
        }

        /// <summary>
        /// Grants that a stream is valid for read
        /// </summary>
        /// <param name="stream">Stream with the stream being read</param>
        private static void grantStreamIsValidForRead(Stream stream){
            if(stream==null||!stream.CanRead)throw new ArgumentException(INVALID_STREAM_FOR_READ);
        }
    }
}