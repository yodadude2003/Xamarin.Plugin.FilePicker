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
            return new FileStream(FilePath, FileMode.Open, FileAccess.Read);
        }

        public override Stream GetOutputStream()
        {
            return new FileStream(FilePath, FileMode.Open, FileAccess.Write);
        }
    }
}
