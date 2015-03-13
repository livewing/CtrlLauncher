using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Reactive.Linq;
using System.Windows.Media.Imaging;

using Livet;

namespace CtrlLauncher.Models
{
    public class AppInfo : NotificationObject
    {
        private LauncherCore core;

        public AppSpec AppSpec { get; private set; }

        public string Path { get; private set; }

        public BitmapImage ScreenshotImage { get; private set; }

        public int StartCount { get { return core.GetCount(this); } }

        public AppInfo(LauncherCore core, AppSpec spec, string path)
        {
            this.core = core;
            AppSpec = spec;
            Path = path;
            try
            {
                ScreenshotImage = new BitmapImage(new Uri(toAbsolutePath(spec.ScreenshotPath)));
                ScreenshotImage.Freeze();
            }
            catch { }
        }

        public void Start(Action timeoutHandler)
        {
            var process = new Process();
            process.StartInfo.ErrorDialog = false;
            process.StartInfo.FileName = toAbsolutePath(AppSpec.ExecutablePath);
            process.StartInfo.Arguments = AppSpec.Argument;
            process.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(toAbsolutePath(AppSpec.ExecutablePath));
            process.Start();

            core.SetCount(this, core.GetCount(this) + 1);
            RaisePropertyChanged("StartCount");

            if (AppSpec.TimeLimit > TimeSpan.Zero)
            {
                var timer = Observable.Interval(TimeSpan.FromMilliseconds(100)).Where(_ => !process.HasExited).Subscribe(_ =>
                {
                    var remaining = AppSpec.TimeLimit - (DateTime.Now - process.StartTime);
                    if (remaining < TimeSpan.Zero)
                    {
                        process.Kill();
                        if (timeoutHandler != null)
                            timeoutHandler();
                    }
                    else
                    {
                        process.Refresh();
                        var title = Utils.GetWindowCaption(process.MainWindowHandle);
                        if (title.Length > 0 && title[0] == '\t')
                            title = title.Remove(0, title.IndexOf('\t', 1) + 1);
                        if (title.Length == 0)
                            title = string.Format("\t残り{0:0}:{1:00}\t", (int)remaining.TotalMinutes, remaining.Seconds);
                        else
                            title = string.Format("\t残り{1:0}:{2:00} - \t{0}", title, (int)remaining.TotalMinutes, remaining.Seconds);
                        Utils.SetWindowCaption(process.MainWindowHandle, title);
                    }
                });

                process.Exited += (_, __) => timer.Dispose();
            }
        }

        private string toAbsolutePath(string relative)
        {
            return new Uri(new Uri(Path + "\\"), relative).LocalPath;
        }
    }
}
