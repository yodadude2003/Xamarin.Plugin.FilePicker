﻿using Gtk;
using Plugin.FilePicker.Abstractions;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Plugin.FilePicker
{
    /// <summary>
    /// Implementation for file picking on WPF platform
    /// </summary>
    public class FilePickerImplementation : IFilePicker
    {
        /// <summary>
        /// File picker implementation for WPF; uses the Win32 OpenFileDialog from
        /// PresentationFoundation reference assembly.
        /// </summary>
        /// <param name="allowedTypes">
        /// Specifies one or multiple allowed types. When null, all file types
        /// can be selected while picking.
        /// On WPF, specify strings like this: "Data type (*.ext)|*.ext", which
        /// corresponds how the Windows file open dialog specifies file types.
        /// </param>
        /// <returns>file data of picked file, or null when picking was cancelled</returns>
        public Task<FileData> PickFile(string[] allowedTypes = null, bool saving = false)
        {
            var filter = new FileFilter();
            foreach (var ext in allowedTypes)
            {
                filter.AddPattern($"*.{ext}");
            }

            var picker = new Gtk.FileChooserDialog(
                saving ? "Save As" : "Open",
                null, saving ? FileChooserAction.Save : FileChooserAction.Open,
                "Cancel", ResponseType.Cancel,
                saving ? "Save As" : "Open", ResponseType.Accept
                );
            picker.SetCurrentFolder(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

            var result = picker.Run();

            if (result == (int)Gtk.ResponseType.Accept)
            {
                var fileName = Path.GetFileName(picker.Filename);
                var data = new FileData(picker.Filename, fileName, () => File.OpenRead(picker.Filename), () => File.OpenWrite(picker.Filename));
                picker.Hide();
                picker.Dispose();
                return Task.FromResult(data);
            }
            else
            {
                picker.Hide();
                picker.Dispose();
                return Task.FromResult<FileData>(null);
            }
        }
    }
}
