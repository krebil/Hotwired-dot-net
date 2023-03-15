using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace HotwiredDotNet.Core.ActionResults;

/// <summary>
/// A <see cref="IActionResultExecutor{TResult}"/> for <see cref="RedirectToPageResult"/>.
/// </summary>
public partial class SeeOtherResultExecutor : IActionResultExecutor<SeeOtherResult>
{
    private readonly IUrlHelperFactory _urlHelperFactory;

    /// <summary>
    /// Initializes a new instance of <see cref="SeeOtherResultExecutor"/>.
    /// </summary>
    /// <param name="urlHelperFactory">The factory used to create url helpers.</param>
    public SeeOtherResultExecutor(IUrlHelperFactory urlHelperFactory)
    {
        if (urlHelperFactory == null)
        {
            throw new ArgumentNullException(nameof(urlHelperFactory));
        }

        _urlHelperFactory = urlHelperFactory;
    }

    /// <inheritdoc />
    public virtual Task ExecuteAsync(ActionContext context, SeeOtherResult result)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (result == null)
        {
            throw new ArgumentNullException(nameof(result));
        }

        var urlHelper = result.UrlHelper ?? _urlHelperFactory.GetUrlHelper(context);
        var destinationUrl = urlHelper.Page(
            result.PageName,
            result.PageHandler,
            result.RouteValues,
            result.Protocol,
            result.Host,
            fragment: result.Fragment);

        if (string.IsNullOrEmpty(destinationUrl))
        {
            throw new InvalidOperationException();
        }


        context.HttpContext.Response.StatusCode = StatusCodes.Status303SeeOther;
        context.HttpContext.Response.Headers["Location"] = destinationUrl;
        context.HttpContext.Response.Headers["location"] = destinationUrl;

        return Task.CompletedTask;
    }
}