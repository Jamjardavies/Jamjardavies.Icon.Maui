namespace Jamjardavies.Icons.Generator;

internal interface IGenerator
{
    int Generate(IEnumerable<string> args);
}