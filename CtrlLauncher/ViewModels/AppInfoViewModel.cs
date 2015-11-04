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
    public class AppInfoViewModel : ViewModelBase<AppInfo>
    {
        public AppSpecViewModel AppSpec { get; }

        public string Path => Model.Path;

        public int StartCount => Model.StartCount;


        public BitmapImage ScreenshotImage => Model.ScreenshotImage;

        public bool IsAvailableExecutable => !string.IsNullOrEmpty(AppSpec.ExecutablePath);

        public bool IsAvailableSourceCode => !string.IsNullOrEmpty(AppSpec.SourcePath);

        public string SourceAbsolutePath => Model.SourceAbsolutePath;

        public AppInfoViewModel(AppInfo model) : base(model, false)
        {
            AppSpec = new AppSpecViewModel(model.AppSpec);

            CompositeDisposable.Add(new PropertyChangedEventListener(model, (sender, e) =>
            {
                if (e.PropertyName == nameof(StartCount))
                    RaisePropertyChanged(nameof(StartCount));
            }));
        }

        public void Start(Action timeoutHandler) => Model.Start(timeoutHandler);

        public void OpenDirectory() => Model.OpenDirectory();
    }
}
