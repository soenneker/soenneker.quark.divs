using Microsoft.AspNetCore.Components;

namespace Soenneker.Quark.Divs;

public abstract class Element : Component
{
    [Parameter] 
    public RenderFragment ChildContent { get; set; }
}