using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Orchard.Park.AspNetCore.Filters
{
    public class SwaggerShowEnumDescriptionFilter : IDocumentFilter
    {
        private static string[] _assemblyStrings;

        public SwaggerShowEnumDescriptionFilter(string assemblyStrings)
        {
            if (!string.IsNullOrWhiteSpace(assemblyStrings))
                _assemblyStrings = assemblyStrings.Split(',');
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var enumDictionary = GetAllEnum();

            foreach (var (k, v) in swaggerDoc.Components.Schemas)
            {
                if (v.Enum is not { Count: > 0 }) continue;

                var itemType = enumDictionary.GetValueOrDefault(k);

                if (itemType == null) continue;

                var list = new List<OpenApiInteger>();

                foreach (var val in v.Enum)
                {
                    if (val is OpenApiInteger integer)
                        list.Add(integer);
                }

                if (string.IsNullOrWhiteSpace(v.Description))
                {
                    var descAttr = itemType.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(DescriptionAttribute));
                    if (descAttr != null && descAttr.ConstructorArguments.Any())
                    {
                        v.Description = descAttr.ConstructorArguments.First().Value?.ToString();
                    }
                }

                v.Description += DescribeEnum(itemType, list);
            }
        }

        private static Dictionary<string, Type> GetAllEnum()
        {
            var result = new Dictionary<string, Type>();

            if (_assemblyStrings == null) return result;

            foreach (var assemblyString in _assemblyStrings)
            {
                var ass = Assembly.Load(assemblyString);
                var types = ass.GetTypes();

                foreach (var item in types)
                {
                    if (!string.IsNullOrWhiteSpace(item.Name) && item.IsEnum)
                    {
                        result.TryAdd(item.Name, item);
                    }
                }
            }

            return result;
        }

        private static string DescribeEnum(Type type, IEnumerable<OpenApiInteger> enums)
        {
            if (type == null) return string.Empty;

            var enumDescriptions = (from item in enums
                                    let value = Enum.Parse(type, item.Value.ToString())
                                    let desc = GetDescription(type, value)
                                    select string.IsNullOrEmpty(desc)
                                        ? $"{item.Value} = {Enum.GetName(type, value)}"
                                        : $"{item.Value} = {desc}").ToList();

            return $"<br/>{Environment.NewLine}{string.Join("<br/>" + Environment.NewLine, enumDescriptions)}";
        }

        private static string GetDescription(Type t, object value)
        {
            foreach (var mInfo in t.GetMembers())
            {
                if (mInfo.Name != t.GetEnumName(value)) continue;

                foreach (var attr in Attribute.GetCustomAttributes(mInfo))
                {
                    if (attr.GetType() == typeof(DescriptionAttribute))
                    {
                        return ((DescriptionAttribute)attr).Description;
                    }
                }
            }

            return string.Empty;
        }
    }
}