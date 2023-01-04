using System;
using System.Collections.Generic;
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
            this.Root.OverwriteWith(overwriteFS.Root, progress, ct);
        }
    }
}
