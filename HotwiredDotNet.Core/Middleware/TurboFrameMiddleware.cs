using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;

namespace HotwiredDotNet.Core.Middleware;

public class TurboFrameMiddleware
{
    private readonly RequestDelegate _next;

    public TurboFrameMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        
        if (context.Request.Headers.ContainsKey("turbo-frame") && context.Request.Headers.TryGetValue("turbo-frame",out var frameName))
        {
            var response = context.Response;
            var originBody = response.Body;
            using var newBody = new MemoryStream();
            response.Body = newBody;

            await _next(context).ConfigureAwait(false);

            newBody.Position = 0;
            var responseBody = new StreamReader(newBody);
            var documentString =await responseBody.ReadToEndAsync();
            var document = new HtmlDocument();
            document.LoadHtml(documentString);
            
            //Get current frame target
            var frame = document.GetElementbyId(frameName.ToString());
            
            //Get any turbo-streams and append them after the frame
            var streams = document.DocumentNode.Descendants("turbo-stream");
            var responseHtml = frame.OuterHtml + " " + string.Join(" ", streams.Select(x => x.OuterHtml));
            
            await ModifyResponseAsync(context.Response, responseHtml);
            newBody.Seek(0, SeekOrigin.Begin);
            await newBody.CopyToAsync(originBody);
            context.Response.Body = originBody;
        }
        else
        {
            await _next(context);
        }
    }
    
    private async Task ModifyResponseAsync(HttpResponse response, string frame)
    {
        var stream = response.Body;
        using var reader = new StreamReader(stream, leaveOpen: true);
        stream.SetLength(0);
        await using var writer = new StreamWriter(stream, leaveOpen: true);
        await writer.WriteAsync(frame);
        await writer.FlushAsync();
        response.ContentLength = stream.Length;
    }
}