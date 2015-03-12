using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media.Imaging;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using CtrlLauncher.Models;

namespace CtrlLauncher.ViewModels
{
    public class AppInfoViewModel : ViewModel
    {
        private AppInfo model;

        #region AppSpecプロパティ
        public AppSpecViewModel AppSpec { get; private set; }
        #endregion

        public BitmapImage ScreenshotImage { get { return model.ScreenshotImage; } }

        public AppInfoViewModel(AppInfo model)
        {
            this.model = model;
            AppSpec = new AppSpecViewModel(model.AppSpec);
        }
    }
}
