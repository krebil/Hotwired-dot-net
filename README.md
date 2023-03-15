# Hotwired .Net

## Installation

Once installed add the following to your program.cs file:

``` 
builder.RegisterHotwiredDotNetCore(); 
```

If you want intellisense for the TurboStream ViewComponent, add the following to your _ViewImports.cshtml file:

```
@addTagHelper *, HotwiredDotNet.Ui
```

## Usage

### TurboStream

#### Views

In view you can use your view-component with either tag-helper syntax,

```razor
<vc:turbo-stream action="Update" target="stream-target" template="Html"></vc:turbo-stream>
```

or Component.Invoke

```razor
@await Component.InvokeAsync(TurboStream.Name, new
{
    target = "stream-target",
    action = TurboStreamAction.Append,
    template = "Html content"
})
```

#### Controller

Sometimes it can be useful to get the Turbo-stream view component as a string, for example when you want to [chain multiple
Turbo-streams in one request](https://turbo.hotwired.dev/handbook/streams#actions-with-multiple-targets)

```c#
public async Task<IActionResult> OnGet()
{
    //the HttpContext.SetTurboStreamMimeType(), is an extension method that sets the content-type header to text/vnd.turbo-stream.html, which allows turbo to pick up stream responses from normal http requests.
    HttpContext.SetTurboStreamMimeType();
    
    var resultString = await TurboStream.AsStringAsync(_razorViewComponentStringRenderer, "first-target", TurboStreamAction.Append, "Html");
    resultString += await TurboStream.AsStringAsync(_razorViewComponentStringRenderer, "second-target", TurboStreamAction.Append, "Html");
    return new ContentResult(resultString);
}
```

### Rendering ViewComponent inside a stream response
The RazorViewComponentStringRenderer can be used to make a view component render as a string.
```c#
public async Task<IActionResult> OnGetRenderComponent()
{
    HttpContext.SetTurboStreamMimeType();
        
    var template = await _razorViewComponentStringRenderer.RenderAsync(AlertViewComponent.Name, new
    {
        alertType = AlertType.Success,
        text = "I'm a view component rendered from a turbo stream"
    });
    return ViewComponent(TurboStream.Name,new { target = "http-stream", action = TurboStreamAction.Append, template = template });
}
```
Alternatively there is another helper method on TurboStream that can be used to render a view component inside a stream response
```c#
public async Task<IActionResult> OnGetRenderComponent()
{
   HttpContext.SetTurboStreamMimeType();

   return ContentResult(await TurboStream.AsStringAsync(_razorViewComponentStringRenderer, "target", TurboStreamAction.Append,
            AlertViewComponent.Name, new
            {
                alertType = AlertType.Success,
                text = "I'm a view component rendered from a turbo stream"
            });
}
```
### Post / Redirect / Get
Turbo [expects the server to return 303 on form submission](https://turbo.hotwired.dev/handbook/drive#redirecting-after-a-form-submission). To better allow for the a new ActionResult has been introduces called SeeOther
```c#
    public IActionResult OnPost()
    {
        //You can pass data between pages using the TempData dictionary as normal
        AlertHelper.SetAlert(TempData, "I'm an alert from before the redirect");
        AlertHelper.SetAlertType(TempData, AlertType.Warning);
        
        //The see other result allows ud to use the Post/Redirect/Get pattern https://en.wikipedia.org/wiki/Post/Redirect/Get
        //This is necessary since turbo expects form redirects to be 303 and not 301 or 302
        return new SeeOtherResult("SeeOtherPage");
    }
```

### Other stuff
This is all the stuff that works behind the scenes, that you don't have to actively use.

#### AnchorTagHelper
A tag-helper that looks whether an anchor tag has the **[data-turbo-method](https://turbo.hotwired.dev/reference/attributes)** attribute. If it does, it will add an anti-forgery token so that when turbo generates a form it validate correctly.
