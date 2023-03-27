using HotwiredDotNet.Core.ActionResults;
using HotwiredDotNet.Core.Helpers;
using HotwiredDotNet.Core.Models;
using HotwiredDotNet.Core.Services;
using HotwiredDotNet.Ui.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Components;

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly RazorViewComponentStringRenderer _razorViewComponentStringRenderer;

    public IndexModel(ILogger<IndexModel> logger, RazorViewComponentStringRenderer razorViewComponentStringRenderer)
    {
        _logger = logger;
        _razorViewComponentStringRenderer = razorViewComponentStringRenderer;
    }

    public void OnGet()
    {
    }

    public IActionResult OnPostError()
    {
        ModelState.AddModelError("", "I'm an error");
        
        //Turbo form responses must be 422 or 50x
        /*var pageResult = Page() ;
        pageResult.StatusCode = 422;*/
        //Same as above.
        //there is also a version of SetUnprocessableEntityStatusCode for ViewResult
        return Page().SetUnprocessableEntityStatusCode();
    }
    
    public async Task<IActionResult> OnGetRenderComponent()
    {
        HttpContext.SetTurboStreamMimeType();
        
        var template = await _razorViewComponentStringRenderer.RenderAsync(AlertViewComponent.Name, new
        {
            alertType = AlertType.Success,
            text = "I'm a view component rendered from a turbo stream"
        });
        return ViewComponent(TurboStream.Name,
            new { target = "http-stream", action = TurboStreamAction.Append, template = template });
    }
    

    
    public IActionResult OnPost()
    {
        //Extension method to allow for turbo-stream responses
        HttpContext.SetTurboStreamMimeType();
        
        //View component that renders a turbo-stream
        //Action defaults to append
        //Template defaults to null
        return ViewComponent(TurboStream.Name,
            new { 
                target = "http-stream",
                action = TurboStreamAction.Append,
                template = "<h1>I've been appended from a turbo stream</h1>"
            });
    }
}