using AnnoRDA.Builder;
using System.Diagnostics;
using System.IO.Enumeration;

var builder = FileSystemBuilder.Create()
    .FromPath("F:\\SteamLibrary\\steamapps\\common\\Anno 1800\\maindata")
    .WithDefaultSorting()
    .OnlyArchivesMatchingWildcard(@"data*.rda")
    .AddWhitelisted("*.a7tinfo", "*.png", "*.a7minfo", "*.a7t", "*.a7te", "assets.xml", "templates.xml");

Console.WriteLine(String.Join("\n", builder.ArchiveFileNames));
Stopwatch stopwatch = Stopwatch.StartNew();
var filesystem = builder.Build();
stopwatch.Stop();
Console.WriteLine($"loaded RDA System in " + stopwatch.Elapsed.TotalMilliseconds + " ms");

#region NewFileApi

filesystem.File.OpenRead("data/config/export/main/asset/templates.xml");
var specialistImgs = filesystem.Directory.EnumerateFiles("data/ui/2kimages/3dicons/specialist");
var properties = filesystem.File.ReadAllText("data/config/export/main/asset/properties.xml");

#endregion