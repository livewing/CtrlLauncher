using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using CtrlLauncher.Models;

namespace CtrlLauncher.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        #region LauncherCoreViewModel プロパティ
        public LauncherCoreViewModel LauncherCoreViewModel { get; private set; }
        #endregion

        #region IsMaintenanceModeプロパティ
        public bool IsMaintenanceMode { get; private set; }
        #endregion

        #region Titleプロパティ
        public string Title
        {
            get
            {
                return "CTRL Launcher" + (IsMaintenanceMode ? " [メンテナンスモード]" : "");
            }
        }
        #endregion

        #region SelectedAppInfo変更通知プロパティ
        private AppInfoViewModel _SelectedAppInfo = null;

        public AppInfoViewModel SelectedAppInfo
        {
            get
            { return _SelectedAppInfo; }
            set
            { 
                if (_SelectedAppInfo == value)
                    return;
                _SelectedAppInfo = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsVisibleNoSelectionText");
            }
        }
        #endregion

        #region IsVisibleNoSelectionText変更通知プロパティ
        public bool IsVisibleNoSelectionText { get { return !LauncherCoreViewModel.IsAppsEmpty && SelectedAppInfo == null; } }
        #endregion

        #region IsOpenAboutFlyout変更通知プロパティ
        private bool _IsOpenAboutFlyout = false;

        public bool IsOpenAboutFlyout
        {
            get
            { return _IsOpenAboutFlyout; }
            set
            { 
                if (_IsOpenAboutFlyout == value)
                    return;
                _IsOpenAboutFlyout = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region OpenAboutFlyoutCommand
        private ViewModelCommand _OpenAboutFlyoutCommand;

        public ViewModelCommand OpenAboutFlyoutCommand
        {
            get
            {
                if (_OpenAboutFlyoutCommand == null)
                {
                    _OpenAboutFlyoutCommand = new ViewModelCommand(OpenAboutFlyout);
                }
                return _OpenAboutFlyoutCommand;
            }
        }

        public void OpenAboutFlyout()
        {
            IsOpenAboutFlyout = true;
        }
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
                if (e.PropertyName == "IsAppsEmpty") RaisePropertyChanged("IsVisibleNoSelectionText");
            }));
        }

        public async void Initialize()
        {
            await LauncherCoreViewModel.LoadAppsAsync();
        }
    }
}
