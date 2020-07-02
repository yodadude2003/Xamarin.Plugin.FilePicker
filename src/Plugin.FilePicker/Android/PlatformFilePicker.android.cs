using Android.App;
using Android.Content;
using Android.Runtime;
using Plugin.FilePicker.Abstractions;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

// Adds permission for READ_EXTERNAL_STORAGE to the AndroidManifest.xml of the app project without
// the user of the plugin having to add it by himself/herself.
[assembly: UsesPermission(Android.Manifest.Permission.WriteExternalStorage)]

namespace Plugin.FilePicker
{
    /// <summary>
    /// Implementation for file picking on Android
    /// </summary>
    [Preserve(AllMembers = true)]
    public class PlatformFilePicker : IFilePicker
    {
        /// <summary>
        /// Android context to use for picking
        /// </summary>
        private readonly Context context;

        /// <summary>
        /// Request ID for current picking call
        /// </summary>
        private int requestId;

        /// <summary>
        /// Task completion source for task when finished picking
        /// </summary>
        private TaskCompletionSource<FileData> completionSource;

        /// <summary>
        /// Creates a new file picker implementation
        /// </summary>
        public PlatformFilePicker()
        {
            context = Application.Context;
        }

        /// <summary>
        /// Implementation for picking a file on Android.
        /// </summary>
        /// <param name="allowedTypes">
        /// Specifies one or multiple allowed types. When null, all file types
        /// can be selected while picking.
        /// On Android you can specify one or more MIME types, e.g.
        /// "image/png"; also wild card characters can be used, e.g. "image/*".
        /// </param>
        /// <returns>
        /// File data object, or null when user cancelled picking file
        /// </returns>
        public async Task<FileData> PickFile(string[] allowedTypes, string defaultName, bool saving)
        {
            var fileData = await PickFileAsync(allowedTypes, defaultName, saving);
            return fileData;
        }

        /// <summary>
        /// File picking implementation
        /// </summary>
        /// <param name="allowedTypes">list of allowed types; may be null</param>
        /// <param name="action">Android intent action to use; unused</param>
        /// <returns>picked file data, or null when picking was cancelled</returns>
        private Task<FileData> PickFileAsync(string[] allowedTypes, string defaultName, bool saving)
        {
            var id = GetRequestId();

            var ntcs = new TaskCompletionSource<FileData>(id);

            var previousTcs = Interlocked.Exchange(ref completionSource, ntcs);
            if (previousTcs != null)
            {
                previousTcs.TrySetResult(null);
            }

            try
            {
                var pickerIntent = new Intent(context, typeof(FilePickerActivity));
                pickerIntent.SetFlags(ActivityFlags.NewTask);

                pickerIntent.PutExtra(FilePickerActivity.ExtraAllowedTypes, allowedTypes);
                pickerIntent.PutExtra(FilePickerActivity.FileName, defaultName);
                pickerIntent.PutExtra(FilePickerActivity.PromptType, saving);

                context.StartActivity(pickerIntent);

                EventHandler<FilePickerEventArgs> handler = null;
                EventHandler<FilePickerCancelledEventArgs> cancelledHandler = null;

                handler = (s, e) =>
                {
                    var tcs = Interlocked.Exchange(ref completionSource, null);

                    FilePickerActivity.FilePickCancelled -= cancelledHandler;
                    FilePickerActivity.FilePicked -= handler;

                    tcs?.SetResult(new PlatformFileData(e.FilePath));
                };

                cancelledHandler = (s, e) =>
                {
                    var tcs = Interlocked.Exchange(ref completionSource, null);

                    FilePickerActivity.FilePickCancelled -= cancelledHandler;
                    FilePickerActivity.FilePicked -= handler;

                    if (e?.Exception != null)
                    {
                        tcs?.SetException(e.Exception);
                    }
                    else
                    {
                        tcs?.SetResult(null);
                    }
                };

                FilePickerActivity.FilePickCancelled += cancelledHandler;
                FilePickerActivity.FilePicked += handler;
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                completionSource.SetException(ex);
            }

            return completionSource.Task;
        }

        /// <summary>
        /// Returns a new request ID for a new call to PickFile()
        /// </summary>
        /// <returns>new request ID</returns>
        private int GetRequestId()
        {
            int id = requestId;

            if (requestId == int.MaxValue)
            {
                requestId = 0;
            }
            else
            {
                requestId++;
            }

            return id;
        }
    }
}
