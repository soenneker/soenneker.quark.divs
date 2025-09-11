namespace Soenneker.Quark.Divs.Padding;

/// <summary>
/// Represents a padding value that can be either a PaddingBuilder or a string.
/// Provides implicit conversions for seamless usage.
/// </summary>
public readonly struct PaddingValue : IEquatable<PaddingValue>
{
    private readonly string _value;

    private PaddingValue(string value)
    {
        _value = value ?? string.Empty;
    }

    /// <summary>
    /// Implicit conversion from PaddingBuilder to PaddingValue.
    /// </summary>
    public static implicit operator PaddingValue(PaddingBuilder builder)
    {
        return new PaddingValue(builder.ToClass());
    }

    /// <summary>
    /// Implicit conversion from string to PaddingValue.
    /// </summary>
    public static implicit operator PaddingValue(string value)
    {
        return new PaddingValue(value);
    }

    /// <summary>
    /// Implicit conversion from PaddingValue to string.
    /// </summary>
    public static implicit operator string(PaddingValue paddingValue)
    {
        return paddingValue._value;
    }

    /// <summary>
    /// Returns the string representation of the padding value.
    /// </summary>
    public override string ToString()
    {
        return _value;
    }

    /// <summary>
    /// Determines if this padding value is empty.
    /// </summary>
    public bool IsEmpty => string.IsNullOrEmpty(_value);

    /// <summary>
    /// Determines if this padding value contains CSS properties (contains ':'). 
    /// </summary>
    public bool IsCssStyle => _value.Contains(':');

    /// <summary>
    /// Determines if this padding value is a CSS class.
    /// </summary>
    public bool IsCssClass => !IsCssStyle && !IsEmpty;

    public bool Equals(PaddingValue other)
    {
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return obj is PaddingValue other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public static bool operator ==(PaddingValue left, PaddingValue right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(PaddingValue left, PaddingValue right)
    {
        return !left.Equals(right);
    }
}
