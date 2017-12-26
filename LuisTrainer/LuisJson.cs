// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using QuickType;
//
//    var data = LuisExample.FromJson(jsonString);
//
namespace LuisTrainer
{
    using System;
    using System.Net;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public partial class LuisExample
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("intentName")]
        public string IntentName { get; set; }

        [JsonProperty("entityLabels")]
        public EntityLabel[] EntityLabels { get; set; }
    }

    public partial class EntityLabel
    {
        [JsonProperty("entityName")]
        public string EntityName { get; set; }

        [JsonProperty("startCharIndex")]
        public long StartCharIndex { get; set; }

        [JsonProperty("endCharIndex")]
        public long EndCharIndex { get; set; }
    }

    public partial class LuisExample
    {
        public static LuisExample FromJson(string json) => JsonConvert.DeserializeObject<LuisExample>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this LuisExample self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
