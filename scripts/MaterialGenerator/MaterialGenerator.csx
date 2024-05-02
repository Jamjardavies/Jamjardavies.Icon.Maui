using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

const string copyright = @"// <copyright file=""MaterialIcon.cs"" author=""Jamjardavies"">
//      Copyright (c) Jamjardavies. All rights reserved.
// </copyright>";

const string faSummary = @"/// <summary>
///     Material Icons v{0}
///     License: https://github.com/google/material-design-icons/blob/master/LICENSE
///     <see href=""https://developers.google.com/fonts/docs/material_icons"" />
///     <seealso href=""https://github.com/google/material-design-icons"" />
/// </summary>)";
/// 
if (Args.Count != 3)
{
	Console.WriteLine("Run using dotnet script MaterialGenerator.csx <codepoints> <version> <style>");
	return -1;
}

string path = Args[0];
string version = Args[1];
string style = Args[2];

if (!File.Exists(path))
{
	Console.WriteLine("codepoints path invalid.");
	return -1;
}

string data = File.ReadAllText(path);
string[] lines = data.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

AutoStringBuilder sb = new();
sb.WriteLine(copyright);
sb.WriteLine();
sb.WriteLine("namespace Jamjardavies.Icon.Maui.Material;");
sb.WriteLine();
sb.WriteLine("using System.ComponentModel;");
sb.WriteLine("using Jamjardavies.Icon.Maui;");
sb.WriteLine();
sb.WriteLine(string.Format(faSummary, version));
sb.WriteLine("public enum MaterialIcon");
sb.WriteLine("{");

Regex pattern = new Regex("[ \\.,\\(\\)]");

using (sb.Indent())
{
	foreach (var line in lines)
	{
		(string label, string unicode) = line.Split(' ', StringSplitOptions.RemoveEmptyEntries) switch
		{
			[string a, string b] => (a, b),
			_ => default
		};

		sb.WriteLine("/// <summary>");
		sb.WriteLine($"/// \tMaterial icon for {label}");
		sb.WriteLine("/// </summary>");
		// sb.WriteLine($"/// <see href=\"URL/{kvp.Key}\" />");
		sb.WriteLine($"[Description(\"{label}\"), IconId(\"{label}\"), IconStyle(\"Material{style}\")]");
		sb.WriteLine($"{Safe(label)} = 0x{unicode},");
	}
}

sb.WriteLine("}");

File.WriteAllText("MaterialIcon.cs", sb.ToString());

private static readonly Regex PropReg = new Regex(@"\([^)]*\)");

string Safe(string text)
{
	var cultureInfo = Thread.CurrentThread.CurrentCulture;
	var textInfo = cultureInfo.TextInfo;
	var stringBuilder = new StringBuilder(textInfo.ToTitleCase(text.Replace("-", " ")));

	stringBuilder
		.Replace("_", "")
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