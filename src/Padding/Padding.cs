using System.Text;
using Soenneker.Extensions.String;
using Soenneker.Quark.Enums.MarginSides;

namespace Soenneker.Quark.Divs.Padding;

/// <summary>
/// Simplified padding utility with fluent API and Bootstrap-first approach.
/// </summary>
public static class Padding
{
    /// <summary>
    /// No padding (0).
    /// </summary>
    public static PaddingBuilder S0 => new(0);

    /// <summary>
    /// Size 1 padding (0.25rem).
    /// </summary>
    public static PaddingBuilder S1 => new(1);

    /// <summary>
    /// Size 2 padding (0.5rem).
    /// </summary>
    public static PaddingBuilder S2 => new(2);

    /// <summary>
    /// Size 3 padding (1rem).
    /// </summary>
    public static PaddingBuilder S3 => new(3);

    /// <summary>
    /// Size 4 padding (1.5rem).
    /// </summary>
    public static PaddingBuilder S4 => new(4);

    /// <summary>
    /// Size 5 padding (3rem).
    /// </summary>
    public static PaddingBuilder S5 => new(5);
}
