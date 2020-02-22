using System;
using System.IO;

namespace Plugin.FilePicker.Abstractions
{
    /// <summary>
    /// File data that specifies a file that was picked by the user.
    /// </summary>
    public sealed class FileData
    {
        /// <summary>
        /// Full file path to the picked file.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// File name of the picked file.
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Function to get a readonly stream to the picked file.
        /// </summary>
        public Func<Stream> GetInputStream { get; }

        /// <summary>
        /// Function to get a writable stream to the picked file.
        /// </summary>
        public Func<Stream> GetOutputStream { get; }

        /// <summary>
        /// Creates a new file data object with property values
        /// </summary>
        /// <param name="filePath">
        /// Full file path to the picked file.
        /// </param>
        /// <param name="fileName">
        /// File name of the picked file.
        /// </param>
        /// <param name="inputGetter">
        /// Function to get a readonly stream to the picked file.
        /// </param>
        /// <param name="outputGetter">
        /// Function to get a writable stream to the picked file.
        /// </param>
        public FileData(string filePath, string fileName, Func<Stream> inputGetter, Func<Stream> outputGetter)
        {
            FilePath = filePath;
            FileName = fileName;
            GetInputStream = inputGetter;
            GetOutputStream = outputGetter;
        }
    }
}
