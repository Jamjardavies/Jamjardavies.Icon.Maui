namespace Jamjardavies.Icons.Generator.Extensions;

internal static class StringExtensions
{
    public static string ToTitleCase(this string input) =>
        input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => input[0].ToString().ToUpper() + input[1..]
        };
}