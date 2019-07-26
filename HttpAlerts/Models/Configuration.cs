using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace HttpAlerts.Models
{
    public class Configuration
    {
        public List<NotificationChannel> Notifications { get; set; }

        public List<HttpConfiguration> HttpConfigs { get; set; }

        public static Configuration FromYml()
        {
            string text = File.ReadAllText("http_alert.yml");

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new UnderscoredNamingConvention())
                .Build();

            return deserializer.Deserialize<Configuration>(text);
        }

        public static string GenerateYml(Configuration configuration)
        {
            var serializer = new SerializerBuilder()
                .WithNamingConvention(new UnderscoredNamingConvention())
                .Build();

            return serializer.Serialize(configuration);
        }
    }
}
