using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AnnoRDA
{
    public class Folder : IFileSystemItem
    {
        public string Name { get; set; }
        
        public FileSystemItemType ItemType { get; } = FileSystemItemType.Folder;

        private IDictionary<string, IFileSystemItem> _childMap = new SortedDictionary<string, IFileSystemItem>();

        public IEnumerable<Folder> Folders { get => Children.Where(x => x.ItemType == FileSystemItemType.Folder).Select(x => (Folder)x); }
        public IEnumerable<File> Files { get => Children.Where(x => x.ItemType == FileSystemItemType.File).Select(x => (File)x); }
        public IEnumerable<IFileSystemItem> Children { get => _childMap.Values; }
        
        public int ChildCount { get => _childMap.Count; }

        public Folder(string name)
        {
            this.Name = name;
        }

        public File? GetFile(String name)
        { 
            var child = GetChild(name);
            if (child?.ItemType != FileSystemItemType.File)
                return null;
            return (File)child;
        }

        public Folder? GetFolder(String name)
        {
            var child = GetChild(name);
            if (child?.ItemType != FileSystemItemType.Folder)
                return null;
            return (Folder)child;
        }

        public IFileSystemItem? GetChild(String name)
        {
            if (_childMap.TryGetValue(name, out var fsItem))
                return fsItem;
            return null;
        }

        public void Add(IFileSystemItem item, AddMode addMode = AddMode.NewOrReplace)
        {
            _childMap.TryGetValue(item.Name, out var existingItem);

            if (existingItem != null && addMode == AddMode.New)
                throw new ArgumentException("A folder or file with this name already exists", item.ItemType.ToString());
            else if (existingItem == null && addMode == AddMode.Replace)
                throw new ArgumentException("No such folder exists", item.ItemType.ToString());

            if (existingItem != null)
                _childMap.Remove(item.Name);
            _childMap.Add(item.Name, item);
        }

        public enum AddMode
        {
            /// <summary>
            /// Add a new item. An item with this name must not exist yet.
            /// </summary>
            New,
            /// <summary>
            /// Replace an existing item. An item with this name and type must already exist.
            /// </summary>
            Replace,
            /// <summary>
            /// If no item with this name and type does not exist yet, add the item. Else replace the existing item.
            /// </summary>
            NewOrReplace,
        }

        public void OverwriteWith(Folder overwriteFolder, IProgress<string>? progress, System.Threading.CancellationToken ct)
        {
            if (progress != null)
            {
                progress.Report(this.Name);
            }

            foreach (var overwriteSubFolder in overwriteFolder.Folders)
            {
                ct.ThrowIfCancellationRequested();

                var baseSubFolder = this.Folders.FirstOrDefault((f) => f.Name == overwriteSubFolder.Name);
                if (baseSubFolder == null)
                {
                    baseSubFolder = new Folder(overwriteSubFolder.Name);
                    this.Add(baseSubFolder);
                }

                baseSubFolder.OverwriteWith(overwriteSubFolder, progress, ct);
            }

            foreach (var overwriteFile in overwriteFolder.Files)
            {
                ct.ThrowIfCancellationRequested();

                this.Add(overwriteFile.DeepClone());
            }
        }
    }
}
