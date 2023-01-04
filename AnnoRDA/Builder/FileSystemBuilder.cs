using AnnoRDA.Loader;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
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
            Sort();
            return this;
        }
        public FileSystemBuilder AddFile(string filepath)
        {
            ArchiveFileNames.Add(filepath);
            Sort();
            return this;
        }

        public FileSystemBuilder AddFiles(IEnumerable<string> filepaths)
        {
            foreach (string s in filepaths)
                ArchiveFileNames.Add(s);
            Sort();
            return this;
        }


        public FileSystemBuilder OnlyMatchingRegex(string regex)
        {
            ArchiveFileNames = ArchiveFileNames.Where(f => Regex.IsMatch(Path.GetFileName(f), regex)).ToList();
            return this;
        }

        public FileSystemBuilder OnlyMatchingWildcard(string wildcard)
        {
            ArchiveFileNames = ArchiveFileNames.Where(f => FileSystemName.MatchesSimpleExpression(wildcard, Path.GetFileName(f))).ToList();
            return this;
        }

        public FileSystemBuilder WithDefaultSorting()
        {
            SortComparer = new Util.NaturalFilenameStringComparer(); 
            Sort();
            return this;
        }

        public FileSystemBuilder WithSorting(IComparer<string> comp)
        {
            SortComparer = comp;
            Sort();
            return this;
        }

        private void Sort()
        {
            if (SortsItems)
                ArchiveFileNames = ArchiveFileNames.OrderBy(x => x, SortComparer).ToList();
        }

        public FileSystem Build()
        {
            Sort();
            if (!ArchiveFileNames.Any())
                throw new InvalidOperationException("Cannot load an RDA from zero archive files");
            FileSystemLoader loader = new FileSystemLoader(ArchiveFileNames);
            return loader.Load();
        }
    }
}
