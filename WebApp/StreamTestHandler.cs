using System.Net.WebSockets;
using System.Text;
using HotwiredDotNet.Core.Models;
using HotwiredDotNet.Core.Services;
using HotwiredDotNet.Ui.Components;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using sockets.Socket;
using Sockets.Socket;

namespace WebApp;

public class StreamTestHandler : WebSocketHandler
{
    private readonly RazorViewComponentStringRenderer _razorViewComponentStringRenderer;

    public StreamTestHandler(ConnectionManager webSocketConnectionManager, IHttpContextAccessor httpContextAccessor, IActionContextAccessor actionContextAccessor, RazorViewComponentStringRenderer razorViewComponentStringRenderer)
        : base(actionContextAccessor, webSocketConnectionManager, httpContextAccessor, razorViewComponentStringRenderer)
    {
        _razorViewComponentStringRenderer = razorViewComponentStringRenderer;
    }

    public override async Task OnConnected(WebSocket socket)
    {
        await base.OnConnected(socket);
        var socketId = WebSocketConnectionManager.GetId(socket);
        var message = await TurboStream.AsStringAsync(_razorViewComponentStringRenderer, "stream-test", TurboStreamAction.Append,
            $"<li>{socketId} is now connected</li>");
        await SendMessageToAllAsync(message);
    }

    public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
    {
        var socketId = WebSocketConnectionManager.GetId(socket);

        var message = await TurboStream.AsStringAsync(_razorViewComponentStringRenderer, "stream-test", TurboStreamAction.Append,
            $"<li>{socketId} said: {Encoding.UTF8.GetString(buffer, 0, result.Count)}");

        await this.SendMessageToAllAsync(message.ToString());
    }
}
