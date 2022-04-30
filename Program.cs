namespace WebProject
{
    using System.IO;
    using System.Diagnostics;
    using System.Linq;
    using System.Text.RegularExpressions;
    class Program
    {
        static List<Component> Components = new List<Component>();
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
            if(Directory.Exists(DistDirectory)) Directory.Delete(DistDirectory, true);
            Directory.CreateDirectory(DistDirectory);
            Console.WriteLine($"Deleted old views ({stopwatch.ElapsedMilliseconds} ms).");

            stopwatch.Restart();
            await InitializeComponents();
            Console.WriteLine($"Initialized {Components.Count} component{(Components.Count != 1 ? "s" : "")} ({stopwatch.ElapsedMilliseconds} ms).");

            stopwatch.Restart();
            var layoutTemplate = InsertComponents(await File.ReadAllLinesAsync(LayoutDirectory), Array.Empty<string>(), false);
            Console.WriteLine($"Created layout template ({stopwatch.ElapsedMilliseconds} ms).");

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
            if(location.StartsWith("\\views", StringComparison.CurrentCultureIgnoreCase)) location = location.Substring("\\views".Length);
            var destDirectory = $"{DistDirectory}{location}";
            Directory.CreateDirectory(destDirectory); //Automatically skips if directory already exists
            var destFile = $"{destDirectory}\\{file.Name}";
            if(file.Extension.Equals(".html", StringComparison.CurrentCultureIgnoreCase))
            {
                var content = await File.ReadAllLinesAsync(file.FullName);
                var newContent = EvaluateAbsolutePath(InsertComponents(layout, content), location);
                await File.WriteAllLinesAsync(destFile, newContent);
                Console.WriteLine($"Copied view '{destFile}'.");
            }
            else
            {
                File.Copy(file.FullName, destFile);
                Console.WriteLine($"Copied generic file '{destFile}'.");
            }
        }

        static string[] EvaluateAbsolutePath(string[] content, string location)
        {
            return content
                .Select(line => line.Replace(
                    "{AbsolutePath}",
                    location == string.Empty
                        ? string.Empty
                        : Enumerable
                            .Repeat("../", location.Split('\\').Length - 1)
                            .Aggregate((a, b) => $"{a}{b}"))
                ).ToArray();
        }

        static async Task InitializeComponents()
        {
            var directoryInfo = new DirectoryInfo(SrcComponentsDirectory);
            var tasks = directoryInfo.GetFiles().ToDictionary(f => f, f => File.ReadAllLinesAsync(f.FullName));
            Components = (await Task.WhenAll(tasks.Select(async k => new Component(k.Key.Name[..^(k.Key.Extension.Length)], await k.Value)))).ToList();
        }

        static string[] InsertComponents(string[] view, string[] content, bool injectContent = true)
        {
            var retval = new List<string>();
            //Could use an HTML parser but I'm lazy and don't want to include more libraries
            //Though this is arguably harder...
            view.ToList().ForEach(line => 
            {
                var matches = Regex.Matches(line.Trim(), "<component\\s*?name=\"\\S+?\"\\s*?\\/>");
                if(matches.Any())
                {
                    if(matches[0].Index > 0) retval.Add(line[0..(matches[0].Index)]);
                    var spaces = line[..(line.Length - line.TrimStart().Length)];
                    matches.ToList().ForEach(m =>
                    {
                        var componentName = Regex.Match(m.Value, "(?<=name=\")\\S*?(?=\")").Value;
                        var isContentComponent = "content".Equals(componentName, StringComparison.CurrentCultureIgnoreCase);
                        var component = isContentComponent
                            ? new Component("content", content)
                            : Components.FirstOrDefault(c => c.Name.Equals(componentName, StringComparison.CurrentCultureIgnoreCase));
                        if(component == null)
                            throw new ArgumentException($"Unable to find a component '{componentName}' (case-insensitive).");
                        if(isContentComponent && !injectContent) retval.Add(line);
                        else retval.AddRange(InsertComponents(component.Content.Select(c => $"{spaces}{c}").ToArray(), content));
                    });
                }
                else retval.Add(line);
            });
            return retval.ToArray();
        }
    }

    //Defines a module which can be injected into a view by the <component name="" /> tag.
    class Component
    {
        public Component(string name, string[] content)
        {
            Name = name;
            Content = content.ToList();
        }

        public string Name { get; }
        public List<string> Content { get; }
    }
}