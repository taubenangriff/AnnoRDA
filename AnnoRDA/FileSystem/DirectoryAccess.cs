using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AnnoRDA
{
    public class DirectoryAccess
    {

        #region Constructor 
        //File System for accessing stuff
        private FileSystem _fileSystem;

        public DirectoryAccess(FileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }
        #endregion

        public bool Exists(string path) => throw new NotImplementedException();

        #region Enumerate
        public IEnumerable<string> EnumerateFiles(string path) => throw new NotImplementedException();
        public IEnumerable<string> EnumerateFiles(string path, string searchPattern) => throw new NotImplementedException();
        public IEnumerable<string> EnumerateFiles(string path, string searchPattern, EnumerationOptions enumerationOptions)
            => throw new NotImplementedException();
        public IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
            => throw new NotImplementedException();

        public IEnumerable<string> EnumerateDirectories(string path) => throw new NotImplementedException();
        public IEnumerable<string> EnumerateDirectories(string path, string searchPattern) => throw new NotImplementedException();
        public IEnumerable<string> EnumerateDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions)
            => throw new NotImplementedException();
        public IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
            => throw new NotImplementedException();

        public IEnumerable<string> EnumerateFileSystemEntries(string path) => throw new NotImplementedException();
        public IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern) => throw new NotImplementedException();
        public IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, EnumerationOptions enumerationOptions)
            => throw new NotImplementedException();
        public IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
            => throw new NotImplementedException();
        #endregion

        #region Get
        public string[] GetFiles(string path)
            => EnumerateFiles(path).ToArray(); 
        public string[] GetFiles(string path, string searchPattern)
            => EnumerateFiles(path, searchPattern).ToArray();
        public string[] GetFiles(string path, string searchPattern, EnumerationOptions enumerationOptions)
            => EnumerateFiles(path, searchPattern, enumerationOptions).ToArray(); 
        public string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
            => EnumerateFiles(path, searchPattern, searchOption).ToArray();

        public string[] GetDirectories(string path)
            => EnumerateDirectories(path).ToArray();
        public string[] GetDirectories(string path, string searchPattern)
            => EnumerateDirectories(path, searchPattern).ToArray();
        public string[] GetDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions)
            => EnumerateDirectories(path, searchPattern, enumerationOptions).ToArray();
        public string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
            => EnumerateDirectories(path, searchPattern, searchOption).ToArray();

        public string[] GetFileSystemEntries(string path)
            => EnumerateFileSystemEntries(path).ToArray();
        public string[] GetFileSystemEntries(string path, string searchPattern)
            => EnumerateFileSystemEntries(path, searchPattern).ToArray();
        public string[] GetFileSystemEntries(string path, string searchPattern, EnumerationOptions enumerationOptions)
            => EnumerateFileSystemEntries(path, searchPattern, enumerationOptions).ToArray();
        public string[] GetFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
            => EnumerateFileSystemEntries(path, searchPattern, searchOption).ToArray();
        #endregion

    }
}
