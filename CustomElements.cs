namespace WebProject
{
    public abstract class CustomElement
    {
        static CustomElement()
        {
            AvailableElements = new Dictionary<string, Type>
            {
                {ComponentElement.Tag, typeof(ComponentElement)},
                {ContentElement.Tag, typeof(ContentElement)},
                {VariableElement.Tag, typeof(VariableElement)},
                {NoLayoutElement.Tag, typeof(NoLayoutElement)},
            };
        }

        public CustomElement(IEnumerable<string> args)
        {
            Args = new Dictionary<string, string>();
            var formattedArgs = args.Select(s => s.Replace("\"", string.Empty).Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();
            for (int i = 0; i < formattedArgs.Count; i += 2)
            {
                Args[formattedArgs[i]] = formattedArgs[i + 1];
            }
        }

        public abstract string TagName { get; }

        public Dictionary<string, string> Args { get; set; }

        /// <summary>
        /// A dictionary of all custom elements defined.
        /// </summary>
        public static Dictionary<string, Type> AvailableElements { get; }

        /// <summary>
        /// Attempts to find a tag with the following name.
        /// </summary>
        /// <remarks>
        /// The search is case-insensitive.
        /// </remarks>
        /// <param name="tag">The tag to search for.</param>
        /// <returns>A <see cref="CustomElement"/> matching the tag, or <see langword="null"/> if none is found.</returns>
        public static CustomElement? FindElement(string tag, IEnumerable<string> args)
        {
            return AvailableElements.ContainsKey(tag)
                ? (CustomElement)(Activator.CreateInstance(AvailableElements[tag], args) ?? throw new Exception($"Failed to create an instance of the component '{tag}'."))
                : null;
        }
    }

    /// <summary>
    /// Defines a custom element which returns a list of lines to append.
    /// </summary>
    public abstract class MultiLineElement : CustomElement
    {
        public MultiLineElement(IEnumerable<string> args) : base(args) { }

        public abstract string[] Execute(PageBuilder builder);
    }

    /// <summary>
    /// Defines a custom element which does not return anything.
    /// </summary>
    public abstract class ActionElement : CustomElement
    {
        public ActionElement(IEnumerable<string> args) : base(args) { }
        public abstract void Execute(PageBuilder builder);
    }

    /// <summary>
    /// Inserts a component from the components directory
    /// </summary>
    public sealed class ComponentElement : MultiLineElement
    {
        public ComponentElement(IEnumerable<string> args) : base(args) { }
        public const string Tag = "component";
        public override string TagName => Tag;
        public override string[] Execute(PageBuilder builder)
        {
            if (!Args.ContainsKey("name")) builder.ThrowError($"Component has no name attribute.");
            if (!Component.Components.ContainsKey(Args["name"])) builder.ThrowError($"Unable to find component '{Args["name"]}'.");
            return Component.Components[Args["name"]].Content;
        }
    }

    /// <summary>
    /// Inserts the page content. Used in a layout template.
    /// </summary>
    public sealed class ContentElement : MultiLineElement
    {
        public ContentElement(IEnumerable<string> args) : base(args) { }
        public override string TagName => Tag;
        public const string Tag = "content";
        public override string[] Execute(PageBuilder builder)
        {
            return builder.Page ?? Array.Empty<string>();
        }
    }

    /// <summary>
    /// Sets a variable in the page builder.
    /// </summary>
    public sealed class VariableElement : ActionElement
    {
        public VariableElement(IEnumerable<string> args) : base(args) { }
        public override string TagName => Tag;
        public const string Tag = "variable";
        public override void Execute(PageBuilder builder)
        {
            if (!Args.ContainsKey("name")) builder.ThrowError("Variable has no name attribute.");
            if (!Args.ContainsKey("value")) builder.ThrowError("Variable has no value attribute.");
            builder.Variables[Args["name"]] = Args["value"];
        }
    }

    /// <summary>
    /// Removes the page layout.
    /// </summary>
    public sealed class NoLayoutElement : ActionElement
    {
        public NoLayoutElement(IEnumerable<string> args) : base(args) { }
        public override string TagName => Tag;
        public const string Tag = "nolayout";
        public override void Execute(PageBuilder builder)
        {
            builder.Layout = null;
        }
    }
}