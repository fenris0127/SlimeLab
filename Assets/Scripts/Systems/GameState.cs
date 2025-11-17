using System;
using System.Collections.Generic;
using System.Text.Json;

namespace SlimeLab.Systems
{
    public class GameState
    {
        public string Version { get; set; }
        public long SaveTimestamp { get; set; }
        public Dictionary<string, JsonElement> Data { get; set; }

        public GameState()
        {
            Version = "1.0.0";
            SaveTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            Data = new Dictionary<string, JsonElement>();
        }

        public void SetValue<T>(string key, T value)
        {
            string jsonString = JsonSerializer.Serialize(value);
            JsonElement element = JsonSerializer.Deserialize<JsonElement>(jsonString);
            Data[key] = element;
        }

        public T GetValue<T>(string key)
        {
            if (Data.ContainsKey(key))
            {
                try
                {
                    return JsonSerializer.Deserialize<T>(Data[key].GetRawText());
                }
                catch
                {
                    return default(T);
                }
            }
            return default(T);
        }

        public string ToJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(this, options);
        }

        public static GameState FromJson(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<GameState>(json);
            }
            catch
            {
                return null;
            }
        }
    }
}
