using System;
using System.IO;
using System.ServiceModel.Syndication;
using System.Xml;
using ProductCatalog.Notifications.Http;
using ProductCatalog.Notifications.Model;
using ProductCatalog.Tests.Shared.Utility;

namespace ProductCatalog.Tests.Notifications.Utility
{
    public class StreamBackedRepresentation : IRepresentation
    {
        public static StreamBackedRepresentation CreateCurrentFeed()
        {
            return new StreamBackedRepresentation(ResourceStreams.CurrentFeed);
        }

        public static StreamBackedRepresentation CreateArchiveFeed()
        {
            return new StreamBackedRepresentation(ResourceStreams.ArchiveFeed);
        }

        private readonly Func<Stream> func;

        private StreamBackedRepresentation(Func<Stream> func)
        {
            this.func = func;
        }

        public bool IsMatch(ETag eTag)
        {
            throw new NotImplementedException();
        }

        public void UpdateContext(IResponseContext context)
        {
            context.AddEntityBodyStreamAccessMethod(func);
        }

        public string GetContents()
        {
            using (StreamReader reader = new StreamReader(func.Invoke()))
            {
                return reader.ReadToEnd();
            }
        }

        public SyndicationFeed ToSyndicationFeed()
        {
            StringReader sr = new StringReader(GetContents());
            return SyndicationFeed.Load(XmlReader.Create(sr));
        }
    }
}