using AnnoRDA.Loader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AnnoRDA.Builder
{
    public class FileSystemBuilder
    {
        private FileSystemBuilder() { }

        public IList<string> ArchiveFileNames { get; private set; } = new List<string>();
        public IComparer<string>? SortComparer { get; private set; }
        public bool SortsItems { get => SortComparer is not null; }

        public static FileSystemBuilder Create() { return new FileSystemBuilder(); }

        public FileSystemBuilder FromPath(string path, string pattern = "*.rda")
        {
            ArchiveFileNames = Directory.GetFiles(path, pattern);
            return this;
        }
        public FileSystemBuilder AddFile(string filepath)
        {
            ArchiveFileNames.Add(filepath);
            return this;
        }

        public FileSystemBuilder AddFiles(IEnumerable<string> filepaths)
        {
            foreach(string s in filepaths)
                ArchiveFileNames.Add(s);
            return this;
        }


        public FileSystemBuilder OnlyFilesLike(string pattern)
        {
            ArchiveFileNames = ArchiveFileNames.Where(f => Regex.IsMatch(f, pattern)).ToList();
            return this;
        }

        public FileSystemBuilder WithDefaultSorting()
        {
            SortComparer = new Util.NaturalFilenameStringComparer();
            return this;
        }

        public FileSystem Build()
        {
            IEnumerable<string> fileNames = ArchiveFileNames;
            if (SortsItems)
                fileNames = fileNames.OrderBy(x => x, SortComparer);
            FileSystemLoader loader = new FileSystemLoader(fileNames);
            return loader.Load();
        }
    }
}
