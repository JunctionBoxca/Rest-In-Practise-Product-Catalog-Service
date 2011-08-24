using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using ProductCatalog.Writer.Persistence;

namespace ProductCatalog.Tests.Writer.Utility
{
    public class InMemoryFileSystem : IFileSystem
    {
        private static readonly XmlWriterSettings WriterSettings = new XmlWriterSettings {Encoding = Encoding.UTF8, Indent = true, CloseOutput = true};
        private static readonly XmlReaderSettings ReaderSettings = new XmlReaderSettings {CloseInput = true};

        private readonly FakeDirectory currentDirectory = new FakeDirectory();
        private readonly FakeDirectory archive = new FakeDirectory();
        private readonly FakeDirectory entry = new FakeDirectory();

        public IDirectory CurrentDirectory
        {
            get { return currentDirectory; }
        }

        public IDirectory ArchiveDirectory
        {
            get { return archive; }
        }

        public IDirectory EntryDirectory
        {
            get { return entry; }
        }

        public int FileCount(IDirectory directory)
        {
            return ((FakeDirectory)directory).Count;
        }

        public bool FileExists(IDirectory directory, FileName fileName)
        {
            return ((FakeDirectory) directory).FileExists(fileName);
        }

        private class FakeDirectory : IDirectory
        {
            private readonly Dictionary<FileName, MemoryStream> files = new Dictionary<FileName, MemoryStream>();
            private FileName latest;

            public XmlWriter GetXmlWriter(FileName fileName)
            {
                MemoryStream stream = new MemoryStream();
                XmlWriter writer = XmlWriter.Create(stream, WriterSettings);
                if (files.ContainsKey(fileName))
                {
                    files[fileName] = stream;
                }
                else
                {
                    files.Add(fileName, stream);
                }
                latest = fileName;
                return writer;
            }

            public XmlReader GetXmlReader(FileName fileName)
            {
                MemoryStream stream = new MemoryStream(files[fileName].ToArray());
                stream.Seek(0, SeekOrigin.Begin);
                return XmlReader.Create(stream, ReaderSettings);
            }

            public FileName GetLatest()
            {
                return latest;
            }

            public void DeleteFiles(Func<FileInfo, bool> condition)
            {
                //Do nothing
            }

            public bool FileExists(FileName fileName)
            {
                return files.ContainsKey(fileName);
            }

            public int Count
            {
                get { return files.Values.Count; }
            }
        }
    }
}