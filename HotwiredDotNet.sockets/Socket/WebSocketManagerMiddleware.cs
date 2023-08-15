using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;

namespace Sockets.Socket;

public class WebSocketManagerMiddleware
{
    private readonly RequestDelegate next;

    public WebSocketManagerMiddleware(
        RequestDelegate next,
        WebSocketHandler webSocketHandler)
    {
        this.next = next;
        WebSocketHandler = webSocketHandler;
    }

    private WebSocketHandler WebSocketHandler { get; }

    public async Task Invoke(HttpContext context)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            return;
        }

        var socket = await context.WebSockets.AcceptWebSocketAsync().ConfigureAwait(false);
        await WebSocketHandler.OnConnected(socket).ConfigureAwait(false);

        await Receive(socket, async (result, buffer) =>
        {
            switch (result.MessageType)
            {
                case WebSocketMessageType.Text:
                    await WebSocketHandler.ReceiveAsync(socket, result, buffer).ConfigureAwait(false);
                    return;
                case WebSocketMessageType.Close:
                    await WebSocketHandler.OnDisconnected(socket).ConfigureAwait(false);
                    return;
            }
        }).ConfigureAwait(false);
        //This throws a kestrel error if it's the last middleware in the pipeline
        try
        {
            await next.Invoke(context);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
    {
        var buffer = new byte[1024 * 4];

        while (socket.State == WebSocketState.Open)
        {
            try
            {
                var result = await socket.ReceiveAsync(
                    buffer: new ArraySegment<byte>(buffer),
                    cancellationToken: CancellationToken.None).ConfigureAwait(false);

                handleMessage(result, buffer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
