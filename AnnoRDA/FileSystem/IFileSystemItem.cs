using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnoRDA
{
    public interface IFileSystemItem
    {
        string Name { get; set; }

        IEnumerable<IFileSystemItem> Children { get; }
        int ChildCount { get; }
    }

}
