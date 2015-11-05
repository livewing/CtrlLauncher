using Livet.Commands;
using Livet.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlLauncher.ViewModels
{
    public class SettingsWindowViewModel : ViewModelBase
    {
        private LauncherCoreViewModel launcherCoreViewModel;

        private bool isTimeLimitModeDisabled;
        public bool IsTimeLimitModeDisabled
        {
            get { return isTimeLimitModeDisabled; }
            set
            {
                if (SetProperty(ref isTimeLimitModeDisabled, value))
                {
                    RaisePropertyChanged(nameof(IsEnabledGlobalTimeLimitControls));
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private bool isTimeLimitModeDefault;
        public bool IsTimeLimitModeDefault
        {
            get { return isTimeLimitModeDefault; }
            set { SetProperty(ref isTimeLimitModeDefault, value); }
        }

        private bool isTimeLimitModeEnabled;
        public bool IsTimeLimitModeEnabled
        {
            get { return isTimeLimitModeEnabled; }
            set { SetProperty(ref isTimeLimitModeEnabled, value); }
        }

        private bool isTimeLimitModeForced;
        public bool IsTimeLimitModeForced
        {
            get { return isTimeLimitModeForced; }
            set { SetProperty(ref isTimeLimitModeForced, value); }
        }

        private int globalTimeLimitMinutes = 0;
        public int GlobalTimeLimitMinutes
        {
            get { return globalTimeLimitMinutes; }
            set
            {
                if (SetProperty(ref globalTimeLimitMinutes, value))
                    SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private int globalTimeLimitSeconds = 0;
        public int GlobalTimeLimitSeconds
        {
            get { return globalTimeLimitSeconds; }
            set
            {
                if (SetProperty(ref globalTimeLimitSeconds, value))
                    SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsEnabledGlobalTimeLimitControls => !IsTimeLimitModeDisabled;

        public SettingsWindowViewModel(LauncherCoreViewModel launcherCore)
        {
            launcherCoreViewModel = launcherCore;

            switch (launcherCore.SettingsViewModel.GlobalTimeLimitMode)
            {
                case Models.Settings.TimeLimitMode.Disabled:
                    IsTimeLimitModeDisabled = true;
                    break;
                case Models.Settings.TimeLimitMode.Default:
                    IsTimeLimitModeDefault = true;
                    break;
                case Models.Settings.TimeLimitMode.Enabled:
                    IsTimeLimitModeEnabled = true;
                    break;
                case Models.Settings.TimeLimitMode.Forced:
                    IsTimeLimitModeForced = true;
                    break;
            }

            GlobalTimeLimitMinutes = launcherCore.SettingsViewModel.GlobalTimeLimit.Minutes;
            GlobalTimeLimitSeconds = launcherCore.SettingsViewModel.GlobalTimeLimit.Seconds;
        }

        #region SaveCommand
        private ViewModelCommand _SaveCommand;
        public ViewModelCommand SaveCommand => _SaveCommand ?? (_SaveCommand = new ViewModelCommand(Save, CanSave));

        public bool CanSave()
        {
            if (IsTimeLimitModeDisabled) return true;
            return !(GlobalTimeLimitSeconds == 0 && GlobalTimeLimitMinutes == 0);
        }

        public async void Save()
        {
            if (!CanSave()) return;

            if (IsTimeLimitModeDisabled) launcherCoreViewModel.SettingsViewModel.GlobalTimeLimitMode = Models.Settings.TimeLimitMode.Disabled;
            if (IsTimeLimitModeDefault) launcherCoreViewModel.SettingsViewModel.GlobalTimeLimitMode = Models.Settings.TimeLimitMode.Default;
            if (IsTimeLimitModeEnabled) launcherCoreViewModel.SettingsViewModel.GlobalTimeLimitMode = Models.Settings.TimeLimitMode.Enabled;
            if (IsTimeLimitModeForced) launcherCoreViewModel.SettingsViewModel.GlobalTimeLimitMode = Models.Settings.TimeLimitMode.Forced;

            launcherCoreViewModel.SettingsViewModel.GlobalTimeLimit = new TimeSpan(0, GlobalTimeLimitMinutes, GlobalTimeLimitSeconds);

            await launcherCoreViewModel.SaveSettingsAsync();
            Messenger.Raise(new InteractionMessage("Close"));
        }
        #endregion

    }
}
