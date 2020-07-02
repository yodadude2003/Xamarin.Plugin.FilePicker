using Android.App;
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
            if (IOUtil.IsMediaStore(FilePath))
            {
                var contentUri = Android.Net.Uri.Parse(FilePath);
                return Application.Context.ContentResolver.OpenInputStream(contentUri);
            }
            else
            {
                return File.OpenRead(FilePath);
            }
        }

        public override Stream GetOutputStream()
        {
            if (IOUtil.IsMediaStore(FilePath))
            {
                var contentUri = Android.Net.Uri.Parse(FilePath);
                return Application.Context.ContentResolver.OpenOutputStream(contentUri);
            }
            else
            {
                return File.OpenWrite(FilePath);
            }
        }
    }
}
