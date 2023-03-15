using System.Collections.Concurrent;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using sockets;

namespace Sockets.Socket;

public class ConnectionManager
{
    private readonly ConcurrentDictionary<string, WebSocket> sockets = new ();

    private readonly IHttpContextAccessor httpContextAccessor;

    public ConnectionManager(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public WebSocket GetSocketById(string id)
    {
        return sockets.FirstOrDefault(p => p.Key == id).Value;
    }

    public ConcurrentDictionary<string, WebSocket> GetAll()
    {
        return sockets;
    }

    public string GetId(WebSocket socket)
    {
        return sockets.FirstOrDefault(p => p.Value == socket).Key;
    }

    public void AddSocket(WebSocket socket)
    {
        sockets.TryAdd(httpContextAccessor.HttpContext?.Session.GetOrCreateSessionId() ?? "no-session-" + Guid.NewGuid().ToString("D"), socket);
    }

    public async Task RemoveSocket(string id)
    {
        sockets.TryRemove(id, out var socket);

        if (socket == null)
        {
            return;
        }

        if (socket.State != WebSocketState.Open)
        {
            return;
        }

        await socket.CloseAsync(
            closeStatus: WebSocketCloseStatus.NormalClosure,
            statusDescription: "Closed by the ConnectionManager",
            cancellationToken: CancellationToken.None).ConfigureAwait(false);
    }
}
