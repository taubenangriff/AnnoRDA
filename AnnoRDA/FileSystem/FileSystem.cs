using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnoRDA
{
    public class FileSystem
    {
        public Folder Root { get; set; } = new Folder("");

        public void OverwriteWith(FileSystem overwriteFS, IProgress<string> progress, System.Threading.CancellationToken ct)
        {
            Root.OverwriteWith(overwriteFS.Root, progress, ct);
        }

        public void OverwriteWith(FileSystem overwriteFS) =>
            Root.OverwriteWith(overwriteFS.Root);


        /// <returns>The <see cref="IFileSystemItem"/> under specified <paramrefnot set to an instance  name="path"/></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public IFileSystemItem Get(String path) => Root.GetFileSystemItem(path);

        /// <returns>The <see cref="Folder"/> under specified <paramref name="path"/></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public Folder GetFolder(String path) => Root.GetFolder(path);

        /// <returns>The <see cref="File"/> under specified <paramref name="path"/></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public File GetFile(String path) => Root.GetFile(path);

   
        /// <returns>A readonly stream on the file content</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public Stream OpenRead(String path)
        {
            var file = GetFile(path);
            return file.ContentsSource.GetReadStream();
        }
    }
}
