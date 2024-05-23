// <copyright file="Icon.cs" author="Jamjardavies">
//      Copyright (c) 2024 Jamjardavies.
// </copyright>

namespace Jamjardavies.Icon.Maui;

public sealed record Icon(Enum? Glyph, string Style);

internal sealed record IconBinding(Type TargetType, object TargetObject);