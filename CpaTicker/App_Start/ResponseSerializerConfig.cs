using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json;

namespace CpaTicker
{
    public static class ResponseSerializerConfig
    {
        public static void Register(HttpConfiguration config)
        {
            JsonResponse(config.Formatters.JsonFormatter);
            XmlResponse(config);    //no xml response
        }

        private static void XmlResponse(HttpConfiguration config)
        {
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }

        private static void JsonResponse(JsonMediaTypeFormatter json)
        {
            json.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            json.SerializerSettings.Formatting = Formatting.Indented;
        }
    }
}
