﻿using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AppKit;
using Foundation;
using MobileCoreServices;
using Plugin.FilePicker.Abstractions;

namespace Plugin.FilePicker
{
    public class PlatformFilePicker : NSObject, IFilePicker
    {
        public Task<FileData> PickFile(string[] allowedTypes, string defaultName, bool saving)
        {
            // for consistency with other platforms, only allow selecting of a single file.
            // would be nice if we passed a "file options" to override picking multiple files & directories
            var openPanel = new NSOpenPanel();
            openPanel.CanChooseFiles = true;
            openPanel.AllowsMultipleSelection = false;
            openPanel.CanChooseDirectories = false;

            // macOS allows the file types to contain UTIs, filename extensions or a combination of the two.
            // If no types are specified, all files are selectable.
            if (allowedTypes != null)
            {
                openPanel.AllowedFileTypes = allowedTypes;
            }

            FileData data = null;

            var result = openPanel.RunModal();
            if (result == 1)
            {
                // Nab the first file
                var url = openPanel.Urls[0];

                if (url != null)
                {
                    var path = url.Path;
                    data = new PlatformFileData(path);
                }
            }

            return Task.FromResult(data);
        }
    }
}
