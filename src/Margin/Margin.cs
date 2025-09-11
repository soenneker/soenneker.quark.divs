using System.Text;
using Soenneker.Extensions.String;
using Soenneker.Quark.Enums.MarginSides;

namespace Soenneker.Quark.Divs.Margin;

/// <summary>
/// Simplified margin utility with fluent API and Bootstrap-first approach.
/// </summary>
public static class Margin
{
    /// <summary>
    /// No margin (0).
    /// </summary>
    public static MarginBuilder S0 => new(0);

    /// <summary>
    /// Size 1 margin (0.25rem).
    /// </summary>
    public static MarginBuilder S1 => new(1);

    /// <summary>
    /// Size 2 margin (0.5rem).
    /// </summary>
    public static MarginBuilder S2 => new(2);

    /// <summary>
    /// Size 3 margin (1rem).
    /// </summary>
    public static MarginBuilder S3 => new(3);

    /// <summary>
    /// Size 4 margin (1.5rem).
    /// </summary>
    public static MarginBuilder S4 => new(4);

    /// <summary>
    /// Size 5 margin (3rem).
    /// </summary>
    public static MarginBuilder S5 => new(5);

    /// <summary>
    /// Auto margin (auto).
    /// </summary>
    public static MarginBuilder Auto => new(-1);
}