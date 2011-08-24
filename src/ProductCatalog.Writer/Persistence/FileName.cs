using System;
using System.IO;
using System.Xml;

namespace ProductCatalog.Writer.Persistence
{
    public class FileName
    {
        public const string Extension = ".atom";
        
        public static FileName TempFileName()
        {
            return new FileName(Guid.NewGuid().ToString());
        }

        private readonly string value;

        public FileName(string value)
        {
            if (Path.HasExtension(value) && (!Path.GetExtension(value).Equals(".atom")))
            {
                throw new ArgumentException(string.Format("Invalid extension: [{0}]. Filename only supports .atom extension.", Path.GetExtension(value)));
            }
            this.value = Path.ChangeExtension(value, Extension);
        }

        public FileName Clone()
        {
            return new FileName(value);
        }

        public XmlWriter CreateXmlWriter(string directory, XmlWriterSettings settings)
        {
            return XmlWriter.Create(Path.Combine(directory, value), settings);
        }

        public XmlReader CreateXmlReader(string directory, XmlReaderSettings settings)
        {
            return XmlReader.Create(Path.Combine(directory, value), settings);
        }

        public override string ToString()
        {
            return value;
        }

        public bool Equals(FileName other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.value, value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (FileName)) return false;
            return Equals((FileName) obj);
        }

        public override int GetHashCode()
        {
            return (value != null ? value.GetHashCode() : 0);
        }
    }
}