using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnnoRDA
{
    [DebuggerDisplay("Folder: {Name}")]
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

        /// <exception cref="FileNotFoundException"></exception>
        /// <param name="path">the filepath relative to the folder</param>
        public File GetFile(String path)
        { 
            var child = GetFileSystemItem(path);
            if (child?.ItemType != FileSystemItemType.File)
                throw new FileNotFoundException(path);
            return (File)child;
        }
        /// <exception cref="FileNotFoundException"></exception>
        /// <param name="path">the filepath relative to the folder</param>
        public Folder GetFolder(String path)
        {
            var child = GetFileSystemItem(path);
            if (child?.ItemType != FileSystemItemType.Folder)
                throw new FileNotFoundException(path);
            return (Folder)child;
        }

        /// <exception cref="FileNotFoundException"></exception>
        /// <param name="path">the filepath relative to the folder</param>
        public IFileSystemItem GetFileSystemItem(String path)
        {
            var components = new Queue<string>(path.Split("/"));
            return GetFileSystemItem(path, components)!;
        }

        private IFileSystemItem GetFileSystemItem(String fullPath, Queue<String> pathComponents)
        {
            var itemname = pathComponents.Dequeue();

            if (_childMap.TryGetValue(itemname, out var item))
            {
                if (pathComponents.Count == 0)
                    return item;

                //recurse into subfolder
                if (item.ItemType != FileSystemItemType.Folder)

                    throw new FileNotFoundException(fullPath);
                var folder = (Folder)item;
                return folder.GetFileSystemItem(fullPath, pathComponents);
            }
            throw new FileNotFoundException(fullPath);
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

        public void OverwriteWith(Folder overwriteFolder) => OverwriteWith(overwriteFolder, null, CancellationToken.None);

        public void OverwriteWith(Folder overwriteFolder, IProgress<string>? progress, CancellationToken ct)
        {
            if (progress != null)
                progress.Report(this.Name);

            foreach (var overwriteSubFolder in overwriteFolder.Folders)
            {
                ct.ThrowIfCancellationRequested();

                Folder baseSubFolder;
                if (_childMap.TryGetValue(overwriteSubFolder.Name, out var item))
                {
                    baseSubFolder = (Folder)item;
                }
                else {
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
