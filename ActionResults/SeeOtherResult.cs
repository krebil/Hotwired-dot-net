// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ***REMOVED***.***REMOVED***.ActionResults;

/// <summary>
/// An <see cref="ActionResult"/> that returns a Found (302)
/// or Moved Permanently (301) response with a Location header.
/// Targets a registered route.
/// </summary>
public class SeeOtherResult : ActionResult, IKeepTempDataResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SeeOtherResult"/> with the values
    /// provided.
    /// </summary>
    /// <param name="pageName">The page to redirect to.</param>
    public SeeOtherResult(string? pageName)
        : this(pageName, routeValues: null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SeeOtherResult"/> with the values
    /// provided.
    /// </summary>
    /// <param name="pageName">The page to redirect to.</param>
    /// <param name="pageHandler">The page handler to redirect to.</param>
    public SeeOtherResult(string? pageName, string? pageHandler)
        : this(pageName, pageHandler, routeValues: null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SeeOtherResult"/> with the values
    /// provided.
    /// </summary>
    /// <param name="pageName">The page to redirect to.</param>
    /// <param name="routeValues">The parameters for the route.</param>
    public SeeOtherResult(string? pageName, object? routeValues)
        : this(pageName, pageHandler: null, routeValues: routeValues)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SeeOtherResult"/> with the values
    /// provided.
    /// </summary>
    /// <param name="pageName">The page to redirect to.</param>
    /// <param name="pageHandler">The page handler to redirect to.</param>
    /// <param name="routeValues">The parameters for the route.</param>
    public SeeOtherResult(string? pageName, string? pageHandler, object? routeValues)
        : this(pageName, pageHandler, routeValues, null)
    {
    }






    /// <summary>
    /// Initializes a new instance of the <see cref="SeeOtherResult"/> with the values
    /// provided.
    /// </summary>
    /// <param name="pageName">The name of the page.</param>
    /// <param name="pageHandler">The page handler to redirect to.</param>
    /// <param name="routeValues">The parameters for the page.</param>
    /// <param name="fragment">The fragment to add to the URL.</param>
    public SeeOtherResult(
        string? pageName,
        string? pageHandler,
        object? routeValues,
        string? fragment)
    {
        PageName = pageName;
        PageHandler = pageHandler;
        RouteValues = routeValues == null ? null : new RouteValueDictionary(routeValues);
        Fragment = fragment;
    }

    /// <summary>
    /// Gets or sets the <see cref="IUrlHelper" /> used to generate URLs.
    /// </summary>
    public IUrlHelper? UrlHelper { get; set; }

    /// <summary>
    /// Gets or sets the name of the page to route to.
    /// </summary>
    public string? PageName { get; set; }

    /// <summary>
    /// Gets or sets the page handler to redirect to.
    /// </summary>
    public string? PageHandler { get; set; }

    /// <summary>
    /// Gets or sets the route data to use for generating the URL.
    /// </summary>
    public RouteValueDictionary? RouteValues { get; set; }

    /// <summary>
    /// Gets or sets the fragment to add to the URL.
    /// </summary>
    public string? Fragment { get; set; }

    /// <summary>
    /// Gets or sets the protocol for the URL, such as "http" or "https".
    /// </summary>
    public string? Protocol { get; set; }

    /// <summary>
    /// Gets or sets the host name of the URL.
    /// </summary>
    public string? Host { get; set; }

    /// <inheritdoc />
    public override Task ExecuteResultAsync(ActionContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var executor = context.HttpContext.RequestServices.GetRequiredService<IActionResultExecutor<SeeOtherResult>>();
        return executor.ExecuteAsync(context, this);
    }
}
