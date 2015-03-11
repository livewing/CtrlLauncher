using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Livet;
using YamlDotNet.Serialization;
using System.IO;

namespace CtrlLauncher.Models
{
    public class AppSpec : NotificationObject
    {
        public string Title { get; set; }

        public string Genre { get; set; }

        public string ScreenshotPath { get; set; }

        public string ExecutablePath { get; set; }

        public string Argument { get; set; }

        public string SourcePath { get; set; }

        public TimeSpan TimeLimit { get; set; }

        public AppSpec()
        {

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
