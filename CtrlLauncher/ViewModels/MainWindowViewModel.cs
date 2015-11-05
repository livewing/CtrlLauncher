using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.IO;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CtrlLauncher.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public LauncherCoreViewModel LauncherCoreViewModel { get; }

        public SourceCodeContentViewModel SourceCodeContentViewModel { get; } = new SourceCodeContentViewModel();

        public bool IsMaintenanceMode { get; }

        public string Title => "CTRL Launcher" + (IsMaintenanceMode ? " [メンテナンスモード]" : "");

        private AppInfoViewModel _SelectedAppInfo = null;
        public AppInfoViewModel SelectedAppInfo
        {
            get { return _SelectedAppInfo; }
            set
            {
                if (SetProperty(ref _SelectedAppInfo, value))
                {
                    RaisePropertyChanged(nameof(IsVisibleNoSelectionText));
                    RaisePropertyChanged(nameof(IsVisibleLate));
                    StartCommand.RaiseCanExecuteChanged();
                    ShowSourceCodeCommand.RaiseCanExecuteChanged();
                    OpenDirectoryCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public bool IsVisibleNoSelectionText => !IsLoading && !LauncherCoreViewModel.IsAppsEmpty && SelectedAppInfo == null;

        private bool _IsLoading;
        public bool IsLoading
        {
            get { return _IsLoading; }
            set
            { 
                if (SetProperty(ref _IsLoading, value))
                    RaisePropertyChanged(nameof(IsVisibleNoSelectionText));
            }
        }

        private bool _IsCheckedVisibleStartCount = false;
        public bool IsCheckedVisibleStartCount
        {
            get { return _IsCheckedVisibleStartCount; }
            set
            {
                if (SetProperty(ref _IsCheckedVisibleStartCount, value))
                    RaisePropertyChanged(nameof(IsVisibleStartCount));
            }
        }

        public bool IsVisibleStartCount => IsMaintenanceMode && IsCheckedVisibleStartCount;

        public bool IsVisibleLate => SelectedAppInfo != null && SelectedAppInfo.AppSpec.IsLate;

        private bool _IsOpenSourceCodeFlyout;
        public bool IsOpenSourceCodeFlyout
        {
            get { return _IsOpenSourceCodeFlyout; }
            set { SetProperty(ref _IsOpenSourceCodeFlyout, value); }
        }

        private bool _IsOpenAboutFlyout = false;
        public bool IsOpenAboutFlyout
        {
            get { return _IsOpenAboutFlyout; }
            set { SetProperty(ref _IsOpenAboutFlyout, value); }
        }


        #region StartCommand
        private ViewModelCommand _StartCommand;
        public ViewModelCommand StartCommand => _StartCommand ?? (_StartCommand = new ViewModelCommand(Start, CanStart));

        public bool CanStart() => SelectedAppInfo == null ? false : SelectedAppInfo.IsAvailableExecutable;

        public void Start()
        {
            if (!CanStart()) return;
            var app = SelectedAppInfo;
            try
            {
                LauncherCoreViewModel.StartApp(app, () => Messenger.Raise(new InteractionMessage("Timeout")));
            }
            catch(Exception ex)
            {
                if (IsMaintenanceMode)
                    Messenger.Raise(new InformationMessage(
                        ex.Message + "\r\n\r\nファイルの配置とアクセス権限を確認して下さい。\r\nディレクトリ: " + app.Path + "\r\n実行ファイル相対パス: " + app.AppSpec.ExecutablePath,
                        "エラー", MessageBoxImage.Error, "Information"));
                else
                    Messenger.Raise(new InformationMessage(ex.Message, "エラー", System.Windows.MessageBoxImage.Error, "Information"));
            }
        }
        #endregion

        #region ShowSourceCodeCommand
        private ViewModelCommand _ShowSourceCodeCommand;

        public ViewModelCommand ShowSourceCodeCommand => _ShowSourceCodeCommand ?? (_ShowSourceCodeCommand = new ViewModelCommand(ShowSourceCode, CanShowSourceCode));
        public bool CanShowSourceCode() => SelectedAppInfo == null ? false : SelectedAppInfo.IsAvailableSourceCode;

        public void ShowSourceCode()
        {
            if (!CanShowSourceCode()) return;
            SourceCodeContentViewModel.AppInfoViewModel = SelectedAppInfo;
            IsOpenSourceCodeFlyout = true;
        }
        #endregion

        #region OpenDirectoryCommand
        private ViewModelCommand _OpenDirectoryCommand;
        public ViewModelCommand OpenDirectoryCommand => _OpenDirectoryCommand ?? (_OpenDirectoryCommand = new ViewModelCommand(OpenDirectory, CanOpenDirectory));

        public bool CanOpenDirectory() => SelectedAppInfo != null;

        public void OpenDirectory() => SelectedAppInfo.OpenDirectory();
        #endregion

        #region ExportCountDataCommand
        private ListenerCommand<SavingFileSelectionMessage> _ExportCountDataCommand;
        public ListenerCommand<SavingFileSelectionMessage> ExportCountDataCommand => _ExportCountDataCommand ?? (_ExportCountDataCommand = new ListenerCommand<SavingFileSelectionMessage>(ExportCountData));

        public async void ExportCountData(SavingFileSelectionMessage parameter)
        {
            if (parameter.Response != null && parameter.Response.Count() == 1)
            {
                try
                {
                    await LauncherCoreViewModel.ExportCountDataAsync(parameter.Response[0]);
                    Messenger.Raise(new InformationMessage("データが保存されました。", "開始回数データをエクスポート", System.Windows.MessageBoxImage.Information, "Information"));
                }
                catch (Exception ex)
                {
                    Messenger.Raise(new InformationMessage(ex.Message, "エラー", System.Windows.MessageBoxImage.Error, "Information"));
                }
            }
        }
        #endregion

        #region ExportApplicationIdCommand
        private ListenerCommand<SavingFileSelectionMessage> _ExportApplicationIdCommand;
        public ListenerCommand<SavingFileSelectionMessage> ExportApplicationIdCommand => _ExportApplicationIdCommand ?? (_ExportApplicationIdCommand = new ListenerCommand<SavingFileSelectionMessage>(ExportApplicationId));

        public async void ExportApplicationId(SavingFileSelectionMessage parameter)
        {
            if (parameter.Response != null && parameter.Response.Count() == 1)
            {
                try
                {
                    await LauncherCoreViewModel.ExportApplicationIdAsync(parameter.Response[0]);
                    Messenger.Raise(new InformationMessage("データが保存されました。", "アプリケーション ID リストをエクスポート", System.Windows.MessageBoxImage.Information, "Information"));
                }
                catch (Exception ex)
                {
                    Messenger.Raise(new InformationMessage(ex.Message, "エラー", System.Windows.MessageBoxImage.Error, "Information"));
                }
            }
        }
        #endregion

        #region ClearCountCommand
        private ViewModelCommand _ClearCountCommand;
        public ViewModelCommand ClearCountCommand => _ClearCountCommand ?? (_ClearCountCommand = new ViewModelCommand(ClearCount));

        public void ClearCount()
        {
            var msg = new ConfirmationMessage("開始回数データをすべて消去します。よろしいですか?", "警告", System.Windows.MessageBoxImage.Warning, System.Windows.MessageBoxButton.YesNo, "Confirmation");
            Messenger.Raise(msg);
            if (msg.Response ?? false == true)
            {
                LauncherCoreViewModel.ClearCount();
                Initialize();
            }
        }
        #endregion

        #region OpenAboutFlyoutCommand
        private ViewModelCommand _OpenAboutFlyoutCommand;
        public ViewModelCommand OpenAboutFlyoutCommand => _OpenAboutFlyoutCommand ?? (_OpenAboutFlyoutCommand = new ViewModelCommand(OpenAboutFlyout));

        public void OpenAboutFlyout() => IsOpenAboutFlyout = true;
        #endregion

        public MainWindowViewModel()
        {
#if DEBUG
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                IsMaintenanceMode = false;
            else
                IsMaintenanceMode = true;
#else
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                IsMaintenanceMode = true;
            else
                IsMaintenanceMode = false;
#endif

            LauncherCoreViewModel = new LauncherCoreViewModel();
            CompositeDisposable.Add(new PropertyChangedEventListener(LauncherCoreViewModel, (sender, e) =>
            {
                if (e.PropertyName == "IsAppsEmpty") RaisePropertyChanged(nameof(IsVisibleNoSelectionText));
            }));

            SourceCodeContentViewModel = new SourceCodeContentViewModel();
        }

        public async void Initialize()
        {
            IsLoading = true;
            await LauncherCoreViewModel.InitializeAsync();
            IsLoading = false;
        }

        public void Restart()
        {
            Application.Current.Shutdown();
            Process.Start(Application.ResourceAssembly.Location);
        }

        public void OpenSettingsWindow()
        {
            Messenger.Raise(new TransitionMessage(new SettingsWindowViewModel(LauncherCoreViewModel), "OpenSettings"));
        }
    }
}
