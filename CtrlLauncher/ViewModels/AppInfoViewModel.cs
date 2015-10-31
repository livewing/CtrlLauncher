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

        public AppSpecViewModel AppSpec { get; }

        public string Path => model.Path;

        public int StartCount => model.StartCount;


        public BitmapImage ScreenshotImage => model.ScreenshotImage;

        public bool IsAvailableExecutable => !string.IsNullOrEmpty(AppSpec.ExecutablePath);

        public bool IsAvailableSourceCode => !string.IsNullOrEmpty(AppSpec.SourcePath);

        public string SourceAbsolutePath => model.SourceAbsolutePath;

        public AppInfoViewModel(AppInfo model)
        {
            this.model = model;
            AppSpec = new AppSpecViewModel(model.AppSpec);

            CompositeDisposable.Add(new PropertyChangedEventListener(model, (sender, e) =>
            {
                if (e.PropertyName == nameof(StartCount))
                    RaisePropertyChanged(nameof(StartCount));
            }));
        }

        public void Start(Action timeoutHandler) => model.Start(timeoutHandler);

        public void OpenDirectory() => model.OpenDirectory();
    }
}
