using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ZenithDiscordBot.config
{
    public class JSONReader
    {
        public string token { get; set; } = string.Empty;
        public string prefix { get; set; } = string.Empty;

        public async Task ReadJSON()
        {
            string configPath = "config.json";
            
            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException("Configuration file not found", configPath);
            }

            using (StreamReader sr = new StreamReader(configPath)) 
            {
                string json = await sr.ReadToEndAsync();
                JSONStructure? data = JsonConvert.DeserializeObject<JSONStructure>(json);

                if (data == null)
                {
                    throw new InvalidDataException("Failed to deserialize configuration");
                }

                this.token = data.token ?? throw new InvalidDataException("Token is missing in config");
                this.prefix = data.prefix ?? "!"; // Default prefix if null
            }
        }
    }

    internal sealed class JSONStructure
    {
        public string token { get; set; } = string.Empty;
        public string prefix { get; set; } = string.Empty;
    }
}