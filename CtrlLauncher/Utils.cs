using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

        public static string GetWindowCaption(IntPtr hWnd)
        {
            const int WM_GETTEXTLENGTH = 0x0E;
            const int WM_GETTEXT = 0x0D;
            int byteLengthWithoutNull = Win32.SendMessage(hWnd, WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero).ToInt32();
            IntPtr buffer = IntPtr.Zero;
            string value = null;
            try
            {
                buffer = Marshal.AllocCoTaskMem((byteLengthWithoutNull + 1) * 2);
                Win32.SendMessage(hWnd, WM_GETTEXT, new IntPtr(byteLengthWithoutNull + 1), buffer);
                value = Marshal.PtrToStringAuto(buffer);
            }
            finally
            {
                if (buffer != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(buffer);
            }
            return value;
        }

        public static void SetWindowCaption(IntPtr hWnd, string caption)
        {
            const int WM_SETTEXT = 0x0C;
            IntPtr buffer = IntPtr.Zero;
            try
            {
                buffer = Marshal.StringToCoTaskMemAuto(caption);
                Win32.SendMessage(hWnd, WM_SETTEXT, IntPtr.Zero, buffer);
            }
            finally
            {
                if (buffer != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(buffer);
            }
        }
    }
}
