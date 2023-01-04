using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnoRDA
{
    public class Folder : IFileSystemItem
    {
        public string Name { get; set; }

        private IDictionary<string, Folder> folders = new Dictionary<string, Folder>();
        public IEnumerable<Folder> Folders { get { return folders.Values; } }
        private IDictionary<string, File> files = new Dictionary<string, File>();
        public IEnumerable<File> Files { get { return files.Values; } }

        public IEnumerable<IFileSystemItem> Children { get { return ((IEnumerable<IFileSystemItem>)this.Folders).Concat(this.Files); } }
        public int ChildCount { get { return this.folders.Count + this.files.Count; } }

        public Folder(string name)
        {
            this.Name = name;
        }

        public void Add(Folder folder, AddMode addMode = AddMode.NewOrReplace)
        {
            Folder existingFolder;
            this.folders.TryGetValue(folder.Name, out existingFolder);

            if (existingFolder != null && addMode == AddMode.New)
            {
                throw new ArgumentException("A folder with this name already exists", "folder");
            }
            else if (existingFolder == null && addMode == AddMode.Replace)
            {
                throw new ArgumentException("No such folder exists", "folder");
            }

            if (existingFolder != null)
            {
                this.folders.Remove(folder.Name);
            }
            this.folders.Add(folder.Name, folder);
        }
        public void Add(File file, AddMode addMode = AddMode.NewOrReplace)
        {
            File existingFile;
            this.files.TryGetValue(file.Name, out existingFile);

            if (existingFile != null && addMode == AddMode.New)
            {
                throw new ArgumentException("A file with this name already exists", "file");
            }
            else if (existingFile == null && addMode == AddMode.Replace)
            {
                throw new ArgumentException("No such file exists", "file");
            }

            if (existingFile != null)
            {
                this.files.Remove(file.Name);
            }
            this.files.Add(file.Name, file);
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

        public void OverwriteWith(Folder overwriteFolder, IProgress<string> progress, System.Threading.CancellationToken ct)
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
