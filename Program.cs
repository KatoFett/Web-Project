namespace WebProject
{
    using System.Linq;
    class Program
    {
        public static List<Component> Components { get; private set; } = new List<Component>();
        static void Main(string[] args)
        {
            InitializeComponents();
        }

        static async void InitializeComponents()
        {
            var directoryInfo = new System.IO.DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "components"));
            var tasks = directoryInfo.GetFiles().ToDictionary(f => f, f => f.OpenText().ReadToEndAsync());
            Components = (await Task.WhenAll(tasks.Select(async k => new Component(k.Key.Name, await k.Value)))).ToList();
            Components.ForEach(c => Console.WriteLine(c));
        }
    }

    class Component
    {
        public Component(string name, string content)
        {
            Name = name;
            Content = content;
        }

        public override string ToString()
        {
            return $"{Name} | {Content[0..8]}...";
        }

        public string Name { get; }
        public string Content { get; }
    }
}