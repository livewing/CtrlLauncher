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
    public class SourceCodeContentViewModel : ViewModel
    {
        #region FlyoutHeader変更通知プロパティ
        public string FlyoutHeader
        {
            get
            {
                return "ソースコードビューア" + (AppInfoViewModel == null ? "" : " - " + AppInfoViewModel.AppSpec.Title);
            }
        }
        #endregion

        #region AppInfoViewModel変更通知プロパティ
        private AppInfoViewModel _AppInfoViewModel;

        public AppInfoViewModel AppInfoViewModel
        {
            get
            { return _AppInfoViewModel; }
            set
            { 
                _AppInfoViewModel = value;
                RaisePropertyChanged();
                RaisePropertyChanged("FlyoutHeader");
            }
        }
        #endregion

        public SourceCodeContentViewModel()
        {

        }
    }
}
