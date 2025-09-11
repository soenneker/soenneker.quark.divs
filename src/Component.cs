using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Soenneker.Blazor.Extensions.EventCallback;
using Soenneker.Extensions.String;
using Soenneker.Quark.Enums.DisplayTypes;
using Soenneker.Quark.Enums.Floats;
using Soenneker.Quark.Enums.Shadows;
using Soenneker.Quark.Enums.TextOverflows;
using Soenneker.Quark.Enums.VerticalAligns;
using Soenneker.Quark.Enums.Visibilities;
using Soenneker.Quark.Divs.Margin;
using Soenneker.Quark.Divs.Padding;

namespace Soenneker.Quark.Divs;

/// <summary>
/// Base component class that serves as the building block for all HTML elements in Quark.
/// </summary>
public abstract class Component : ComponentBase, IDisposable, IAsyncDisposable
{
    private bool _disposed;
    private bool _asyncDisposed;

    [Parameter]
    public string? Id { get; set; }

    [Parameter]
    public string? Class { get; set; }

    [Parameter]
    public string? Style { get; set; }

    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public int? TabIndex { get; set; }

    [Parameter]
    public bool Hidden { get; set; }

    [Parameter]
    public DisplayType? Display { get; set; }

    [Parameter]
    public Visibility? Visibility { get; set; }

    [Parameter]
    public Float? Float { get; set; }

    [Parameter]
    public VerticalAlign? VerticalAlign { get; set; }

    [Parameter]
    public TextOverflow? TextOverflow { get; set; }

    [Parameter]
    public Shadow? BoxShadow { get; set; }

    [Parameter]
    public MarginValue? Margin { get; set; }

    [Parameter]
    public PaddingValue? Padding { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnDoubleClick { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnMouseOver { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnMouseOut { get; set; }

    [Parameter]
    public EventCallback<KeyboardEventArgs> OnKeyDown { get; set; }

    [Parameter]
    public EventCallback<FocusEventArgs> OnFocus { get; set; }

    [Parameter]
    public EventCallback<FocusEventArgs> OnBlur { get; set; }

    [Parameter]
    public ElementReference ElementRef { get; set; }

    [Parameter]
    public Action<ElementReference>? ElementRefChanged { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? Attributes { get; set; }

    protected bool Disposed => _disposed;

    protected bool AsyncDisposed => _asyncDisposed;

    protected virtual Dictionary<string, object> BuildAttributes()
    {
        var attributes = new Dictionary<string, object>();

        if (!Id.IsNullOrEmpty())
            attributes["id"] = Id;

        if (!Class.IsNullOrEmpty())
            attributes["class"] = Class;

        if (!Style.IsNullOrEmpty())
            attributes["style"] = Style;

        if (!Title.IsNullOrEmpty())
            attributes["title"] = Title;

        if (TabIndex.HasValue)
            attributes["tabindex"] = TabIndex.Value;

        if (Hidden)
            attributes["hidden"] = true;

        if (Display != null)
            attributes["style"] = AppendToStyle(attributes.GetValueOrDefault("style")
                    ?.ToString(), $"display: {Display.Value}");

        if (Visibility != null)
            attributes["style"] = AppendToStyle(attributes.GetValueOrDefault("style")
                    ?.ToString(), $"visibility: {Visibility.Value}");

        if (Float != null)
            attributes["style"] = AppendToStyle(attributes.GetValueOrDefault("style")
                    ?.ToString(), $"float: {Float.Value}");

        if (VerticalAlign != null)
            attributes["style"] = AppendToStyle(attributes.GetValueOrDefault("style")
                    ?.ToString(), $"vertical-align: {VerticalAlign.Value}");

        if (TextOverflow != null)
            attributes["style"] = AppendToStyle(attributes.GetValueOrDefault("style")
                    ?.ToString(), $"text-overflow: {TextOverflow.Value}");

        if (BoxShadow != null)
            attributes["style"] = AppendToStyle(attributes.GetValueOrDefault("style")
                    ?.ToString(), $"box-shadow: {BoxShadow.Value}");

        if (Margin.HasValue && !Margin.Value.IsEmpty)
        {
            MarginValue marginValue = Margin.Value;
            
            if (marginValue.IsCssStyle)
            {
                attributes["style"] = AppendToStyle(attributes.GetValueOrDefault("style")
                        ?.ToString(), marginValue.ToString());
            }
            else
            {
                attributes["class"] = AppendToClass(attributes.GetValueOrDefault("class")
                        ?.ToString(), marginValue.ToString());
            }
        }

        if (Padding.HasValue && !Padding.Value.IsEmpty)
        {
            PaddingValue paddingValue = Padding.Value;
            
            if (paddingValue.IsCssStyle)
            {
                attributes["style"] = AppendToStyle(attributes.GetValueOrDefault("style")
                        ?.ToString(), paddingValue.ToString());
            }
            else
            {
                attributes["class"] = AppendToClass(attributes.GetValueOrDefault("class")
                        ?.ToString(), paddingValue.ToString());
            }
        }

        if (Attributes != null)
        {
            foreach (KeyValuePair<string, object> attribute in Attributes)
            {
                attributes[attribute.Key] = attribute.Value;
            }
        }

        return attributes;
    }

    protected virtual string AppendToStyle(string? existingStyle, string newStyle)
    {
        if (existingStyle.IsNullOrEmpty())
            return newStyle;

        return $"{existingStyle}; {newStyle}";
    }

    protected virtual string AppendToClass(string? existingClass, string newClass)
    {
        if (existingClass.IsNullOrEmpty())
            return newClass;

        return $"{existingClass} {newClass}";
    }

    protected virtual async Task HandleClick(MouseEventArgs args)
    {
        await OnClick.InvokeIfHasDelegate(args);
    }

    protected virtual async Task HandleDoubleClick(MouseEventArgs args)
    {
        await OnDoubleClick.InvokeIfHasDelegate(args);
    }

    protected virtual async Task HandleKeyDown(KeyboardEventArgs args)
    {
        await OnKeyDown.InvokeIfHasDelegate(args);
    }

    protected virtual async Task HandleFocus(FocusEventArgs args)
    {
        await OnFocus.InvokeIfHasDelegate(args);
    }

    protected virtual async Task HandleMouseOver(MouseEventArgs args)
    {
        await OnMouseOver.InvokeIfHasDelegate(args);
    }

    protected virtual async Task HandleMouseOut(MouseEventArgs args)
    {
        await OnMouseOut.InvokeIfHasDelegate(args);
    }

    protected virtual async Task HandleBlur(FocusEventArgs args)
    {
        await OnBlur.InvokeIfHasDelegate(args);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsync(true);
        Dispose(false);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            OnDispose();
            _disposed = true;
        }
    }

    protected virtual ValueTask DisposeAsync(bool disposing)
    {
        if (!_asyncDisposed && disposing)
        {
            OnDisposeAsync();
            _asyncDisposed = true;
        }

        return ValueTask.CompletedTask;
    }

    protected virtual void OnDispose()
    {
    }

    protected virtual Task OnDisposeAsync()
    {
        return Task.CompletedTask;
    }
}