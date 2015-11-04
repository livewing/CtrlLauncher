using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media.Imaging;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using CtrlLauncher.Models;

namespace CtrlLauncher.ViewModels
{
    public class FileEntryViewModel : ViewModelBase
    {
        private FileEntry model;
        private string customName = null;

        public string Name => customName ?? model.Name;

        public string Path => model.Path;

        public bool IsDirectory => model.IsDirectory;

        public long Size => model.Size;

        public IEnumerable<FileEntryViewModel> Children => model.Children.Select(m => new FileEntryViewModel(m));

        public BitmapSource IconImage => model.IconImage;

        public FileEntryViewModel(string path) : this(path, null) { }
        public FileEntryViewModel(string path, string customName)
        {
            this.model = new FileEntry(path);
            this.customName = customName;
        }

        private FileEntryViewModel(FileEntry model)
        {
            this.model = model;
        }
    }
}
