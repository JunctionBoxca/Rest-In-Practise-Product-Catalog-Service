using System;
using System.IO;
using ProductCatalog.Notifications.Models;
using ProductCatalog.Notifications.Persistence;
using ProductCatalog.Shared;

namespace ProductCatalog.Notifications.Model
{
    public class ResourceIdToStoreIdConverter : IAcceptResourceIdParameters
    {
        public static ResourceIdToStoreIdConverter Default = new ResourceIdToStoreIdConverter(p => new StoreId<string>( Path.ChangeExtension(p[0], ".atom")));

        private readonly Func<ResourceIdParameters, IStoreId> convert;

        public ResourceIdToStoreIdConverter(Func<ResourceIdParameters, IStoreId> convert)
        {
            Check.IsNotNull(convert, "convert");

            this.convert = convert;
        }

        public IStoreId Convert(ResourceId resourceId)
        {
            return resourceId.ToStoreId(this);
        }

        IStoreId IAcceptResourceIdParameters.Accept(ResourceIdParameters parameters)
        {
            return convert(parameters);
        }
    }
}