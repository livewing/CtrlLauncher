using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

using Livet;

namespace CtrlLauncher.Models
{
    public class AppInfo : NotificationObject
    {
        public AppSpec AppSpec { get; private set; }

        public string Path { get; private set; }

        public BitmapImage ScreenshotImage { get; private set; }

        public AppInfo(AppSpec spec, string path)
        {
            AppSpec = spec;
            Path = path;
            try
            {
                ScreenshotImage = new BitmapImage(new Uri(toAbsolutePath(spec.ScreenshotPath)));
                ScreenshotImage.Freeze();
            }
            finally { }
        }

        private string toAbsolutePath(string relative)
        {
            return new Uri(new Uri(Path + "\\"), relative).LocalPath;
        }
    }
}
