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

        #region Pathプロパティ
        public string Path { get { return model.Path; } }
        #endregion

        #region StartCount変更通知プロパティ
        public int StartCount { get { return model.StartCount; } }
        #endregion


        public BitmapImage ScreenshotImage { get { return model.ScreenshotImage; } }

        public bool IsAvailableExecutable { get { return !string.IsNullOrEmpty(AppSpec.ExecutablePath); } }

        public bool IsAvailableSourceCode { get { return !string.IsNullOrEmpty(AppSpec.SourcePath); } }

        public string SourceAbsolutePath { get { return model.SourceAbsolutePath; } }

        public AppInfoViewModel(AppInfo model)
        {
            this.model = model;
            AppSpec = new AppSpecViewModel(model.AppSpec);

            CompositeDisposable.Add(new PropertyChangedEventListener(model, (sender, e) =>
            {
                if (e.PropertyName == "StartCount")
                    RaisePropertyChanged("StartCount");
            }));
        }

        public void Start(Action timeoutHandler)
        {
            model.Start(timeoutHandler);
        }

        public void OpenDirectory()
        {
            model.OpenDirectory();
        }
    }
}
