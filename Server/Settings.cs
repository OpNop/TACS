using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace TACS_Server
{
    public class Settings
    {
        [JsonProperty("build")]
        public int Build { get; private set; }
        
        [JsonProperty("log")]
        public LogSettings Log { get; private set; }

        [JsonProperty("guilds")]
        public ReadOnlyCollection<GuildSettings> Guilds { get; private set; }

        public static Settings Load()
        {
            string reader = "";
            try
            {
                reader = File.ReadAllText("./settings.json");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("settings.json is missing. Press any key to exit.");
                Console.ReadKey();
            }
            
            return JsonConvert.DeserializeObject<Settings>(reader);
        }
    }

    public class LogSettings
    {
        [JsonProperty("prefix")]
        public string Prefix { get; private set; }

        [JsonProperty("suffix")]
        public string Suffix { get; private set; }

        [JsonProperty("directory")]
        public string Directory { get; private set; }
    }

    public class GuildSettings
    {
        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("guid")]
        public Guid Guid { get; private set; }
    }

}
