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
using System.Threading.Tasks;

namespace CtrlLauncher.ViewModels
{
    public class AppSpecViewModel : ViewModel
    {
        private AppSpec model;

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

        public AppSpecViewModel() : this(new AppSpec()) { }

        public AppSpecViewModel(AppSpec model)
        {
            this.model = model;
        }

        public async Task SaveAsync(string path) => await model.SaveAsync(path);
    }
}
