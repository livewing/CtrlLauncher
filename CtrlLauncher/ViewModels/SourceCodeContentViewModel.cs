using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using ICSharpCode.AvalonEdit.Highlighting;

using CtrlLauncher.Models;

namespace CtrlLauncher.ViewModels
{
    public class SourceCodeContentViewModel : ViewModelBase
    {
        private IEnumerable<FileEntryViewModel> _SourceDirectoryEntry;
        public IEnumerable<FileEntryViewModel> SourceDirectoryEntry
        {
            get { return _SourceDirectoryEntry; }
            set { SetProperty(ref _SourceDirectoryEntry, value); }
        }

        private string _SourceCode;
        public string SourceCode
        {
            get { return _SourceCode; }
            set { SetProperty(ref _SourceCode, value); }
        }

        private IHighlightingDefinition _HighlightingDefinition;
        public IHighlightingDefinition HighlightingDefinition
        {
            get { return _HighlightingDefinition; }
            set { SetProperty(ref _HighlightingDefinition, value); }
        }

        public string FlyoutHeader => "ソースコードビューア" + (AppInfoViewModel == null ? "" : string.IsNullOrEmpty(AppInfoViewModel.AppSpec.Title) ? "" : " - " + AppInfoViewModel.AppSpec.Title);

        private string _FileName;
        public string FileName
        {
            get { return _FileName; }
            set { SetProperty(ref _FileName, value); }
        }

        private bool _IsLoading;
        public bool IsLoading
        {
            get { return _IsLoading; }
            set { SetProperty(ref _IsLoading, value); }
        }

        private string _ErrorMessage;
        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set
            {
                if (SetProperty(ref _ErrorMessage, value))
                    RaisePropertyChanged(nameof(IsVisibleErrorMessage));
            }
        }

        public bool IsVisibleErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        private AppInfoViewModel _AppInfoViewModel;
        public AppInfoViewModel AppInfoViewModel
        {
            get { return _AppInfoViewModel; }
            set
            {
                _AppInfoViewModel = value;

                SourceDirectoryEntry = new[] { new FileEntryViewModel(value.SourceAbsolutePath, value.AppSpec.Title) };
                SourceCode = "";
                FileName = "";
                ErrorMessage = "左のツリーからコードファイルを選択してください";

                RaisePropertyChanged();
                RaisePropertyChanged(nameof(FlyoutHeader));
            }
        }

        public async void ItemSelected(FileEntryViewModel entry)
        {
            if (entry == null || entry.IsDirectory) return;

            FileName = entry.Name;
            SourceCode = "";
            ErrorMessage = "";
            
            try
            {
                var size = entry.Size;
                if (size > 1024 * 1024) // 1 MB より大きいファイルは読み込まない
                {
                    ErrorMessage = "1 MB より大きいファイルは表示できません";
                    return;
                }

                IsLoading = true;

                var def = HighlightingManager.Instance.GetDefinitionByExtension(Path.GetExtension(entry.Path));
                HighlightingDefinition = def;

                // ファイルを読み込む
                byte[] data;
                using (var fs = new FileStream(entry.Path, FileMode.Open, FileAccess.Read))
                {
                    data = new byte[fs.Length];
                    await fs.ReadAsync(data, 0, data.Length);
                }

                // とりあえず UTF-8 として読み込んで NUL があったらバイナリ (簡易判定)
                if (Encoding.UTF8.GetString(data).Contains('\0'))
                {
                    ErrorMessage = "(バイナリファイル)";
                    return;
                }

                // 実際に判定する
                var encoding = Utils.GetEncoding(data);
                if (encoding == null)
                {
                    ErrorMessage = "(バイナリファイル)";
                    return;
                }
                SourceCode = encoding.GetString(data);

            }
            catch (Exception ex)
            {
                ErrorMessage = "エラー: " + ex.Message;
            }
            finally
            {
                IsLoading = false;
            }
        }

        public SourceCodeContentViewModel()
        {
        }
    }
}
