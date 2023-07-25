using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnnoRDA
{
    public class FileAccess
    {
        #region Constructor 
        //File System for accessing stuff
        private FileSystem _fileSystem; 

        public FileAccess(FileSystem fileSystem) 
        {
            _fileSystem= fileSystem;
        }
        #endregion
        public bool Exists() => throw new NotImplementedException();

        #region Metadata 

        public DateTime GetCreationTime(String path) => throw new NotImplementedException();

        public DateTime GetCreationTimeUtc(String path) => throw new NotImplementedException();

        #endregion

        #region Opening

        public Stream Open(String path, FileMode mode) => throw new NotImplementedException();

        public Stream OpenRead(String path) => throw new NotImplementedException();

        public StreamReader OpenText(string path) => throw new NotImplementedException();

        #endregion

        #region ReadContents

        public byte[] ReadAllBytes(string path) => throw new NotImplementedException();

        public Task<byte[]> ReadAllBytesAsync(string path, CancellationToken token) => throw new NotImplementedException();

        public string[] ReadAllLines(string path) => ReadAllLines(path, Encoding.Default);
        public string[] ReadAllLines(string path, Encoding encoding ) => throw new NotImplementedException();

        public Task<string[]> ReadAllLinesAsync(string path) => ReadAllLinesAsync(path, Encoding.Default);
        public Task<string[]> ReadAllLinesAsync(string path, Encoding encoding) => throw new NotImplementedException();

        public string ReadAllText(string path) => ReadAllText(path, Encoding.Default);
        public string ReadAllText(string path, Encoding encoding) => throw new NotImplementedException();

        public Task<string> ReadAllTextAsync(string path) => ReadAllTextAsync(path, Encoding.Default);
        public Task<string> ReadAllTextAsync(string path, Encoding encoding) => throw new NotImplementedException();

        public IEnumerable<string> ReadLines(string path) => ReadLines(path, Encoding.Default);
        public IEnumerable<string> ReadLines(string path, Encoding encoding) => throw new NotImplementedException();

        public Task<IEnumerable<string>> ReadLinesAsync(string path) => ReadLinesAsync(path, Encoding.Default);
        public Task<IEnumerable<string>> ReadLinesAsync(string path, Encoding encoding) => throw new NotImplementedException();

        #endregion
    }
}
