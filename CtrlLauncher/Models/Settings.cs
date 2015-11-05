using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace CtrlLauncher.Models
{
    public class Settings
    {
        public TimeLimitMode GlobalTimeLimitMode { get; set; } = TimeLimitMode.Disabled;

        public TimeSpan GlobalTimeLimit { get; set; } = TimeSpan.FromMinutes(5);

        public Settings()
        {

        }

        public static async Task<Settings> LoadAsync(string path)
        {
            return await Task.Run(() =>
            {
                using (var sr = new StreamReader(path, Encoding.UTF8))
                {
                    return new Deserializer().Deserialize<Settings>(sr);
                }
            });
        }

        public async Task SaveAsync(string path)
        {
            await Task.Run(() =>
            {
                using (var sw = new StreamWriter(path))
                {
                    new Serializer(SerializationOptions.EmitDefaults).Serialize(sw, this);
                }
            });
        }

        public enum TimeLimitMode
        {
            Disabled,
            Default,
            Enabled,
            Forced
        }
    }
}
