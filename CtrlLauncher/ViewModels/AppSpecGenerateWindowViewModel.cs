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
using System.IO;

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
                RaisePropertyChanged(nameof(RelativeScreenshotPath));
                RaisePropertyChanged(nameof(RelativeExecutablePath));
                RaisePropertyChanged(nameof(RelativeSourceDirectory));

                GenerateCommand.RaiseCanExecuteChanged();
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
                RaisePropertyChanged(nameof(RelativeScreenshotPath));
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
                RaisePropertyChanged(nameof(RelativeExecutablePath));
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
                RaisePropertyChanged(nameof(RelativeSourceDirectory));
            }
        }
        #endregion

        #region TimeLimitMinutes変更通知プロパティ
        private int _TimeLimitMinutes = 0;

        public int TimeLimitMinutes
        {
            get
            { return _TimeLimitMinutes; }
            set
            { 
                if (_TimeLimitMinutes == value)
                    return;
                _TimeLimitMinutes = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region TimeLimitSeconds変更通知プロパティ
        private int _TimeLimitSeconds = 0;

        public int TimeLimitSeconds
        {
            get
            { return _TimeLimitSeconds; }
            set
            { 
                if (_TimeLimitSeconds == value)
                    return;
                _TimeLimitSeconds = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Description変更通知プロパティ
        private string _Description;

        public string Description
        {
            get
            { return _Description; }
            set
            { 
                if (_Description == value)
                    return;
                _Description = value;
                RaisePropertyChanged();
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


        #region IsSaving変更通知プロパティ
        private bool _IsSaving = false;

        public bool IsSaving
        {
            get
            { return _IsSaving; }
            set
            { 
                if (_IsSaving == value)
                    return;
                _IsSaving = value;
                RaisePropertyChanged();
                GenerateCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion


        #region SetTargetDirectoryCommand
        private ListenerCommand<FolderSelectionMessage> _SetTargetDirectoryCommand;
        public ListenerCommand<FolderSelectionMessage> SetTargetDirectoryCommand => _SetTargetDirectoryCommand ?? (_SetTargetDirectoryCommand = new ListenerCommand<FolderSelectionMessage>(SetTargetDirectory));

        public void SetTargetDirectory(FolderSelectionMessage parameter)
        {
            if (parameter.Response != null)
                TargetDirectory = parameter.Response + "\\";
        }
        #endregion

        #region SetScreenshotPathCommand
        private ListenerCommand<OpeningFileSelectionMessage> _SetScreenshotPathCommand;
        public ListenerCommand<OpeningFileSelectionMessage> SetScreenshotPathCommand => _SetScreenshotPathCommand ?? (_SetScreenshotPathCommand = new ListenerCommand<OpeningFileSelectionMessage>(SetScreenshotPath));

        public void SetScreenshotPath(OpeningFileSelectionMessage parameter)
        {
            if (parameter.Response != null && parameter.Response.Count() > 0)
                ScreenshotPath = parameter.Response[0];
        }
        #endregion

        #region SetExecutablePathCommand
        private ListenerCommand<OpeningFileSelectionMessage> _SetExecutablePathCommand;
        public ListenerCommand<OpeningFileSelectionMessage> SetExecutablePathCommand => _SetExecutablePathCommand ?? (_SetExecutablePathCommand = new ListenerCommand<OpeningFileSelectionMessage>(SetExecutablePath));

        public void SetExecutablePath(OpeningFileSelectionMessage parameter)
        {
            if (parameter.Response != null && parameter.Response.Count() > 0)
                ExecutablePath = parameter.Response[0];
        }
        #endregion

        #region SetSourceDirectoryCommand
        private ListenerCommand<FolderSelectionMessage> _SetSourceDirectoryCommand;
        public ListenerCommand<FolderSelectionMessage> SetSourceDirectoryCommand => _SetSourceDirectoryCommand ?? (_SetSourceDirectoryCommand = new ListenerCommand<FolderSelectionMessage>(SetSourceDirectory));

        public void SetSourceDirectory(FolderSelectionMessage parameter)
        {
            if (parameter.Response != null)
                SourceDirectory = parameter.Response + "\\";
        }
        #endregion

        #region GenerateCommand
        private ViewModelCommand _GenerateCommand;
        public ViewModelCommand GenerateCommand => _GenerateCommand ?? (_GenerateCommand = new ViewModelCommand(Generate, CanGenerate));

        public bool CanGenerate() => !IsSaving && !string.IsNullOrEmpty(TargetDirectory);

        public async void Generate()
        {
            if (!CanGenerate()) return;

            if (!Directory.Exists(TargetDirectory))
            {
                Messenger.Raise(new InformationMessage("ターゲットディレクトリが存在しません。", "エラー", System.Windows.MessageBoxImage.Error, "Information"));
                return;
            }

            var paths = new[] { ScreenshotPath, ExecutablePath, SourceDirectory };
            var rels = paths.Select(s => { string rel; return Utils.TryGetRelativePath(TargetDirectory, s, out rel) ? rel : null; });
            if (paths.Concat(rels).Concat(new[] { Title }).Any(s => string.IsNullOrEmpty(s)))
            {
                var msg = new ConfirmationMessage("正しく入力されていない項目があります。続行しますか?\r\nこのまま続行した場合、CTRL Launcher に正しく表示されない可能性があります。",
                    "入力が不十分です", System.Windows.MessageBoxImage.Question, System.Windows.MessageBoxButton.YesNo, "Confirmation");
                Messenger.Raise(msg);
                if (!(msg.Response ?? false))
                    return;
            }
            else if (rels.Any(s => s.StartsWith(@"..\")))
            {
                var msg = new ConfirmationMessage("ターゲットディレクトリ内に含まれていないパス設定があります。続行しますか?\r\nこのまま続行した場合、CTRL Launcher に正しく表示されない可能性があります。",
                    "ファイル配置に問題があります", System.Windows.MessageBoxImage.Question, System.Windows.MessageBoxButton.YesNo, "Confirmation");
                Messenger.Raise(msg);
                if (!(msg.Response ?? false))
                    return;
            }

            try
            {
                IsSaving = true;

                AppSpecViewModel spec = new AppSpecViewModel();

                spec.Title = Title;
                spec.Genre = Genre;
                spec.ScreenshotPath = rels.ElementAt(0);
                spec.ExecutablePath = rels.ElementAt(1);
                spec.Argument = Argument;
                spec.SourcePath = rels.ElementAt(2);
                spec.TimeLimit = new TimeSpan(0, TimeLimitMinutes, TimeLimitSeconds);
                spec.Description = Description;

                await spec.SaveAsync(Path.Combine(TargetDirectory, "spec.yml"));

                Messenger.Raise(new InformationMessage("保存に成功しました。\r\n" + TargetDirectory + " を zip 圧縮して所定の場所にアップロードしてください。",
                    "完了", System.Windows.MessageBoxImage.Information, "Information"));
                Messenger.Raise(new InteractionMessage("Close"));
            }
            catch (Exception ex)
            {
                Messenger.Raise(new InformationMessage(ex.Message, "エラー", System.Windows.MessageBoxImage.Error, "Information"));
            }
            finally
            {
                IsSaving = false;
            }
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
