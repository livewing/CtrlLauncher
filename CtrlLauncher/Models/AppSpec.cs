using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace CtrlLauncher.Models
{
    public class AppSpec
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Genre { get; set; }

        public string ScreenshotPath { get; set; }

        public string ExecutablePath { get; set; }

        public string Argument { get; set; }

        public string SourcePath { get; set; }

        public TimeSpan TimeLimit { get; set; }

        public string Description { get; set; }

        public bool IsLate { get; set; } = false;

        public static async Task<AppSpec> LoadAsync(string path)
        {
            return await Task.Run<AppSpec>(() =>
            {
                using (var sr = new StreamReader(path, Encoding.UTF8))
                {
                    return new Deserializer().Deserialize<AppSpec>(sr);
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
    }
}
