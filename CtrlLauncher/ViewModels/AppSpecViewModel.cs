using CtrlLauncher.Models;
using System;
using System.Threading.Tasks;

namespace CtrlLauncher.ViewModels
{
    public class AppSpecViewModel : ViewModelBase
    {
        private AppSpec model;

        public string Id
        {
            get { return model.Id; }
            set { model.Id = value; }
        }

        public string Title
        {
            get { return model.Title; }
            set { model.Title = value; }
        }

        public string Genre
        {
            get { return model.Genre; }
            set { model.Genre = value; }
        }

        public string ScreenshotPath
        {
            get { return model.ScreenshotPath; }
            set { model.ScreenshotPath = value; }
        }

        public string ExecutablePath
        {
            get { return model.ExecutablePath; }
            set { model.ExecutablePath = value; }
        }

        public string Argument
        {
            get { return model.Argument; }
            set { model.Argument = value; }
        }

        public string SourcePath
        {
            get { return model.SourcePath; }
            set { model.SourcePath = value; }
        }

        public TimeSpan TimeLimit
        {
            get { return model.TimeLimit; }
            set { model.TimeLimit = value; }
        }

        public string Description
        {
            get { return model.Description; }
            set { model.Description = value; }
        }

        public bool IsLate
        {
            get { return model.IsLate; }
            set { model.IsLate = value; }
        }

        public AppSpecViewModel() : this(new AppSpec()) { }

        public AppSpecViewModel(AppSpec model)
        {
            this.model = model;
        }

        public async Task SaveAsync(string path) => await model.SaveAsync(path);
    }
}
