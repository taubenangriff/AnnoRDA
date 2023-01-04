using AnnoRDA.Loader;
using System.Diagnostics;

FileSystemLoader loader = new FileSystemLoader("F:\\SteamLibrary\\steamapps\\common\\Anno 1800\\maindata");
Console.WriteLine(String.Join("\n", loader.Archives));

Stopwatch stopwatch = Stopwatch.StartNew();
var filesystem = loader.Load();
stopwatch.Stop();
Console.WriteLine($"loaded RDA System in " + stopwatch.Elapsed.TotalMilliseconds + " ms");

using var templates = filesystem.OpenRead("data/config/export/main/asset/templates.xml");
using var filestream = File.Create("templates.xml");
templates.CopyTo(filestream);