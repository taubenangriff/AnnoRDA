﻿using System;
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
        public IEnumerable<string> Archives { get; init; }
        public RdaArchiveLoaderConfig LoaderConfig { get; init; }

        public FileSystemLoader(IEnumerable<string> archives) : this(archives, RdaArchiveLoaderConfig.Default)
        {
            Archives = archives;
        }
        public FileSystemLoader(IEnumerable<string> archives, RdaArchiveLoaderConfig loaderConfig)
        {
            LoaderConfig = loaderConfig;
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
            var rdaarchiveloader = new RdaArchiveLoader(LoaderConfig);
            foreach (string containerPath in Archives) {
                ct.ThrowIfCancellationRequested();

                FileSystem containerFileSystem = rdaarchiveloader.Load(containerPath, null, ct);
                fileSystem.OverwriteWith(containerFileSystem, null, ct);
            }
            return fileSystem;
        }

        public async Task<FileSystem> LoadAsync(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            List<Task<FileSystem>> fileSystemTasks = new List<Task<FileSystem>>();
            foreach (string containerPath in Archives)
            {
                fileSystemTasks.Add(Task.Run(() =>
                {
                    var rdaarchiveloader = new RdaArchiveLoader(LoaderConfig);
                    ct.ThrowIfCancellationRequested();
                    var containerFileSystem = rdaarchiveloader.Load(containerPath, null, ct);
                    return containerFileSystem;
                }));
            }
            var fileSystems = await Task.WhenAll(fileSystemTasks);

            var result = MergeFileSystems(fileSystems);
            return result;
        }

        private FileSystem MergeFileSystems(IEnumerable<FileSystem> systems)
        {
            var filesys = systems.First();
            foreach (FileSystem fs in systems.Skip(1))
            { 
                filesys.OverwriteWith(fs);
            }
            return filesys;
        }
    }
}
