using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;

namespace CtrlLauncher.Models
{
    public class AppSpec : NotificationObject
    {
        public string Title { get; set; }

        public string ScreenshotPath { get; set; }

        public string ExecutablePath { get; set; }

        public string Argument { get; set; }

        public string SourcePath { get; set; }

        public TimeSpan TimeLimit { get; set; }

        public AppSpec()
        {

        }
    }
}
