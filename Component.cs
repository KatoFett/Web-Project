namespace WebProject
{
    /// <summary>
    ///Defines a module which can be injected into a view by the <component name="" /> tag.
    /// </summary>
    public class Component
    {
        static Component()
        {
            Components = new Dictionary<string, Component>();
        }

        public Component(string[] content)
        {
            Content = content;
        }

        public static Dictionary<string, Component> Components { get; set; }
        public string[] Content { get; }
    }
}