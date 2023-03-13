namespace SaudiAramcoXGpt.PdfIngestor.Helpers;

public static class TurboStreamHelpers
{
    public static void SetTurboStreamMimeType(this HttpContext context)
    {
        context.Response.SetTurboStreamMimeType();
    }
    
    public static void SetTurboStreamMimeType(this HttpResponse response)
    {
        response.ContentType = "text/vnd.turbo-stream.html";
    }
}