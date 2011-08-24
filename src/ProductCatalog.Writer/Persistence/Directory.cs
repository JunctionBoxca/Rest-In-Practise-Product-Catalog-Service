using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using log4net;

namespace ProductCatalog.Writer.Persistence
{
    public class Directory : IDirectory
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly XmlWriterSettings WriterSettings = new XmlWriterSettings {Encoding = Encoding.UTF8, Indent = true, CloseOutput = true};
        private static readonly XmlReaderSettings ReaderSettings = new XmlReaderSettings {CloseInput = true};

        private readonly string directory;

        public Directory(string directory)
        {
            this.directory = directory;
        }

        public XmlWriter GetXmlWriter(FileName fileName)
        {
            return fileName.CreateXmlWriter(directory, WriterSettings);
        }

        public XmlReader GetXmlReader(FileName fileName)
        {
            return fileName.CreateXmlReader(directory, ReaderSettings);
        }

        public FileName GetLatest()
        {
            var fileInfo = (from file in new DirectoryInfo(directory).GetFiles("*" + FileName.Extension)
                            orderby file.LastWriteTime descending
                            select file).FirstOrDefault();
            if (fileInfo == null)
            {
                return null;
            }
            return new FileName(fileInfo.Name);
        }

        public void DeleteFiles(Func<FileInfo, bool> condition)
        {
            Log.DebugFormat("Start deleting files. Directory: [{0}].", directory);
            
            var files = from file in new DirectoryInfo(directory).GetFiles()
                        where condition(file)
                        select file;

            foreach (FileInfo file in files)
            {
                try
                {
                    Log.DebugFormat("Deleting file. FileName: [{0}].", file.Name);
                    file.Delete();
                }
                catch (IOException ex)
                {
                    Log.WarnFormat("Unable to delete file. FileName: [{0}]. Reason: [{1}]", file.Name, ex.Message);
                }
            }

            Log.Debug("End deleting files.");
        }
    }
}