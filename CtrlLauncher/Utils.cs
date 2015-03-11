using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlLauncher
{
    public static class Utils
    {
        public static string GetRelativePath(string basePath, string targetPath)
        {
            string result;
            if (TryGetRelativePath(basePath, targetPath, out result))
                return result;
            else
                throw new Exception("相対パスを取得できませんでした。");
        }

        public static bool TryGetRelativePath(string basePath, string targetPath, out string result)
        {
            StringBuilder sb = new StringBuilder(1024);
            var ret = Win32.PathRelativePathTo(sb, basePath, FileAttributes.Directory, targetPath, FileAttributes.Normal);
            if (ret)
            {
                var s = sb.ToString();
                if (s.StartsWith(@".\") && s.Length > 2)
                    result = s.Substring(2);
                else
                    result = s;
            }
            else
                result = null;
            return ret;
        }
    }
}
