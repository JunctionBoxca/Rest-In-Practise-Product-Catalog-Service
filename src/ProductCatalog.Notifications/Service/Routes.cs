using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using ProductCatalog.Notifications.Model;
using ProductCatalog.Shared;

namespace ProductCatalog.Notifications.Service
{
    public class Routes
    {
        private readonly Uri baseAddress;
        private readonly UriTemplateTable uriTemplates;

        private static readonly Func<NameValueCollection, IRepositoryCommand> GetFeedOfRecentEvents =
            parameters => GetFeedOfRecentEventsCommand.Instance;

        private static readonly Func<NameValueCollection, IRepositoryCommand> GetFeed =
            parameters => new GetFeedCommand(new ResourceId(parameters.GetValues("id")[0]));
            
        public Routes(UriConfiguration uriConfiguration)
        {
            Check.IsNotNull(uriConfiguration, "uriConfiguration");

            baseAddress = uriConfiguration.BaseAddress;

            uriTemplates = new UriTemplateTable(baseAddress);
            uriTemplates.KeyValuePairs.Add(new KeyValuePair<UriTemplate, object>(uriConfiguration.RecentFeedTemplate, GetFeedOfRecentEvents));
            uriTemplates.KeyValuePairs.Add(new KeyValuePair<UriTemplate, object>(uriConfiguration.FeedTemplate, GetFeed));
        }

        public Uri BaseAddress
        {
            get { return baseAddress; }
        }

        public IRepositoryCommand CreateCommand(Uri uri)
        {
            UriTemplateMatch match = uriTemplates.MatchSingle(uri);

            if (match == null)
            {
                throw new InvalidUriException(string.Format("Invalid uri: [{0}]", uri.AbsoluteUri));
            }

            var commandFactoryMethod = (Func<NameValueCollection, IRepositoryCommand>)match.Data;
            return commandFactoryMethod.Invoke(match.BoundVariables);
        }
    }
}