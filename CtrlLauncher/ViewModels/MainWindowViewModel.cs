using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

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
        #region Title変更通知プロパティ
        private string _Title = "CTRL Launcher";

        public string Title
        {
            get
            { return _Title; }
            set
            { 
                if (_Title == value)
                    return;
                _Title = value;
                RaisePropertyChanged();
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


        public void Initialize()
        {
        }
    }
}
