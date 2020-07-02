using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Plugin.FilePicker
{
    public class PlatformFileData : Abstractions.FileData
    {
        public PlatformFileData(string filePath) : base(filePath)
        {
        }

        public override Stream GetInputStream()
        {
            throw new NotImplementedException();
        }

        public override Stream GetOutputStream()
        {
            throw new NotImplementedException();
        }
    }
}
