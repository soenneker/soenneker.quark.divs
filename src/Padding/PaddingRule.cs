using Soenneker.Quark.Enums.Breakpoints;
using Soenneker.Quark.Enums.MarginSides;

namespace Soenneker.Quark.Divs.Padding;

/// <summary>
/// Represents a single padding rule with optional breakpoint.
/// </summary>
internal record PaddingRule(int Size, ElementSide Side, Breakpoint Breakpoint = null!);
