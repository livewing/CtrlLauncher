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
    public class AppSpecGenerateWindowViewModel : ViewModel
    {
        private AppSpec model;

        #region TargetDirectory変更通知プロパティ
        private string _TargetDirectory = "";

        public string TargetDirectory
        {
            get
            { return _TargetDirectory; }
            set
            { 
                if (_TargetDirectory == value)
                    return;
                _TargetDirectory = value;
                RaisePropertyChanged();
                RaisePropertyChanged("RelativeScreenshotPath");
                RaisePropertyChanged("RelativeExecutablePath");
                RaisePropertyChanged("RelativeSourceDirectory");
            }
        }
        #endregion

        #region Title変更通知プロパティ
        private string _Title = "";

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

        #region Genre変更通知プロパティ
        private string _Genre;

        public string Genre
        {
            get
            { return _Genre; }
            set
            {
                if (_Genre == value)
                    return;
                _Genre = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region ScreenshotPath変更通知プロパティ
        private string _ScreenshotPath = "";

        public string ScreenshotPath
        {
            get
            { return _ScreenshotPath; }
            set
            { 
                if (_ScreenshotPath == value)
                    return;
                _ScreenshotPath = value;
                RaisePropertyChanged();
                RaisePropertyChanged("RelativeScreenshotPath");
            }
        }
        #endregion

        #region ExecutablePath変更通知プロパティ
        private string _ExecutablePath = "";

        public string ExecutablePath
        {
            get
            { return _ExecutablePath; }
            set
            { 
                if (_ExecutablePath == value)
                    return;
                _ExecutablePath = value;
                RaisePropertyChanged();
                RaisePropertyChanged("RelativeExecutablePath");
            }
        }
        #endregion

        #region Argument変更通知プロパティ
        private string _Argument = "";

        public string Argument
        {
            get
            { return _Argument; }
            set
            { 
                if (_Argument == value)
                    return;
                _Argument = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region SourceDirectory変更通知プロパティ
        private string _SourceDirectory = "";

        public string SourceDirectory
        {
            get
            { return _SourceDirectory; }
            set
            { 
                if (_SourceDirectory == value)
                    return;
                _SourceDirectory = value;
                RaisePropertyChanged();
                RaisePropertyChanged("RelativeSourceDirectory");
            }
        }
        #endregion


        #region RelativeScreenshotPath変更通知プロパティ
        public string RelativeScreenshotPath
        {
            get
            {
                string result;
                return Utils.TryGetRelativePath(TargetDirectory, ScreenshotPath, out result) ? result : "?";
            }
        }
        #endregion

        #region RelativeExecutablePath変更通知プロパティ
        public string RelativeExecutablePath
        {
            get
            {
                string result;
                return Utils.TryGetRelativePath(TargetDirectory, ExecutablePath, out result) ? result : "?";
            }
        }
        #endregion

        #region RelativeSourceDirectory変更通知プロパティ
        public string RelativeSourceDirectory
        {
            get
            {
                string result;
                return Utils.TryGetRelativePath(TargetDirectory, SourceDirectory, out result) ? result : "?";
            }
        }
        #endregion


        #region SetTargetDirectoryCommand
        private ListenerCommand<FolderSelectionMessage> _SetTargetDirectoryCommand;

        public ListenerCommand<FolderSelectionMessage> SetTargetDirectoryCommand
        {
            get
            {
                if (_SetTargetDirectoryCommand == null)
                {
                    _SetTargetDirectoryCommand = new ListenerCommand<FolderSelectionMessage>(SetTargetDirectory);
                }
                return _SetTargetDirectoryCommand;
            }
        }

        public void SetTargetDirectory(FolderSelectionMessage parameter)
        {
            if (parameter.Response != null)
                TargetDirectory = parameter.Response;
        }
        #endregion

        #region SetScreenshotPathCommand
        private ListenerCommand<OpeningFileSelectionMessage> _SetScreenshotPathCommand;

        public ListenerCommand<OpeningFileSelectionMessage> SetScreenshotPathCommand
        {
            get
            {
                if (_SetScreenshotPathCommand == null)
                {
                    _SetScreenshotPathCommand = new ListenerCommand<OpeningFileSelectionMessage>(SetScreenshotPath);
                }
                return _SetScreenshotPathCommand;
            }
        }

        public void SetScreenshotPath(OpeningFileSelectionMessage parameter)
        {
            if (parameter.Response != null && parameter.Response.Count() > 0)
                ScreenshotPath = parameter.Response[0];
        }
        #endregion

        #region SetExecutablePathCommand
        private ListenerCommand<OpeningFileSelectionMessage> _SetExecutablePathCommand;

        public ListenerCommand<OpeningFileSelectionMessage> SetExecutablePathCommand
        {
            get
            {
                if (_SetExecutablePathCommand == null)
                {
                    _SetExecutablePathCommand = new ListenerCommand<OpeningFileSelectionMessage>(SetExecutablePath);
                }
                return _SetExecutablePathCommand;
            }
        }

        public void SetExecutablePath(OpeningFileSelectionMessage parameter)
        {
            if (parameter.Response != null && parameter.Response.Count() > 0)
                ExecutablePath = parameter.Response[0];
        }
        #endregion

        #region SetSourceDirectoryCommand
        private ListenerCommand<FolderSelectionMessage> _SetSourceDirectoryCommand;

        public ListenerCommand<FolderSelectionMessage> SetSourceDirectoryCommand
        {
            get
            {
                if (_SetSourceDirectoryCommand == null)
                {
                    _SetSourceDirectoryCommand = new ListenerCommand<FolderSelectionMessage>(SetSourceDirectory);
                }
                return _SetSourceDirectoryCommand;
            }
        }

        public void SetSourceDirectory(FolderSelectionMessage parameter)
        {
            if (parameter.Response != null)
                SourceDirectory = parameter.Response;
        }
        #endregion

        #region GenerateCommand
        private ViewModelCommand _GenerateCommand;

        public ViewModelCommand GenerateCommand
        {
            get
            {
                if (_GenerateCommand == null)
                {
                    _GenerateCommand = new ViewModelCommand(Generate);
                }
                return _GenerateCommand;
            }
        }

        public void Generate()
        {

        }
        #endregion


        public AppSpecGenerateWindowViewModel()
        {
            model = new AppSpec();
        }

        public void Initialize()
        {
        }
    }
}
