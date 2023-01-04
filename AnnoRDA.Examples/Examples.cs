using AnnoRDA.Builder;
using System.Diagnostics;

var builder = FileSystemBuilder.Create()
    .FromPath("F:\\SteamLibrary\\steamapps\\common\\Anno 1800\\maindata")
    .WithDefaultSorting()
    .OnlyFilesLike("data*[0-9].rda")
    .AddFile("F:\\SteamLibrary\\steamapps\\common\\Anno 1800\\maindata\\data25.rda");
Console.WriteLine(String.Join("\n", builder.ArchiveFileNames));

Stopwatch stopwatch = Stopwatch.StartNew();
var filesystem = builder.Build();
stopwatch.Stop();
Console.WriteLine($"loaded RDA System in " + stopwatch.Elapsed.TotalMilliseconds + " ms");

using var templates = filesystem.OpenRead("data/config/export/main/asset/templates.xml");
using var filestream = File.Create("templates.xml");
templates.CopyTo(filestream);