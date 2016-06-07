using CtrlLauncher.Models;
using Livet;
using System;
using System.Threading.Tasks;

namespace CtrlLauncher.ViewModels
{
    public class LauncherCoreViewModel : ViewModelBase<LauncherCore>
    {
        private bool isLoading = false;

        public SettingsViewModel SettingsViewModel { get; private set; }

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

        public async Task InitializeAsync()
        {
            await LoadSettingsAsync();
            await LoadAppsAsync();
            SettingsViewModel = new SettingsViewModel(Model.Settings);
        }

        public async Task LoadSettingsAsync() => await Model.LoadSettingsAsync();

        public async Task SaveSettingsAsync() => await Model.SaveSettingsAsync();

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

        public async Task ExportApplicationIdAsync(string path) => await Model.ExportApplicationIdAsync(path);

        public void ClearCount() => Model.ClearCount();
    }
}
