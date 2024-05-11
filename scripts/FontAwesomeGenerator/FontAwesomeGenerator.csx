#r "nuget:Newtonsoft.Json,13.0.3"

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;

const string copyright = @"// <copyright file=""FontAwesomeIcon.cs"" author=""Jamjardavies"">
//      Copyright (c) Jamjardavies. All rights reserved.
// </copyright>";

const string faSummary = @"/// <summary>
///     FontAwesomeLabel v{0}
///     License: http://fontawesome.io/license
///     <see href=""http://fontawesome.io"" />
///     <seealso href=""https://github.com/FortAwesome/Font-Awesome"" />
/// </summary>";
/// 
if (Args.Count != 2)
{
	Console.WriteLine("Run using dotnet script FontAwesomeGenerator.csx <icons.json> <version>");
	return -1;
}

string path = Args[0];

// ToDo: Get from Json.
string version = Args[1];

if (!File.Exists(path))
{
	Console.WriteLine("icons.json path invalid.");
	return -1;
}

string json = File.ReadAllText(path);
Dictionary<string, FontAwesomeData> jsonData = JsonConvert.DeserializeObject<Dictionary<string, FontAwesomeData>>(json);

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

Regex pattern = new Regex("[ \\.,\\(\\)]");

using (sb.Indent())
{
	foreach (var kvp in jsonData)
	{
		string label = kvp.Value.Label;
		string style = kvp.Value.Free.Contains("solid") ? "Solid" : ToTitleCase(kvp.Value.Free.First());

		sb.WriteLine("/// <summary>");
		sb.WriteLine($"/// \tFont Awesome icon for {label}");
		sb.WriteLine("/// </summary>");
		sb.WriteLine($"/// <see href=\"http://fontawesome.io/icon/{kvp.Key}\" />");
		sb.WriteLine($"[Description(\"{label}\"), IconId(\"{kvp.Key}\"), IconStyle(\"FontAwesome{style}\")]");
		sb.WriteLine($"{Safe(kvp.Key)} = 0x{kvp.Value.Unicode},");
	}
}

sb.WriteLine("}");

File.WriteAllText("FontAwesomeIcons.cs", sb.ToString());

private static readonly Regex PropReg = new Regex(@"\([^)]*\)");

string Safe(string text)
{
	var cultureInfo = Thread.CurrentThread.CurrentCulture;
	var textInfo = cultureInfo.TextInfo;
	var stringBuilder = new StringBuilder(textInfo.ToTitleCase(text.Replace("-", " ")));

	stringBuilder
		.Replace("-", string.Empty).Replace("/", "_")
		.Replace(" ", string.Empty).Replace(".", string.Empty)
		.Replace("'", string.Empty);

	var matches = PropReg.Matches(stringBuilder.ToString());
	stringBuilder = new StringBuilder(PropReg.Replace(stringBuilder.ToString(), string.Empty));
	var hasMatch = false;

	for (var i = 0; i < matches.Count; i++)
	{
		var match = matches[i];
		if (match.Value.IndexOf("Hand", StringComparison.InvariantCultureIgnoreCase) > -1)
		{
			hasMatch = true;
			break;
		}
	}

	if (hasMatch)
	{
		stringBuilder.Insert(0, "Hand");
	}

	if (char.IsDigit(stringBuilder[0]))
		stringBuilder.Insert(0, '_');

	return stringBuilder.ToString();
}

public record FontAwesomeData(string Label, string Unicode, string[] Free);

public class AutoStringBuilder
{
	private StringBuilder sb = new();
	private int indent = 0;

	public Indenter Indent() => new Indenter(() => this.indent++, () => this.indent--);

	public void WriteLine(string line = "")
	{
		if (this.indent > 0)
		{
			this.sb.Append(new string('\t', this.indent));
		}

		this.sb.AppendLine(line);
	}

	public override string ToString() => this.sb.ToString();
}

public class Indenter : IDisposable
{
	private Action decrease;

	public Indenter(Action increase, Action decrease)
	{
		this.decrease = decrease;

		increase.Invoke();
	}

	public void Dispose()
	{
		this.decrease.Invoke();
	}
}

public static string ToTitleCase(string input) =>
		input switch
		{
			null => throw new ArgumentNullException(nameof(input)),
			"" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
			_ => input[0].ToString().ToUpper() + input.Substring(1)
		};