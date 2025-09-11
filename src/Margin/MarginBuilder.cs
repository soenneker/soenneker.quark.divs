using Soenneker.Quark.Enums.Breakpoints;
using Soenneker.Quark.Enums.MarginSides;

namespace Soenneker.Quark.Divs.Margin;

/// <summary>
/// Simplified margin builder with fluent API for chaining margin rules.
/// </summary>
public sealed class MarginBuilder
{
    private readonly List<MarginRule> _rules = [];

    internal MarginBuilder(int size, Breakpoint? breakpoint = null)
    {
        if (size >= 0)
            _rules.Add(new MarginRule(size, MarginSide.All, breakpoint));
        else
            _rules.Add(new MarginRule(-1, MarginSide.All, breakpoint)); // Auto
    }

    internal MarginBuilder(List<MarginRule> rules)
    {
        _rules.AddRange(rules);
    }

    /// <summary>
    /// Apply margin to the top.
    /// </summary>
    public MarginBuilder FromTop => AddRule(MarginSide.Top);

    /// <summary>
    /// Apply margin to the right.
    /// </summary>
    public MarginBuilder FromRight => AddRule(MarginSide.Right);

    /// <summary>
    /// Apply margin to the bottom.
    /// </summary>
    public MarginBuilder FromBottom => AddRule(MarginSide.Bottom);

    /// <summary>
    /// Apply margin to the left.
    /// </summary>
    public MarginBuilder FromLeft => AddRule(MarginSide.Left);

    /// <summary>
    /// Apply margin to horizontal sides (left and right).
    /// </summary>
    public MarginBuilder OnX => AddRule(MarginSide.Horizontal);

    /// <summary>
    /// Apply margin to vertical sides (top and bottom).
    /// </summary>
    public MarginBuilder OnY => AddRule(MarginSide.Vertical);

    /// <summary>
    /// Apply margin to all sides.
    /// </summary>
    public MarginBuilder OnAll => AddRule(MarginSide.All);

    /// <summary>
    /// Apply margin to start (left in LTR, right in RTL).
    /// </summary>
    public MarginBuilder FromStart => AddRule(MarginSide.InlineStart);

    /// <summary>
    /// Apply margin to end (right in LTR, left in RTL).
    /// </summary>
    public MarginBuilder FromEnd => AddRule(MarginSide.InlineEnd);

    /// <summary>
    /// Chain with a new size for the next margin rule.
    /// </summary>
    public MarginBuilder S0 => ChainWithSize(0);

    /// <summary>
    /// Chain with a new size for the next margin rule.
    /// </summary>
    public MarginBuilder S1 => ChainWithSize(1);

    /// <summary>
    /// Chain with a new size for the next margin rule.
    /// </summary>
    public MarginBuilder S2 => ChainWithSize(2);

    /// <summary>
    /// Chain with a new size for the next margin rule.
    /// </summary>
    public MarginBuilder S3 => ChainWithSize(3);

    /// <summary>
    /// Chain with a new size for the next margin rule.
    /// </summary>
    public MarginBuilder S4 => ChainWithSize(4);

    /// <summary>
    /// Chain with a new size for the next margin rule.
    /// </summary>
    public MarginBuilder S5 => ChainWithSize(5);

    /// <summary>
    /// Chain with auto size for the next margin rule.
    /// </summary>
    public MarginBuilder Auto => ChainWithSize(-1);

    /// <summary>
    /// Apply margin on phone devices (portrait phones, less than 576px).
    /// </summary>
    public MarginBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);

    /// <summary>
    /// Apply margin on mobile devices (landscape phones, 576px and up).
    /// </summary>
    public MarginBuilder OnMobile => ChainWithBreakpoint(Breakpoint.Mobile);

    /// <summary>
    /// Apply margin on tablet devices (tablets, 768px and up).
    /// </summary>
    public MarginBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);

    /// <summary>
    /// Apply margin on laptop devices (laptops, 992px and up).
    /// </summary>
    public MarginBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);

    /// <summary>
    /// Apply margin on desktop devices (desktops, 1200px and up).
    /// </summary>
    public MarginBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);

    /// <summary>
    /// Apply margin on wide screen devices (larger desktops, 1400px and up).
    /// </summary>
    public MarginBuilder OnWideScreen => ChainWithBreakpoint(Breakpoint.ExtraExtraLarge);

    private MarginBuilder AddRule(MarginSide side)
    {
        MarginRule? lastRule = _rules.LastOrDefault();
        int size = lastRule?.Size ?? 0;
        Breakpoint? breakpoint = lastRule?.Breakpoint;

        var newRules = new List<MarginRule>(_rules);

        // If the last rule was "All" and we're specifying a side, replace it
        if (lastRule?.Side == MarginSide.All)
        {
            newRules[newRules.Count - 1] = new MarginRule(size, side, breakpoint);
        }
        else
        {
            newRules.Add(new MarginRule(size, side, breakpoint));
        }

        return new MarginBuilder(newRules);
    }

    private MarginBuilder ChainWithSize(int size)
    {
        var newRules = new List<MarginRule>(_rules) { new(size, MarginSide.All, null) };
        return new MarginBuilder(newRules);
    }

    private MarginBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        MarginRule? lastRule = _rules.LastOrDefault();
        if (lastRule == null)
            return new MarginBuilder(0, breakpoint);

        var newRules = new List<MarginRule>(_rules);
        // Update the last rule with the new breakpoint
        newRules[newRules.Count - 1] = new MarginRule(lastRule.Size, lastRule.Side, breakpoint);
        return new MarginBuilder(newRules);
    }

    /// <summary>
    /// Gets the CSS class string for the current margin configuration.
    /// </summary>
    public string ToClass()
    {
        if (_rules.Count == 0)
            return string.Empty;

        var classes = new List<string>(_rules.Count);

        foreach (MarginRule rule in _rules)
        {
            string sizeClass = GetSizeClass(rule.Size);
            string sideClass = GetSideClass(rule.Side);
            string breakpointClass = GetBreakpointClass(rule.Breakpoint);

            if (!string.IsNullOrEmpty(sizeClass))
            {
                if (!string.IsNullOrEmpty(sideClass))
                {
                    // Build the class name correctly: "m" + side + "-" + size
                    string baseClass = sizeClass.Substring(0, 1); // "m"
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
    /// Gets the CSS style string for the current margin configuration.
    /// </summary>
    public string ToStyle()
    {
        if (_rules.Count == 0)
            return string.Empty;

        var styles = new List<string>(_rules.Count);

        foreach (MarginRule rule in _rules)
        {
            string? sizeValue = GetSizeValue(rule.Size);
            if (sizeValue == null) continue;

            switch (rule.Side.Value)
            {
                case "all":
                    styles.Add($"margin: {sizeValue}");
                    break;
                case "top":
                    styles.Add($"margin-top: {sizeValue}");
                    break;
                case "right":
                    styles.Add($"margin-right: {sizeValue}");
                    break;
                case "bottom":
                    styles.Add($"margin-bottom: {sizeValue}");
                    break;
                case "left":
                    styles.Add($"margin-left: {sizeValue}");
                    break;
                case "horizontal":
                case "left-right":
                    styles.Add($"margin-left: {sizeValue}; margin-right: {sizeValue}");
                    break;
                case "vertical":
                case "top-bottom":
                    styles.Add($"margin-top: {sizeValue}; margin-bottom: {sizeValue}");
                    break;
                case "inline-start":
                    styles.Add($"margin-inline-start: {sizeValue}");
                    break;
                case "inline-end":
                    styles.Add($"margin-inline-end: {sizeValue}");
                    break;
            }
        }

        return string.Join("; ", styles);
    }

    private static string GetSizeClass(int size)
    {
        return size switch
        {
            0 => "m-0",
            1 => "m-1",
            2 => "m-2",
            3 => "m-3",
            4 => "m-4",
            5 => "m-5",
            -1 => "m-auto",
            _ => string.Empty
        };
    }

    private static string GetSideClass(MarginSide side)
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