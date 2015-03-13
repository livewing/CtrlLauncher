﻿using System;
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
    public class LauncherCoreViewModel : ViewModel
    {
        private LauncherCore model;

        private bool isLoading = false;

        #region Apps変更通知プロパティ
        private ReadOnlyDispatcherCollection<AppInfoViewModel> _Apps;

        public ReadOnlyDispatcherCollection<AppInfoViewModel> Apps
        {
            get
            { return _Apps; }
            set
            { 
                if (_Apps == value)
                    return;
                _Apps = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region IsAppsEmpty変更通知プロパティ
        public bool IsAppsEmpty { get { return !isLoading && Apps.Count == 0; } }
        #endregion

        public LauncherCoreViewModel()
        {
            model = new LauncherCore();
            Apps = ViewModelHelper.CreateReadOnlyDispatcherCollection(model.Apps, (ai) => new AppInfoViewModel(ai), DispatcherHelper.UIDispatcher);
            CompositeDisposable.Add(() => Apps.Dispose());
        }

        public async Task LoadAppsAsync()
        {
            isLoading = true;
            RaisePropertyChanged("IsAppsEmpty");
            await model.LoadAppsAsync();
            isLoading = false;
            RaisePropertyChanged("IsAppsEmpty");
        }

        public void StartApp(AppInfoViewModel app, Action timeoutHandler)
        {
            app.Start(timeoutHandler);
        }

        public void ClearCount()
        {
            model.ClearCount();
        }
    }
}
