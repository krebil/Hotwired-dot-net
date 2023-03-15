using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Sockets.Socket;

namespace sockets.Socket;

public static class WebSocketManagerExtensions
{
    public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

        services.AddTransient<ConnectionManager>();

        // ReSharper disable once PossibleNullReferenceException
        foreach (var type in Assembly.GetEntryAssembly()!.ExportedTypes)
        {
            if (type.GetTypeInfo().BaseType == typeof(WebSocketHandler))
            {
                services.AddSingleton(type);
            }
        }

        return services;
    }

    public static IApplicationBuilder MapWebSocketManager(
        this IApplicationBuilder app,
        PathString path,
        WebSocketHandler handler)
    {
        return app.Map(path, (x) => x.UseMiddleware<WebSocketManagerMiddleware>(handler));
    }
}
