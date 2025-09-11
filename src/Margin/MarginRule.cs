using Soenneker.Quark.Enums.Breakpoints;
using Soenneker.Quark.Enums.MarginSides;

namespace Soenneker.Quark.Divs.Margin;

/// <summary>
/// Represents a single margin rule with optional breakpoint.
/// </summary>
internal record MarginRule(int Size, MarginSide Side, Breakpoint Breakpoint = null!);
