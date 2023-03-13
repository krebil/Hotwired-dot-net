using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace ***REMOVED***.***REMOVED***.ActionResults;

/// <summary>
/// A <see cref="IActionResultExecutor{RedirectToPageResult}"/> for <see cref="RedirectToPageResult"/>.
/// </summary>
public partial class SeeOtherResultExecutor : IActionResultExecutor<SeeOtherResult>
{
    private readonly ILogger _logger;
    private readonly IUrlHelperFactory _urlHelperFactory;

    /// <summary>
    /// Initializes a new instance of <see cref="SeeOtherResultExecutor"/>.
    /// </summary>
    /// <param name="loggerFactory">The factory used to create loggers.</param>
    /// <param name="urlHelperFactory">The factory used to create url helpers.</param>
    public SeeOtherResultExecutor(ILoggerFactory loggerFactory, IUrlHelperFactory urlHelperFactory)
    {
        if (loggerFactory == null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        if (urlHelperFactory == null)
        {
            throw new ArgumentNullException(nameof(urlHelperFactory));
        }

        _logger = loggerFactory.CreateLogger<SeeOtherResultExecutor>();
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
        context.HttpContext.Response.Headers.Location = destinationUrl;

        return Task.CompletedTask;
    }

   
}