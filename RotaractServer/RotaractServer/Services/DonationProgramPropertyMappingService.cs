using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RotaractServer.Models;

namespace RotaractServer.Services
{
    public class DonationProgramPropertyMappingService : IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _productPropertyMapping =
             new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
             {
               { "DonationProgramId", new PropertyMappingValue(new List<string>() { "DonationProgramId" } ) },
               { "DonationName", new PropertyMappingValue(new List<string>() { "DonationName" } )},
               { "Description", new PropertyMappingValue(new List<string>() { "Description" }) },
               { "ImageRef", new PropertyMappingValue(new List<string>() { "ImageRef" }) },
               { "StartDate", new PropertyMappingValue(new List<string>() { "StartDate" }) },
               { "EndDate", new PropertyMappingValue(new List<string>() { "EndDate" }) },
               { "Total", new PropertyMappingValue(new List<string>() { "Total" }) }
             };

        private IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public DonationProgramPropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<DonationProgramDto, DonationProgram>(_productPropertyMapping));
        }

        public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            // the string is separated by ",", so we split it.
            var fieldsAfterSplit = fields.Split(',');

            // run through the fields clauses
            foreach (var field in fieldsAfterSplit)
            {
                // trim
                var trimmedField = field.Trim();

                // remove everything after the first " " - if the fields 
                // are coming from an orderBy string, this part must be 
                // ignored
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedField : trimmedField.Remove(indexOfFirstSpace);

                // find the matching property
                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
        }


        public Dictionary<string, PropertyMappingValue> GetPropertyMapping
           <TSource, TDestination>()
        {
            // get matching mapping
            var matchingMapping = _propertyMappings
                .OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }

            throw new Exception($"Cannot find exact property mapping instance " +
                $"for <{typeof(TSource)},{typeof(TDestination)}");
        }
    }
}
