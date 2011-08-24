using System;
using System.IO;
using System.Reflection;
using log4net;
using ProductCatalog.Shared;

namespace ProductCatalog.Writer.Persistence
{
    public class Reaper
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IFileSystem fileSystem;

        public Reaper(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public void OnFeedMappingsChanged(object sender, FeedMappingsChangedEventArgs args)
        {
            Log.Debug("Cleaning up current directory.");
            fileSystem.CurrentDirectory.DeleteFiles(fileInfo => FileNameDoesNotEqualCurrentFileName(fileInfo, args) && FileIsMoreThanOneMinuteOld(fileInfo));
        }

        private static bool FileIsMoreThanOneMinuteOld(FileInfo fileInfo)
        {
            return DateTime.Now.Subtract(fileInfo.LastWriteTime).TotalSeconds > 60;
        }

        private static bool FileNameDoesNotEqualCurrentFileName(FileInfo fileInfo, FeedMappingsChangedEventArgs args)
        {
            return fileInfo.Name != args.RecentEventsFeedStoreId;
        }
    }
}