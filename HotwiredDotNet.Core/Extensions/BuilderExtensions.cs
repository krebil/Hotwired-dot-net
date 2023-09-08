using HotwiredDotNet.Core.ActionResults;
using HotwiredDotNet.Core.Middleware;
using HotwiredDotNet.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace HotwiredDotNet.Core.Extensions;

public static class BuilderExtensions
{
    public static void RegisterHotwiredDotNetCore(this WebApplicationBuilder? builder)
    {
        if (builder == null)
            return;
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        builder.Services.AddSingleton<IActionResultExecutor<SeeOtherResult>, SeeOtherResultExecutor>();
        builder.Services.AddTransient<RazorViewComponentStringRenderer, RazorViewComponentStringRenderer>();
    }


    public static IApplicationBuilder UseTurboFrameMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TurboFrameMiddleware>();
    }
}