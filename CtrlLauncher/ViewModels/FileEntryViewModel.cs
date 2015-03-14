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
    public class FileEntryViewModel : ViewModel
    {
        private FileEntry model;
        private string customName = null;

        public string Name
        {
            get
            {
                if (customName != null) return customName;
                return model.Name;
            }
        }

        public string Path
        {
            get { return model.Path; }
        }

        public bool IsDirectory
        {
            get { return model.IsDirectory; }
        }

        public long Size
        {
            get { return model.Size; }
        }

        public IEnumerable<FileEntryViewModel> Children
        {
            get { return model.Children.Select(m => new FileEntryViewModel(m)); }
        }

        public BitmapSource IconImage
        {
            get { return model.IconImage; }
        }

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
