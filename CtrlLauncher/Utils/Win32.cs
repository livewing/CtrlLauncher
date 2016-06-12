using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace CtrlLauncher
{
    public static class Win32
    {
        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0;
        public const uint SHGFI_SMALLICON = 0x1;

        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        public static extern bool PathRelativePathTo(
             [Out] StringBuilder pszPath,
             [In] string pszFrom,
             [In] FileAttributes dwAttrFrom,
             [In] string pszTo,
             [In] FileAttributes dwAttrTo
        );

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(
            IntPtr hWnd,
            int msg,
            IntPtr wParam,
            IntPtr lParam
        );

        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(
            string pszPath,
            uint dwFileAttributes,
            ref SHFILEINFO psfi,
            uint cbSizeFileInfo,
            uint uFlags
        );

        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        public const uint SWP_NOSIZE = 0x1;
        public const uint SWP_NOMOVE = 0x2;
        public const uint SWP_SHOWWINDOW = 0x40;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowPos(
            IntPtr hWnd,
            IntPtr hWndInsertAfter,
            int x,
            int y,
            int cx,
            int cy,
            uint uFlags
        );

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int IsIconic(
            IntPtr hWnd
        );

        public const int SW_SHOW = 0x5;
        public const int SW_SHOWNORMAL = 0x1;
        public const int SW_RESTORE = 0x9;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int ShowWindow(
            IntPtr hWnd,
            int nCmdShow
        );
    }
}
