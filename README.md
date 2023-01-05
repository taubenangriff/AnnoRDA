# AnnoRDA

A .NET standard library for reading RDA 2.2 files as used in Anno 2205 and Anno 1800.

## Setup

* Clone this repository
* Install [.NET 6 Core SDK](https://www.microsoft.com/net/core)

* **Install library dependencies:**

        $ cd /path/to/project
        $ dotnet restore

* **Build library & unit tests:**

        $ cd /path/to/project
        $ dotnet build

* **Run unit tests:**

        $ cd /path/to/project
        $ cd AnnoRDA.Tests
        $ dotnet test

    This will also build the library and tests.

# New Features

- Migrated to .NET 6

- Easily control loading of RDA archives with FileSystemBuilder. 
- Filter files from being loaded altogether to cut load times and memory usage.
- FileSystem.OpenRead(String path)
- Folder.MatchFiles(String pattern)
- Performance improvements

For code snippets of the new features, see [Examples.cs](https://github.com/taubenangriff/AnnoRDA/blob/master/AnnoRDA.Examples/Examples.cs)