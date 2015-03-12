using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Livet;

namespace CtrlLauncher.Models
{
    public class LauncherCore : NotificationObject
    {
        public string DataDirectoryPath
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data");
            }
        }

        #region Apps変更通知プロパティ
        private ObservableCollection<AppInfo> _Apps = new ObservableCollection<AppInfo>();

        public ObservableCollection<AppInfo> Apps
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

        public LauncherCore()
        {

        }

        public async Task LoadAppsAsync()
        {
            Apps.Clear();

            if (Directory.Exists(DataDirectoryPath))
            {
                var dirs = Directory.GetDirectories(DataDirectoryPath);
                foreach (var dir in dirs)
                {
                    try
                    {
                        var yamlPath = Path.Combine(dir, "spec.yaml");
                        if (File.Exists(yamlPath))
                        {
                            var spec = await AppSpec.LoadAsync(yamlPath);
                            Apps.Add(new AppInfo(spec, dir));
                        }
                    }
                    catch { }
                }
            }
        }
    }
}
