using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace CtrlLauncher.Models
{
    public class FileEntry
    {
        public string Name { get; }

        public string Path { get; }

        public bool IsDirectory { get; }

        public long Size => IsDirectory ? 0 : new FileInfo(Path).Length;

        #region Childrenプロパティ
        private IEnumerable<FileEntry> _Children = null;
        public IEnumerable<FileEntry> Children
        {
            get
            {
                if (_Children != null)
                    return _Children;
                else if (Directory.Exists(Path))
                {
                    IEnumerable<string> dirs = Enumerable.Empty<string>();
                    IEnumerable<string> files = Enumerable.Empty<string>();

                    try
                    {
                        dirs = Directory.GetDirectories(Path);
                    }
                    catch { }
                    try
                    {
                        files = Directory.GetFiles(Path);
                    }
                    catch { }

                    _Children = dirs.Concat(files).Select(p => new FileEntry(p)).Where(e => !e.Name.StartsWith("."));
                }
                else
                    _Children = Enumerable.Empty<FileEntry>();
                return _Children;
            }
        }
        #endregion

        #region IconImage遅延評価プロパティ
        private bool isLoaded = false;
        private BitmapSource _IconImage = null;
        public BitmapSource IconImage
        {
            get
            {
                if (!isLoaded)
                {
                    IntPtr hIcon = Utils.GetIconHandle(Path);
                    if (hIcon != IntPtr.Zero)
                    {
                        _IconImage = Imaging.CreateBitmapSourceFromHIcon(hIcon, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                        _IconImage.Freeze();
                    }
                    isLoaded = true;
                }
                return _IconImage;
            }
        }
        #endregion

        public FileEntry(string path)
        {
            Path = path;
            Name = System.IO.Path.GetFileName(path);
            try
            {
                IsDirectory = Directory.Exists(Path);
            }
            catch { }
        }
    }
}
