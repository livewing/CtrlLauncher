using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using CtrlLauncher.Models;

namespace CtrlLauncher.ViewModels
{
    public class LauncherCoreViewModel : ViewModelBase<LauncherCore>
    {
        private bool isLoading = false;

        private ReadOnlyDispatcherCollection<AppInfoViewModel> _Apps;
        public ReadOnlyDispatcherCollection<AppInfoViewModel> Apps
        {
            get { return _Apps; }
            set { SetProperty(ref _Apps, value); }
        }

        public bool IsAppsEmpty => !isLoading && Apps.Count == 0;

        public LauncherCoreViewModel() : base(new LauncherCore())
        {
            Apps = ViewModelHelper.CreateReadOnlyDispatcherCollection(Model.Apps, (ai) => new AppInfoViewModel(ai), DispatcherHelper.UIDispatcher);
            CompositeDisposable.Add(() => Apps.Dispose());
        }

        public async Task LoadAppsAsync()
        {
            isLoading = true;
            RaisePropertyChanged(nameof(IsAppsEmpty));
            await Model.LoadAppsAsync();
            isLoading = false;
            RaisePropertyChanged(nameof(IsAppsEmpty));
        }

        public void StartApp(AppInfoViewModel app, Action timeoutHandler) => app.Start(timeoutHandler);

        public async Task ExportCountDataAsync(string path) => await Model.ExportCountDataAsync(path);

        public void ClearCount() => Model.ClearCount();
    }
}
