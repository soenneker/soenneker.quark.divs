using Soenneker.Quark.Enums.Breakpoints;
using Soenneker.Quark.Enums.MarginSides;

namespace Soenneker.Quark.Divs.Padding;

/// <summary>
/// Simplified padding builder with fluent API for chaining padding rules.
/// </summary>
public sealed class PaddingBuilder
{
    private readonly List<PaddingRule> _rules = [];

    internal PaddingBuilder(int size, Breakpoint? breakpoint = null)
    {
        if (size >= 0)
            _rules.Add(new PaddingRule(size, ElementSide.All, breakpoint));
        else
            _rules.Add(new PaddingRule(-1, ElementSide.All, breakpoint)); // Auto
    }

    internal PaddingBuilder(List<PaddingRule> rules)
    {
        _rules.AddRange(rules);
    }

    /// <summary>
    /// Apply padding to the top.
    /// </summary>
    public PaddingBuilder FromTop => AddRule(ElementSide.Top);

    /// <summary>
    /// Apply padding to the right.
    /// </summary>
    public PaddingBuilder FromRight => AddRule(ElementSide.Right);

    /// <summary>
    /// Apply padding to the bottom.
    /// </summary>
    public PaddingBuilder FromBottom => AddRule(ElementSide.Bottom);

    /// <summary>
    /// Apply padding to the left.
    /// </summary>
    public PaddingBuilder FromLeft => AddRule(ElementSide.Left);

    /// <summary>
    /// Apply padding to horizontal sides (left and right).
    /// </summary>
    public PaddingBuilder OnX => AddRule(ElementSide.Horizontal);

    /// <summary>
    /// Apply padding to vertical sides (top and bottom).
    /// </summary>
    public PaddingBuilder OnY => AddRule(ElementSide.Vertical);

    /// <summary>
    /// Apply padding to all sides.
    /// </summary>
    public PaddingBuilder OnAll => AddRule(ElementSide.All);

    /// <summary>
    /// Apply padding to start (left in LTR, right in RTL).
    /// </summary>
    public PaddingBuilder FromStart => AddRule(ElementSide.InlineStart);

    /// <summary>
    /// Apply padding to end (right in LTR, left in RTL).
    /// </summary>
    public PaddingBuilder FromEnd => AddRule(ElementSide.InlineEnd);

    /// <summary>
    /// Chain with a new size for the next padding rule.
    /// </summary>
    public PaddingBuilder S0 => ChainWithSize(0);

    /// <summary>
    /// Chain with a new size for the next padding rule.
    /// </summary>
    public PaddingBuilder S1 => ChainWithSize(1);

    /// <summary>
    /// Chain with a new size for the next padding rule.
    /// </summary>
    public PaddingBuilder S2 => ChainWithSize(2);

    /// <summary>
    /// Chain with a new size for the next padding rule.
    /// </summary>
    public PaddingBuilder S3 => ChainWithSize(3);

    /// <summary>
    /// Chain with a new size for the next padding rule.
    /// </summary>
    public PaddingBuilder S4 => ChainWithSize(4);

    /// <summary>
    /// Chain with a new size for the next padding rule.
    /// </summary>
    public PaddingBuilder S5 => ChainWithSize(5);

    /// <summary>
    /// Apply padding on phone devices (portrait phones, less than 576px).
    /// </summary>
    public PaddingBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);

    /// <summary>
    /// Apply padding on mobile devices (landscape phones, 576px and up).
    /// </summary>
    public PaddingBuilder OnMobile => ChainWithBreakpoint(Breakpoint.Mobile);

    /// <summary>
    /// Apply padding on tablet devices (tablets, 768px and up).
    /// </summary>
    public PaddingBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);

    /// <summary>
    /// Apply padding on laptop devices (laptops, 992px and up).
    /// </summary>
    public PaddingBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);

    /// <summary>
    /// Apply padding on desktop devices (desktops, 1200px and up).
    /// </summary>
    public PaddingBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);

    /// <summary>
    /// Apply padding on wide screen devices (larger desktops, 1400px and up).
    /// </summary>
    public PaddingBuilder OnWideScreen => ChainWithBreakpoint(Breakpoint.ExtraExtraLarge);

    private PaddingBuilder AddRule(ElementSide side)
    {
        PaddingRule? lastRule = _rules.LastOrDefault();
        int size = lastRule?.Size ?? 0;
        Breakpoint? breakpoint = lastRule?.Breakpoint;

        var newRules = new List<PaddingRule>(_rules);

        // If the last rule was "All" and we're specifying a side, replace it
        if (lastRule?.Side == ElementSide.All)
        {
            newRules[newRules.Count - 1] = new PaddingRule(size, side, breakpoint);
        }
        else
        {
            newRules.Add(new PaddingRule(size, side, breakpoint));
        }

        return new PaddingBuilder(newRules);
    }

    private PaddingBuilder ChainWithSize(int size)
    {
        var newRules = new List<PaddingRule>(_rules) { new(size, ElementSide.All, null) };
        return new PaddingBuilder(newRules);
    }

    private PaddingBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        PaddingRule? lastRule = _rules.LastOrDefault();
        if (lastRule == null)
            return new PaddingBuilder(0, breakpoint);

        var newRules = new List<PaddingRule>(_rules);
        // Update the last rule with the new breakpoint
        newRules[newRules.Count - 1] = new PaddingRule(lastRule.Size, lastRule.Side, breakpoint);
        return new PaddingBuilder(newRules);
    }

    /// <summary>
    /// Gets the CSS class string for the current padding configuration.
    /// </summary>
    public string ToClass()
    {
        if (_rules.Count == 0)
            return string.Empty;

        var classes = new List<string>(_rules.Count);

        foreach (PaddingRule rule in _rules)
        {
            string sizeClass = GetSizeClass(rule.Size);
            string sideClass = GetSideClass(rule.Side);
            string breakpointClass = GetBreakpointClass(rule.Breakpoint);

            if (!string.IsNullOrEmpty(sizeClass))
            {
                if (!string.IsNullOrEmpty(sideClass))
                {
                    // Build the class name correctly: "p" + side + "-" + size
                    string baseClass = sizeClass.Substring(0, 1); // "p"
                    string size = sizeClass.Substring(2); // "3"
                    var className = $"{baseClass}{sideClass}-{size}";

                    if (!string.IsNullOrEmpty(breakpointClass))
                        className = $"{breakpointClass}-{className}";

                    classes.Add(className);
                }
                else
                {
                    string className = sizeClass;
                    if (!string.IsNullOrEmpty(breakpointClass))
                        className = $"{breakpointClass}-{className}";

                    classes.Add(className);
                }
            }
        }

        return string.Join(" ", classes);
    }

    /// <summary>
    /// Gets the CSS style string for the current padding configuration.
    /// </summary>
    public string ToStyle()
    {
        if (_rules.Count == 0)
            return string.Empty;

        var styles = new List<string>(_rules.Count);

        foreach (PaddingRule rule in _rules)
        {
            string? sizeValue = GetSizeValue(rule.Size);
            if (sizeValue == null) continue;

            switch (rule.Side.Value)
            {
                case "all":
                    styles.Add($"padding: {sizeValue}");
                    break;
                case "top":
                    styles.Add($"padding-top: {sizeValue}");
                    break;
                case "right":
                    styles.Add($"padding-right: {sizeValue}");
                    break;
                case "bottom":
                    styles.Add($"padding-bottom: {sizeValue}");
                    break;
                case "left":
                    styles.Add($"padding-left: {sizeValue}");
                    break;
                case "horizontal":
                case "left-right":
                    styles.Add($"padding-left: {sizeValue}; padding-right: {sizeValue}");
                    break;
                case "vertical":
                case "top-bottom":
                    styles.Add($"padding-top: {sizeValue}; padding-bottom: {sizeValue}");
                    break;
                case "inline-start":
                    styles.Add($"padding-inline-start: {sizeValue}");
                    break;
                case "inline-end":
                    styles.Add($"padding-inline-end: {sizeValue}");
                    break;
            }
        }

        return string.Join("; ", styles);
    }

    private static string GetSizeClass(int size)
    {
        return size switch
        {
            0 => "p-0",
            1 => "p-1",
            2 => "p-2",
            3 => "p-3",
            4 => "p-4",
            5 => "p-5",
            -1 => "p-auto",
            _ => string.Empty
        };
    }

    private static string GetSideClass(ElementSide side)
    {
        return side.Value switch
        {
            "all" => string.Empty,
            "top" => "t",
            "right" => "e",
            "bottom" => "b",
            "left" => "s",
            "horizontal" or "left-right" => "x",
            "vertical" or "top-bottom" => "y",
            "inline-start" => "s",
            "inline-end" => "e",
            _ => string.Empty
        };
    }

    private static string GetBreakpointClass(Breakpoint? breakpoint)
    {
        if (breakpoint == null) return string.Empty;

        return breakpoint.Value switch
        {
            "phone" or "xs" => string.Empty, // xs is default, no prefix needed
            "mobile" or "sm" => "sm",
            "tablet" or "md" => "md",
            "laptop" or "lg" => "lg",
            "desktop" or "xl" => "xl",
            "xxl" => "xxl",
            _ => string.Empty
        };
    }

    private static string? GetSizeValue(int size)
    {
        return size switch
        {
            0 => "0",
            1 => "0.25rem",
            2 => "0.5rem",
            3 => "1rem",
            4 => "1.5rem",
            5 => "3rem",
            -1 => "auto",
            _ => null
        };
    }
}
