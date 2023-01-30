using AnnoRDA.Loader;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Threading;

namespace AnnoRDA.Builder
{
    public class FileSystemBuilder
    {
        private FileSystemBuilder() {
            LoaderConfig = RdaArchiveLoaderConfig.Default;
        }

        public IList<string> ArchiveFileNames { get; private set; } = new List<string>();
        public IComparer<string>? SortComparer { get; private set; }
        public bool SortsItems { get => SortComparer is not null; }

        public RdaArchiveLoaderConfig LoaderConfig { get; private set; }

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

        public FileSystemBuilder OnlyArchivesMatchingRegex(string regex)
        {
            ArchiveFileNames = ArchiveFileNames.Where(f => Regex.IsMatch(Path.GetFileName(f), regex)).ToList();
            return this;
        }

        public FileSystemBuilder OnlyArchivesMatchingWildcard(string wildcard)
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

        public FileSystemBuilder AddWhitelisted(IEnumerable<string> whitelisted)
        {
            LoaderConfig.UseWhitelist = true;
            List<string> newwhitelist = new List<string>(LoaderConfig.WhitelistPatterns);
            newwhitelist.AddRange(whitelisted);
            LoaderConfig.WhitelistPatterns = newwhitelist;
            return this;
        }

        public FileSystemBuilder AddWhitelisted(params string[] whitelisted) => AddWhitelisted(whitelisted.AsEnumerable());

        public FileSystemBuilder AddBlacklisted(IEnumerable<string> blacklisted)
        {
            LoaderConfig.UseBlacklist = true;
            List<string> newblacklist = new List<string>(LoaderConfig.BlacklistPatterns);
            newblacklist.AddRange(blacklisted);
            LoaderConfig.BlacklistPatterns = newblacklist;
            return this;
        }

        public FileSystemBuilder AddBlacklisted(params string[] blacklisted) => AddBlacklisted(blacklisted.AsEnumerable());


        public FileSystemBuilder PreferBlacklistOverWhitelist()
        {
            LoaderConfig.PreferWhitelist = false;
            return this;
        }

        public FileSystemBuilder MatchFilenamesUsingRegex()
        { 
            LoaderConfig.UseRegexInsteadOfWildcard = true;
            return this;
        }

        public FileSystemBuilder ConfigureLoadZeroByteFiles(bool loadThem)
        {
            LoaderConfig.LoadZeroByteFiles = loadThem;
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
            FileSystemLoader loader = new FileSystemLoader(ArchiveFileNames, LoaderConfig);
            return loader.Load();
        }
    }
}
