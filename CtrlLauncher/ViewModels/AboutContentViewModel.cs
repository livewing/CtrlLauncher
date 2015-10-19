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
    public class AboutContentViewModel : ViewModel
    {
        private About model;

        #region Versionプロパティ
        public string Version
        {
            get
            {
                return model.Version;
            }
        }
        #endregion

        #region Licensesプロパティ
        public IEnumerable<Tuple<string, string, string>> Licenses
        {
            get
            {
                return model.Licenses;
            }
        }
        #endregion

        #region SelectedLicenseIndex変更通知プロパティ
        private int _SelectedLicenseIndex = 0;

        public int SelectedLicenseIndex
        {
            get
            { return _SelectedLicenseIndex; }
            set
            { 
                if (_SelectedLicenseIndex == value)
                    return;
                _SelectedLicenseIndex = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(SelectedLicense));
            }
        }
        #endregion

        #region SelectedLicense変更通知プロパティ
        public Tuple<string, string, string> SelectedLicense
        {
            get
            {
                return Licenses.ElementAtOrDefault(SelectedLicenseIndex);
            }
        }
        #endregion

        #region OpenUriCommand
        private ListenerCommand<string> _OpenUriCommand;

        public ListenerCommand<string> OpenUriCommand
        {
            get
            {
                if (_OpenUriCommand == null)
                {
                    _OpenUriCommand = new ListenerCommand<string>(OpenUri);
                }
                return _OpenUriCommand;
            }
        }

        public void OpenUri(string parameter)
        {
            model.OpenUri(parameter);
        }
        #endregion

        public AboutContentViewModel()
        {
            model = new About();
        }
    }
}
