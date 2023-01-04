using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnoRDA
{

    public class File : IFileSystemItem
    {
        public string Name { get; set; }
        public FileSystemItemType ItemType { get; } = FileSystemItemType.File;

        public IEnumerable<IFileSystemItem> Children { get { return Enumerable.Empty<IFileSystemItem>(); } }
        public int ChildCount { get { return 0; } }

        public long ModificationTimestamp { get; set; }
        public DateTime ModificationDate
        {
            get
            {
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dateTime = dateTime.AddSeconds(this.ModificationTimestamp);
                return dateTime;
            }
            set
            {
                DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                TimeSpan diff = value.ToUniversalTime() - origin;
                this.ModificationTimestamp = (long)Math.Floor(diff.TotalSeconds);
            }
        }

        public FileContentsSource ContentsSource { get; set; }

        public File(string name)
        {
            this.Name = name;
        }

        public File DeepClone()
        {
            return new File(this.Name)
            {
                ModificationTimestamp = this.ModificationTimestamp,
                ContentsSource = this.ContentsSource != null ? this.ContentsSource.DeepClone() : null,
            };
        }
    }
}
