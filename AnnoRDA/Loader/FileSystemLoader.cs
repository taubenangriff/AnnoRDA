using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AnnoRDA.Loader
{
    /// <summary>
    /// Loads a collection of RDA container files into one file system.
    /// </summary>
    public class FileSystemLoader
    {
        private RdaArchiveLoader fileLoader = new RdaArchiveLoader();

        public IEnumerable<string> Archives { get; init; }

        public FileSystemLoader(String path)
        {
            Archives = SortContainerPaths(Directory.GetFiles(path, "*.rda"));
        }

        public FileSystemLoader(IEnumerable<string> archives)
        { 
             Archives = archives;
        }

        public FileSystem Load()
        {
            return Task.Run( async() => await LoadAsync(CancellationToken.None)).Result;
        }

        public FileSystem Load(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            FileSystem fileSystem = new FileSystem();
            foreach (string containerPath in Archives) {
                ct.ThrowIfCancellationRequested();

                FileSystem containerFileSystem = this.fileLoader.Load(containerPath, null, ct);
                fileSystem.OverwriteWith(containerFileSystem, null, ct);
            }
            return fileSystem;
        }

        public async Task<FileSystem> LoadAsync(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            Stopwatch stopwatch = Stopwatch.StartNew();
            List<Task<FileSystem>> fileSystemTasks = new List<Task<FileSystem>>();
            foreach (string containerPath in Archives)
            {
                fileSystemTasks.Add(Task.Run(() =>
                {
                    var rdaarchiveloader = new RdaArchiveLoader();
                    ct.ThrowIfCancellationRequested();
                    var containerFileSystem = rdaarchiveloader.Load(containerPath, null, ct);
                    return containerFileSystem;
                }));
            }
            var fileSystems = await Task.WhenAll(fileSystemTasks);
            stopwatch.Stop();
            Console.WriteLine($"Loading archives took us: {stopwatch.Elapsed.TotalMilliseconds} ms");

            Console.WriteLine("Loading finished! starting merge...");

            stopwatch = Stopwatch.StartNew();
            var result = MergeFileSystems(fileSystems);
            stopwatch.Stop();
            Console.WriteLine($"Merging took us: {stopwatch.Elapsed.TotalMilliseconds} ms");
            return result;
        }

        private FileSystem MergeFileSystems(IEnumerable<FileSystem> systems)
        { 
            var filesys = new FileSystem();
            foreach (FileSystem fs in systems)
            { 
                filesys.OverwriteWith(fs);
            }
            return filesys;
        }

        [Obsolete]
        public static IEnumerable<string> SortContainerPaths(IEnumerable<string> paths)
        {
            return paths.OrderBy((p) => p, new Util.NaturalFilenameStringComparer());
        }
    }
}
