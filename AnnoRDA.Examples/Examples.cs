using AnnoRDA.Builder;
using System.Diagnostics;
using System.IO.Enumeration;

var builder = FileSystemBuilder.Create()
    .FromPath("F:\\SteamLibrary\\steamapps\\common\\Anno 1800\\maindata")
    .WithDefaultSorting()
    .OnlyArchivesMatchingWildcard(@"data*.rda")
    .AddWhitelisted("*.xml", "*.dds", "*.a7m", "*.a7tinfo");

Console.WriteLine(String.Join("\n", builder.ArchiveFileNames));
Stopwatch stopwatch = Stopwatch.StartNew();
var filesystem = builder.Build();
stopwatch.Stop();
Console.WriteLine($"loaded RDA System in " + stopwatch.Elapsed.TotalMilliseconds + " ms");

using var templates = filesystem.OpenRead("data/config/export/main/asset/templates.xml");
using var filestream = File.Create("templates.xml");
templates.CopyTo(filestream);

var islands = filesystem.Root.MatchFiles("*.a7tinfo");
var xmls = filesystem.Root.MatchFiles("*.xml");
var dds = filesystem.Root.MatchFiles("*.dds");
int i = 0;