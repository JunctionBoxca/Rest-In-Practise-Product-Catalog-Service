using System.IO;

namespace ProductCatalog.Notifications.Model
{
    public class CopyEntityBody : IEntityBodyTransformationStrategy
    {
        private readonly Stream source;

        public CopyEntityBody(Stream source)
        {
            this.source = source;
        }

        public void WriteEntityBody(Stream destination)
        {
            int size = 1024;

            using (source)
            {
                byte[] buffer = new byte[size];
                int count = source.Read(buffer, 0, size);
                while (count > 0)
                {
                    destination.Write(buffer, 0, count);
                    count = source.Read(buffer, 0, size);
                }
            }
        }
    }
}