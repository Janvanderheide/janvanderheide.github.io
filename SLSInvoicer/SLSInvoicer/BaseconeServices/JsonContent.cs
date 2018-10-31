using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SLSInvoicer.BaseconeServices
{
    public class JsonContent : StringContent
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { new StringEnumConverter() }
        };

        public JsonContent(object contentObject)
            : base(JsonConvert.SerializeObject(contentObject, JsonSerializerSettings), Encoding.UTF8, "application/json"
            )
        {
        }

        public JsonContent(object contentObject, string mediaType)
            : base(JsonConvert.SerializeObject(contentObject, JsonSerializerSettings), Encoding.UTF8, mediaType)
        {
        }

        public JsonContent(string content) : base(content, Encoding.UTF8, "application/json")
        {
        }

        public JsonContent(string content, Encoding encoding) : base(content, encoding)
        {
        }

        public JsonContent(string content, Encoding encoding, string mediaType) : base(content, encoding, mediaType)
        {
        }
    }
}
