using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Jamjardavies.Icons.Generator.Generators;

internal partial class MaterialGenerator : IGenerator
{
    const string copyright = @"// <copyright file=""MaterialIcon.cs"" author=""Jamjardavies"">
//      Copyright (c) Jamjardavies. All rights reserved.
// </copyright>";

    const string summary = @"/// <summary>
///     Material Icons v{0}
///     License: https://github.com/google/material-design-icons/blob/master/LICENSE
///     <see href=""https://developers.google.com/fonts/docs/material_icons"" />
///     <seealso href=""https://github.com/google/material-design-icons"" />
/// </summary>";
    
    private static Regex PropReg = new(@"\([^)]*\)", RegexOptions.Compiled);
    private static Regex FileRegex = new(@"MaterialSymbols(.*)\[.*\]", RegexOptions.Compiled);

    /// <inheritdoc />
    public int Generate(IEnumerable<string> arguments)
    {
        string[] args = arguments.ToArray();

        if (args.Length != 2)
        {
            Console.Error.WriteLine("Run using Material <directory> <version>");
            return -1;
        }

        string path = args[0];
        string version = args[1];
        
        if (!Directory.Exists(path))
        {
            Console.Error.WriteLine("codepoints path invalid.");
            return -1;
        }

        Dictionary<string, Icon> icons = [];

        IEnumerable<string> files = Directory.GetFiles(path, "*.codepoints");

        foreach (string file in files)
        {
            string style = Path.GetFileNameWithoutExtension(FileRegex.Replace(file, "$1"));
            string data = File.ReadAllText(file).ReplaceLineEndings();
            string[] lines = data.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                (string name, string glyph) = line.Split(' ', StringSplitOptions.RemoveEmptyEntries) switch
                {
                    [{ } a, { } b] => (a, b),
                    _ => (string.Empty, string.Empty)
                };

                if (!icons.TryGetValue(name, out Icon? icon))
                {
                    icon = new Icon
                    {
                        Name = name,
                        Glyph = glyph
                    };

                    icons[name] = icon;
                }

                icon.Styles.Add(style);
            }
        }

        AutoStringBuilder sb = new();
        sb.WriteLine(copyright);
        sb.WriteLine();
        sb.WriteLine("namespace Jamjardavies.Icon.Maui.Material;");
        sb.WriteLine();
        sb.WriteLine("using System.ComponentModel;");
        sb.WriteLine("using Jamjardavies.Icon.Maui;");
        sb.WriteLine();
        sb.WriteLine(string.Format(summary, version));
        sb.WriteLine("public enum MaterialIcon");
        sb.WriteLine("{");

        using (sb.Indent())
        {
            foreach (Icon icon in icons.Values)
            {
                IEnumerable<string> styles = icon.Styles.Select(s => $"IconStyle(\"Material{s}\")");

                sb.WriteLine("/// <summary>");
                sb.WriteLine($"/// \tMaterial icon for {icon.Name}");
                sb.WriteLine("/// </summary>");
                sb.WriteLine($"[Description(\"{icon.Name}\"), IconId(\"{icon.Name}\"), {string.Join(", ", styles)}]");
                sb.WriteLine($"{Safe(icon.Name)} = 0x{icon.Glyph},");
            }
        }

        sb.WriteLine("}");

        File.WriteAllText("MaterialIcon.cs", sb.ToString());

        return 0;
    }

    private static string Safe(string text)
    {
        CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
        TextInfo textInfo = cultureInfo.TextInfo;
        StringBuilder stringBuilder = new(textInfo.ToTitleCase(text.Replace("-", " ")));

        stringBuilder
            .Replace("_", "")
            .Replace("-", string.Empty).Replace("/", "_")
            .Replace(" ", string.Empty).Replace(".", string.Empty)
            .Replace("'", string.Empty);

        MatchCollection matches = PropReg.Matches(stringBuilder.ToString());
        stringBuilder = new StringBuilder(PropReg.Replace(stringBuilder.ToString(), string.Empty));
        bool hasMatch = false;

        for (int i = 0; i < matches.Count; i++)
        {
            Match match = matches[i];
            
            if (match.Value.IndexOf("Hand", StringComparison.InvariantCultureIgnoreCase) <= -1)
            {
                continue;
            }
            
            hasMatch = true;
            break;
        }

        if (hasMatch)
        {
            stringBuilder.Insert(0, "Hand");
        }

        if (char.IsDigit(stringBuilder[0]))
        {
            stringBuilder.Insert(0, '_');
        }

        return stringBuilder.ToString();
    }

    private class Icon
    {
        public string Name { get; set; }

        public string Glyph { get; set; } = string.Empty;

        public List<string> Styles { get; } = [];
    }
}