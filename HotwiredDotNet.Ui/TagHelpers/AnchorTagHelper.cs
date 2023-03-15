using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HotwiredDotNet.Ui.TagHelpers;

[HtmlTargetElement("a", Attributes = Method)]
public class AnchorTagHelper : Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper
{
    private readonly IAntiforgery _antiforgery;
    private const string Method = "data-turbo-method";
    private const string Get = "Get";
    private const string AllRouteData = "asp-all-route-data";
    private const string Href = "href";

    public AnchorTagHelper(IHtmlGenerator generator, IAntiforgery antiforgery) : base(generator)
    {
        _antiforgery = antiforgery;
    }

    //Run after the built in tag helpers
    public override int Order => int.MaxValue;
    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var method = context.AllAttributes.FirstOrDefault(a => Method == a.Name);

        if (!IsGetMethod(method))
        {
            var af = _antiforgery.GetTokens(ViewContext.HttpContext);

            var url = "";
            //Get the href that's been processed by the built in .Net tag helpers  
            if(output.Attributes.TryGetAttribute(Href, out var href))
            {
                url = href.Value.ToString() ?? "";
            }
            if (url.Contains('?'))
            {
                url += $"&{af.FormFieldName}={af.RequestToken}";
            }
            else
            {
                url += $"?{af.FormFieldName}={af.RequestToken}";
            }
            output.Attributes.SetAttribute(Href,url);
           
        }
        
        return Task.CompletedTask;
    }

    private static bool IsGetMethod(TagHelperAttribute? method)
    {
        if (method?.Value == null)
            return false;
        var comparison = string.Compare(method.Value.ToString(), Get, StringComparison.InvariantCultureIgnoreCase);
        return comparison == 0;
    }

}