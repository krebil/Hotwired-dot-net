using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SaudiAramcoXGpt.PdfIngestor.Services;


public class MyViewComponentContext
{
    public HttpContext? HttpContext { get; init; }

    public ActionContext? ActionContext { get; init; }

    public ViewDataDictionary? ViewData { get; init; }

    public ITempDataDictionary? TempData { get; init; }
}

public class RazorViewComponentStringRenderer
{
    private readonly ITempDataProvider _tempDataProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IActionContextAccessor _actionContext;

    public RazorViewComponentStringRenderer(ITempDataProvider tempDataProvider, IHttpContextAccessor httpContextAccessor, IActionContextAccessor actionContext)
    {
        _tempDataProvider = tempDataProvider;
        _httpContextAccessor = httpContextAccessor;
        _actionContext = actionContext;
    }

    public Task<string> RenderAsync(string viewName, object model)
    {
        if (_httpContextAccessor == null)
        {
            throw new ArgumentNullException(nameof(_httpContextAccessor));
        }

        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null || _actionContext.ActionContext == null)
        {
            throw new ArgumentNullException();
        }

        var localActionContext = new ActionContext(httpContext, httpContext.GetRouteData(), _actionContext.ActionContext.ActionDescriptor);

        var context = new MyViewComponentContext
        {
            HttpContext = httpContext,
            ActionContext = localActionContext,
            ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            },
            TempData = new TempDataDictionary(
                httpContext,
                _tempDataProvider),
        };

        return Render(context, viewName, model);
    }

    private IViewComponentHelper GetViewComponentHelper(MyViewComponentContext context, StringWriter sw)
    {
        if (context.HttpContext == null || context.ActionContext == null || context.ViewData == null || context.TempData == null)
        {
            throw new ArgumentNullException();
        }

        var viewContext = new ViewContext(context.ActionContext, NullView.Instance, context.ViewData, context.TempData, sw, new HtmlHelperOptions());
        var helper = context.HttpContext.RequestServices.GetRequiredService<IViewComponentHelper>();
        (helper as IViewContextAware)?.Contextualize(viewContext);
        return helper;
    }

    private async Task<string> Render(MyViewComponentContext myViewComponentContext, string viewComponentName, object args)
    {
        await using var writer = new StringWriter();
        var helper = GetViewComponentHelper(myViewComponentContext, writer);
        var result = await helper.InvokeAsync(viewComponentName, args);
        result.WriteTo(writer, HtmlEncoder.Default);
        await writer.FlushAsync();
        return writer.ToString();
    }

    private class NullView : IView
    {
        public static readonly NullView Instance = new NullView();

        public string Path => string.Empty;

        public Task RenderAsync(ViewContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return Task.CompletedTask;
        }
    }
}