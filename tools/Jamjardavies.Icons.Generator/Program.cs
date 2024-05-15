// <copyright file="Program.cs" author="Jamjardavies">
//      Copyright (c) 2024 Jamjardavies.
// </copyright>

using Jamjardavies.Icons.Generator;
using Jamjardavies.Icons.Generator.Generators;

Dictionary<string, IGenerator> generatorMap = new()
{
    { "FontAwesome", new FontAwesomeGenerator() },
    { "Material", new MaterialGenerator() }
};

string ListGenerators()
{
    return $"Valid Generators:\n{string.Join("\n\t", generatorMap.Keys)}";
}

if (args.Length == 0)
{
    Console.Error.WriteLine($"Missing arguments, supply a valid generator.\n{ListGenerators()}");
    return -1;
}

string generatorKey = args.Take(1).First();

if (!generatorMap.TryGetValue(generatorKey, out IGenerator? generator))
{
    Console.Error.WriteLine($"Must supply generator.\n{ListGenerators()}");
    return -1;
}

return generator.Generate(args.Skip(1));