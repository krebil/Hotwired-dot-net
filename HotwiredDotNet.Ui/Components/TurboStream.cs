using HotwiredDotNet.Core.Models;
using HotwiredDotNet.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotwiredDotNet.Ui.Components;

[ViewComponent]
public class TurboStream : ViewComponent
{
    public const string Name = "TurboStream";

    public string Target { get; set; } = "";
    public string Action { get; set; } = "";
    public string? Template { get; set; } = "";

    public IViewComponentResult Invoke(string target, TurboStreamAction action = TurboStreamAction.Append,
        string? template = null)
    {
        Target = target;
        Action = action.ToString().ToLower();
        Template = template;
        return View(this);
    }

    public static async Task<string> AsStringAsync(RazorViewComponentStringRenderer razorViewComponentStringRenderer, string target, TurboStreamAction action, string? template = null)
    {
       return await razorViewComponentStringRenderer.RenderAsync(TurboStream.Name,
            new { target = target, action = action, template = template });
    }
    
    public static async Task<string> AsStringAsync(RazorViewComponentStringRenderer razorViewComponentStringRenderer, string target, TurboStreamAction action, string viewComponentName, object model)
    {
        var innerViewComponent = razorViewComponentStringRenderer.RenderAsync(viewComponentName, model);
        return await razorViewComponentStringRenderer.RenderAsync(TurboStream.Name,
            new { target = target, action = action, template = innerViewComponent });
    }
}