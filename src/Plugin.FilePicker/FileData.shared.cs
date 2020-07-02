using System.IO;

namespace Plugin.FilePicker.Abstractions
{
    public abstract class FileData
    {
        public string FilePath { get; }
        public string FileName => Path.GetFileName(FilePath);

        public FileData(string filePath)
        {
            FilePath = filePath;
        }

        public abstract Stream GetInputStream();
        public abstract Stream GetOutputStream();
    }
}
