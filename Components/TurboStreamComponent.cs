using Microsoft.AspNetCore.Mvc;
using ***REMOVED***.***REMOVED***.Constants;

namespace ***REMOVED***.***REMOVED***.Components;

[ViewComponent]
public class TurboStreamComponent : ViewComponent
{
    public string Target { get; set; } = "";
    public string Action { get; set; } = "";
    public string? Template { get; set; } = "";
    public IViewComponentResult Invoke(string target, string action = TurboStreamActions.Append, string? template = null)
    {
        Target = target;
        Action = action;
        Template = template;
        return View(this);
    }
}