

using Microsoft.AspNetCore.Mvc;

namespace WebApp.Components;

[ViewComponent]
public class AlertViewComponent : ViewComponent
{
    public const string Name = "Alert";
    public AlertType AlertType { get; set; } = AlertType.Info;
    public string Text { get; set; } = "";
    public IViewComponentResult Invoke(string text, AlertType? alertType)
    {
        AlertType = alertType ?? AlertType.Info;
        Text = text;   
        return View(this);
    }
}

public enum AlertType
{
    Success,
    Warning,
    Info,
    Error
}