using System;
using System.IO;
using System.Xml;

namespace ProductCatalog.Writer.Persistence
{
    public interface IDirectory
    {
        XmlWriter GetXmlWriter(FileName fileName);
        XmlReader GetXmlReader(FileName fileName);
        FileName GetLatest();
        void DeleteFiles(Func<FileInfo, bool> condition);
    }
}