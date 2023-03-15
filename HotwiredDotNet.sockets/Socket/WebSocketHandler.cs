using System.Net.WebSockets;
using System.Text;
using HotwiredDotNet.Core.Models;
using HotwiredDotNet.Core.Services;
using HotwiredDotNet.Ui.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Sockets.Socket;

namespace sockets.Socket;

public abstract class WebSocketHandler
{
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly RazorViewComponentStringRenderer _razorViewComponentStringRenderer;
    protected ConnectionManager WebSocketConnectionManager { get; set; }
    protected WebSocketHandler(IActionContextAccessor actionContextAccessor,ConnectionManager webSocketConnectionManager, IHttpContextAccessor httpContextAccessor, RazorViewComponentStringRenderer razorViewComponentStringRenderer)
    {
        _actionContextAccessor = actionContextAccessor;
        _razorViewComponentStringRenderer = razorViewComponentStringRenderer;
        this.WebSocketConnectionManager = webSocketConnectionManager;
    }

    public virtual async Task OnConnected(WebSocket socket)
    {
        this.WebSocketConnectionManager.AddSocket(socket);

        var message = await TurboStream.AsStringAsync(_razorViewComponentStringRenderer, "stream-test", TurboStreamAction.Append,
            $"<li>Socket connected {DateTime.Now}</li>");

        await this.SendMessageAsync(socket, message).ConfigureAwait(false);
    }
    
    public virtual async Task OnDisconnected(WebSocket socket)
    {
        try
        {
            await this.WebSocketConnectionManager.RemoveSocket(this.WebSocketConnectionManager.GetId(socket)).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public async Task SendMessageAsync(WebSocket socket, string message)
    {
        if (socket.State != WebSocketState.Open)
        {
            return;
        }

        var encodedMessage = Encoding.UTF8.GetBytes(message);
        await socket.SendAsync(
            buffer: new ArraySegment<byte>(array: encodedMessage, offset: 0, count: encodedMessage.Length),
            messageType: WebSocketMessageType.Text,
            endOfMessage: true,
            cancellationToken: CancellationToken.None).ConfigureAwait(false);
    }

    public async Task SendMessageAsync(string socketId, string message)
    {
        await this.SendMessageAsync(this.WebSocketConnectionManager.GetSocketById(socketId), message).ConfigureAwait(false);
    }

    public async Task SendMessageToAllAsync(string message)
    {
        foreach (var pair in this.WebSocketConnectionManager.GetAll())
        {
            if (pair.Value.State == WebSocketState.Open)
            {
                await this.SendMessageAsync(pair.Value, message).ConfigureAwait(false);
            }
        }
    }

    // TODO: Decide if exposing the message string is better than exposing the result and buffer
    public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
}