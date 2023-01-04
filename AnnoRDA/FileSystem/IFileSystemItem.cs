using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnoRDA
{
    public enum FileSystemItemType { 
        Folder,
        File
    }

    public interface IFileSystemItem
    {
        string Name { get; set; }
        FileSystemItemType ItemType { get; }

        IEnumerable<IFileSystemItem> Children { get; }
        int ChildCount { get; }
    }

}
