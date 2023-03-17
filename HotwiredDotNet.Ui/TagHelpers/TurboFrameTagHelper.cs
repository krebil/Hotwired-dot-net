using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;

namespace HotwiredDotNet.Ui.TagHelpers;

[HtmlTargetElement("turbo-frame", Attributes = ActionAttributeName)]
[HtmlTargetElement("turbo-frame", Attributes = ControllerAttributeName)]
[HtmlTargetElement("turbo-frame", Attributes = AreaAttributeName)]
[HtmlTargetElement("turbo-frame", Attributes = PageAttributeName)]
[HtmlTargetElement("turbo-frame", Attributes = PageHandlerAttributeName)]
[HtmlTargetElement("turbo-frame", Attributes = FragmentAttributeName)]
[HtmlTargetElement("turbo-frame", Attributes = HostAttributeName)]
[HtmlTargetElement("turbo-frame", Attributes = ProtocolAttributeName)]
[HtmlTargetElement("turbo-frame", Attributes = RouteAttributeName)]
[HtmlTargetElement("turbo-frame", Attributes = RouteValuesDictionaryName)]
[HtmlTargetElement("turbo-frame", Attributes = RouteValuesPrefix + "*")]
public class TurboFrameTagHelper : TagHelper
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IUrlHelperFactory _urlHelperFactory;
    private IHtmlGenerator Generator { get; }

    public TurboFrameTagHelper(IHtmlGenerator generator, LinkGenerator linkGenerator, IUrlHelperFactory urlHelperFactory)
    {
        _linkGenerator = linkGenerator;
        _urlHelperFactory = urlHelperFactory;
        Generator = generator;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (context == null || ViewContext == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (output == null)
        {
            throw new ArgumentNullException(nameof(output));
        }
        
        var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

        // If "src" is already set, it means the user is attempting to use a normal anchor.
        if (output.Attributes.ContainsName(Src))
        {
            if (Action != null ||
                Controller != null ||
                Area != null ||
                Page != null ||
                PageHandler != null ||
                Route != null ||
                Protocol != null ||
                Host != null ||
                Fragment != null ||
                (_routeValues != null && _routeValues.Count > 0))
            {
                // User specified an src and one of the bound attributes; can't determine the src attribute.
                throw new InvalidOperationException(
                    "Turbo-frame cannot have src and asp attributes");
            }

            return;
        }

        var routeLink = Route != null;
        var actionLink = Controller != null || Action != null;
        var pageLink = Page != null || PageHandler != null;

        if ((routeLink && actionLink) || (routeLink && pageLink) || (actionLink && pageLink))
        {
            throw new InvalidOperationException("Turbo-frame conflicting asp attributes");
        }

        RouteValueDictionary? routeValues = null;
        if (_routeValues != null && _routeValues.Count > 0)
        {
            routeValues = new RouteValueDictionary(_routeValues!);
        }

        if (Area != null)
        {
            // Unconditionally replace any value from asp-route-area.
            if (routeValues == null)
            {
                routeValues = new RouteValueDictionary();
            }
            routeValues["area"] = Area;
        }

        var tagBuilder = new TagBuilder("turbo-frame");
        if (pageLink)
        {
            var url = urlHelper.Page(Page, PageHandler, routeValues, Protocol, Host, Fragment);
            tagBuilder.Attributes.Add("src", url);
        }
        else if (routeLink)
        {
            
            var url = urlHelper.RouteUrl(Route, routeValues, Protocol, Host, Fragment);
            tagBuilder.Attributes.Add("src", url);
        }
        else
        {
            var url = urlHelper.Action(Action, Controller, routeValues, Protocol, Host, Fragment);
            tagBuilder.Attributes.Add("src", url);
        }

        output.MergeAttributes(tagBuilder);
    }

    
    /// <summary>
    /// The id of the target frame
    /// </summary>
    /// <remarks>
    /// Must be non-<c>null</c>.
    /// </remarks>
    [HtmlAttributeName(ActionAttributeName)]
    public string? Id { get; set; }
    
    /// <summary>
    /// The name of the action method.
    /// </summary>
    /// <remarks>
    /// Must be <c>null</c> if <see cref="Route"/> or <see cref="Page"/> is non-<c>null</c>.
    /// </remarks>
    [HtmlAttributeName(ActionAttributeName)]
    public string? Action { get; set; }
    
    
    

    /// <summary>
    /// The name of the controller.
    /// </summary>
    /// <remarks>
    /// Must be <c>null</c> if <see cref="Route"/> or <see cref="Page"/> is non-<c>null</c>.
    /// </remarks>
    [HtmlAttributeName(ControllerAttributeName)]
    public string? Controller { get; set; }

    /// <summary>
    /// The name of the area.
    /// </summary>
    /// <remarks>
    /// Must be <c>null</c> if <see cref="Route"/> is non-<c>null</c>.
    /// </remarks>
    [HtmlAttributeName(AreaAttributeName)]
    public string? Area { get; set; }

    /// <summary>
    /// The name of the page.
    /// </summary>
    /// <remarks>
    /// Must be <c>null</c> if <see cref="Route"/> or <see cref="Action"/>, <see cref="Controller"/>
    /// is non-<c>null</c>.
    /// </remarks>
    [HtmlAttributeName(PageAttributeName)]
    public string? Page { get; set; }

    /// <summary>
    /// The name of the page handler.
    /// </summary>
    /// <remarks>
    /// Must be <c>null</c> if <see cref="Route"/> or <see cref="Action"/>, or <see cref="Controller"/>
    /// is non-<c>null</c>.
    /// </remarks>
    [HtmlAttributeName(PageHandlerAttributeName)]
    public string? PageHandler { get; set; }

    /// <summary>
    /// The protocol for the URL, such as &quot;http&quot; or &quot;https&quot;.
    /// </summary>
    [HtmlAttributeName(ProtocolAttributeName)]
    public string? Protocol { get; set; }

    /// <summary>
    /// The host name.
    /// </summary>
    [HtmlAttributeName(HostAttributeName)]
    public string? Host { get; set; }

    /// <summary>
    /// The URL fragment name.
    /// </summary>
    [HtmlAttributeName(FragmentAttributeName)]
    public string? Fragment { get; set; }

    /// <summary>
    /// Name of the route.
    /// </summary>
    /// <remarks>
    /// Must be <c>null</c> if one of <see cref="Action"/>, <see cref="Controller"/>, <see cref="Area"/>
    /// or <see cref="Page"/> is non-<c>null</c>.
    /// </remarks>
    [HtmlAttributeName(RouteAttributeName)]
    public string? Route { get; set; }

    /// <summary>
    /// Additional parameters for the route.
    /// </summary>
    [HtmlAttributeName(RouteValuesDictionaryName, DictionaryAttributePrefix = RouteValuesPrefix)]
    public IDictionary<string, string>? RouteValues
    {
        get
        {
            if (_routeValues == null)
            {
                _routeValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }

            return _routeValues;
        }
        set
        {
            _routeValues = value;
        }
    }

    /// <summary>
    /// Gets or sets the <see cref="Microsoft.AspNetCore.Mvc.Rendering.ViewContext"/> for the current request.
    /// </summary>
    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext? ViewContext { get; set; }
    
    private const string ActionAttributeName = "asp-action";
    private const string ControllerAttributeName = "asp-controller";
    private const string AreaAttributeName = "asp-area";
    private const string PageAttributeName = "asp-page";
    private const string PageHandlerAttributeName = "asp-page-handler";
    private const string FragmentAttributeName = "asp-fragment";
    private const string HostAttributeName = "asp-host";
    private const string ProtocolAttributeName = "asp-protocol";
    private const string RouteAttributeName = "asp-route";
    private const string RouteValuesDictionaryName = "asp-all-route-data";
    private const string RouteValuesPrefix = "asp-route-";
    private const string Src = "src";

    private IDictionary<string, string>? _routeValues = new Dictionary<string, string>();

}