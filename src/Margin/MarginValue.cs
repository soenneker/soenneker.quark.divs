namespace Soenneker.Quark.Divs.Margin;

/// <summary>
/// Represents a margin value that can be either a MarginBuilder or a string.
/// Provides implicit conversions for seamless usage.
/// </summary>
public readonly struct MarginValue : IEquatable<MarginValue>
{
    private readonly string _value;

    private MarginValue(string value)
    {
        _value = value ?? string.Empty;
    }

    /// <summary>
    /// Implicit conversion from MarginBuilder to MarginValue.
    /// </summary>
    public static implicit operator MarginValue(MarginBuilder builder)
    {
        return new MarginValue(builder.ToClass());
    }

    /// <summary>
    /// Implicit conversion from string to MarginValue.
    /// </summary>
    public static implicit operator MarginValue(string value)
    {
        return new MarginValue(value);
    }

    /// <summary>
    /// Implicit conversion from MarginValue to string.
    /// </summary>
    public static implicit operator string(MarginValue marginValue)
    {
        return marginValue._value;
    }

    /// <summary>
    /// Returns the string representation of the margin value.
    /// </summary>
    public override string ToString()
    {
        return _value;
    }

    /// <summary>
    /// Determines if this margin value is empty.
    /// </summary>
    public bool IsEmpty => string.IsNullOrEmpty(_value);

    /// <summary>
    /// Determines if this margin value contains CSS properties (contains ':').
    /// </summary>
    public bool IsCssStyle => _value.Contains(':');

    /// <summary>
    /// Determines if this margin value is a CSS class.
    /// </summary>
    public bool IsCssClass => !IsCssStyle && !IsEmpty;

    public bool Equals(MarginValue other)
    {
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return obj is MarginValue other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public static bool operator ==(MarginValue left, MarginValue right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(MarginValue left, MarginValue right)
    {
        return !left.Equals(right);
    }
}
