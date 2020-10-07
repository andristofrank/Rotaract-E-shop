using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RotaractServer.Services
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
        bool ValidMappingExistsFor<TSource, TDestination>(string fields);
    }
}