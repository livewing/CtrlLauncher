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

        public string Name
        {
            get { return model.Name; }
        }

        public IEnumerable<FileEntryViewModel> Children
        {
            get { return model.Children.Select(m => new FileEntryViewModel(m)); }
        }

        public BitmapSource IconImage
        {
            get { return model.IconImage; }
        }

        public FileEntryViewModel(string path)
        {
            this.model = new FileEntry(path);
        }

        private FileEntryViewModel(FileEntry model)
        {
            this.model = model;
        }
    }
}
