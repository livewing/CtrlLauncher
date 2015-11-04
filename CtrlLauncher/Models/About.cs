using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

using Livet;
using System.IO;

namespace CtrlLauncher.Models
{
    public class About
    {
        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString(3);

        public IEnumerable<Tuple<string, string, string>> Licenses { get; }

        public void OpenUri(string uriString) => Process.Start(uriString);

        public About()
        {
            // 埋め込まれたライセンス条項ファイルの読み込み
            var asm = Assembly.GetExecutingAssembly();
            var licenses = asm.GetManifestResourceNames().Where(n => n.StartsWith("CtrlLauncher.Resources.Licenses."));
            var list = new List<Tuple<string, string, string>>();
            foreach (var lic in licenses)
            {
                using (var sr = new StreamReader(asm.GetManifestResourceStream(lic)))
                {
                    var name = sr.ReadLine();
                    var url = sr.ReadLine();
                    sr.ReadLine();
                    var text = sr.ReadToEnd();
                    list.Add(new Tuple<string, string, string>(name, url, text));
                }
            }
            Licenses = list;
        }
    }
}
