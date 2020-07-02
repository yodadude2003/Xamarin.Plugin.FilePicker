using System.IO;

namespace Plugin.FilePicker
{
    public class PlatformFileData : Abstractions.FileData
    {
        public PlatformFileData(string filePath) : base(filePath)
        {
        }

        public override Stream GetInputStream()
        {
            return File.OpenRead(FilePath);
        }

        public override Stream GetOutputStream()
        {
            return File.OpenWrite(FilePath);
        }
    }
}
