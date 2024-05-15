// <copyright file="FontAwesomeGenerator.cs" author="Jamjardavies">
//      Copyright (c) 2024 Jamjardavies.
// </copyright>

using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Jamjardavies.Icons.Generator.Extensions;
using Newtonsoft.Json;

namespace Jamjardavies.Icons.Generator.Generators;

internal class FontAwesomeGenerator : IGenerator
{
    const string copyright = @"// <copyright file=""FontAwesomeIcon.cs"" author=""Jamjardavies"">
//      Copyright (c) Jamjardavies. All rights reserved.
// </copyright>";

    const string faSummary = @"/// <summary>
///     FontAwesomeLabel v{0}
///     License: http://fontawesome.io/license
///     <see href=""http://fontawesome.io"" />
///     <seealso href=""https://github.com/FortAwesome/Font-Awesome"" />
/// </summary>";

    private static readonly Regex PropReg = new(@"\([^)]*\)");

    /// <inheritdoc />
    public int Generate(IEnumerable<string> arguments)
    {
        string[] args = arguments.ToArray();

        if (args.Length != 2)
        {
            Console.Error.WriteLine("Run using FontAwesome <icons.json> <version>");
            return -1;
        }

        string path = args[0];
        string version = args[1];

        if (!File.Exists(path))
        {
            Console.Error.WriteLine("icons.json path invalid.");
            return -1;
        }

        string json = File.ReadAllText(path);
        Dictionary<string, FontAwesomeData>? jsonData = JsonConvert.DeserializeObject<Dictionary<string, FontAwesomeData>>(json);

        if (jsonData is null)
        {
            Console.Error.WriteLine("Unable to read json file.");
            return -1;
        }

        AutoStringBuilder sb = new();
        sb.WriteLine(copyright);
        sb.WriteLine();
        sb.WriteLine("namespace Jamjardavies.Icon.Maui.FontAwesome;");
        sb.WriteLine();
        sb.WriteLine("using System.ComponentModel;");
        sb.WriteLine("using Jamjardavies.Icon.Maui;");
        sb.WriteLine();
        sb.WriteLine(string.Format(faSummary, version));
        sb.WriteLine("public enum FontAwesomeIcon");
        sb.WriteLine("{");

        using (sb.Indent())
        {
            foreach (KeyValuePair<string, FontAwesomeData> kvp in jsonData)
            {
                string label = kvp.Value.Label;
                IEnumerable<string> styles = kvp.Value.Free.OrderDescending().Select(s => $"IconStyle(\"FontAwesome{s.ToTitleCase()}\")");

                sb.WriteLine("/// <summary>");
                sb.WriteLine($"/// \tFont Awesome icon for {label}");
                sb.WriteLine("/// </summary>");
                sb.WriteLine($"/// <see href=\"http://fontawesome.io/icon/{kvp.Key}\" />");
                sb.WriteLine($"[Description(\"{label}\"), IconId(\"{kvp.Key}\"), {string.Join(", ", styles)}]");
                sb.WriteLine($"{this.Safe(kvp.Key)} = 0x{kvp.Value.Unicode},");
            }
        }

        sb.WriteLine("}");

        File.WriteAllText("FontAwesomeIcons.cs", sb.ToString());

        return 0;
    }

    string Safe(string text)
    {
        CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
        TextInfo textInfo = cultureInfo.TextInfo;
        StringBuilder stringBuilder = new(textInfo.ToTitleCase(text.Replace("-", " ")));

        stringBuilder
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

    private record FontAwesomeData(string Label, string Unicode, string[] Free);
}