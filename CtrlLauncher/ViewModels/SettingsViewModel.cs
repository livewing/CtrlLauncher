using CtrlLauncher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlLauncher.ViewModels
{
    public class SettingsViewModel
    {
        private Settings model;

        public Settings.TimeLimitMode GlobalTimeLimitMode
        {
            get { return model.GlobalTimeLimitMode; }
            set { model.GlobalTimeLimitMode = value; }
        }

        public TimeSpan GlobalTimeLimit
        {
            get { return model.GlobalTimeLimit; }
            set { model.GlobalTimeLimit = value; }
        }

        public SettingsViewModel(Settings model)
        {
            this.model = model;
        }
    }
}
