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
        }

        public void Initialize()
        {
        }
    }
}
