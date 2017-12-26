using System;
using System.Collections.Generic;

namespace LuisTrainer
{
    public class Example
    {
        public string Text { get; }
        public int Status { get; set; }
        public string UpdateTs { get; set; }
        public string Intent { get; }
        public IDictionary<string, string> EntitiyMap { get; }

        public Example(string text, int status, string updateTs, string intent, Dictionary<string, string> entitiyMap)
        {
            Text = text;
            Status = status;
            UpdateTs = updateTs;
            Intent = intent;
            EntitiyMap = entitiyMap;
        }
    }

    public static class ExampleExtensions
    {
        public static LuisExample ToLuisExample(this Example self)
        {
            var entitityLabels = self.EntitiyMap.Let(map =>
            {
                var labels = new List<EntityLabel>();
                foreach (var ent in map)
                {
                    labels.AddRange(ToEntities(ent.Key, ent.Value, self.Text));
                }

                return labels;
            });
        
            return new LuisExample
            {
                IntentName = self.Intent,
                Text = self.Text,
                EntityLabels = entitityLabels.ToArray()
            };
        }

        public static EntityLabel[] ToEntities(string entityName, string entityValue, string text) 
        {
            var words = entityValue.Contains("|") ? entityValue.Split('|') : new string[]{entityValue};
            var result = new List<EntityLabel>();
            foreach (var w in words)
            {
                if (string.IsNullOrEmpty(w))
                {
                    throw new InvalidOperationException($"{w} is null or empty");
                }

                var start = text.IndexOf(w, StringComparison.Ordinal);
                if (start < 0) {
                    throw new InvalidOperationException($"{w} is not contain in text '{text}'");
                }

                var end = start + w.Length - 1;
                result.Add(new EntityLabel 
                {
                    EntityName = entityName,
                    StartCharIndex = start,
                    EndCharIndex = end
                });
            }

            return result.ToArray();
        }
    }
}
