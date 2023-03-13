using Microsoft.AspNetCore.Mvc;
using SaudiAramcoXGpt.PdfIngestor.Constants;

namespace SaudiAramcoXGpt.PdfIngestor.Components;

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