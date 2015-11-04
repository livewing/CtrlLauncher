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
    public class AboutContentViewModel : ViewModelBase
    {
        private About model;

        public string Version =>  model.Version;

        public IEnumerable<Tuple<string, string, string>> Licenses => model.Licenses;

        private int _SelectedLicenseIndex = 0;
        public int SelectedLicenseIndex
        {
            get { return _SelectedLicenseIndex; }
            set
            {
                if (SetProperty(ref _SelectedLicenseIndex, value))
                    RaisePropertyChanged(nameof(SelectedLicense));
            }
        }

        public Tuple<string, string, string> SelectedLicense => Licenses.ElementAtOrDefault(SelectedLicenseIndex);

        private ListenerCommand<string> _OpenUriCommand;
        public ListenerCommand<string> OpenUriCommand => _OpenUriCommand ?? (_OpenUriCommand = new ListenerCommand<string>(OpenUri));

        public void OpenUri(string parameter) => model.OpenUri(parameter);

        public AboutContentViewModel() : base()
        {
            model = new About();
        }
    }
}
