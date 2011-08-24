using System.IO;
using System.Reflection;
using log4net;

namespace ProductCatalog.Shared
{
    public class FileSystemConfiguration
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string currentDirectoryPath;
        private readonly string archiveDirectoryPath;
        private readonly string entryDirectoryPath;

        private const string Current = "productcatalog";
        private const string Archive = "archive";
        private const string Entries = "entries";

        public FileSystemConfiguration(string rootDirectory)
        {
            currentDirectoryPath = Path.Combine(rootDirectory, Current);
            archiveDirectoryPath = Path.Combine(currentDirectoryPath, Archive);
            entryDirectoryPath = Path.Combine(currentDirectoryPath, Entries);

            CheckDirectoryExists(currentDirectoryPath);
            CheckDirectoryExists(archiveDirectoryPath);
            CheckDirectoryExists(entryDirectoryPath);
        }

        private static void CheckDirectoryExists(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (directoryInfo.Exists)
            {
                Log.DebugFormat("Directory exists: [{0}].", directoryInfo.FullName);
            }
            else
            {
                Log.DebugFormat("Creating directory: [{0}].", directoryInfo.FullName);
                directoryInfo.Create();
            }
        }

        public string CurrentDirectoryPath
        {
            get { return currentDirectoryPath; }
        }

        public string ArchiveDirectoryPath
        {
            get { return archiveDirectoryPath; }
        }

        public string EntryDirectoryPath
        {
            get { return entryDirectoryPath; }
        }
    }
}