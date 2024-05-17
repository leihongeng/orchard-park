using Newtonsoft.Json;
using System;

namespace Orchard.Park.Core.Converters
{
    /// <summary>
    /// 长数字转字符串
    /// 如：长雪花Id会导致前端精度丢失的情况
    /// </summary>
    public class LongToStringConverter : JsonConverter
    {
        public override bool CanRead => false;

        public override bool CanConvert(Type objectType) => objectType == typeof(long) || objectType == typeof(long?);

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false.The type will skip the converter.");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                var str = value.ToString();
                if (str != null && str.Length >= 15)
                    writer.WriteValue(str);
                else
                    writer.WriteValue(value);
            }
        }
    }
}