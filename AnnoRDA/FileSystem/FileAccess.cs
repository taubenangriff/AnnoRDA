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

        public Stream Open(String path, FileMode mode = FileMode.Open, System.IO.FileAccess access = System.IO.FileAccess.Read)
        {
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));

            //TODO Make sure this accepts '|' in the Path! 
            if (path.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                throw new NotSupportedException(nameof(path));

            if (mode != FileMode.Open)
                throw new NotSupportedException("FileModes other than Open will be supported in future releases");
            if (access != System.IO.FileAccess.Read)
                throw new NotSupportedException("FileAccess other than Read will be supported in future releases");

            var file = _fileSystem.Root.GetFile(path);
            return file.ContentsSource.GetReadStream();
        }

        public Stream OpenRead(String path) => Open(path, FileMode.Open, System.IO.FileAccess.Read);

        public StreamReader OpenText(string path) => new StreamReader(OpenRead(path));

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
