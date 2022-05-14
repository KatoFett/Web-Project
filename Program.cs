namespace WebProject
{
    using System.IO;
    using System.Diagnostics;
    using System.Linq;
    using System.Text.RegularExpressions;
    public class Program
    {
        static string SrcDirectory = $"{Environment.CurrentDirectory}\\src";
        static string SrcComponentsDirectory = $"{SrcDirectory}\\components";
        static string SrcViewsDirectory = $"{SrcDirectory}\\views";
        static string LayoutDirectory = $"{SrcDirectory}\\layout.html";
        static string DistDirectory = $"{Environment.CurrentDirectory}\\docs";

        static string[] InternalDirectories = new string[]
        {
            SrcComponentsDirectory,
            LayoutDirectory
        };

        static void Main(string[] args)
        {
            Compile().Wait();
        }

        static async Task Compile()
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            if (Directory.Exists(DistDirectory)) Directory.Delete(DistDirectory, true);
            Directory.CreateDirectory(DistDirectory);
            Console.WriteLine($"Deleted old views ({stopwatch.ElapsedMilliseconds} ms).");

            stopwatch.Restart();
            await InitializeComponents();
            Console.WriteLine($"Initialized {Component.Components.Count} component{(Component.Components.Count != 1 ? "s" : "")} ({stopwatch.ElapsedMilliseconds} ms).");

            stopwatch.Restart();
            var layoutTemplate = await File.ReadAllLinesAsync(LayoutDirectory);
            Console.WriteLine($"Loaded layout template ({stopwatch.ElapsedMilliseconds} ms).");

            stopwatch.Restart();
            await LoopThroughDirectory(string.Empty, layoutTemplate);
            Console.WriteLine($"Finished copying files ({stopwatch.ElapsedMilliseconds} ms).");
        }

        static async Task LoopThroughDirectory(string location, string[] layout)
        {
            var directoryInfo = new DirectoryInfo($"{SrcDirectory}{location}");
            var fileTasks = directoryInfo
                .GetFiles()
                .Where(file => !InternalDirectories.Any(directory => file.FullName.StartsWith(directory, StringComparison.CurrentCultureIgnoreCase)))
                .Select(file => CreateView(location, file, layout));
            await Task.WhenAll(fileTasks);
            var directoryTasks = directoryInfo
                .GetDirectories()
                .Where(subDirectory => !InternalDirectories.Any(directory => subDirectory.FullName.StartsWith(directory, StringComparison.CurrentCultureIgnoreCase)))
                .Select(directory => LoopThroughDirectory(directory.FullName.Substring(SrcDirectory.Length), layout));
            await Task.WhenAll(directoryTasks);
        }

        static async Task CreateView(string location, FileInfo file, string[] layout)
        {
            if (location.StartsWith("\\views", StringComparison.CurrentCultureIgnoreCase)) location = location.Substring("\\views".Length);
            var destDirectory = $"{DistDirectory}{location}";
            Directory.CreateDirectory(destDirectory); //Automatically skips if directory already exists
            var destFile = $"{destDirectory}\\{file.Name}";
            if (file.Extension.Equals(".html", StringComparison.CurrentCultureIgnoreCase))
            {
                var builder = new PageBuilder(file, layout);
                builder.Variables["AbsolutePath"] = EvaluateAbsolutePath(location);
                builder.Variables["PageTitle"] = "Page";
                await File.WriteAllLinesAsync(destFile, await builder.BuildPage());
                Console.WriteLine($"Built view '{destFile}'.");
            }
            else
            {
                File.Copy(file.FullName, destFile);
                Console.WriteLine($"Copied file '{destFile}'.");
            }
        }

        static string EvaluateAbsolutePath(string location)
        {
            return location == string.Empty
                    ? string.Empty
                    : Enumerable
                        .Repeat("../", location.Split('\\').Length - 1)
                        .Aggregate((a, b) => $"{a}{b}");
        }

        static async Task InitializeComponents()
        {
            var directoryInfo = new DirectoryInfo(SrcComponentsDirectory);
            var tasks = directoryInfo.GetFiles().ToDictionary(f => f, f => File.ReadAllLinesAsync(f.FullName));
            await Task.WhenAll(tasks.Values);
            Component.Components = tasks.ToDictionary(k => k.Key.Name[..^(k.Key.Extension.Length)], k => new Component(k.Value.Result));
        }
    }
}