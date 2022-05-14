namespace WebProject
{
    using System.Text.RegularExpressions;
    public class PageBuilder
    {
        public PageBuilder(FileInfo page, string[] layout)
        {
            PageFile = page;
            Layout = layout;
        }
        public string[]? Layout { get; set; }
        public FileInfo PageFile { get; }
        public string[]? Page { get; private set; }
        public Dictionary<string, string> Variables { get; set; } = new Dictionary<string, string>();
        public async Task ReadFile()
        {
            Page = await File.ReadAllLinesAsync(PageFile.FullName);
        }
        public async Task<string[]> BuildPage()
        {
            await ReadFile();
            if (Page == null) throw new InvalidOperationException("Cannot build an empty page.");
            try
            {
                BuildPage(false);
                if (Layout != null) BuildPage(true);
                EvaluateVariables();
                return Page;
            }
            catch (Exception ex)
            {
                ThrowError(ex.Message);
                return Array.Empty<string>();
            }
        }
        private void BuildPage(bool isLayout)
        {
            var input = (isLayout ? Layout : Page) ?? throw new InvalidOperationException("Attempted to build an empty page.");
            var retval = new List<string>();
            input.ToList().ForEach(line =>
            {
                //Could use an HTML parser but I'm lazy and don't want to include more libraries
                //Though this is arguably harder...
                var elements = line
                    .Split('<')
                    .Select(l => l.Trim())
                    .Where(l => !string.IsNullOrEmpty(l) && !l.StartsWith('/'))
                    .Select(e =>
                    {
                        e = Regex.Replace(e, "/?>", string.Empty);
                        var spaceIdx = e.IndexOfAny(new char[] { ' ', '/', '>' });
                        if (spaceIdx == -1) return new KeyValuePair<string, IEnumerable<string>>(e, Array.Empty<string>());
                        var name = e[0..(spaceIdx)].ToLower();
                        var args = e[(spaceIdx + 1)..].Split('=').SelectMany(l =>
                        {
                            var idx = l.LastIndexOf("\" ");
                            return idx == -1 ? new string[] { l } : new string[] { l[0..(idx)], l[(idx + 1)..] };
                        });
                        return new KeyValuePair<string, IEnumerable<string>>(name, args);
                        //return e.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    })
                    .ToList();
                var customElements = elements?
                    .Select(e => CustomElement.FindElement(e.Key, e.Value))
                    .Where(c => c != null)
                    .ToList();
                if (customElements?.Any() == true)
                {
                    var firstTag = customElements.First()?.TagName ?? string.Empty;
                    var spaces = line[..(line.Length - line.TrimStart().Length)];
                    var startIdx = line.ToLower().IndexOf($"<{firstTag}");
                    var endIdx = 0;
                    if (startIdx > 0) retval.Add(line[0..(startIdx)]);
                    customElements.ForEach(elm =>
                    {
                        endIdx = line.IndexOf('<', startIdx + 1);
                        if (endIdx > -1) retval.Add(line[startIdx..endIdx]);
                        
                        if (elm is MultiLineElement multiLineElement)
                            retval.AddRange(multiLineElement.Execute(this));
                        if(elm is ActionElement actionElement)
                            actionElement.Execute(this);

                        if (endIdx > -1) startIdx = line.ToLower().IndexOf($">", endIdx);
                    });
                }
                else retval.Add(line);
            });
            Page = retval.ToArray();
        }

        private void EvaluateVariables()
        {
            if (Page == null) throw new InvalidOperationException("Attempted to evaluate variables in a null page.");
            ulong idx = 0;
            Page.ToList().ForEach(line =>
            {
                var variables = Regex.Matches(line, "\\{(.*?)\\}").ToList();
                variables.ForEach(v =>
                {
                    var variableName = v.Value[1..^1];
                    if (!string.IsNullOrEmpty(variableName))
                    {
                        if (!Variables.ContainsKey(variableName)) throw new KeyNotFoundException($"Unable to find the variable '{variableName}'.");
                        line = line.Replace(v.Value, Variables[variableName]);
                    }
                    else line = line.Replace(v.Value, string.Empty);
                    Page[idx] = line;
                });
                idx++;
            });
        }

        public void ThrowError(string message)
        {
            throw new Exception($"Failed to build '{PageFile.FullName}': {message}");
        }
    }
}