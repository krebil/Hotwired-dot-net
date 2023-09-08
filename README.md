# Hotwired .Net

## Installation

Find the NuGet packages here:

[HotwiredDotNet.Core](https://www.nuget.org/packages/HotwiredDotNet.Core/)

[HotwiredDotNet.Ui](https://www.nuget.org/packages/HotwiredDotNet.Ui/)


Once installed add the following to your program.cs file:

``` 
builder.RegisterHotwiredDotNetCore(); 
```

If you want intellisense for the TurboStream ViewComponent, add the following to your _ViewImports.cshtml file:

```
@addTagHelper *, HotwiredDotNet.Ui
```

## Usage

### TurboFrames

Turbo frames can be rendered using a tag-helper to generate the url like with the .net anchorTagHelper and formTagHelper.

```html
<turbo-frame id="frame" asp-page="TurboFrame" asp-page-handler="OnGet" >
        
</turbo-frame>
```

This will result in:
```html
<turbo-frame id="frame" src="https://localhost:44362/TurboFrame?handler=OnGet" complete="">

</turbo-frame>
```

It also works for mvc routes eg. **asp-controller** and **asp-action**

### TurboStream

#### Views

In a view you can use your view-component with either tag-helper syntax,

```html
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
Turbo [expects the server to return 303 on form submission](https://turbo.hotwired.dev/handbook/drive#redirecting-after-a-form-submission). To better allow for this, a new ActionResult has been introduces called SeeOther. While a standard 302 redirect will sometimes work it's important to note the differences between 302 and 303. 302 will redirect but if you use put, delete or patch the same method will be used for the redirect while 303 will always use **GET** after a redirect:.

| fetchoriginal method | fetchoriginal response | redirect method |
| -------- | ------- | ------- |
| GET | 302 | GET |
| GET |	303 | GET |
| POST | 302 | GET |
| POST | 303 | GET |
| PUT | 302 | **PUT** |
| PUT | 303 | GET |
| PATCH | 302 | **PATCH** |
| PATCH | 303 |	GET |
| DELETE | 302 | **DELETE** |
| DELETE | 303 | GET |



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
Sometimes it makes sense to return the current page after a form submission, for this there is an extension method on Page and View results. 
```c#
    public IActionResult OnPost()
    {
        return Page().SeeOther();
    }
```
```c#
    public IActionResult OnPost()
    {
        return View().SeeOther();
    }
```

### Form errors
for form errors you can use a similar extension method on the Page and View results to change the status code to 422, since turbo expects either 422 or 50X on form errors.
```c#
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page().UnprocessableEntity();
        }
        return Page().SeeOther();
    }
```
```c#
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return View().UnprocessableEntity();
        }
        return View().SeeOther();
    }
```

## Other stuff
This is all the stuff that works behind the scenes, that you don't have to actively use.

### AnchorTagHelper
A tag-helper that looks whether an anchor tag has the **[data-turbo-method](https://turbo.hotwired.dev/reference/attributes)** attribute. If it does, it will add an anti-forgery token so that when turbo generates a form it validate correctly.

## Middleware

### TurboFrame Middleware
The TurboFrame middleware removes unused html from the response body when the request has the **turbo-frame** header present, which is default in turbo-frame requests. This is done to reduce the amount of data sent over the wire.
It also ensures that any turbo-streams will **not** be removed from the response.

To add the middleware to your pipeline, add the following to your Startup.cs/Program.cs file:
```c#
app.UseTurboFrameMiddleware();
```

## Work in progress
The socket implementation seen in the demo project is a work in progress. I'm still trying to figure out if there is a good way to make some generic implementation.

